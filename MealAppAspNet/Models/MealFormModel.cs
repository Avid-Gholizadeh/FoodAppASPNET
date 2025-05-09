namespace MealAppAspNet.Models;

public class MealFormModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public IFormFile Image { get; set; } = null!;
}