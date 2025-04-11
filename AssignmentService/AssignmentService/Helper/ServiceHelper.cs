using Share.Extentions;
using System.Reflection;

namespace AssignmentService.Helper
{
    public static class ServiceHelper
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(ServiceHelper));
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

        public static IServiceCollection AddService(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(ServiceHelper));
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
}
