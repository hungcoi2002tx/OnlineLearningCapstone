using System.IO.Compression;
using AssignmentService.Repository;
using Share.Other;
using Share.Other.SearchModel;
using Share.ResponseModel;

namespace AssignmentService.Service.ImpService
{
    public class AttachmentService : IAttachmentService
    {
        readonly IAttachmentRepository _attachmentRepository;
        readonly string _attachmentPath;

        public AttachmentService(IAttachmentRepository attachmentRepository,
            IConfiguration configuration)
        {
            _attachmentRepository = attachmentRepository;
            _attachmentPath = configuration["UrlFolder:Attachment"];
        }

        public async Task<ServiceResult> GetAttachmentAsync(AttachmentSearch search)
        {
            try
            {
                var result = await _attachmentRepository.GetAllByFilterAsync(search);
                return new ServiceResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Data = "Lỗi server",
                    DevMsg = ex.Message
                };
            }
        }

        public async Task<byte[]> GetFileAsync(AttachmentSearch search)
        {
            try
            {
                var result = await _attachmentRepository.GetAllByFilterAsync(search);
                using var memoryStream = new MemoryStream();

                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var item in result)
                    {
                        var filePath = Path.Combine(_attachmentPath, item.FileUrl);
                        if (!File.Exists(filePath)) continue;

                        var entry = zip.CreateEntry(item.FileUrl, CompressionLevel.Fastest);
                        using var entryStream = entry.Open();
                        using var fileStream = File.OpenRead(filePath);
                        await fileStream.CopyToAsync(entryStream);
                    }
                }

                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<ServiceResult> DeleteAsync(string id)
        {
            try
            {
                var data = await _attachmentRepository.GetAllByFilterAsync(new AttachmentSearch()
                {
                    Id = id
                });

                if (data?.Any() != true)
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Lỗi server",
                        DevMsg = "Id not exist"
                    };
                }

                var result = await _attachmentRepository.DeleteAsync(data.First());
                return new ServiceResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
