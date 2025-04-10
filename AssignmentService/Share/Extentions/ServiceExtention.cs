using Microsoft.Extensions.DependencyInjection;
using Share.Model;
using Share.RequestModel;
using System.Net.Sockets;
using System.Reflection;

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

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(ServiceExtention));
            var classes = assembly.ExportedTypes
               .Where(a => !a.Name.StartsWith("I") && a.Name.EndsWith("Repository") && (a.Name != "Repository"));
            foreach (Type implement in classes)
            {
                foreach (var @interface in implement.GetInterfaces())
                {
                    services.AddScoped(@interface, implement);
                }
            }

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(ServiceExtention));
            var classes = assembly.ExportedTypes.Where(a => !a.Name.StartsWith("I") && a.Name.EndsWith("Service"));

            foreach (Type implement in classes)
            {
                foreach (var @interface in implement.GetInterfaces())
                {
                    services.AddScoped(@interface, implement);
                }
            }

            return services;
        }
    }



    public class MapperConfig : AutoMapper.Profile
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

            CreateMap<Assignment, ExamRequestModel>().ReverseMap();
        }
    }
}
