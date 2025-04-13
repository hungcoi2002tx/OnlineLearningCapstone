using Microsoft.EntityFrameworkCore;
using Share.Model;
using Share.Other.SearchModel;

namespace AssignmentService.Repository.ImpRepository
{
    public class AttachmentRepository : Repository<AssignmentAttachment>, IAttachmentRepository
    {
        public AttachmentRepository(AssignmentDbContext context) : base(context)
        {
        }

        public async Task<List<AssignmentAttachment>> GetAllByFilterAsync(AttachmentSearch search)
        {
            try
            {
                OpenTransaction();
                var query = _context.AssignmentAttachments.AsQueryable();
                if (!String.IsNullOrEmpty(search.Id))
                {
                    query = query.Where(a => a.AttachmentId == search.Id);
                }
                if (!String.IsNullOrEmpty(search.AssignmentId))
                {
                    query = query.Where(a => a.AssignmentId == search.AssignmentId);
                }
                if (!String.IsNullOrEmpty(search.FileType))
                {
                    query = query.Where(a => a.FileType == search.FileType);
                }
                if (!String.IsNullOrEmpty(search.FileUrl))
                {
                    query = query.Where(a => a.FileUrl == search.FileUrl);
                }

                if (search.Ids?.Any() == true)
                {
                    query = query.Where(x => search.Ids.Contains(x.AttachmentId));
                }
                var result = await query.ToListAsync();
                await CommitTransactionAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(AssignmentAttachment data)
        {
            try
            {
                OpenTransaction();
                _context.Attach(data).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                await CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
