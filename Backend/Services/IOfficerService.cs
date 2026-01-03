using Backend.DTOs.Grievance;
using Backend.DTOs.Officer;

namespace Backend.Services;

public interface IOfficerService
{
    Task<IEnumerable<GrievanceResponseDto>> GetAssignedGrievancesAsync(int officerId);

    Task<GrievanceResponseDto> GetGrievanceByIdAsync(int grievanceId, int officerId);
    Task UpdateStatusAsync(int grievanceId, int officerId, UpdateGrievanceStatusDto dto);
}
