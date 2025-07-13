using Microsoft.AspNetCore.Mvc;
using tdd_productRestApi.data;
using tdd_productRestApi.models;

namespace tdd_productRestApi.controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    public static readonly Dictionary<string, Product> Products = new();

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        await product.CreateAsync(_context);

        return Created("/", product);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await Product.FindByIdAsync(_context, id);
        return Ok(product);
    }
}