using ProductCatalouge.EntityFramework.Models.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalouge.Core.Interfaces
{
    public interface IProductRepository
    {
        public Task<bool> AddProducts(List<TblProductDetail> requestModel);
        public Task<bool> DeleteProduct(TblProductDetail entity);
        public Task<TblProductDetail> GetProduct(int id);
        public Task<List<TblProductDetail>> GetProduct();
        public Task<bool> UpdateProduct(TblProductDetail entity);
    }
}