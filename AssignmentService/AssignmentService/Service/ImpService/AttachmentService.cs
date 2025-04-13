using System.IO.Compression;
using System.Net.Mail;
using AssignmentService.Repository;
using AutoMapper;
using Share.Other;
using Share.Other.SearchModel;
using Share.RequestModel;
using Share.ResponseModel;
using Share.Extentions;

namespace AssignmentService.Service.ImpService
{
    public class AttachmentService : IAttachmentService
    {
        readonly IAttachmentRepository _attachmentRepository;
        readonly string _attachmentPath;
        readonly IMapper _mapper;

        public AttachmentService(IAttachmentRepository attachmentRepository,
            IConfiguration configuration,
            IMapper mapper)
        {
            _attachmentRepository = attachmentRepository;
            _attachmentPath = configuration["UrlFolder:Attachment"];
            _mapper = mapper;
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
                return new ServiceResult
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Data = "Lỗi server",
                    DevMsg = ex.Message
                };
            }
        }

        public async Task<ServiceResult> UpdateAsync(string id, UpdateAttachmentRequestModel updateModel)
        {
            try
            {
                #region Valid
                var isValid = updateModel.IsValid(out var validationErrors);
                if (!isValid)
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = validationErrors,
                        DevMsg = "Validation"
                    };
                }
                #endregion
                var entity = await _attachmentRepository.GetAllByFilterAsync(new AttachmentSearch()
                {
                    Id = id
                });
                if (entity?.Any() != true)
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Lỗi server",
                        DevMsg = "Id not exist"
                    };
                }
                var data = entity.First();
                if (updateModel.Attachment == null)
                {
                    var result = await _attachmentRepository.DeleteAsync(data);
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Success = true,
                    };
                }
                #region save file
                var fileName = $"{Guid.NewGuid()}_{updateModel.Attachment.FileName}";
                var filePath = Path.Combine(_attachmentPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateModel.Attachment.CopyToAsync(stream);
                }
                data.FileUrl = fileName;
                data.FileType = AssignmentType.Essay.ToString();
                #endregion
                _mapper.Map(updateModel, data);
                var resultUpdate = await _attachmentRepository.UpdateAsync(data);
                if (!resultUpdate)
                {
                    throw new Exception();
                }
                return new ServiceResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true,
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
    }
}
