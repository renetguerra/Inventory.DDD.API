using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DDD.Domain
{
    /// <summary>
    /// Interface para implementar el patrón Unit of work.
    /// </summary>
    public interface IUnitOfWork
    {
        IArticleRepository ArticleRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
