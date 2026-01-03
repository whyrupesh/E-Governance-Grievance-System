using System.Security.Claims;
using Backend.DTOs.Grievance;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/grievances")]
    [ApiController]
    [Authorize(Roles = "Citizen")]
    public class GrievancesController : ControllerBase
    {
        private readonly IGrievanceService _grievanceService;

        public GrievancesController(IGrievanceService grievanceService)
        {
            _grievanceService = grievanceService;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Grievance api is working!");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGrievanceDto dto)
        {
            var citizenId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var result = await _grievanceService.CreateAsync(citizenId, dto);
            return Ok(result);
        }


        [HttpGet("my")]
        public async Task<IActionResult> MyGrievances()
        {
            var citizenId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var result = await _grievanceService.GetMyGrievancesAsync(citizenId);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMyGrievanceById(int id)
        {
            int citizenId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var grievance = await _grievanceService.GetMyGrievanceByIdAsync(id, citizenId);

            if (grievance == null)
                return NotFound(new { message = "Grievance not found" });

            return Ok(grievance);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMyGrievance(int id)
        {
            int citizenId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _grievanceService.DeleteMyGrievanceAsync(id, citizenId);

            return Ok(result);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> AllCategories()
        {

            var result = await _grievanceService.GetAllCategories();

            return Ok(result);
        }
    }
}
