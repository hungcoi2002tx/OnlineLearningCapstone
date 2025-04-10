using Microsoft.EntityFrameworkCore.Storage;

namespace AssignmentService.Repository
{
    public interface IUnitOfWork
    {
        IAssignmentRepository Assigment { get; }

        IDbContextTransaction BeginTransaction();

        Task CommitTransactionAsync();

        Task RollBackTransactionAsync();
    }

}
