using SneakersShop.Core.Entities;

namespace SneakersShop.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

public interface IProductRepository : IRepository<Product> { }
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
public interface ICartRepository : IRepository<CartItem>
{
    Task<List<CartItem>> GetByUserIdAsync(int userId);
    Task<CartItem?> GetByUserAndProductAsync(int userId, int productId);
}