using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductCatalouge.Core.Interfaces;
using ProductCatalouge.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalouge.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductCatalougeController : Controller
    {
        private readonly IProductService _productService;
        public ProductCatalougeController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("post/products")]
        public async Task<ActionResult> AddProductsAsync([FromBody] List<ProductRequestModel> model)
        {
            var res = await _productService.AddProducts(ProductRequestModel.ToProductDTO(model));
            return Ok($"Products added for {JsonConvert.SerializeObject(res.ProductDetails.Select(x => x.ProductId))}");
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Developer")]
        [Route("put/product")]
        public async Task<ActionResult> UpdateProductAsync(ProductRequestModel model)
        {
            var res = await _productService.UpdateProduct(ProductRequestModel.ToProductDTO(model));
            return Ok($"Product with id {model.ProductId} is updated");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("delete/product/{id}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            var res = await _productService.DeleteProduct(id);
            return Ok($"Product with id {id} is deleted");
        }

        [HttpGet]
        [Route("product/{id}")]
        [Authorize(Roles = "Admin, Developer,Viewer")]
        public async Task<ActionResult> GetProductAsync(int id)
        {
            var res = await _productService.GetProduct(id);
            return Ok(res.ProductDetails);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Developer,Viewer")]
        [Route("products")]
        public async Task<ActionResult> GetProductAsync()
        {
            var res = await _productService.GetProduct();
            return Ok(res.ProductDetails);
        }
    }
}
