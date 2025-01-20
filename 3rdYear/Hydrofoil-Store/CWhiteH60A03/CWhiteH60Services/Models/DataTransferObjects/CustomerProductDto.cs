namespace CWhiteH60Services.Models.DataTransferObjects;

public class CustomerProductDto {
    public int ProductId { get; set; }
    public int ProdCatId { get; set; }
    public string Description { get; set; }
    public string ProdCatName { get; set; }
    public string Manufacturer { get; set; }
    public int Stock { get; set; }
    public decimal SellPrice { get; set; }
    public string? ImageName { get; set; }
    public byte[]? ImageData { get; set; }

    public CustomerProductDto(Product product) {
        ProductId = product.ProductId;
        ProdCatId = product.ProdCatId;
        Description = product.Description;
        ProdCatName = product.ProdCat.ProdCat;
        Manufacturer = product.Manufacturer;
        Stock = product.Stock;
        SellPrice = product.SellPrice;
        ImageName = product.ImageName;
        ImageData = product.ImageData;
    }
}