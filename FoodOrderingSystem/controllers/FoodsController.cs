using Microsoft.AspNetCore.Mvc;
using FoodOrderingSystem.DTOs;
using FoodOrderingSystem.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FoodOrderingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly FoodService _foodService;

        public FoodsController(FoodService foodService)
        {
            _foodService = foodService;
        }

        [Authorize(Roles = "Admin")] // Only admins can create food
        [HttpPost("create")]
        public async Task<IActionResult> CreateFood(CreateFoodDTO createFoodDTO)
        {
            try
            {
                var food = await _foodService.CreateFood(createFoodDTO);
                return Ok(new 
                { 
                    message = "Food created successfully!", 
                    food = food 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("all")]
public async Task<IActionResult> GetAllFoods()
{
    try
    {
        var foods = await _foodService.GetAllFoods();
        return Ok(new 
        { 
            message = "All foods retrieved successfully!", 
            foods = foods 
        });
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}

[HttpGet("search")]
public async Task<IActionResult> GetFoodsByName([FromQuery] string name)
{
    try
    {
        var foods = await _foodService.GetFoodsByName(name);
        return Ok(new 
        { 
            message = "Foods retrieved successfully!", 
            foods = foods 
        });
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}
[Authorize(Roles = "Admin")] // Only admins can update food
[HttpPut("{id}")]
public async Task<IActionResult> UpdateFood(int id, UpdateFoodDTO updateFoodDTO)
{
    try
    {
        var food = await _foodService.UpdateFood(id, updateFoodDTO);
        return Ok(new 
        { 
            message = "Food updated successfully!", 
            food = food 
        });
    }
    catch (Exception ex)
    {
        return NotFound(new { message = ex.Message });
    }
}
[Authorize(Roles = "Admin")] // Only admins can delete food
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteFood(int id)
{
    try
    {
        await _foodService.DeleteFood(id);
        return Ok(new { message = "Food deleted successfully!" });
    }
    catch (Exception ex)
    {
        return NotFound(new { message = ex.Message });
    }
}
    }
}