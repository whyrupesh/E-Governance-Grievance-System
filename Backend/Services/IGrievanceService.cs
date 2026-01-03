using Backend.DTOs.Admin;
using Backend.DTOs.Grievance;

namespace Backend.Services;

public interface IGrievanceService
{
    Task<GrievanceResponseDto> CreateAsync(int citizenId, CreateGrievanceDto dto);
    Task<IEnumerable<GrievanceResponseDto>> GetMyGrievancesAsync(int citizenId);
    Task<GrievanceResponseDto?> GetMyGrievanceByIdAsync(int grievanceId, int citizenId);
    Task<object> DeleteMyGrievanceAsync(int grievanceId, int citizenId);
    Task<IEnumerable<CategoryResponseDto>> GetAllCategories();
}
