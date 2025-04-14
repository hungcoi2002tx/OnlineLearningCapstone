using Microsoft.EntityFrameworkCore;
using Share.Model;
using Share.Other.SearchModel;

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

        public async Task<(List<AssignmentSubmission>,int)> GetAllByFilterAsync(SubmissionSearch search)
        {
            try
            {
                OpenTransaction();
                var query = _context.AssignmentSubmissions.AsQueryable();
                if (!String.IsNullOrEmpty(search.Id))
                {
                    query = query.Where(a => a.SubmissionId == search.Id);
                }
                if (!String.IsNullOrEmpty(search.AssignmentId))
                {
                    query = query.Where(a => a.AssignmentId == search.AssignmentId);
                }
                if (!String.IsNullOrEmpty(search.StudentId))
                {
                    query = query.Where(a => a.StudentId == search.StudentId);
                }
                if (search.IsQuiz == true)
                {
                    query = query.Where(a => a.QuizAnswer != null);
                }
                if (!String.IsNullOrEmpty(search.Status))
                {
                    query = query.Where(a => a.Status == search.Status);
                }

                if (search.Ids?.Any() == true)
                {
                    query = query.Where(x => search.Ids.Contains(x.SubmissionId));
                }
                var total = await query.CountAsync();
                if (search.IsAll != true)
                {
                    var pageNumber = search.Page.Index <= 0 ? 1 : search.Page.Index;
                    var pageSize = search.Page.Size <= 0 ? 10 : search.Page.Size;
                    query = query
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);
                }
                var result = await query.Include(x => x.Attachments).ToListAsync();
                await CommitTransactionAsync();
                return (result,total);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(AssignmentSubmission dataSubmission)
        {
            try
            {
                OpenTransaction();
                _context.Attach(dataSubmission).State = EntityState.Modified;
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
    }
}
