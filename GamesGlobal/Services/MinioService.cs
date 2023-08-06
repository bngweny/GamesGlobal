namespace GamesGlobal.Services
{
    using System.IO;
    using System.Threading.Tasks;
    using Minio;
    using Minio.Exceptions;

    public class MinioService : IMinioService
    {
        private readonly MinioClient _minioClient;

        public MinioService(string minioServerUrl, string accessKey, string secretKey)
        {
            _minioClient = new MinioClient()
                .WithEndpoint("localhost:9000")
                .WithCredentials("minioadmin", "minioadmin")
                 .WithSSL(true)

                .Build();
        }

        public async Task UploadObjectAsync(string bucketName, string objectName, string filePath, string contentType = "image/jpg")
        {
            try
            {
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithFileName(filePath)
                    .WithContentType(contentType);
                await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
            }
            catch (MinioException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Stream> GetObjectAsync(string bucketName, string objectName, string fileName )
        {
            Stream output = File.Create(fileName);
            try
            {
                var args = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithFile(fileName)
                .WithCallbackStream(async (stream, cancellationToken) =>
                {
                    await stream.CopyToAsync(output).ConfigureAwait(false);
                    var writtenInfo = new FileInfo(fileName);
                     stream.Dispose();
                });
                var stat = await _minioClient.GetObjectAsync(args).ConfigureAwait(false);
                return output;
            }
            catch (MinioException ex)
            {
                throw ex;
            }
        }
    }
}
