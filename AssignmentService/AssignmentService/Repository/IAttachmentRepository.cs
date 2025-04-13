using System.Net;
using Share.Model;
using Share.Other.SearchModel;

namespace AssignmentService.Repository
{
    public interface IAttachmentRepository : IRepository<AssignmentAttachment>
    {
        Task<List<AssignmentAttachment>> GetAllByFilterAsync(AttachmentSearch search);
    }
}
