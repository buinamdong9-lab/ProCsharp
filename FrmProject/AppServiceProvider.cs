using System;
using Microsoft.Extensions.DependencyInjection;

namespace FrmProject
{
    public static class AppServiceProvider
    {
        public static IServiceProvider? Provider { get; set; }

        public static T Get<T>() where T : class
        {
            if (Provider == null)
                throw new InvalidOperationException("AppServiceProvider has not been initialized.");

            return Provider.GetRequiredService<T>();
        }
    }
}
