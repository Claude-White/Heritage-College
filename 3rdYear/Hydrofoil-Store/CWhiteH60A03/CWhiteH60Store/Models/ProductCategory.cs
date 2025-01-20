using System;
using System.Collections.Generic;

namespace CWhiteH60Store.Models;

public partial class ProductCategory
{
    public int CategoryID { get; set; }

    public string ProdCat { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
