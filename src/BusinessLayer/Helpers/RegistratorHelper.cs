using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Helpers
{
    public static class RegistratorHelper
    {
        public static IServiceCollection Scan(
            this IServiceCollection serviceCollection,
            Assembly assembly,
            Type serviceType,
            ServiceLifetime lifetime)
        {
            return Scan(serviceCollection, new Assembly[] {assembly}, serviceType, lifetime);
        }

        public static IServiceCollection Scan(
            this IServiceCollection serviceCollection,
            IEnumerable<Assembly> assemblies,
            Type interfaceType,
            ServiceLifetime lifetime)
        {
            foreach (var type in assemblies.SelectMany(x =>
                x.GetTypes().Where(t => t.IsClass && !t.IsAbstract)))
            {
                foreach (var i in type.GetInterfaces())
                {
                    // Check for generic
                    if (i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
                    {
                        var genericInterfaceType = interfaceType.MakeGenericType(i.GetGenericArguments());
                        serviceCollection.Add(new ServiceDescriptor(genericInterfaceType, type, lifetime));
                    }
                    // Check for non-generic
                    else if (!i.IsGenericType && i == interfaceType)
                    {
                        serviceCollection.Add(new ServiceDescriptor(interfaceType, type, lifetime));
                    }
                }
            }

            return serviceCollection;
        }

    }
}
