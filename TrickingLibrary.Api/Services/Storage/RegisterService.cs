using System;
using Microsoft.Extensions.Configuration;
using TrickingLibrary.Api;
using TrickingLibrary.Api.BackgroundServices.VideoEditing;
using TrickingLibrary.Api.Services.Storage;
using TrickingLibrary.Api.Settings;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class RegisterService
    {
        public static IServiceCollection AddFileServices(this IServiceCollection services, IConfiguration config)
        {
            var settingsSection = config.GetSection(nameof(FileSettings));
            var settings = settingsSection.Get<FileSettings>();
            services.Configure<FileSettings>(settingsSection);

            services.AddSingleton<TemporaryFileStorage>();
            if (settings.Provider.Equals(TrickingLibraryConstants.Files.Providers.Local))
            {
                services.AddSingleton<IFileProvider, LocalFileProvider>();
            }
            else if (settings.Provider.Equals(TrickingLibraryConstants.Files.Providers.S3))
            {
                services.Configure<S3Settings>(config.GetSection(nameof(S3Settings)));
                services.AddSingleton<IFileProvider, S3FileProvider>();
                services.AddSingleton<IS3Client, LinodeS3Client>();
            }
            else
            {
                throw new Exception($"Invalid File Manager Provider: {settings.Provider}");
            }

            return services;
        }
    }
}