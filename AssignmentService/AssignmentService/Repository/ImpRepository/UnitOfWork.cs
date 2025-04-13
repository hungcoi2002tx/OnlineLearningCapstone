using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace AssignmentService.Repository.ImpRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        protected readonly AssignmentDbContext _context;
        public IAssignmentRepository Assigment { get; }

        public UnitOfWork(
            AssignmentDbContext context)
        {
            _context = context;
        }

        public IDbContextTransaction BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();

            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollBackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }
    }
}
