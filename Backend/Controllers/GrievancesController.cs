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
    [Authorize(Roles ="Citizen")]
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
    }
}
