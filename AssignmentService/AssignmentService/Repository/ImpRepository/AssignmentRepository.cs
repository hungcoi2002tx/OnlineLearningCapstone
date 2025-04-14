using Microsoft.EntityFrameworkCore;
using Share.Model;
using Share.Other;
using Share.Other.SearchModel;

namespace AssignmentService.Repository.ImpRepository
{
    public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(AssignmentDbContext context) : base(context)
        {

        }

        public async Task<bool> CreateAssignmentAsync(Assignment assignment)
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

        public async Task<(List<Assignment>,int)> GetAllByFilterAsync(AssignmentSearch search)
        {
            try
            {
                OpenTransaction();
                var query = _context.Assignments.AsQueryable();

                #region GetFilter
                if (!string.IsNullOrWhiteSpace(search.Id))
                    query = query.Where(a => a.AssignmentId == search.Id);

                if (!string.IsNullOrWhiteSpace(search.ClassroomId))
                    query = query.Where(a => a.ClassroomId == search.ClassroomId);

                if (!string.IsNullOrWhiteSpace(search.TeacherId))
                    query = query.Where(a => a.TeacherId == search.TeacherId);
                if (search.StartDate.HasValue || search.EndDate.HasValue)
                {
                    if (search.StartDate.HasValue)
                        query = query.Where(a => a.Deadline >= search.StartDate.Value);

                    if (search.EndDate.HasValue)
                        query = query.Where(a => a.Deadline <= search.EndDate.Value);
                }
                if (!string.IsNullOrWhiteSpace(search.Status))
                {
                    if (Enum.TryParse<AssignmentStatus>(search.Status, true, out var statusEnum))
                        query = query.Where(a => a.Status == statusEnum.ToString());
                }
                if (!string.IsNullOrWhiteSpace(search.Type))
                {
                    if (Enum.TryParse<AssignmentType>(search.Type, true, out var statusEnum))
                        query = query.Where(a => a.Type == statusEnum.ToString());
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
                var data = await query.Include(x => x.Attachments)
                    .Include(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                    .ToListAsync();
                #endregion

                await CommitTransactionAsync();
                return (data, total);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await RollBackTransactionAsync();
                return (new List<Assignment>(),0);
            }
        }

        public async Task<bool> UpdateAssignment(Assignment assignment)
        {
            try
            {
                OpenTransaction();
                _context.Attach(assignment).State = EntityState.Modified;
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
