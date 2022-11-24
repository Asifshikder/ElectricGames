namespace Project.Services
{
    public interface IFileHandler
    {
        string UploadFile( IFormFile file);
        void DeleteFile(string foldername);
        string UpdateFile(string property,  IFormFile file);
    }
}
