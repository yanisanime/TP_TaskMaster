using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace taskMasterProjet.Services;

public class ProjetService
{
    private readonly AppDbContext _context;

    public ProjetService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Projet>> GetUserProjects(int userId)
    {
        return await _context.Projets
            .Where(p => p.CreateurId == userId)
            .ToListAsync();
    }

    public async Task CreateProject(Projet projet)
    {
        _context.Projets.Add(projet);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Projet>> GetAllProjects()
    {
        return await _context.Projets.ToListAsync();
    }
}