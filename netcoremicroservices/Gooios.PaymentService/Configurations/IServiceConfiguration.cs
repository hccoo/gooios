using Microsoft.Extensions.Options;

namespace Gooios.PaymentService.Configurations
{
    public interface IServiceConfigurationProxy
    {
        string ConnectionString { get; }

        string ApiHeaderKey { get; }

        string ApiHeaderValue { get; }

        string WeChatAppId { get;  }
        string WeChatMchId { get;}
        string WeChatKey { get;}
        string WeChatAppSecret { get; }
        string WeChatNotifyUrl { get;}
        string WeChatSslCertPath { get;}
        string WeChatSslCertPassword { get; }
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
                return _options.Value.ServiceDb;
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

        public string WeChatAppId
        {
            get
            {
                return _appSettingsOptions.Value.WeChatAppId;
            }
        }
        public string WeChatMchId
        {
            get
            {
                return _appSettingsOptions.Value.WeChatMchId;
            }
        }
        public string WeChatKey
        {
            get
            {
                return _appSettingsOptions.Value.WeChatKey;
            }
        }
        public string WeChatAppSecret
        {
            get
            {
                return _appSettingsOptions.Value.WeChatAppSecret;
            }
        }
        public string WeChatNotifyUrl
        {
            get
            {
                return _appSettingsOptions.Value.WeChatNotifyUrl;
            }
        }
        public string WeChatSslCertPath
        {
            get
            {
                return _appSettingsOptions.Value.WeChatSslCertPath;
            }
        }
        public string WeChatSslCertPassword
        {
            get
            {
                return _appSettingsOptions.Value.WeChatSslCertPassword;
            }
        }
    }

    public class ConnectionStrings
    {
        public string ServiceDb { get; set; }
    }

    public class ApplicationSettings
    {
        public string AppHeaderKey { get; set; }

        public string AppHeaderValue { get; set; }
        public string WeChatAppId { get; set; }
        public string WeChatMchId { get; set; }
        public string WeChatKey { get; set; }
        public string WeChatAppSecret { get; set; }
        public string WeChatNotifyUrl { get; set; }
        public string WeChatSslCertPath { get; set; }
        public string WeChatSslCertPassword { get; set; }
    }
}
