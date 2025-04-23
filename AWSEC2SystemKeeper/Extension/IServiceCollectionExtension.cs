using System.Reflection;
using AWSEC2SystemKeeper.Model;
using AWSEC2SystemKeeper.Service;

namespace AWSEC2SystemKeeper.Extension
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddAWSService(
            this IServiceCollection serviceDescriptors,
            Action<AWSServiceConfig> option
        )
        {
            var config = new AWSServiceConfig();
            option(config);
            Console.WriteLine(
                $"AccessKey: {config.AccessKey}\nSecretKey: {config.SecretKey}\nregion: {config.Region}\n"
            );
            foreach (
                var property in typeof(AWSServiceConfig).GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                )
            )
                _ =
                    property.GetValue(config)
                    ?? throw new ArgumentNullException(
                        property.Name,
                        $"{property.Name} cannot be null"
                    );
            serviceDescriptors.AddSingleton<AWSService>(new AWSService(config));
            return serviceDescriptors;
        }
    }
}
