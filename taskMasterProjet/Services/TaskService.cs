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
        //await Shell.Current.DisplayAlert("Debug", $"Chargement des tâches pour l'utilisateur :  '{userId}'.", "OK");

        // return await _context.Taches
        //.Where(t => t.AuteurId == userId || t.RealisateurId == userId)
        //.ToListAsync();
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
        if (task == null)
        {
            // Gérer le cas où la tâche n'existe pas
            return;
        }
        //Créé une invite de dialogue pour debug
        //await Shell.Current.DisplayAlert("Debug", $"Suppression de la tâche :  '{task.Description}'.", "OK");
        if (task != null)
        {
            _context.Taches.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    public async Task CreateTask(Tache task)
    {
        _context.Taches.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Utilisateur>> GetTeamMembers(int userId)
    {
        return await _context.Utilisateurs
            .Where(u => u.Id != userId) // Exclure l'utilisateur courant
            .ToListAsync();
    }

    public async Task UpdateTask(Tache task)
    {
        if (task == null)
        {
            
            return;
        }
        _context.Taches.Update(task);
        await _context.SaveChangesAsync();
    }

    //fonction pour avoir une tache par son id
    public async Task<Tache> GetTaskById(int taskId)
    {
        // Je fais ça pour géré le cas ou il y a un problème de null
        var task = await _context.Taches
            .Include(t => t.Auteur)
            .Include(t => t.Realisateur)
            .Include(t => t.SousTaches)
            .Include(t => t.Commentaires)
            .Include(t => t.Etiquettes)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task == null)
        {
            return null; 
        }

        return task;
    }

    //Fonction pour supprimer tous les commentaire de la base qui ont un TasheId = NULL
    public async Task DeleteAllCommentaireByTacheIdNull()
    {
        var commentaires = await _context.Commentaires
            .Where(c => c.TacheId == null)
            .ToListAsync();
        await Shell.Current.DisplayAlert("Debug", $"Nombre de commentaires supprimer à cause de null : {commentaires.Count}", "OK");
        _context.Commentaires.RemoveRange(commentaires);
        await _context.SaveChangesAsync();
    }

}