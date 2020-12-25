
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using Microsoft.WindowsAzure.Storage.Blob;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenDo.Services
{
    public class VideoService : IVideoService
    {
        private IAzureMediaServicesClient client = null;
        public async Task ConnectAsync()
        {
            try
            {
                client = await CreateMediaServicesClientAsync();
                Console.WriteLine("connected");
            }
            catch (Exception exception)
            {
                if (exception.Source.Contains("ActiveDirectory"))
                {
                    Console.Error.WriteLine("TIP: Make sure that you have filled out the appsettings.json file before running this sample.");
                }

                Console.Error.WriteLine($"{exception.Message}");

                ApiErrorException apiException = exception.GetBaseException() as ApiErrorException;
                if (apiException != null)
                {
                    Console.Error.WriteLine(
                        $"ERROR: API call failed with error code '{apiException.Body.Error.Code}' and message '{apiException.Body.Error.Message}'.");
                }
            }

            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
        }

        private async Task<ServiceClientCredentials> GetCredentialsAsync()
        {
            // Use ApplicationTokenProvider.LoginSilentWithCertificateAsync or UserTokenProvider.LoginSilentAsync to get a token using service principal with certificate
            //// ClientAssertionCertificate
            //// ApplicationTokenProvider.LoginSilentWithCertificateAsync

            // Use ApplicationTokenProvider.LoginSilentAsync to get a token using a service principal with symmetric key
            var aadClientId = "ff79ecc9-27de-43ef-91ac-f0daf5263db6";
            var aadClientSecret = "8i2o07Zzl0MRgjov-M6mc.q9i~_ogAsa19";
            ClientCredential clientCredential = new ClientCredential(aadClientId, aadClientSecret);
            var aadTenantId = "1d9ed556-b09d-48f2-a5ba-173b21b5fea8";
            return await ApplicationTokenProvider.LoginSilentAsync(aadTenantId, clientCredential, ActiveDirectoryServiceSettings.Azure);
        }

        private async Task<IAzureMediaServicesClient> CreateMediaServicesClientAsync()
        {
            var credentials = await GetCredentialsAsync();

            var armEndpoint = "https://management.azure.com";
            var subscritpionId = "d398b4a9-91dc-4a66-bee1-0dc9e54746d2";
            return new AzureMediaServicesClient(new Uri(armEndpoint), credentials)
            {
                SubscriptionId = subscritpionId,
            };
        }

        public async Task<string> UploadVideoAsync(MediaFile file)
        {
            var inputAsset = await CreateInputAssetAsync("GreenDo", "play4u", "randomName123_" + Guid.NewGuid().ToString(), file);
            var outputAsset = await CreateOutputAssetAsync("GreenDo", "play4u", "random123_" + Guid.NewGuid().ToString());
            var transform = await GetOrCreateTransformAsync("GreenDo", "play4u", "MyTransform");
            var job = await SubmitJobAsync("GreenDo", "play4u", transform.Name, "MyJob_" + Guid.NewGuid().ToString(),
                inputAsset.Name, outputAsset.Name);
            var completedJob = await WaitForJobToFinishAsync("GreenDo", "play4u", "MyTransform", job.Name);
            var locator = await CreateStreamingLocatorAsync("GreenDo", "play4u", outputAsset.Name, "MyLocator_" + Guid.NewGuid().ToString());
            var urls = await GetStreamingUrlsAsync("GreenDo", "play4u", locator.Name);
            return urls.FirstOrDefault();
        }

        private async Task<Asset> CreateInputAssetAsync(
        string resourceGroupName,
        string accountName,
        string assetName,
        MediaFile fileToUpload)
        {
            // In this example, we are assuming that the asset name is unique.
            //
            // If you already have an asset with the desired name, use the Assets.Get method
            // to get the existing asset. In Media Services v3, the Get method on entities returns null 
            // if the entity doesn't exist (a case-insensitive check on the name).

            // Call Media Services API to create an Asset.
            // This method creates a container in storage for the Asset.
            // The files (blobs) associated with the asset will be stored in this container.
            Asset asset = await client.Assets.CreateOrUpdateAsync(resourceGroupName, accountName, assetName, new Asset());

            // Use Media Services API to get back a response that contains
            // SAS URL for the Asset container into which to upload blobs.
            // That is where you would specify read-write permissions 
            // and the exparation time for the SAS URL.
            var response = await client.Assets.ListContainerSasAsync(
                resourceGroupName,
                accountName,
                assetName,
                permissions: AssetContainerPermission.ReadWrite,
                expiryTime: DateTime.UtcNow.AddHours(4).ToUniversalTime());

            var sasUri = new Uri(response.AssetContainerSasUrls.First());

            // Use Storage API to get a reference to the Asset container
            // that was created by calling Asset's CreateOrUpdate method.  
            CloudBlobContainer container = new CloudBlobContainer(sasUri);
            var blob = container.GetBlockBlobReference(fileToUpload.Path.Split('/').Last());

            byte[] bytes = null;
            using (var memoryStream = new MemoryStream())
            {
                fileToUpload.GetStream().CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            // Use Strorage API to upload the file into the container in storage.
            await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);

            return asset;
        }

        private async Task<Asset> CreateOutputAssetAsync(string resourceGroupName, string accountName, string assetName)
        {
            // Check if an Asset already exists
            Asset outputAsset = await client.Assets.GetAsync(resourceGroupName, accountName, assetName);
            Asset asset = new Asset();
            string outputAssetName = assetName;

            if (outputAsset != null)
            {
                // Name collision! In order to get the sample to work, let's just go ahead and create a unique asset name
                // Note that the returned Asset can have a different name than the one specified as an input parameter.
                // You may want to update this part to throw an Exception instead, and handle name collisions differently.
                string uniqueness = $"-{Guid.NewGuid():N}";
                outputAssetName += uniqueness;

                Console.WriteLine("Warning – found an existing Asset with name = " + assetName);
                Console.WriteLine("Creating an Asset with this name instead: " + outputAssetName);
            }

            return await client.Assets.CreateOrUpdateAsync(resourceGroupName, accountName, outputAssetName, asset);
        }

        private async Task<Transform> GetOrCreateTransformAsync(string resourceGroupName, string accountName, string transformName)
        {
            // Does a Transform already exist with the desired name? Assume that an existing Transform with the desired name
            // also uses the same recipe or Preset for processing content.
            Transform transform = await client.Transforms.GetAsync(resourceGroupName, accountName, transformName);

            if (transform == null)
            {
                // You need to specify what you want it to produce as an output
                TransformOutput[] output = new TransformOutput[]
                {
            new TransformOutput
            {
                // The preset for the Transform is set to one of Media Services built-in sample presets.
                // You can  customize the encoding settings by changing this to use "StandardEncoderPreset" class.
                Preset = new BuiltInStandardEncoderPreset()
                {
                    // This sample uses the built-in encoding preset for Adaptive Bitrate Streaming.
                    PresetName = EncoderNamedPreset.AdaptiveStreaming
                }
            }
                };

                // Create the Transform with the output defined above
                transform = await client.Transforms.CreateOrUpdateAsync(resourceGroupName, accountName, transformName, output);
            }

            return transform;
        }

        private async Task<Job> SubmitJobAsync(
            string resourceGroupName,
            string accountName,
            string transformName,
            string jobName,
            string inputAssetName,
            string outputAssetName)
        {
            // Use the name of the created input asset to create the job input.
            JobInput jobInput = new JobInputAsset(assetName: inputAssetName);

            JobOutput[] jobOutputs =
            {
                new JobOutputAsset(outputAssetName),
                };

            // In this example, we are assuming that the job name is unique.
            //
            // If you already have a job with the desired name, use the Jobs.Get method
            // to get the existing job. In Media Services v3, the Get method on entities returns null 
            // if the entity doesn't exist (a case-insensitive check on the name).
            Job job = await client.Jobs.CreateAsync(
                resourceGroupName,
                accountName,
                transformName,
                jobName,
                new Job
                {
                    Input = jobInput,
                    Outputs = jobOutputs,
                });

            return job;
        }

        private async Task<Job> WaitForJobToFinishAsync(
            string resourceGroupName,
            string accountName,
            string transformName,
            string jobName)
        {
            const int SleepIntervalMs = 200 * 10;

            Job job;
            do
            {
                job = await client.Jobs.GetAsync(resourceGroupName, accountName, transformName, jobName);

                Console.WriteLine($"Job is '{job.State}'.");
                for (int i = 0; i < job.Outputs.Count; i++)
                {
                    JobOutput output = job.Outputs[i];
                    Console.Write($"\tJobOutput[{i}] is '{output.State}'.");
                    if (output.State == JobState.Processing)
                    {
                        Console.Write($"  Progress (%): '{output.Progress}'.");
                    }

                    Console.WriteLine();
                }

                if (job.State != JobState.Finished && job.State != JobState.Error && job.State != JobState.Canceled)
                {
                    await Task.Delay(SleepIntervalMs);
                }
            }
            while (job.State != JobState.Finished && job.State != JobState.Error && job.State != JobState.Canceled);

            return job;
        }

        private async Task<StreamingLocator> CreateStreamingLocatorAsync(
            string resourceGroup,
            string accountName,
            string assetName,
            string locatorName)
        {
            StreamingLocator locator = null;
            try
            {
                locator = await client.StreamingLocators.CreateAsync(
                     resourceGroup,
                     accountName,
                     locatorName,
                    new StreamingLocator
                    {
                        AssetName = assetName,
                        StreamingPolicyName = PredefinedStreamingPolicy.ClearStreamingOnly
                    });
            }
            catch (ApiErrorException apiError)
            {
                
            }


            return locator;
        }

        private async Task<IList<string>> GetStreamingUrlsAsync(
            string resourceGroupName,
            string accountName,
            string locatorName)
        {
            const string DefaultStreamingEndpointName = "default";

            IList<string> streamingUrls = new List<string>();

            StreamingEndpoint streamingEndpoint = await client.StreamingEndpoints.GetAsync(resourceGroupName, accountName, DefaultStreamingEndpointName);

            if (streamingEndpoint != null)
            {
                if (streamingEndpoint.ResourceState != StreamingEndpointResourceState.Running)
                {
                    await client.StreamingEndpoints.StartAsync(resourceGroupName, accountName, DefaultStreamingEndpointName);
                }
            }

            ListPathsResponse paths = await client.StreamingLocators.ListPathsAsync(resourceGroupName, accountName, locatorName);

            foreach (StreamingPath path in paths.StreamingPaths)
            {
                UriBuilder uriBuilder = new UriBuilder
                {
                    Scheme = "https",
                    Host = streamingEndpoint.HostName,

                    Path = path.Paths[0]
                };
                streamingUrls.Add(uriBuilder.ToString());
            }

            return streamingUrls;
        }
    }
}
