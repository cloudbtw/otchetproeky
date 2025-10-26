namespace SneakersShop.Shared.Dtos;

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Name, string Email, string Password);
public record AuthResponse(string Token, UserDto User);

public record UserDto(int Id, string Name, string Email, string Role, decimal Balance);
public record ProductDto(int Id, string Name, string Brand, string? Description, decimal Price, string ImageUrl);
public record CartItemDto(int Id, ProductDto Product, int Quantity);

public record CreateProductRequest(string Name, string Brand, string? Description, decimal Price, string ImageUrl);