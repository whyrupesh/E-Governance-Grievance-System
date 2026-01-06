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

        [HttpPost("{id}/feedback")]
        public async Task<IActionResult> AddFeedback(int id, AddFeedbackDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _grievanceService.AddFeedbackAsync(id, userId, dto);
            return Ok(new { message = "Feedback submitted successfully" });
        }

        [HttpPost("{id}/reopen")]
        public async Task<IActionResult> ReopenGrievance(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _grievanceService.ReopenGrievanceAsync(id, userId);
            return Ok(new { message = "Grievance reopened successfully" });
        }

        [HttpPost("{id}/escalate")]
        public async Task<IActionResult> EscalateGrievance(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _grievanceService.EscalateGrievanceAsync(id, userId);
            return Ok(new { message = "Grievance escalated successfully" });
        }
    }
}
