using SneakersShop.Shared.Dtos;

namespace SneakersShop.Core.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductRequest request);
    Task DeleteProductAsync(int id);
}

public interface ICartService
{
    Task<List<CartItemDto>> GetUserCartAsync(int userId);
    Task<CartItemDto> AddToCartAsync(int userId, int productId, int quantity = 1);
    Task RemoveFromCartAsync(int userId, int cartItemId);
}