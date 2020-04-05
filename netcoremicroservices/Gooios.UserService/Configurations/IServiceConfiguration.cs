namespace Gooios.UserService.Configurations
{
    public interface IServiceConfigurationAgent
    {
        string ConnectionString { get; }

        string ApiHeaderKey { get; }

        string ApiHeaderValue { get; }

        public string VerificationServiceRootUrl { get; }

        public string VerificationServiceHeaderValue { get; }

        public string AppInternalHeaderKey { get; }

        public string AppInternalHeaderValue { get; }

        public string WeChatAppId { get; }

        public string WeChatAppSecret { get; }

    }
}
