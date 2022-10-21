using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Inventory.DDD.Domain.DTOs;
using Inventory.DDD.Domain.Helpers;

namespace Inventory.DDD.Domain
{
    /// <summary>
    /// Interface para el repositorio de la tabla de Article.
    /// </summary>
    public interface IArticleRepository
    {
        /// <summary>
        /// Obtiene un listado de artículos
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Article>> GetArticlesAsync();
        Task<Article> GetArticleByIdAsync(int id);
        /// <summary>
        /// Obtiene un artículo por su nombre
        /// </summary>
        /// <param name="name">Nombre del artículo</param>
        /// <returns></returns>
        Task<Article> GetArticleByNameAsync(string name);    
        
        /// <summary>
        /// Filtra articulos según parámetros 
        /// </summary>
        /// <param name="articleParams">Parámetros(en este caso lo hice para textos)</param>
        /// <returns></returns>
        Task<PagedList<Article>> FilterArticlesAsync(ArticleParams articleParams);

        /// <summary>
        /// Filtra listado de los artículos expirados.
        /// </summary>
        /// <param name="articleParams"></param>
        /// <returns></returns>
        Task<PagedList<ArticleDTO>> FilterArticlesExpiredAsync(ArticleParams articleParams);        

        /// <summary>
        /// Agrega un nuevo registro de Article.
        /// </summary>
        /// <param name="article">Entidad o registro article a añadir</param>
        void AddArticle(Article article);

        /// <summary>
        /// Actualiza un registro de la base de datos.
        /// </summary>
        /// <param name="article">article a actualizar</param>
        /// <returns></returns>
        void Update(Article article);

        /// <summary>
        /// Establece el borrado lógico de un registro de tipo Article.
        /// </summary>
        /// <param name="id">Identificador del registro article a borrar</param>
        /// <returns>Identificador del registro article a borrar</returns>        
        void DeleteArticle(int id);

        /// <summary>
        /// Establece el borrado de un registro de tipo Article.
        /// </summary>
        /// <param name="article">Artículo a borrar</param>
        /// <returns></returns>        
        void RemoveArticle(Article article);
    }
}
