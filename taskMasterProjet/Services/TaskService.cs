using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace taskMasterProjet.Services;

public class TaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tache>> GetUserTasks(int userId)
    {
        return await _context.Taches
            .Include(t => t.Auteur)
            .Include(t => t.Realisateur)
            .Include(t => t.SousTaches)
            .Where(t => t.AuteurId == userId || t.RealisateurId == userId)
            .OrderByDescending(t => t.Priorite)
            .ThenBy(t => t.Echeance)
            .ToListAsync();
    }

    public async Task DeleteTask(int taskId)
    {
        var task = await _context.Taches.FindAsync(taskId);
        if (task != null)
        {
            _context.Taches.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    // Ajouter les autres méthodes CRUD ici...
}