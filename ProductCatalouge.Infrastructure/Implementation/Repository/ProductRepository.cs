using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ProductCatalouge.Core.Interfaces;
using ProductCatalouge.Core.Models;
using ProductCatalouge.EntityFramework.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalouge.Infrastructure.Implementation.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDetailsContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(IMapper mapper, ProductDetailsContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> AddProducts(List<TblProductDetail> productEntity)
        {
            try
            {
                await _context.AddRangeAsync(productEntity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception sqlEx) when (sqlEx.InnerException is SqlException ex)
            {
                _logger.LogError($"Sql Error occured : {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteProduct(TblProductDetail entity)
        {
            try
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }

        public async Task<TblProductDetail> GetProduct(int id)
        {
            var response = new ProductResponseModel();
            try
            {
                var entity = _context.TblProductDetails.FirstOrDefault(x => x.ProductId == id);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }

        public async Task<List<TblProductDetail>> GetProduct()
        {
            try
            {
                var entity = _context.TblProductDetails.ToList();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateProduct(TblProductDetail entity)
        {
            try
            {
                entity.ProductName = entity.ProductName;
                entity.ProductQuantity = entity.ProductQuantity;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }
    }
}
