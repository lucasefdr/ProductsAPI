namespace ProductsAPI.DTOs;

public class CategoryDTO
{
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<ProductDTO>? Products { get; set; }
}
