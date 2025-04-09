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
}
