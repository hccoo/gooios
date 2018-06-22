using Microsoft.Extensions.Options;

namespace Gooios.VerificationService.Configurations
{
    public interface IServiceConfigurationProxy
    {
        string ConnectionString { get; }

        string ApiHeaderKey { get; }

        string ApiHeaderValue { get; }
    }

    public class ServiceConfigurationProxy : IServiceConfigurationProxy
    {
        readonly IOptions<ConnectionStrings> _options;
        readonly IOptions<ApplicationSettings> _appSettingsOptions;
        public ServiceConfigurationProxy(IOptions<ConnectionStrings> options,
        IOptions<ApplicationSettings> appSettingsOptions)
        {
            _options = options;
            _appSettingsOptions = appSettingsOptions;
        }

        public string ConnectionString
        {
            get
            {
                return _options.Value.VerificationServiceDb;
            }
        }

        public string ApiHeaderKey
        {
            get
            {
                return _appSettingsOptions.Value.AppHeaderKey;
            }
        }

        public string ApiHeaderValue
        {
            get
            {
                return _appSettingsOptions.Value.AppHeaderValue;
            }
        }
    }

    public class ConnectionStrings
    {
        public string VerificationServiceDb { get; set; }
    }

    public class ApplicationSettings
    {
        public string AppHeaderKey { get; set; }

        public string AppHeaderValue { get; set; }
    }
}
