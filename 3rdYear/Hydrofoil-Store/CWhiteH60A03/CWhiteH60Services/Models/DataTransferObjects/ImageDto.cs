namespace CWhiteH60Services.Models.DataTransferObjects;

public class ImageDto {
    public string? ImageName { get; set; }
    public byte[]? ImageData { get; set; }

    public ImageDto() {
    }

    public ImageDto(Product product) {
        ImageName = product.ImageName;
        ImageData = product.ImageData;
    }
}   