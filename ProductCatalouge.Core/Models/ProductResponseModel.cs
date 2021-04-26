using ProductCatalouge.Core.DataTransferObject;
using System.Collections.Generic;

namespace ProductCatalouge.Core.Models
{
    public class ProductResponseModel
    {
        public bool IsSuccess { get; set; }
        public List<ProductDTO> ProductDetails { get; set; }
    }
}
