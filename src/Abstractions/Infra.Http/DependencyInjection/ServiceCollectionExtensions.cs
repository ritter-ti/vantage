using System;
using System.Linq;
using System.Reflection;
using Goal.Infra.Crosscutting.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace Goal.Infra.Http.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTypeAdapterFactory(this IServiceCollection services, ITypeAdapterFactory typeAdapterFactory)
    {
        ArgumentNullException.ThrowIfNull(typeAdapterFactory);

        services.AddSingleton(typeof(ITypeAdapterFactory), typeAdapterFactory);
        services.AddSingleton(factory => factory.GetService<ITypeAdapterFactory>()!.Create());

        return services;
    }

    public static IServiceCollection AddTypeAdapterFactory<TTypeAdapterFactory>(this IServiceCollection services)
        where TTypeAdapterFactory : class, ITypeAdapterFactory
    {
        services.AddSingleton<ITypeAdapterFactory, TTypeAdapterFactory>();
        services.AddSingleton(factory => factory.GetService<ITypeAdapterFactory>()!.Create());

        return services;
    }

    public static void RegisterAllTypesOf<TService>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TService : class
    {
        Type serviceType = typeof(TService);
        services.RegisterAllTypes(serviceType, serviceType.Assembly, lifetime);
    }

    public static void RegisterAllTypesOf<TService>(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(assembly);
        services.RegisterAllTypes(typeof(TService), assembly, lifetime);
    }

    public static void RegisterAllTypes(this IServiceCollection services, Type serviceType, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(serviceType);
        services.RegisterAllTypes(serviceType, serviceType.Assembly, lifetime);
    }

    public static void RegisterAllTypes(this IServiceCollection services, Type serviceType, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(serviceType);
        ArgumentNullException.ThrowIfNull(assembly);

        var types = assembly.GetTypes()
           .Where(
               type => type.IsClass
               && !type.IsAbstract
               && serviceType.IsAssignableFrom(type))
           .Select(
               type => new
               {
                   Service = type.GetInterfaces().Last(),
                   Implementation = type
               })
           .ToList();

        types.ForEach(registration =>
        {
            services.Add(new ServiceDescriptor(
                registration.Service,
                registration.Implementation,
                lifetime));
        });
    }
}
