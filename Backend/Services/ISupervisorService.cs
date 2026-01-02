using Backend.DTOs.Grievance;

namespace Backend.Services;

public interface ISupervisorService
{
    Task<IEnumerable<GrievanceResponseDto>> GetAllGrievances();
    Task<IEnumerable<object>> GetOverdueGrievancesAsync(int days);
    Task EscalateAsync(int grievanceId);
}
