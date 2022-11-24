namespace Project.Services
{
    public class FileHandler : IFileHandler
    {
        private IWebHostEnvironment webHost;

        public FileHandler(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
        }

        public void DeleteFile(string foldername)
        {
            string dbpath = foldername.Replace("~/", "").ToString();
            string uppath = dbpath.Replace("/", "\\").ToString();
            string fullpath = webHost.WebRootPath + "\\" + uppath;
            System.IO.File.Delete(fullpath);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

   

        public string UpdateFile(string property, IFormFile file)
        {
            if (property != null)
            {
                string dbpath = property.Replace("~/", "").ToString();
                string uppath = dbpath.Replace("/", "\\").ToString();
                string fullpath = webHost.WebRootPath + "\\" + uppath;
                System.IO.File.Delete(fullpath);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            Guid nameguid = Guid.NewGuid();
            string webrootpath = webHost.WebRootPath;
            string filename = nameguid.ToString();
            string extension = Path.GetExtension(file.FileName);
            filename = filename + extension;
            string path = Path.Combine(webrootpath, filename);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            string pathName = Path.Combine( filename);
            string fileUrl = "~/" + filename;
            return fileUrl;
        }

        public string UploadFile( IFormFile file)
        {
            Guid nameguid = Guid.NewGuid();
            string webrootpath = webHost.WebRootPath;
            string filename = nameguid.ToString();
            string extension = Path.GetExtension(file.FileName);
            filename = filename + extension;
            string path = Path.Combine(webrootpath, filename);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            string pathName = Path.Combine( filename);
            string fileUrl = "~/" + filename;
            return fileUrl;
        }
    }
}

