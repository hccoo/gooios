using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserService.Configurations
{
    public interface IServiceConfigurationAgent
    {
        string ConnectionString { get; }

        string UserDbConnectionString { get; }

        string ApiHeaderKey { get; }

        string ApiHeaderValue { get; }

        public string VerificationServiceRootUrl { get; }

        public string VerificationServiceHeaderValue { get;}

        public string AppInternalHeaderKey { get;  }

        public string AppInternalHeaderValue { get; }

        public string WeChatAppId { get; }

        public string WeChatAppSecret { get; }

        public string UserServiceRootUrl { get; }

        public string UserServiceHeaderValue { get; }

    }

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

        public string VerificationServiceRootUrl
        {
            get
            {
                return _appSettingsOptions.Value.VerificationServiceRootUrl;
            }
        }

        public string VerificationServiceHeaderValue
        {
            get
            {
                return _appSettingsOptions.Value.VerificationServiceHeaderValue;
            }
        }

        public string AppInternalHeaderKey
        {
            get
            {
                return _appSettingsOptions.Value.AppInternalHeaderKey;
            }
        }

        public string AppInternalHeaderValue
        {
            get
            {
                return _appSettingsOptions.Value.AppInternalHeaderValue;
            }
        }

        public string WeChatAppId => _appSettingsOptions.Value.WeChatAppId;

        public string WeChatAppSecret => _appSettingsOptions.Value.WeChatAppSecret;

        public string UserServiceRootUrl => _appSettingsOptions.Value.UserServiceRootUrl;

        public string UserServiceHeaderValue => _appSettingsOptions.Value.UserServiceHeaderValue;
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

        public string UserServiceRootUrl { get; set; }

        public string UserServiceHeaderValue { get; set; }
    }
}
