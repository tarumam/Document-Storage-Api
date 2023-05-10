using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;
using DocStorageApi.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Services
{
    public class S3Service : IS3Service
    {
        readonly ILogger _logger;
        readonly IConfiguration _configuration;

        public S3Service(ILogger<S3Service> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<FileUploadResponse> UploadFileAsync(FileUploadRequest uploadInfo)
        {
            //TODO: improve this methods
            var bucket = _configuration.GetSection("AWS:Bucket").Value;
            try
            {
                var postedDate = DateTime.UtcNow;
                var request = CreateUploadRequest(uploadInfo, bucket, postedDate);

                var transferUtility = new TransferUtility(GetS3Client());

                await transferUtility.UploadAsync(request);

                return new FileUploadResponse(true, request.Key, postedDate);
            }
            catch (AmazonS3Exception s3Ex)
            {
                _logger.LogError("AmazonS3Exception: {Message} - {StatusCode}", s3Ex.Message, s3Ex.StatusCode);
                return new FileUploadResponse(false, string.Empty, null, (int)s3Ex.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError("AmazonS3Exception: {Message}", ex.Message);
                return new FileUploadResponse(false, string.Empty, null);
            }
        }


        public async Task<MemoryStream> DownloadWithMetadata(string filename)
        {
            var bucket = _configuration.GetSection("AWS:Bucket").Value;

            var s3Client = GetS3Client();

            var getObjectRequest = new GetObjectRequest
            {
                BucketName = bucket,
                Key = filename
            };

            // Adds the header response-cache-control to include metadata to http response.
            getObjectRequest.ResponseHeaderOverrides.CacheControl = "no-cache";

            var objectResponse = await s3Client.GetObjectAsync(getObjectRequest);

            var memoryStream = new MemoryStream();
            await objectResponse.ResponseStream.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }


        /// <summary>
        /// Get the file url from server
        /// </summary>
        /// <param name="bucketName">Bucket where the file is stored</param>
        /// <param name="key">Filename</param>
        /// <param name="expires">Time to expire in minutes</param>
        /// <returns></returns>
        private string GetPreSignedUrl([Required] string bucketName, string key, DateTime expires)
        {
            using var client = GetS3Client();

            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = key,
                Expires = expires
            };

            return client.GetPreSignedURL(request);
        }

        /// <summary>
        /// Get S3 Client
        /// </summary>
        /// <returns>Amazon s3 client</returns>
        private AmazonS3Client GetS3Client()
        {
            var accessKeyId = _configuration.GetSection("AWS:AccessKeyId").Value;
            var secretAccessKey = _configuration.GetSection("AWS:SecretAccessKey").Value;
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.SAEast1
            };
            var credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
            var client = new AmazonS3Client(credentials, config);
            return client;
        }

        /// <summary>
        /// Create the upload file request
        /// </summary>
        /// <param name="uploadInfo">FileUploadRequest</param>
        /// <param name="bucket">Bucket name</param>
        /// <param name="postedDate">Date of posting</param>
        /// <returns>TransferUtilityUploadRequest</returns>
        private TransferUtilityUploadRequest CreateUploadRequest(FileUploadRequest uploadInfo, string bucket, DateTime postedDate)
        {
            var fileExtension = Path.GetExtension(uploadInfo.File.FileName);
            var request = new TransferUtilityUploadRequest
            {
                InputStream = uploadInfo.File.OpenReadStream(),
                Key = $"{Guid.NewGuid()}{fileExtension}",
                BucketName = bucket,
                CannedACL = S3CannedACL.NoACL
            };

            //TODO: Put this as an array
            request.Metadata.Add("postedDate", postedDate.ToString());
            request.Metadata.Add("name", uploadInfo.Name);
            request.Metadata.Add("description", uploadInfo.Description);
            request.Metadata.Add("category", uploadInfo.Category);

            return request;
        }

    }
}
