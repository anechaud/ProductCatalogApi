using AutoMapper;
using Microsoft.Extensions.Logging;
using ProductCatalouge.Core.DataTransferObject;
using ProductCatalouge.Core.Interfaces;
using ProductCatalouge.Core.Models;
using ProductCatalouge.EntityFramework.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Threading.Tasks;

namespace ProductCatalouge.Infrastructure.Implementation.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProductResponseModel> AddProducts(List<ProductDTO> requestModel)
        {
            try
            {
                var entityList = new List<TblProductDetail>();
                foreach (var item in requestModel)
                {
                    var product = await _productRepository.GetProduct(item.ProductId);
                    if (product == null)
                        entityList.Add(_mapper.Map<TblProductDetail>(item));
                }
                var res = await _productRepository.AddProducts(entityList);
                return new ProductResponseModel { IsSuccess = true, ProductDetails = _mapper.Map<List<ProductDTO>>(entityList) };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }

        public async Task<ProductResponseModel> DeleteProduct(int id)
        {
            try
            {
                var productToDelete = await GetProductEntity(id);
                var res = await _productRepository.DeleteProduct(productToDelete);
                return new ProductResponseModel { IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }

        private async Task<TblProductDetail> GetProductEntity(int id)
        {
            try
            {
                var res = await _productRepository.GetProduct(id);
                if (res == null) throw new ObjectNotFoundException($"Product with id {id} not found");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }

        public async Task<ProductResponseModel> GetProduct(int id)
        {
            try
            {
                var res = await GetProductEntity(id);
                var product = _mapper.Map<ProductDTO>(res);
                _logger.LogInformation($"Successfully get the product info for ProductID = {id}");
                return new ProductResponseModel { IsSuccess = true, ProductDetails = new List<ProductDTO> { product } };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }

        public async Task<ProductResponseModel> GetProduct()
        {
            try
            {
                var res = await _productRepository.GetProduct();
                var product = _mapper.Map<List<ProductDTO>>(res);

                return new ProductResponseModel { IsSuccess = true, ProductDetails = product };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }

        public async Task<ProductResponseModel> UpdateProduct(ProductDTO requestModel)
        {
            try
            {
                var productToUpdate = await GetProductEntity(requestModel.ProductId);
                var res = await _productRepository.UpdateProduct(productToUpdate);
                return new ProductResponseModel { IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }
    }
}
