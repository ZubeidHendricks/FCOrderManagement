using FCOrderManagement.Models;

namespace FCOrderManagement.Services
{
        public interface IApiService
        {
            Task<bool> AuthenticateUser(string username, string password);
            Task<List<Product>> GetProducts();
            Task<bool> PlaceOrder(Order order);
        }

      
    }
