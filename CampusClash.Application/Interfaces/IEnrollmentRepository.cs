using CampusClash.Domain.Entities;

namespace CampusClash.Application.Interfaces;

public interface IEnrollmentRepository
{
    Task<bool> IsUserInActiveTournamentAsync(Guid userId);
    
    Task<Enrollment?> GetByUserAndTournamentAsync(Guid userId, Guid tournamentId); // Para verificar si el usuario ya está inscrito en el torneo
    void Remove(Enrollment enrollment); // Para eliminar la inscripción

    Task AddAsync(Enrollment enrollment);
    Task SaveChangesAsync();
}
