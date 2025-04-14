using Microsoft.Extensions.DependencyInjection;
using Share.Model;
using Share.RequestModel;
using System.Net.Sockets;
using System.Reflection;
using AutoMapper;
using Share.Other;
using Share.DTO;
using Share.ResponseModel;

namespace Share.Extentions
{
    public static class ServiceExtention
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            #region Add automapper
            var assembly = Assembly.GetAssembly(typeof(ServiceExtention));
            services.AddAutoMapper(assembly);
            #endregion
            services.AddAutoMapper(typeof(MapperConfig));
            return services;
        }
    }

    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            var assembly = Assembly.GetAssembly(typeof(MapperConfig));
            var models = assembly.ExportedTypes.Where(x => x.Namespace.Equals("Share.Model"));
            var dtos = assembly.ExportedTypes.Where(x => x.Namespace.Equals("Share.DTO"));
            var responses = assembly.ExportedTypes.Where(x => x.Namespace.Equals("Share.ResponseModel"));
            var requests = assembly.ExportedTypes.Where(x => x.Namespace.Equals("Share.RequestModel"));
            foreach (var type in models)
            {
                CreateMap(type, type).ReverseMap();

                var dtoType = dtos.FirstOrDefault(x => x.Name == type.Name + "Dto");
                if (dtoType != null)
                {
                    CreateMap(type, dtoType).ReverseMap();
                }
                var responseType = responses.FirstOrDefault(x => x.Name == type.Name + "Response");
                if (responseType != null)
                {
                    CreateMap(type, responseType).ReverseMap();
                }
                var updateResponse = requests.FirstOrDefault(x => x.Name == "Update" + type.Name + "RequestModel");
                if (updateResponse != null)
                {
                    CreateMap(type, updateResponse).ReverseMap();
                }
            }

            CreateMap<ExamRequestModel, Assignment>()
                .ForMember(x => x.AssignmentId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.Type, opt => opt.MapFrom(src => AssignmentType.Essay.ToString()))
                .ReverseMap();

            CreateMap<QuizRequestDto, Assignment>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(_ => AssignmentType.Quiz.ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<CreateQuestionRequestModel, Question>();
            CreateMap<CreateAnswerRequestModel, Answer>();

            CreateMap<UpdateAttachmentRequestModel, AssignmentAttachment>()
                .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => DateTime.Now));

        }
    }
}
