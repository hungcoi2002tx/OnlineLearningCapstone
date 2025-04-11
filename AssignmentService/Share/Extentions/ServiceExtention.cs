using Microsoft.Extensions.DependencyInjection;
using Share.Model;
using Share.RequestModel;
using System.Net.Sockets;
using System.Reflection;
using AutoMapper;

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

            foreach (var type in models)
            {
                CreateMap(type, type).ReverseMap();

                var dtoType = dtos.FirstOrDefault(x => x.Name == type.Name + "Dto");
                if (dtoType != null)
                {
                    CreateMap(type, dtoType).ReverseMap();
                }
            }

            CreateMap<Assignment, ExamRequestModel>()
                .ForMember(x => x)
                .ReverseMap();
        }
    }
}
