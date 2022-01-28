using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IImagesService
    {
        string GetNormalizedName(string fileName);

        Task StorageImage(string imageBase64, string fileName, string environmentPath);
    }
}
