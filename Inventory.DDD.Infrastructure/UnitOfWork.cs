using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.DDD.Domain;
using Inventory.DDD.Infrastructure.Repositories;

namespace Inventory.DDD.Infrastructure
{
    /// <summary>
    /// Implementación del patron Unit of work.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Repositories
        public IArticleRepository ArticleRepository => new ArticleRepository(_context, _mapper);


        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            _context.ChangeTracker.DetectChanges();
            var changes = _context.ChangeTracker.HasChanges();

            return changes;
        }
    }
}
