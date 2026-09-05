using GraduatesClub.Application.Interfaces;
using GraduatesClub.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GraduatesClub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAlumniService, AlumniService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IEventService, EventService>();

        return services;
    }
}
