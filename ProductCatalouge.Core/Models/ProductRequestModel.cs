using ProductCatalouge.Core.DataTransferObject;
using System.Collections.Generic;

namespace ProductCatalouge.Core.Models
{
    public class ProductRequestModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }

        public static ProductDTO ToProductDTO(ProductRequestModel productRequestModel)
        {
            return new ProductDTO
            {
                ProductId = productRequestModel.ProductId,
                ProductName = productRequestModel.ProductName,
                ProductQuantity = productRequestModel.ProductQuantity
            };
        }

        public static List<ProductDTO> ToProductDTO(List<ProductRequestModel> productRequestModel)
        {
            var productDtoList = new List<ProductDTO>();
            foreach (var item in productRequestModel)
            {
                productDtoList.Add(ToProductDTO(item));
            }
            return productDtoList;
        }
    }
}
