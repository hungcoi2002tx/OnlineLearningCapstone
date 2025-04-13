using Share.Other;
using Share.RequestModel;
using Share.Extentions;
using AssignmentService.Repository;
using Share.Other.SearchModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Share.Model;
using AutoMapper;

namespace AssignmentService.Service.ImpService
{
    public class QuestionService : IQuestionService
    {
        readonly IAssignmentRepository _assignmentRepository;
        readonly IQuestionRepository _questionRepository;
        readonly IMapper _mapper;

        public QuestionService(IAssignmentRepository assignmentRepository, IQuestionRepository questionRepository, IMapper mapper)
        {
            _assignmentRepository = assignmentRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult> CreateAsync(string assignmentId, CreateQuestionRequestModel question)
        {
			try
			{
                #region Valid
                var isValid = question.IsValid(out var validationErrors);
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
                    Id = assignmentId
                });
                if (assigment.Item2 == 0)
                {
                    return new ServiceResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Lỗi request",
                        DevMsg = "Id not exist"
                    };
                }
                #endregion
                var ques = _mapper.Map<Question>(question);
                ques.AssignmentId = assignmentId;
                var result = await _questionRepository.CreateAsync(ques);

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
    }
}
