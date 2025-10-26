using SneakersShop.Core.Entities;
using SneakersShop.Core.Interfaces;
using SneakersShop.Shared.Dtos;

namespace SneakersShop.Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(p => new ProductDto(
            p.Id, p.Name, p.Brand, p.Description, p.Price, p.ImageUrl
        )).ToList();
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product == null ? null : new ProductDto(
            product.Id, product.Name, product.Brand, product.Description, product.Price, product.ImageUrl
        );
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Brand = request.Brand,
            Description = request.Description,
            Price = request.Price,
            ImageUrl = request.ImageUrl
        };

        await _productRepository.AddAsync(product);
        return new ProductDto(product.Id, product.Name, product.Brand, product.Description, product.Price, product.ImageUrl);
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
            await _productRepository.DeleteAsync(product);
    }
}