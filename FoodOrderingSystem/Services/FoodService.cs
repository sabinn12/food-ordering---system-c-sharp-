using System;
using System.Threading.Tasks;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.DTOs;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet.Actions;

namespace FoodOrderingSystem.Services
{
    public class FoodService
    {
        private readonly AppDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public FoodService(AppDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Food> CreateFood(CreateFoodDTO createFoodDTO)
        {
            string? imageUrl = null;
            if (createFoodDTO.Image != null)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(createFoodDTO.Image);
                imageUrl = uploadResult.SecureUrl.ToString();
            }

            var food = new Food
            {
                Name = createFoodDTO.Name,
                Description = createFoodDTO.Description,
                Price = createFoodDTO.Price,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.Foods.Add(food);
            await _context.SaveChangesAsync();

            return food;
        }
        public async Task<List<Food>> GetAllFoods()
{
    return await _context.Foods.ToListAsync();
}

public async Task<List<Food>> GetFoodsByName(string name)
{
    if (string.IsNullOrEmpty(name))
    {
        // If the name is null or empty, return all foods
        return await _context.Foods.ToListAsync();
    }

    return await _context.Foods
        .Where(f => f.Name != null && f.Name.ToLower().Contains(name.ToLower()))
        .ToListAsync();
}
public async Task<Food> UpdateFood(int id, UpdateFoodDTO updateFoodDTO)
{
    var food = await _context.Foods.FindAsync(id);
    if (food == null)
    {
        throw new Exception("Food not found.");
    }

    // Update only the fields that are provided in the DTO
    if (updateFoodDTO.Name != null)
    {
        food.Name = updateFoodDTO.Name;
    }
    if (updateFoodDTO.Description != null)
    {
        food.Description = updateFoodDTO.Description;
    }
    if (updateFoodDTO.Price.HasValue)
    {
        food.Price = updateFoodDTO.Price.Value;
    }
    if (updateFoodDTO.ImageUrl != null) 
    {
        food.ImageUrl = updateFoodDTO.ImageUrl;
    }

    _context.Foods.Update(food);
    await _context.SaveChangesAsync();

    return food;
}
public async Task DeleteFood(int id)
{
    var food = await _context.Foods.FindAsync(id);
    if (food == null)
    {
        throw new Exception("Food not found.");
    }

    _context.Foods.Remove(food);
    await _context.SaveChangesAsync();
}
    }
}