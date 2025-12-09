using Microservices.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController(ManagementDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var all = await dbContext.Pets.Include(p => p.Breed).ToListAsync();
            return Ok(all);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pet = await dbContext.Pets.Include(p => p.Breed).Where(p  => p.Id == id).FirstOrDefaultAsync();
            return Ok(pet);
        }
    }
}
