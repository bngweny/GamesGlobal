namespace GamesGlobal.Services
{
    public interface IMinioService
    {
        Task UploadObjectAsync(string bucketName, string objectName, string filePath, string contentType = "image/jpg");
        Task<Stream> GetObjectAsync(string bucketName, string objectName, string filename);
    }
}
