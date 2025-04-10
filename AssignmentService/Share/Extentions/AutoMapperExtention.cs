using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Share.Extentions
{
    public static class AutoMapperExtention
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            #region Add automapper
            var assembly = Assembly.GetAssembly(typeof(AutoMapperExtention));
            services.AddAutoMapper(assembly);
            #endregion
            services.AddAutoMapper(typeof(MapperConfig));
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
                if(dtoType != null)
                {
                    CreateMap(type, dtoType).ReverseMap();
                }
            }
        }
    }
}
