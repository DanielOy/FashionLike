using Core.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ImagesService : IImagesService
    {
        public const string PostImageFolder = "images/posts";

        public string GetNormalizedName(string fileName)
        {
            string imageGuid = Guid.NewGuid().ToString();

            string extension = new FileInfo(fileName).Extension;

            return $"{imageGuid}{extension}";
        }

        public async Task StorageImage(string imageBase64, string fileName, string environmentPath)
        {
            string imageFullPath = GetPostsFolderFullPath(environmentPath) + "/" + fileName;

            var imageBytes = Convert.FromBase64String(imageBase64);

            await File.WriteAllBytesAsync(imageFullPath, imageBytes);
        }

        private string GetPostsFolderFullPath(string enviromentPath)
        {
            string filesPath = Path.Combine(enviromentPath, "wwwroot", PostImageFolder);

            if (!Directory.Exists(filesPath))
            {
                Directory.CreateDirectory(filesPath);
            }

            return filesPath;
        }
    }
}
