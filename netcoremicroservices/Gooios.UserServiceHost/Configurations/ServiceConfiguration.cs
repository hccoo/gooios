using Gooios.UserService.Configurations;
using Microsoft.Extensions.Options;

namespace Gooios.UserServiceHost.Configurations
{

    public class ServiceConfiguration : IServiceConfigurationAgent
    {
        readonly IOptions<ConnectionStrings> _options;
        readonly IOptions<ApplicationSettings> _appSettingsOptions;
        public ServiceConfiguration(IOptions<ConnectionStrings> options,
        IOptions<ApplicationSettings> appSettingsOptions)
        {
            _options = options;
            _appSettingsOptions = appSettingsOptions;
        }

        public string ConnectionString => _options.Value.ServiceDb;

        public string UserDbConnectionString => _options.Value.UserDb;

        public string ApiHeaderKey => _appSettingsOptions.Value.AppHeaderKey;

        public string ApiHeaderValue => _appSettingsOptions.Value.AppHeaderValue;

        public string VerificationServiceRootUrl => _appSettingsOptions.Value.VerificationServiceRootUrl;

        public string VerificationServiceHeaderValue =>  _appSettingsOptions.Value.VerificationServiceHeaderValue;

        public string AppInternalHeaderKey => _appSettingsOptions.Value.AppInternalHeaderKey;

        public string AppInternalHeaderValue =>  _appSettingsOptions.Value.AppInternalHeaderValue;

        public string WeChatAppId => _appSettingsOptions.Value.WeChatAppId;

        public string WeChatAppSecret => _appSettingsOptions.Value.WeChatAppSecret;
    }

    public class ConnectionStrings
    {
        public string ServiceDb { get; set; }

        public string UserDb { get; set; }
    }

    public class ApplicationSettings
    {
        public string AppHeaderKey { get; set; }

        public string AppHeaderValue { get; set; }
        public string VerificationServiceRootUrl { get; set; }

        public string VerificationServiceHeaderValue { get; set; }

        public string AppInternalHeaderKey { get; set; }

        public string AppInternalHeaderValue { get; set; }

        public string WeChatAppId { get; set; }

        public string WeChatAppSecret { get; set; }
    }
}
