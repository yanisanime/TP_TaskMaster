# TaskMaster - Gestionnaire de tâches collaboratif

**TaskMaster** est une application multiplateforme de gestion de tâches, développée en .NET MAUI avec une architecture MVVM et une base de données MySQL via Entity Framework Core.  
Elle permet aux utilisateurs de créer, suivre et organiser leurs tâches, projets, sous-tâches, étiquettes et commentaires, dans une approche collaborative.

---

## Fonctionnalités principales

- Authentification et gestion des utilisateurs
- Création, modification, suppression de tâches
- Attribution de tâches à d'autres utilisateurs
- Gestion des étiquettes, sous-tâches, commentaires
- Statut, priorité, catégorie et échéance des tâches
- Tri et filtrage des tâches
- Association des tâches à des projets

---

## Technologies utilisées

| Outil / Technologie         | Rôle                                           |
|----------------------------|------------------------------------------------|
| .NET MAUI                  | Framework multiplateforme (UI)                |
| C#                         | Langage principal                              |
| MVVM (CommunityToolkit.MVVM) | Architecture logicielle                      |
| Entity Framework Core      | ORM pour la base de données                    |
| MySQL (via Pomelo)         | Base de données relationnelle                  |
| GitHub & Git               | Gestion de version / projet                    |
| Scrum                      | Méthodologie Agile (sprints hebdomadaires)     |

---

## Structure du projet


```
TaskMasterProjet/
├── Models/         # Contient les entités du modèle de données (Tâche, Utilisateur, Projet, etc.)
├── ViewModels/     
├── Views/          
├── Services/       # Gestion des services (CRUD, requêtes complexes, logique métier)
├── App.xaml.cs     # Point d'entrée principal de l'application
├── Resources/      
└── ...
```





---

## Lancer le projet

### Prérequis

- .NET SDK 8+
- Visual Studio 2022 (avec le workload MAUI installé)
- Serveur MySQL local ou distant qui est lancé (important !)

### Étapes

1. Cloner le projet depuis GitHub
2. Exécuter le script SQL de création de la base de données disponible dans `/Docs/taskmaster - base de donnée export.sql`
3. Configurer la chaîne de connexion dans `AppDbContext` et `MauiProgram` et `appsettings.json`
 Ce n'est pas encore parfait, mais à cause du temps on ne s'est pas interessé à réglé ce problem c'est pour ça que la chaine de caractère est à plusieurs endroit. 
4. Ouvrir la solution `.sln` dans Visual Studio
5. Lancer l'application (Windows)

---


## Auteurs du projet

- MERIEM Yanis
- BRUNET Clément
