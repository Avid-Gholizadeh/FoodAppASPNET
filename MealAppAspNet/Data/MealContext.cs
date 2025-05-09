using Microsoft.EntityFrameworkCore;
using MealAppAspNet.Models;

namespace MealAppAspNet.Data;

public class MealContext : DbContext
{
    public DbSet<Meal> Meals { get; set; }

    public MealContext(DbContextOptions<MealContext> options) : base(options) { }
}