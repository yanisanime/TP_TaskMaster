using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskMasterProjet;

public class AppDbContext : DbContext
{
    public DbSet<Utilisateur> Utilisateurs { get; set; }
    public DbSet<Tache> Taches { get; set; }
    public DbSet<SousTache> SousTaches { get; set; }
    public DbSet<Commentaire> Commentaires { get; set; }
    public DbSet<Etiquette> Etiquettes { get; set; }
    public DbSet<Projet> Projets { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public AppDbContext()
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        var configuration = new ConfigurationBuilder()
    //            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    //            .AddJsonFile("appsettings.json")
    //            .Build();

    //        optionsBuilder
    //            .UseMySql(
    //                configuration.GetConnectionString("DefaultConnection"),
    //                new MySqlServerVersion(new Version(10, 4, 28))
    //            )
    //            .EnableSensitiveDataLogging()
    //            .EnableDetailedErrors();
    //    }
    //}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(
                "server=localhost;port=3306;database=taskmaster;user=root;password=;",
                new MySqlServerVersion(new Version(8, 0, 29))
            );
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tache - Etiquette : many-to-many
        modelBuilder.Entity<Tache>()
            .HasMany(t => t.Etiquettes)
            .WithMany(e => e.Taches)
            .UsingEntity<Dictionary<string, object>>(
                "TacheEtiquette",
                j => j
                    .HasOne<Etiquette>()
                    .WithMany()
                    .HasForeignKey("EtiquetteId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Tache>()
                    .WithMany()
                    .HasForeignKey("TacheId")
                    .OnDelete(DeleteBehavior.Cascade)
            );

        // Utilisateur - Tache (Auteur)
        modelBuilder.Entity<Tache>()
            .HasOne(t => t.Auteur)
            .WithMany(u => u.TachesCreees)
            .HasForeignKey(t => t.AuteurId)
            .OnDelete(DeleteBehavior.SetNull);

        // Utilisateur - Tache (Realisateur)
        modelBuilder.Entity<Tache>()
            .HasOne(t => t.Realisateur)
            .WithMany(u => u.TachesARealiser)
            .HasForeignKey(t => t.RealisateurId)
            .OnDelete(DeleteBehavior.SetNull);

        // Projet - Tache
        modelBuilder.Entity<Tache>()
            .HasOne(t => t.Projet)
            .WithMany(p => p.Taches)
            .HasForeignKey(t => t.ProjetId)
            .OnDelete(DeleteBehavior.Cascade);

        // Utilisateur - Projet
        modelBuilder.Entity<Projet>()
            .HasOne(p => p.Createur)
            .WithMany(u => u.Projets)
            .HasForeignKey(p => p.CreateurId)
            .OnDelete(DeleteBehavior.SetNull);

        // Commentaire - Utilisateur
        modelBuilder.Entity<Commentaire>()
            .HasOne(c => c.Auteur)
            .WithMany(u => u.Commentaires)
            .HasForeignKey(c => c.AuteurId)
            .OnDelete(DeleteBehavior.SetNull);

        // Commentaire - Tache
        modelBuilder.Entity<Commentaire>()
            .HasOne(c => c.Tache)
            .WithMany(t => t.Commentaires)
            .HasForeignKey(c => c.TacheId)
            .OnDelete(DeleteBehavior.Cascade);

        // SousTache - Tache
        modelBuilder.Entity<SousTache>()
            .HasOne(st => st.Tache)
            .WithMany(t => t.SousTaches)
            .HasForeignKey(st => st.TacheId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

