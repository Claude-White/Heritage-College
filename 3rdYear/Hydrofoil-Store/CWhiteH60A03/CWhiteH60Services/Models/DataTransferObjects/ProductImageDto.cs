namespace CWhiteH60Services.Models.DataTransferObjects;

public class ProductImageDto {
    public int ProductId { get; set; }
    public int ProdCatId { get; set; }
    public ProductCategoryDto ProdCategory { get; set; }
    public string Description { get; set; }
    public string Manufacturer { get; set; }
    public int Stock { get; set; }
    public decimal BuyPrice { get; set; }
    public decimal SellPrice { get; set; }
    public string? Notes { get; set; }
    public string? ImageName { get; set; }
    public byte[]? ImageData { get; set; }

    public ProductImageDto() {
    }

    public ProductImageDto(Product product) {
        ProductId = product.ProductId;
        ProdCatId = product.ProdCatId;
        ProdCategory = new ProductCategoryDto(product.ProdCat);
        Description = product.Description;
        Manufacturer = product.Manufacturer;
        Stock = product.Stock;
        BuyPrice = product.BuyPrice;
        SellPrice = product.SellPrice;
        Notes = product.Notes;
        ImageName = product.ImageName;
        ImageData = product.ImageData;
    }
}