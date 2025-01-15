using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Core.Packages.Application.CrossCuttingConcerns.Storage;
using Microsoft.Extensions.Configuration;

namespace Core.Packages.Infrastructure.Storage.AWS;

public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3StorageService(IConfiguration configuration)
    {
        var options = new AmazonS3Config
        {
            ServiceURL = configuration["Storage:AWS:ServiceUrl"]
        };

        _s3Client = new AmazonS3Client(
            configuration["Storage:AWS:AccessKey"],
            configuration["Storage:AWS:SecretKey"],
            options
        );

        _bucketName = configuration["Storage:AWS:BucketName"] ?? "files";
    }

    public async Task<string> UploadAsync(string fileName, Stream fileStream)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            InputStream = fileStream
        };

        await _s3Client.PutObjectAsync(request);
        return GetFileUrl(fileName);
    }

    public async Task<bool> DeleteAsync(string fileName)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName
        };

        var response = await _s3Client.DeleteObjectAsync(request);
        return response.HttpStatusCode == HttpStatusCode.NoContent;
    }

    public async Task<Stream> DownloadAsync(string fileName)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName
        };

        var response = await _s3Client.GetObjectAsync(request);
        var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public bool Exists(string fileName)
    {
        try
        {
            var request = new GetObjectMetadataRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            _s3Client.GetObjectMetadataAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public string GetFileUrl(string fileName)
    {
        return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
    }
} 