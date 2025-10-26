using Microsoft.AspNetCore.Mvc;
using SneakersShop.Core.Entities;
using SneakersShop.Core.Interfaces;
using SneakersShop.Infrastructure.Data;

namespace SneakersShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DbAdminController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public DbAdminController(AppDbContext context, IProductRepository productRepository, IUserRepository userRepository)
    {
        _context = context;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    [HttpGet("check-data")]
    public async Task<ActionResult> CheckData()
    {
        var users = await _userRepository.GetAllAsync();
        var products = await _productRepository.GetAllAsync();

        return Ok(new
        {
            TotalUsers = users.Count,
            TotalProducts = products.Count,
            Users = users.Select(u => new { u.Id, u.Name, u.Email, u.Role })
        });
    }

    [HttpPost("reset-database")]
    public async Task<ActionResult> ResetDatabase()
    {
        _context.CartItems.RemoveRange(_context.CartItems);
        _context.Products.RemoveRange(_context.Products);
        _context.Users.RemoveRange(_context.Users);
        await _context.SaveChangesAsync();

        var testUser = new User
        {
            Name = "Тестовый Пользователь",
            Email = "user@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = "User",
            Balance = 10000
        };

        var workerUser = new User
        {
            Name = "Работник Магазина",
            Email = "worker@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = "Worker",
            Balance = 50000
        };

        await _userRepository.AddAsync(testUser);
        await _userRepository.AddAsync(workerUser);

        var products = new List<Product>
        {
            new() { Name = "Nike Air Max 270", Brand = "Nike", Price = 12999, ImageUrl = "https://example.com/nike1.jpg" },
            new() { Name = "Adidas Ultraboost", Brand = "Adidas", Price = 14999, ImageUrl = "https://example.com/adidas1.jpg" },
            new() { Name = "New Balance 574", Brand = "New Balance", Price = 8999, ImageUrl = "https://example.com/nb1.jpg" },
            new() { Name = "Puma RS-X", Brand = "Puma", Price = 10999, ImageUrl = "https://example.com/puma1.jpg" }
        };

        foreach (var product in products)
        {
            await _productRepository.AddAsync(product);
        }

        return Ok(new
        {
            Message = "База данных сброшена и заполнена тестовыми данными",
            UsersCount = 2,
            ProductsCount = products.Count
        });
    }
}