using Backend.DTOs.Grievance;

namespace Backend.Services;

public interface IGrievanceService
{
    Task<GrievanceResponseDto> CreateAsync(int citizenId, CreateGrievanceDto dto);
    Task<IEnumerable<GrievanceResponseDto>> GetMyGrievancesAsync(int citizenId);
}
