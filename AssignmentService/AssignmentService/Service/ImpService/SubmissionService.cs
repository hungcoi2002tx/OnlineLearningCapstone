using System.Diagnostics;
using System.IO.Compression;
using AssignmentService.Repository;
using AssignmentService.Repository.ImpRepository;
using AutoMapper;
using Azure.Core;
using Newtonsoft.Json;
using Share.Extentions;
using Share.Model;
using Share.Other;
using Share.Other.ExceptionModel;
using Share.Other.SearchModel;
using Share.RequestModel;
using Share.ResponseModel;

namespace AssignmentService.Service.ImpService
{
    public class SubmissionService : ISubmissionService
    {
        readonly IAssignmentRepository _assignmentRepository;
        readonly IQuestionRepository _questionRepository;
        readonly ISubmissionRepository _submissionRepository;
        readonly string _attachmentPath;
        readonly IMapper _mapper;

        public SubmissionService(IConfiguration configuration, IAssignmentRepository assignmentRepository, IQuestionRepository questionRepository, ISubmissionRepository submissionRepository, IMapper mapper)
        {
            _attachmentPath = configuration["UrlFolder:Attachment"];
            _assignmentRepository = assignmentRepository;
            _questionRepository = questionRepository;
            _submissionRepository = submissionRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult> CreateExamSubmissionAsync(CreateExamSubmissionRequest model)
        {
            try
            {
                #region Valid
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

                var assigment = await _assignmentRepository.GetAllByFilterAsync(new AssignmentSearch()
                {
                    IsAll = true,
                    Id = model.AssignmentId,
                    Type = AssignmentType.Essay.ToString()
                });

                if (assigment.Item2 == 0)
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Lỗi request",
                        DevMsg = "Id assignment not exist"
                    };
                }
                #endregion
                AssignmentSubmission assignmentSubmission = new AssignmentSubmission()
                {
                    AssignmentId = model.AssignmentId,
                    StudentId = model.StudentId,
                    SubmissionId = Guid.NewGuid().ToString(),
                    SubmittedAt = model.SubmitDate,
                    Content = model.Content,
                    Status = SubmissionStatus.Submitted.ToString(),
                };
                if (model.Attachments != null && model.Attachments.Length > 0)
                {
                    #region Save file and Create Attachment
                    SubmissionAttachment submissionAttachment = new SubmissionAttachment()
                    {
                        AttachmentId = Guid.NewGuid().ToString(),
                        UploadedAt = DateTime.Now
                    };

                    var fileName = $"{Guid.NewGuid()}_{model.Attachments.FileName}";
                    var filePath = Path.Combine(_attachmentPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Attachments.CopyToAsync(stream);
                    }
                    submissionAttachment.FileUrl = fileName;
                    submissionAttachment.FileType = AssignmentType.Essay.ToString();
                    submissionAttachment.SubmissionId = assignmentSubmission.SubmissionId;
                    #endregion
                    assignmentSubmission.Attachments = new List<SubmissionAttachment>()
                    {
                        submissionAttachment
                    };
                }
                var result = await _submissionRepository.CreateAsync(assignmentSubmission);

                if (!result)
                {
                    throw new Exception();
                }
                return new ServiceResult
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Success = true
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

        public async Task<ServiceResult> CreateQuizSubmissionAsync(CreateQuizSubmissionRequest model)
        {
            try
            {
                #region Valid
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

                var assigment = await _assignmentRepository.GetAllByFilterAsync(new AssignmentSearch()
                {
                    IsAll = true,
                    Id = model.AssignmentId,
                    Type = AssignmentType.Quiz.ToString()
                });

                if (assigment.Item2 == 0)
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Lỗi request",
                        DevMsg = "Id assignment not exist"
                    };
                }
                var quizAnswerJson = JsonConvert.SerializeObject(model.QuizAnswers);
                #endregion
                var submission = new AssignmentSubmission()
                {
                    SubmissionId = Guid.NewGuid().ToString(),
                    AssignmentId = model.AssignmentId,
                    StudentId = model.StudentId,
                    Status = SubmissionStatus.Graded.ToString(),
                    QuizAnswer = quizAnswerJson
                };
                int correctCount = 0;
                if (model.QuizAnswers != null)
                {
                    var ass = assigment.Item1.First();
                    foreach (var question in ass.Questions)
                    {
                        var questionStudent = model.QuizAnswers.FirstOrDefault(x => x.QuestionId == question.Id);
                        if (questionStudent != null)
                        {
                            var answer = (question?.Answers).FirstOrDefault(x => x.IsCorrect);
                            if (answer.Id == questionStudent.AnswerId)
                            {
                                correctCount++;
                            }
                        }
                    }
                    var totalQuestions = ass.Questions.Count();
                    submission.Grade = totalQuestions == 0 ? 0 : (float)correctCount / totalQuestions * 10f;
                }
                var result = await _submissionRepository.CreateAsync(submission);

                if (!result)
                {
                    throw new Exception();
                }
                return new ServiceResult
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Success = true
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

        public async Task<ServiceResult> GetAllBySearch(SubmissionSearch search)
        {
            try
            {
                var isValid = search.IsValid(out var validationErrors);
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
                var result = await _submissionRepository.GetAllByFilterAsync(search);
                PagedResult<AssignmentSubmissionResponse> pagedResult = new PagedResult<AssignmentSubmissionResponse>()
                {
                    Data = _mapper.Map<List<AssignmentSubmissionResponse>>(result.Item1),
                    Total = result.Item2,
                    Index = search.IsAll != true ? search.Page.Index : 0,
                    Size = search.IsAll != true ? search.Page.Size : 0
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

        public async Task<byte[]> GetFileAsync(SubmissionSearch search)
        {
            try
            {
                var isValid = search.IsValid(out var validationErrors);
                if (!isValid)
                {
                    throw new BadRequestException("Search model is incorrect", 400);
                }
                var result = await _submissionRepository.GetAllByFilterAsync(search);
                var data = result.Item1.First();
                if (data == null)
                {
                    throw new BadRequestException("Not found by search", 400);
                }
                using var memoryStream = new MemoryStream();

                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var item in data.Attachments)
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

        public async Task<ServiceResult> GradeAsync(GradeRequestModel model)
        {
            try
            {
                var data = await _submissionRepository.GetAllByFilterAsync(new SubmissionSearch()
                {
                    IsAll = true,
                    Id = model.SubmissionId
                });
                if(data.Item2 == 0)
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Error",
                        DevMsg = "Id not found"
                    };
                }
                var dataSubmission = data.Item1.First();
                if(model.Feedback != null)
                {
                    dataSubmission.Feedback = model.Feedback;
                }
                if (model.Grade != null)
                {
                    dataSubmission.Grade = model.Grade;
                }
                var result = await _submissionRepository.UpdateAsync(dataSubmission);
                if (!result)
                {
                    throw new Exception();
                }
                return new ServiceResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true
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
