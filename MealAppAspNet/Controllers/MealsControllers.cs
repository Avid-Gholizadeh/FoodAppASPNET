using MealAppAspNet.Data;
using MealAppAspNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace MealAppAspNet.Controllers;

[Route("[controller]")]
[ApiController]
public class MealsController : ControllerBase
{
    private readonly MealContext _context;
    private readonly IWebHostEnvironment _environment;

    public MealsController(MealContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Meal>>> Get() =>
        await _context.Meals.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Meal>> Get(int id)
    {
        var meal = await _context.Meals.FindAsync(id);
        if (meal == null) return NotFound();
        return meal;
    }

    [HttpPost]
    public async Task<ActionResult<Meal>> Create([FromForm] string name, [FromForm] string description, [FromForm] DateTime date, [FromForm] decimal price, IFormFile image)
    {
        if (image != null)
        {
            if (!image.ContentType.StartsWith("image/jpeg") && !image.ContentType.StartsWith("image/png"))
                return BadRequest(new { message = "Only JPEG or PNG images are allowed" });

            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);
            var fileName = $"{DateTime.UtcNow.Ticks}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var meal = new Meal
            {
                Name = name,
                Description = description,
                Date = date,
                Price = price,
                Image = $"uploads/{fileName}"
            };

            _context.Meals.Add(meal);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = meal.Id }, meal);
        }

        return BadRequest(new { message = "Image is required" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] string name, [FromForm] string description, [FromForm] DateTime date, [FromForm] decimal price, IFormFile? image)
    {
        var meal = await _context.Meals.FindAsync(id);
        if (meal == null) return NotFound();

        if (image != null)
        {
            if (!image.ContentType.StartsWith("image/jpeg") && !image.ContentType.StartsWith("image/png"))
                return BadRequest(new { message = "Only JPEG or PNG images are allowed" });

            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);
            var fileName = $"{DateTime.UtcNow.Ticks}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            meal.Image = $"uploads/{fileName}";
        }

        meal.Name = name;
        meal.Description = description;
        meal.Date = date;
        meal.Price = price;

        await _context.SaveChangesAsync();
        return Ok(meal);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var meal = await _context.Meals.FindAsync(id);
        if (meal == null) return NotFound();

        if (!string.IsNullOrEmpty(meal.Image))
        {
            var filePath = Path.Combine(_environment.WebRootPath, meal.Image.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        _context.Meals.Remove(meal);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Meal deleted" });
    }
}