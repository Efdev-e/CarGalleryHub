using CarGalleryHub.Domain.Enum;

namespace CarGalleryHub.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string ToTurkish(this ColorType color) => color switch
        {
            ColorType.Red => "Kırmızı",
            ColorType.Blue => "Mavi",
            ColorType.Green => "Yeşil",
            ColorType.Beige => "Bej",
            ColorType.Brown => "Kahverengi",
            ColorType.Black => "Siyah",
            ColorType.Pink => "Pembe",
            ColorType.Magenta => "Macenta",
            ColorType.Yellow => "Sarı",
            ColorType.Orange => "Turuncu",
            ColorType.White => "Beyaz",
            ColorType.Silver => "Gümüş",
            ColorType.Gray => "Gri",
            ColorType.Null => "Belirtilmemiş",
            _ => color.ToString()
        };

        public static string ToTurkish(this CarStatus status) => status switch
        {
            CarStatus.New => "Sıfır",
            CarStatus.Used => "Kullanılmış",
            CarStatus.SecondHand => "İkinci El",
            CarStatus.Damaged => "Hasarlı",
            CarStatus.Scrapped => "Hurda",
            _ => status.ToString()
        };

        public static string ToTurkish(this CarAvailability avail) => avail switch
        {
            CarAvailability.Available => "Müsait",
            CarAvailability.Reserved => "Rezerve",
            CarAvailability.Sold => "Satıldı",
            CarAvailability.Unavailable => "Müsait Değil",
            _ => avail.ToString()
        };

        public static string ToTurkish(this CategoryType type) => type switch
        {
            CategoryType.HighestPrice => "Pahalı'dan Ucuz'a",
            CategoryType.LowestPrice => "Ucuz'dan Pahalı'ya",
            _ => type.ToString()
        };
    }
}