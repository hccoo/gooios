using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Configurations
{
    public interface IServiceConfigurationProxy
    {
        string ConnectionString { get; }

        string ApiHeaderKey { get; }

        string ApiHeaderValue { get; }

        string VerificationServiceRootUrl { get; }

        string VerificationServiceHeaderValue { get; }

        string AppInternalHeaderKey { get; }

        string AppInternalHeaderValue { get; }

        string WeChatAppId { get; }

        string WeChatAppSecret { get; }
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
                return _options.Value.DefaultConnection;
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
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
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
