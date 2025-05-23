﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using taskMasterProjet;

#nullable disable

namespace taskMasterProjet.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("TacheEtiquette", b =>
                {
                    b.Property<int>("EtiquetteId")
                        .HasColumnType("int");

                    b.Property<int>("TacheId")
                        .HasColumnType("int");

                    b.HasKey("EtiquetteId", "TacheId");

                    b.HasIndex("TacheId");

                    b.ToTable("TacheEtiquette");
                });

            modelBuilder.Entity("taskMasterProjet.Commentaire", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AuteurId")
                        .HasColumnType("int");

                    b.Property<string>("Contenu")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("TacheId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuteurId");

                    b.HasIndex("TacheId");

                    b.ToTable("Commentaires");
                });

            modelBuilder.Entity("taskMasterProjet.Etiquette", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Etiquettes");
                });

            modelBuilder.Entity("taskMasterProjet.Projet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreateurId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Nom")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CreateurId");

                    b.ToTable("Projets");
                });

            modelBuilder.Entity("taskMasterProjet.SousTache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Echeance")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Statut")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("TacheId")
                        .HasColumnType("int");

                    b.Property<string>("Titre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("TacheId");

                    b.ToTable("SousTaches");
                });

            modelBuilder.Entity("taskMasterProjet.Tache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AuteurId")
                        .HasColumnType("int");

                    b.Property<string>("Categorie")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreation")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Echeance")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Priorite")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("ProjetId")
                        .HasColumnType("int");

                    b.Property<int?>("RealisateurId")
                        .HasColumnType("int");

                    b.Property<string>("Statut")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Titre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AuteurId");

                    b.HasIndex("ProjetId");

                    b.HasIndex("RealisateurId");

                    b.ToTable("Taches");
                });

            modelBuilder.Entity("taskMasterProjet.Utilisateur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MotDePasse")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nom")
                        .HasColumnType("longtext");

                    b.Property<string>("Prenom")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Utilisateurs");
                });

            modelBuilder.Entity("TacheEtiquette", b =>
                {
                    b.HasOne("taskMasterProjet.Etiquette", null)
                        .WithMany()
                        .HasForeignKey("EtiquetteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("taskMasterProjet.Tache", null)
                        .WithMany()
                        .HasForeignKey("TacheId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("taskMasterProjet.Commentaire", b =>
                {
                    b.HasOne("taskMasterProjet.Utilisateur", "Auteur")
                        .WithMany("Commentaires")
                        .HasForeignKey("AuteurId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("taskMasterProjet.Tache", "Tache")
                        .WithMany("Commentaires")
                        .HasForeignKey("TacheId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Auteur");

                    b.Navigation("Tache");
                });

            modelBuilder.Entity("taskMasterProjet.Projet", b =>
                {
                    b.HasOne("taskMasterProjet.Utilisateur", "Createur")
                        .WithMany()
                        .HasForeignKey("CreateurId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Createur");
                });

            modelBuilder.Entity("taskMasterProjet.SousTache", b =>
                {
                    b.HasOne("taskMasterProjet.Tache", "Tache")
                        .WithMany("SousTaches")
                        .HasForeignKey("TacheId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Tache");
                });

            modelBuilder.Entity("taskMasterProjet.Tache", b =>
                {
                    b.HasOne("taskMasterProjet.Utilisateur", "Auteur")
                        .WithMany("TachesCreees")
                        .HasForeignKey("AuteurId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("taskMasterProjet.Projet", "Projet")
                        .WithMany("Taches")
                        .HasForeignKey("ProjetId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("taskMasterProjet.Utilisateur", "Realisateur")
                        .WithMany("TachesARealiser")
                        .HasForeignKey("RealisateurId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Auteur");

                    b.Navigation("Projet");

                    b.Navigation("Realisateur");
                });

            modelBuilder.Entity("taskMasterProjet.Projet", b =>
                {
                    b.Navigation("Taches");
                });

            modelBuilder.Entity("taskMasterProjet.Tache", b =>
                {
                    b.Navigation("Commentaires");

                    b.Navigation("SousTaches");
                });

            modelBuilder.Entity("taskMasterProjet.Utilisateur", b =>
                {
                    b.Navigation("Commentaires");

                    b.Navigation("TachesARealiser");

                    b.Navigation("TachesCreees");
                });
#pragma warning restore 612, 618
        }
    }
}
