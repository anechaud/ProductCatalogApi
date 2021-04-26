using ProductCatalouge.Core.DataTransferObject;
using ProductCatalouge.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalouge.Core.Interfaces
{
    public interface IProductService
    {
        public Task<ProductResponseModel> AddProducts(List<ProductDTO> requestModel);
        public Task<ProductResponseModel> DeleteProduct(int id);
        public Task<ProductResponseModel> GetProduct(int id);
        public Task<ProductResponseModel> GetProduct();
        public Task<ProductResponseModel> UpdateProduct(ProductDTO requestModel);

    }
}
