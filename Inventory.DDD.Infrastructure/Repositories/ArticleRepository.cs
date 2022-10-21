using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inventory.DDD.Domain;
using Inventory.DDD.Domain.DTOs;
using Inventory.DDD.Domain.Events;
using Inventory.DDD.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Inventory.DDD.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;     
        /// <summary>
        /// Inicializa una nueva instancia de la clase ArticleRepository.
        /// </summary>
        /// <param name="context">Contexto</param>
        /// <param name="mapper">Interface del Automapper para el mapeo de entidades</param>
        public ArticleRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;            
        }

        /// <summary>
        /// Obtiene el listado de artículos.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        /// <summary>
        /// Obtiene el artículo por Id.
        /// </summary>
        /// <param name="id">Identificador del artículo</param>
        /// <returns></returns>
        public async Task<Article> GetArticleByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        /// <summary>
        /// Obtiene el artículo por name.
        /// </summary>
        /// <param name="name">Nombre del artículo</param>
        /// <returns></returns>
        public async Task<Article> GetArticleByNameAsync(string name)
        {
            return await _context.Articles.FirstOrDefaultAsync(a => a.Name == name);            
        }

        /// <summary>
        /// Filtro de artículos por parámetros
        /// </summary>
        /// <param name="articleParams">Parámetros de filtrado</param>
        /// <returns>Listado de artículos paginados (PagedList<ArticleDTO>)</returns>
        public async Task<PagedList<Article>> FilterArticlesAsync(ArticleParams articleParams)
        {
            var query = _context.Articles.AsQueryable();

            if (query != null && query.Count() > 0 && articleParams != null)
                query = query.Where(r => r.Name.Contains(articleParams.Name) 
                            || r.Description.Contains(articleParams.Description)
                            || r.Brand.Contains(articleParams.Brand )
                            || r.Type.Contains(articleParams.Type));                        

            query = query.OrderByDescending(r => r.Price);

            return await PagedList<Article>.CreateAsync(query.AsNoTracking(),
                    articleParams.PageNumber, articleParams.PageSize);
        }

        /// <summary>
        /// Filtro de artículos expirados
        /// </summary>
        /// <param name="articleParams">Parámetros de filtrado</param>
        /// <returns>Listado de artículos paginados (PagedList<ArticleDTO>)</returns>
        public async Task<PagedList<ArticleDTO>> FilterArticlesExpiredAsync(ArticleParams articleParams)
        {
            var query = _context.Articles.AsQueryable();
            
            query = query.Where(r => r.ExpirationDate > DateTime.Now);            
            query = query.OrderByDescending(r => r.Price);

            return await PagedList<ArticleDTO>.CreateAsync(query.ProjectTo<ArticleDTO>(_mapper
                .ConfigurationProvider).AsNoTracking(),
                    articleParams.PageNumber, articleParams.PageSize);
        }

        /// <summary>
        /// Adiciona un artículo a la bbdd.
        /// </summary>
        /// <param name="article">Artículo a añadir</param>
        public void AddArticle(Article article)
        {            
            _context.Articles.Add(article);            
        }

        /// <summary>
        /// Actualiza un artículo de la bbdd.
        /// </summary>
        /// <param name="article">Artículo a actualizar</param>
        public void Update(Article article)
        {            
            _context.Articles.Attach(article);
            _context.Entry(article).State = EntityState.Modified;            
        }

        /// <summary>
        /// Actualiza un artículo de la bbdd. (Borrado lógico)
        /// </summary>
        /// <param name="id">Identificador del artículo a actualizar</param>
        public void DeleteArticle(int id)
        {
            var article = _context.Articles.Find(id);
            
            _context.Articles.Attach(article);
            _context.Entry(article).State = EntityState.Modified;            
        }

        /// <summary>
        /// Establece el borrado de un registro de tipo Artículo
        /// </summary>
        /// <param name="article">Artículo a borrar</param>
        public void RemoveArticle(Article article)
        {
            _context.Articles.Remove(article);            
        }
    }
}
