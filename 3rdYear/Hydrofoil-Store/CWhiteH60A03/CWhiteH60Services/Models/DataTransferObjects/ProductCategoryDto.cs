namespace CWhiteH60Services.Models.DataTransferObjects;

public class ProductCategoryDto {
    public int CategoryId { get; set; }
    public string ProdCat  { get; set; }

    public ProductCategoryDto() { }

    public ProductCategoryDto(ProductCategory category) {
        CategoryId = category.CategoryId;
        ProdCat  = category.ProdCat;
    }
}