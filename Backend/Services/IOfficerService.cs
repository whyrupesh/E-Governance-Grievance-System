using Backend.DTOs.Officer;

namespace Backend.Services;

public interface IOfficerService
{
    Task<IEnumerable<object>> GetAssignedGrievancesAsync(int officerId);
    Task UpdateStatusAsync(int grievanceId, int officerId, UpdateGrievanceStatusDto dto);
}
