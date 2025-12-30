namespace Backend.Services;

public interface ISupervisorService
{
    Task<IEnumerable<object>> GetOverdueGrievancesAsync(int days);
    Task EscalateAsync(int grievanceId);
}
