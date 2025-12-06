using CoreLayer.Models.ItemVarients;
using Microsoft.VisualBasic.FileIO;

namespace PresentationLayer.Utility
{
    public class ImageService
    {
        public static Result UploadNewImage(IFormFile formFile)
        {
            try
            {
                if (formFile is not null && formFile.Length > 0)
                {
                    // Ensure Images folder exists
                    var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }

                    // Generate unique file name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                    var filePath = Path.Combine(imagesPath, fileName);

                    // Save file
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        formFile.CopyTo(stream);
                    }

                    return new Result { Success = true, ErrorMessage = null!, Image = fileName };
                }

                return new Result { Success = false, ErrorMessage = "Image file is corrupt" };
            }
            catch (Exception ex)
            {
                return new Result { Success = false, ErrorMessage = ex.Message };
            }
        }

        public static Result DeleteImage(string fileName)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                return new Result { Success = true, ErrorMessage = null! };
            }
            catch (Exception ex)
            {
                return new Result { Success = false, ErrorMessage = ex.Message };
            }
        }
    }

}
