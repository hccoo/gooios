﻿using Microsoft.Extensions.Options;

namespace Gooios.ActivityService.Configurations
{
    public interface IServiceConfigurationProxy
    {
        string ConnectionString { get; }

        string ApiHeaderKey { get; }

        string ApiHeaderValue { get; }

        string AmapKey { get; }

        string AmapRootUrl { get; }
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

        public string AmapKey
        {
            get
            {
                return _appSettingsOptions.Value.AmapKey;
            }
        }

        public string AmapRootUrl
        {
            get
            {
                return _appSettingsOptions.Value.AmapRootUrl;
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

        public string AmapKey { get; set; }

        public string AmapRootUrl { get; set; }
    }
}
