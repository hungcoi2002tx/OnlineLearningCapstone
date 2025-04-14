using System.Diagnostics;
using AssignmentService.Repository;
using Newtonsoft.Json;
using Share.Extentions;
using Share.Model;
using Share.Other;
using Share.Other.SearchModel;
using Share.RequestModel;

namespace AssignmentService.Service.ImpService
{
    public class SubmissionService : ISubmissionService
    {
        readonly IAssignmentRepository _assignmentRepository;
        readonly IQuestionRepository _questionRepository;
        readonly ISubmissionRepository _submissionRepository;

        public SubmissionService(IAssignmentRepository assignmentRepository, IQuestionRepository questionRepository, ISubmissionRepository submissionRepository)
        {
            _assignmentRepository = assignmentRepository;
            _questionRepository = questionRepository;
            _submissionRepository = submissionRepository;
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
                    All = true,
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
    }
}
