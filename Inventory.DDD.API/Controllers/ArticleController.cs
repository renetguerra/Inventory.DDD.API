using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.DDD.Application.Commands;
using Inventory.DDD.Application.Extensions;
using Inventory.DDD.Application.Queries;
using Inventory.DDD.Domain;
using Inventory.DDD.Domain.DTOs;
using Inventory.DDD.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Inventory.DDD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticleController : ControllerBase
    {        
        private readonly ILogger<ArticleController> _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;        
        public ArticleController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ArticleController> logger, IMediator mediator)
        {            
            this._logger = logger;
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene el listado de artículos, usando el patrón CQRS(GetArticlesQuery).
        /// Comprueba si algunos de los artículos obtenidos se encuentra expirado y lo desactiva con borrado lógico.
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public IActionResult GetArticles()
        {
            var response = _mediator.Send(new GetArticle.GetArticlesQuery()).GetAwaiter().GetResult();
            var articlesResponse = response;

            foreach (var articleResponse in articlesResponse)
            {
                var article = _mapper.Map<Article>(articleResponse);
                if (article.RowActive && article.ExpirationDate < DateTime.Now)
                {
                    var rowUpdate = new RowUpdate(article);
                    rowUpdate.RowSave(1, RowUpdate.UpdateType.Delete);

                    _mediator.Send(new UpdateArticle.UpdateArticleCommand(article)).GetAwaiter().GetResult();                                        
                }
            }

            return Ok(articlesResponse);
        }

        /// <summary>
        /// Obtiene un artículo mediante su Identificador, usando el patrón CQRS(ByIdQuery).
        /// </summary>
        /// <param name="id">Identificador del artículo</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetArticleById(int id)
        {            
            var response = _mediator.Send(new GetArticle.ByIdQuery(id)).GetAwaiter().GetResult();
            var article = response;

            return Ok(article);
        }

        /// <summary>
        /// Añade un artículo, usando el patrón CQRS(CreateArticleCommand).
        /// </summary>
        /// <param name="article"></param>
        /// <returns>Devuelve el artículo creado</returns>
        [HttpPost("add-article")]        
        public IActionResult AddArticle([FromBody] Article article)
        {            
            var rowUpdate = new RowUpdate(article);
            rowUpdate.RowSave(1, RowUpdate.UpdateType.Insert);

            var response = _mediator.Send(new CreateArticle.CreateArticleCommand(article)).GetAwaiter().GetResult();
            var articleInserted = response;

            if (article.RowActive && article.ExpirationDate < DateTime.Now)
            {                
                rowUpdate.RowSave(1, RowUpdate.UpdateType.Delete);
                _mediator.Send(new UpdateArticle.UpdateArticleCommand(article)).GetAwaiter().GetResult();                
            }

            return Ok(articleInserted);            
        }

        /// <summary>
        /// Actualiza un artículo, usando el patrón CQRS(UpdateArticleCommand).
        /// </summary>
        /// <param name="article"></param>
        /// <returns>Devuelve el artículo actualizado</returns>
        [HttpPut("edit-article")]
        public IActionResult EditArticle([FromBody] Article article)
        {
            var articleToUpdate = _unitOfWork.ArticleRepository.GetArticleByIdAsync(article.Id).Result;

            if (!String.IsNullOrEmpty(article.Name))
                articleToUpdate.Name = article.Name;
            if (!String.IsNullOrEmpty(article.Description))
                articleToUpdate.Description = article.Description;
            if (!String.IsNullOrEmpty(article.Type))
                articleToUpdate.Type = article.Type;
            if (!String.IsNullOrEmpty(article.Brand))
                articleToUpdate.Brand = article.Brand;
            if (article.Price != null && article.Price > 0)
                articleToUpdate.Price = article.Price;
            if (article.Stock != null && article.Stock > 0)
                articleToUpdate.Stock = article.Stock;
            
            var rowUpdate = new RowUpdate(articleToUpdate);
            rowUpdate.RowSave(1, RowUpdate.UpdateType.Update);

            var response = _mediator.Send(new UpdateArticle.UpdateArticleCommand(articleToUpdate)).GetAwaiter().GetResult();
            
            var articleUpdated = response;
            
            return Ok(articleUpdated);            
        }

        /// <summary>
        /// Elmina un artículo por el nombre enviado por parámetro, usando el patrón CQRS(DeleteArticleCommand).
        /// </summary>
        /// <param name="name">Nombre del artículo</param>
        /// <returns>Devuelve el artículo eliminado</returns>
        [HttpDelete("remove-article-name/{name}")]
        public IActionResult DeleteArticleByName(string name)
        {            
            var response = _mediator.Send(new DeleteArticle.DeleteArticleCommand(name)).GetAwaiter().GetResult();
            var articleDeleted = response;            

            return Ok(articleDeleted);
        }        

        /// <summary>
        /// Hace un borrado lígico de la entidad Article
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete-article/{id}")]
        public async Task<ActionResult> DeleteArticle(int id)
        {
            var article = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(id);

            var rowUpdate = new RowUpdate(article);
            rowUpdate.RowSave(1, RowUpdate.UpdateType.Delete);

            _unitOfWork.ArticleRepository.Update(article);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem deleting the article");
        }        

        /// <summary>
        /// Elimina la entidad Article de la bbddd
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        [HttpDelete("remove-article/{id}")]
        public async Task<ActionResult> RemoveArticle(int id)
        {
            var article = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(id);
            _unitOfWork.ArticleRepository.RemoveArticle(article);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem removing the article");
        }        

        /// <summary>
        /// Filtrado de artículos.
        /// </summary>
        /// <param name="articleParams">Parámetros de búsqueda para la entidad Article</param>
        /// <returns>Devuelve el listado de artículos encontrados paginados.</returns>
        [HttpGet("articles-by-params")]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticleList([FromQuery] ArticleParams articleParams)
        {
            var articles = await _unitOfWork.ArticleRepository.FilterArticlesExpiredAsync(articleParams);

            Response.AddPaginationHeader(articles.CurrentPage, articles.PageSize,
                articles.TotalCount, articles.TotalPages);

            return Ok(articles);
        }

        /// <summary>
        /// Filtrado de artículos. 
        /// </summary>
        /// <param name="articleParams">Parámetros de búsqueda para la entidad Article</param>
        /// <returns>Devuelve el listado de artículos encontrados paginados.</returns>
        [HttpGet("filter-articles-by-text/{textFilter}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticleList(string textFilter)
        {
            var articleParams = new ArticleParams();
            articleParams.Name = textFilter;
            articleParams.Description = textFilter;
            articleParams.Type = textFilter;
            articleParams.Brand = textFilter;
            

            var articles = await _unitOfWork.ArticleRepository.FilterArticlesAsync(articleParams);

            Response.AddPaginationHeader(articles.CurrentPage, articles.PageSize,
                articles.TotalCount, articles.TotalPages);

            return Ok(articles);
        }
    }
}
