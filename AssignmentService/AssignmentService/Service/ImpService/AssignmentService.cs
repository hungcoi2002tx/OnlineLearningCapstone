using AssignmentService.Repository;
using Share.DTO;
using Share.Other;
using Share.RequestModel;
using Share.Extentions;
using Share.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OutputCaching;
using Share.Other.SearchModel;
using Share.ResponseModel;

namespace AssignmentService.Service.ImpService
{
    public class AssignmentService : IAssignmentService
    {
        readonly IAssignmentRepository _assignmentRepository;
        readonly string _attachmentPath;
        readonly IMapper _mapper;

        public AssignmentService(
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
                        Data = validationErrors,
                        DevMsg = "Validation"
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
                Assignment assignment = new Assignment()
                {
                    Title = exam.Title,
                    Description = exam.Description,
                    Deadline = exam.Deadline,
                    TeacherId = exam.TeacherId,
                    ClassroomId = exam.ClassroomId,
                    Status = exam.Status,
                };
                assignment.AssignmentId = Guid.NewGuid().ToString();
                assignment.CreatedAt = DateTime.Now;
                assignment.UpdatedAt = DateTime.Now;
                assignment.Attachments = new List<AssignmentAttachment>() { attachment };
                assignment.Type = AssignmentType.Essay.ToString();
                #endregion

                #region Save Attachment
                var result = await _assignmentRepository.CreateAssignmentAsync(assignment);
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
                    Data = "Lỗi server",
                    DevMsg = ex.Message
                };
            }
        }

        public async Task<ServiceResult> CreateQuizAsync(QuizRequestDto quizDto)
        {
            try
            {
                var isValid = quizDto.IsValid(out var validationErrors);
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
                var assignment = _mapper.Map<Assignment>(quizDto);
                assignment.AssignmentId = Guid.NewGuid().ToString();
                foreach (var assignmentQuestion in assignment.Questions)
                {
                    assignmentQuestion.AssignmentId = assignment.AssignmentId;
                    assignmentQuestion.Id = Guid.NewGuid().ToString();
                    foreach (var answer in assignmentQuestion.Answers)
                    {
                        answer.Id = Guid.NewGuid().ToString();
                    }
                }
                var result = await _assignmentRepository.CreateAssignmentAsync(assignment);
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
                    Data = "Lỗi server",
                    DevMsg = ex.Message
                };
            }
        }

        public async Task<ServiceResult> DeleteAsync(string id, string teacherId)
        {
            try
            {
                if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(teacherId))
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Id or teacherId is null or empty",
                        DevMsg = "Validate"
                    };
                }
                var data = await _assignmentRepository.GetAllByFilterAsync(new AssignmentSearch()
                {
                    All = true,
                    Id = id
                });
                if (data.Item2 == 0)
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Lỗi server",
                        DevMsg = "Id not exist"
                    };
                }

                if (data.Item1.First().TeacherId != teacherId)
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Lỗi server",
                        DevMsg = "Not authorization"
                    };
                }

                var result = await _assignmentRepository.DeleteAsync(data.Item1.First());
                if (!result)
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

        public async Task<ServiceResult> GetAllByFilterAsync(AssignmentSearch model)
        {
            try
            {
                var isValid = model.IsValid(out var validationErrors);
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
                var result = await _assignmentRepository.GetAllByFilterAsync(model);
                PagedResult<AssignmentResponse> pagedResult = new PagedResult<AssignmentResponse>()
                {
                    Data = _mapper.Map<List<AssignmentResponse>>(result.Item1),
                    Total = result.Item2,
                    Index = model.All != true ? model.Page.Index : 0,
                    Size = model.All != true ? model.Page.Size : 0
                };
                return new ServiceResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = pagedResult
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

        public async Task<ServiceResult> UpdateAsync(string id,string teacherId, UpdateAssignmentRequestModel updateModel)
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
                if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(teacherId))
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Id or teacherId is null or empty",
                        DevMsg = "Validate"
                    };
                }
                var data = await _assignmentRepository.GetAllByFilterAsync(new AssignmentSearch()
                {
                    All = true,
                    Id = id
                });
                if (data.Item2 == 0)
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Lỗi server",
                        DevMsg = "Id not exist"
                    };
                }

                if (data.Item1.First().TeacherId != teacherId)
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Lỗi server",
                        DevMsg = "Not authorization"
                    };
                }
                var assignment = data.Item1.First();
                _mapper.Map(updateModel, assignment);   
                assignment.UpdatedAt = DateTime.Now;
                var result =await _assignmentRepository.UpdateAssignment(assignment);
                if (!result)
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

        public async Task<ServiceResult> UpdateAsync(string id, string teacherId, UpdateExamRequestModel updateModel)
        {
            throw new NotImplementedException();
        }
    }
}
