using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.Core.Services;
using SneakersShop.Shared.Dtos;

namespace SneakersShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product == null ? NotFound() : Ok(product);
    }

    [Authorize(Roles = "Worker")]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductRequest request)
    {
        var product = await _productService.CreateProductAsync(request);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [Authorize(Roles = "Worker")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}