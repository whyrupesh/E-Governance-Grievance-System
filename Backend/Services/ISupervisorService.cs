using Backend.DTOs.Grievance;

namespace Backend.Services;

public interface ISupervisorService
{
    Task<IEnumerable<GrievanceResponseDto>> GetAllGrievances();

    Task<GrievanceResponseDto> GetGrievanceByIdAsync(int grievanceId);
    Task<IEnumerable<object>> GetOverdueGrievancesAsync(int days);
    Task EscalateAsync(int grievanceId);

    Task<GrievanceResponseDto> ChangeGrievanceStatusAsync(int grievanceId, string status);

    Task<GrievanceResponseDto> GiveResolutionRemarksAsync(int grievanceId, string remarks);
    
    Task<IEnumerable<object>> GetOfficersByDepartmentAsync(int DepartmentId);

    Task<GrievanceResponseDto> AssignGrievanceAsync(int grievanceId, int staffId);

}
