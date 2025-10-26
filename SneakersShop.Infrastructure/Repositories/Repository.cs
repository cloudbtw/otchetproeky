using Microsoft.EntityFrameworkCore;
using SneakersShop.Core.Entities;
using SneakersShop.Core.Interfaces;
using SneakersShop.Infrastructure.Data;

namespace SneakersShop.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public virtual async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);
    public virtual async Task<List<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
    public virtual async Task<T> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public virtual async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
    public virtual async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
}

public class CartRepository : Repository<CartItem>, ICartRepository
{
    public CartRepository(AppDbContext context) : base(context) { }

    public async Task<List<CartItem>> GetByUserIdAsync(int userId) =>
        await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

    public async Task<CartItem?> GetByUserAndProductAsync(int userId, int productId) =>
        await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);
}