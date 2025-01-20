using CWhiteH60Customer.Models;
using CWhiteH60Customer.DAL;
using CWhiteH60Customer.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CWhiteH60Customer.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository<Product> _productRepository;
        private readonly IPartialViewUtility _partialViewUtility;

        public ProductController(IProductRepository<Product> productRepository, IPartialViewUtility partialViewUtility) {
            _productRepository = productRepository;
            _partialViewUtility = partialViewUtility;
        }
        
        public async Task<ActionResult> Index() {
            return View(await _productRepository.GetAll());
        }
        
        public async Task<ActionResult> ReloadProductsPartial([FromQuery]string productName) {
            var productList = await _productRepository.GetByName(productName);
            return Json(new {
                productList = await _partialViewUtility.RenderPartialToStringAsync(this, "_ProductDisplayPartial", productList),
                productCount = productList.Count
            });
        }

        public async Task<ActionResult> Details(int id) {
            return View(await _productRepository.GetById(id));
        }
    }
}
