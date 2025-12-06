using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Item.ViewModels
{
    public class ItemVariantVM<T> where T : new()
    {
        [Required]
        public T ItemVariant { get; set; } = new T();
        public IFormFile? formFile { get; set; }
        public bool deleteImage { get; set; }
    }
}
