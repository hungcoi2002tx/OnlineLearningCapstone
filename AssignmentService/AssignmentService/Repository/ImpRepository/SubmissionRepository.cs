using Share.Model;

namespace AssignmentService.Repository.ImpRepository
{
    public class SubmissionRepository : Repository<AssignmentSubmission>, ISubmissionRepository
    {
        public SubmissionRepository(AssignmentDbContext context) : base(context)
        {
        }


        public async Task<bool> CreateAsync(AssignmentSubmission model)
        {
            try
            {
                OpenTransaction();
                await _context.AssignmentSubmissions.AddAsync(model);
                await _context.SaveChangesAsync();
                await CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
