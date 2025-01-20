namespace Byndyusoft.ApiClient.Extensions
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    public static class MvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddJsonOptions(
            this IMvcCoreBuilder builder,
            string settingsName,
            Action<JsonOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(configure);
            builder.Services.Configure(settingsName, configure);
            return builder;
        }
    }
}
