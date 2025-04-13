using Share.Other;
using Share.Other.SearchModel;
using Share.RequestModel;

namespace AssignmentService.Service
{
    public interface IAttachmentService
    {
        Task<ServiceResult> GetAttachmentAsync(AttachmentSearch search);
        Task<byte[]> GetFileAsync(AttachmentSearch search);
        Task<ServiceResult> DeleteAsync(string id);
        Task<ServiceResult> UpdateAsync(string id, UpdateAttachmentRequestModel updateModel);
    }
}