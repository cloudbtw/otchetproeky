using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.Core.Interfaces;
using SneakersShop.Core.Services;
using SneakersShop.Shared.Dtos;
using System.Security.Claims;

namespace SneakersShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpGet]
    public async Task<ActionResult<List<CartItemDto>>> GetCart()
    {
        var cart = await _cartService.GetUserCartAsync(GetUserId());
        return Ok(cart);
    }

    [HttpPost]
    public async Task<ActionResult<CartItemDto>> AddToCart([FromBody] AddToCartRequest request)
    {
        var cartItem = await _cartService.AddToCartAsync(GetUserId(), request.ProductId, request.Quantity);
        return Ok(cartItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        await _cartService.RemoveFromCartAsync(GetUserId(), id);
        return NoContent();
    }
}

public record AddToCartRequest(int ProductId, int Quantity = 1);