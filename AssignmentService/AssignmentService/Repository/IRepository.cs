using Microsoft.EntityFrameworkCore.Storage;

namespace AssignmentService.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T obj, bool usingTransaction = true);
        Task<bool> DeleteAsync(T obj, bool usingTransaction = true);
        IDbContextTransaction OpenTransaction();
        Task CommitTransactionAsync();
        Task RollBackTransactionAsync();
        Task<bool> AddRangeAsync(List<T> obj, bool usingTransaction = true);
    }
}
