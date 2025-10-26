using SneakersShop.Core.Entities;
using SneakersShop.Core.Interfaces;
using SneakersShop.Shared.Dtos;

namespace SneakersShop.Core.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<List<CartItemDto>> GetUserCartAsync(int userId)
    {
        var cartItems = await _cartRepository.GetByUserIdAsync(userId);
        return cartItems.Select(ci => new CartItemDto(
            ci.Id,
            new ProductDto(ci.Product.Id, ci.Product.Name, ci.Product.Brand,
                         ci.Product.Description, ci.Product.Price, ci.Product.ImageUrl),
            ci.Quantity
        )).ToList();
    }

    public async Task<CartItemDto> AddToCartAsync(int userId, int productId, int quantity = 1)
    {
        var existingItem = await _cartRepository.GetByUserAndProductAsync(userId, productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            await _cartRepository.UpdateAsync(existingItem);

            var product = await _productRepository.GetByIdAsync(productId);
            return new CartItemDto(existingItem.Id,
                new ProductDto(product!.Id, product.Name, product.Brand,
                             product.Description, product.Price, product.ImageUrl),
                existingItem.Quantity);
        }

        var productEntity = await _productRepository.GetByIdAsync(productId);
        if (productEntity == null)
            throw new Exception("Товар не найден");

        var cartItem = new CartItem
        {
            UserId = userId,
            ProductId = productId,
            Quantity = quantity
        };

        await _cartRepository.AddAsync(cartItem);

        return new CartItemDto(cartItem.Id,
            new ProductDto(productEntity.Id, productEntity.Name, productEntity.Brand,
                         productEntity.Description, productEntity.Price, productEntity.ImageUrl),
            cartItem.Quantity);
    }

    public async Task RemoveFromCartAsync(int userId, int cartItemId)
    {
        var cartItem = await _cartRepository.GetByIdAsync(cartItemId);
        if (cartItem == null || cartItem.UserId != userId)
            throw new Exception("Элемент корзины не найден");

        await _cartRepository.DeleteAsync(cartItem);
    }
}