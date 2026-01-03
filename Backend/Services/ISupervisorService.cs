using Backend.DTOs.Grievance;

namespace Backend.Services;

public interface ISupervisorService
{
    Task<IEnumerable<GrievanceResponseDto>> GetAllGrievances();

    Task<GrievanceResponseDto> GetGrievanceByIdAsync(int grievanceId);
    Task<IEnumerable<object>> GetOverdueGrievancesAsync(int days);
    Task EscalateAsync(int grievanceId);

}
