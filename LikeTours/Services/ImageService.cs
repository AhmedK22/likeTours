using LikeTours.Data.Models;

namespace LikeTours.Services
{
    public class ImageService
    {
        private readonly string _imageFolderPath;

        public void DeleteImageFiles(IEnumerable<Image> images)
        {
            foreach (var image in images)
            {
                //var filePath = Path.Combine(_imageFolderPath, image.Url);
                if (File.Exists(image.Url))
                {
                    File.Delete(image.Url);
                }
            }
        }



        public void DeleteImageFile(string image)
        {
            
                if (File.Exists(image))
                {
                    File.Delete(image);
                }
            
        }
    }

}
