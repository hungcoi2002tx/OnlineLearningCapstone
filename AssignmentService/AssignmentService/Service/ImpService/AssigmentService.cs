using AssignmentService.Repository;
using Share.DTO;
using Share.Other;
using Share.RequestModel;
using Share.Extentions;
using Share.Model;
using AutoMapper;
using Microsoft.AspNetCore.OutputCaching;

namespace AssignmentService.Service.ImpService
{
    public class AssigmentService : IAssigmentService
    {
        readonly IAssignmentRepository _assignmentRepository;
        readonly string _attachmentPath;
        readonly IMapper _mapper;

        public AssigmentService(
            IConfiguration configuration,
            IMapper mapper,
            IAssignmentRepository assignmentRepository)
        {
            _attachmentPath = configuration["UrlFolder:Attachment"];
            _mapper = mapper;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<ServiceResult> CreateExamAsync(ExamRequestModel exam)
        {
            try
            {
                #region Valid
                var validationErrors = exam.Validate();

                if (validationErrors.Count > 0)
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = validationErrors
                    };
                }
                #endregion

                #region Save file and Create Attachment
                AssignmentAttachment attachment = new AssignmentAttachment()
                {
                    AttachmentId = Guid.NewGuid().ToString(),
                    UploadedAt = DateTime.Now
                };
                if (exam.Attachments != null && exam.Attachments.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{exam.Attachments.FileName}";
                    var filePath = Path.Combine(_attachmentPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await exam.Attachments.CopyToAsync(stream);
                    }
                    attachment.FileUrl = fileName;
                    attachment.FileType = AssignmentType.Essay.ToString();
                }
                #endregion

                #region Create Assignment   
                Assignment assignment = _mapper.Map<Assignment>(exam);
                assignment.AssignmentId = Guid.NewGuid().ToString();
                assignment.CreatedAt = DateTime.Now;
                assignment.UpdatedAt = DateTime.Now;
                assignment.Attachments = new List<AssignmentAttachment>() { attachment };
                assignment.Type = AssignmentType.Essay.ToString();
                #endregion

                #region Save Attachment
                var result = await _assignmentRepository.CreateExamAsync(assignment);
                #endregion

                if (!result)
                {
                    throw new Exception();
                }
                return new ServiceResult
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Data = "Lỗi server"
                };
            }
        }
    }
}
