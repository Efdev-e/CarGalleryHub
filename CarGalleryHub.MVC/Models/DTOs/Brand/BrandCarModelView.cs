namespace CarGalleryHub.MVC.Models.DTOs.Brand
{
    public class BrandCarModelView
    {
        public int id { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public int carModelId { get; set; }
        public List<BasicCarModelData> Dtos { get; set; } = new List<BasicCarModelData>();

    }
}
