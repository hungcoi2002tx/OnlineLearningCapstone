using Microsoft.EntityFrameworkCore;
using Share.Model;
using Share.Other;
using Share.Other.SearchModel;

namespace AssignmentService.Repository.ImpRepository
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(AssignmentDbContext context) : base(context)
        {
        }

        public async Task<bool> CreateAsync(Question model)
        {
			try
			{
                OpenTransaction();
                await _context.Questions.AddAsync(model);
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
