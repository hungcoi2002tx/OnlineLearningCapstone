using Share.Model;

namespace AssignmentService.Repository.ImpRepository
{
    public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(AssigmentDbContext context) : base(context)
        {

        }

        public async Task<bool> CreateExamAsync(Assignment assignment)
        {
            try
            {
                OpenTransaction();
                await _context.Assignments.AddAsync(assignment);
                await _context.SaveChangesAsync();
                await CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await RollBackTransactionAsync();
                return false;
            }
        }
        public async Task<bool> CreateQuizAsync()
        {
            try
            {
                await CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await RollBackTransactionAsync();
                return false;
            }
        }
    }
}
