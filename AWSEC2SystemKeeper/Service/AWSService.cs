using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using AWSEC2SystemKeeper.Model;

namespace AWSEC2SystemKeeper.Service
{
    public class AWSService(AWSServiceConfig config)
    {
        private readonly string _accessKey = config.AccessKey!;
        private readonly string _secretKey = config.SecretKey!;
        private readonly string _region = config.Region!;
        private readonly string _instanceID = config.InstanceID!;

        public async Task StopEC2InstanceAsync()
        {
            Console.WriteLine($"Stop instance: {_instanceID}");
            //throw new Exception("SystemStop!");
            // 创建 EC2 客户端
            var ec2Client = new AmazonEC2Client(_accessKey, _secretKey, RegionEndpoint.GetBySystemName(_region));

            // 停止实例请求
            var stopInstancesRequest = new StopInstancesRequest
            {
                InstanceIds = [_instanceID]
            };

            // 执行停止实例操作
            var response = await ec2Client.StopInstancesAsync(stopInstancesRequest);

            // 检查响应
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Instance {_instanceID} is stopping.");
            }
            else
            {
                Console.WriteLine($"Failed to stop instance {_instanceID}. Status: {response.HttpStatusCode}");
            }
        }
    }
}
