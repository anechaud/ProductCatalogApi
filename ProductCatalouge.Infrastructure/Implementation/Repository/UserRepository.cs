using Microsoft.Extensions.Logging;
using ProductCatalouge.Core.Interfaces;
using ProductCatalouge.EntityFramework.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalouge.Infrastructure.Implementation.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ProductDetailsContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ProductDetailsContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TblUserRole>> GetRoles(string userId)
        {
            try
            {
                var entity = _context.TblUserRoles.Where(x => x.UserId == userId).ToList();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }

        public async Task<TblUser> GetUser(string userId, string password)
        {
            try
            {
                var entity = _context.TblUsers.Where(x => x.UserId == userId);
                if (entity.Count() == 0)
                    throw new ObjectNotFoundException($"User {userId} does not exist");
                var authenticatedUser = entity.Where(x => x.Password == password).FirstOrDefault();
                return authenticatedUser;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }

        public async Task<List<TblUser>> GetUsers()
        {
            try
            {
                var entity = _context.TblUsers.ToList();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured : {ex.Message}");
                throw;
            }
        }
    }
}
