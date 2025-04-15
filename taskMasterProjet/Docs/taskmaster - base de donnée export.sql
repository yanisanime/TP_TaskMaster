-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1
-- Généré le : mar. 15 avr. 2025 à 14:21
-- Version du serveur : 10.4.28-MariaDB
-- Version de PHP : 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `taskmaster`
--

-- --------------------------------------------------------

--
-- Structure de la table `commentaires`
--

CREATE TABLE `commentaires` (
  `Id` int(11) NOT NULL,
  `AuteurId` int(11) DEFAULT NULL,
  `TacheId` int(11) DEFAULT NULL,
  `Date` datetime(6) NOT NULL,
  `Contenu` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `commentaires`
--

INSERT INTO `commentaires` (`Id`, `AuteurId`, `TacheId`, `Date`, `Contenu`) VALUES
(64, 4, 24, '2025-04-15 14:16:22.835170', 'Ce n\'est pas facile à faire LOL  :)'),
(65, 4, 25, '2025-04-15 14:17:27.963663', 'J\'ai pas envie de le faire'),
(66, 6, 26, '2025-04-15 14:19:31.536433', 'Un truc à commenter'),
(67, 6, 26, '2025-04-15 14:19:31.536450', 'ça ne marche pas '),
(68, 6, 27, '2025-04-15 14:20:36.128709', 'Faire un trucc'),
(69, 6, 28, '2025-04-15 14:21:21.796671', 'Il faut le faire vite'),
(70, 6, 28, '2025-04-15 14:21:21.796672', 'A l\'aide'),
(71, 6, 28, '2025-04-15 14:21:21.796673', 'Je ne comprend pas');

-- --------------------------------------------------------

--
-- Structure de la table `etiquettes`
--

CREATE TABLE `etiquettes` (
  `Id` int(11) NOT NULL,
  `Nom` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `etiquettes`
--

INSERT INTO `etiquettes` (`Id`, `Nom`) VALUES
(29, 'urgent'),
(30, 'bug'),
(31, 'amélioration'),
(32, 'backend'),
(33, 'faire'),
(34, 'travail'),
(35, 'fête'),
(36, '45'),
(37, '423'),
(38, '78');

-- --------------------------------------------------------

--
-- Structure de la table `projets`
--

CREATE TABLE `projets` (
  `Id` int(11) NOT NULL,
  `Nom` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `CreateurId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `projets`
--

INSERT INTO `projets` (`Id`, `Nom`, `Description`, `CreateurId`) VALUES
(5, 'Faire un taskMaster', 'Faire un projet pour un gestionnaire de tache', 3),
(6, 'Application mobile', 'Développement de l\'app iOS/Android', 7),
(7, 'Refonte site vitrine	', 'Nouveau design pour le site web', 7),
(8, 'Automatisation des tests', 'Implémentation de tests E2E', 4),
(9, 'Planification événement', 'Organisation du salon Tech2025', 4),
(10, 'Faire du logiciel', 'Je ne sais pas quoi mettee', 6);

-- --------------------------------------------------------

--
-- Structure de la table `soustaches`
--

CREATE TABLE `soustaches` (
  `Id` int(11) NOT NULL,
  `Titre` longtext NOT NULL,
  `Statut` longtext NOT NULL,
  `Echeance` datetime(6) DEFAULT NULL,
  `TacheId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `soustaches`
--

INSERT INTO `soustaches` (`Id`, `Titre`, `Statut`, `Echeance`, `TacheId`) VALUES
(3, 'Ajouter un test de validation côté client', 'À faire', '2025-05-22 00:00:00.000000', 24),
(4, 'Exporter les maquettes en PNG', 'Annulée', '2025-05-16 00:00:00.000000', 25),
(5, '1', 'Terminée', '2025-05-01 00:00:00.000000', 26),
(6, '2', 'Terminée', '2025-04-15 00:00:00.000000', 26),
(7, 'Faire le portfolio', 'Terminée', '2025-04-15 00:00:00.000000', 27),
(8, '789456123', 'En cours', '2025-05-16 00:00:00.000000', 27);

-- --------------------------------------------------------

--
-- Structure de la table `tacheetiquette`
--

CREATE TABLE `tacheetiquette` (
  `EtiquetteId` int(11) NOT NULL,
  `TacheId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `tacheetiquette`
--

INSERT INTO `tacheetiquette` (`EtiquetteId`, `TacheId`) VALUES
(29, 24),
(30, 24),
(31, 24),
(32, 25),
(33, 26),
(34, 26),
(35, 26),
(36, 28),
(37, 28),
(38, 28);

-- --------------------------------------------------------

--
-- Structure de la table `taches`
--

CREATE TABLE `taches` (
  `Id` int(11) NOT NULL,
  `Titre` longtext NOT NULL,
  `Description` longtext DEFAULT NULL,
  `DateCreation` datetime(6) NOT NULL,
  `Echeance` datetime(6) DEFAULT NULL,
  `Statut` longtext NOT NULL,
  `Priorite` longtext NOT NULL,
  `Categorie` longtext NOT NULL,
  `AuteurId` int(11) DEFAULT NULL,
  `RealisateurId` int(11) DEFAULT NULL,
  `ProjetId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `taches`
--

INSERT INTO `taches` (`Id`, `Titre`, `Description`, `DateCreation`, `Echeance`, `Statut`, `Priorite`, `Categorie`, `AuteurId`, `RealisateurId`, `ProjetId`) VALUES
(24, 'Corriger bug de connexion', 'Erreur 500 lors de la tentative de login', '2025-04-15 14:14:39.829607', '2025-06-25 00:00:00.000000', 'En cours', 'Critique', 'Travail', 4, 5, 6),
(25, 'Créer maquettes UI', 'Design des écrans d\'accueil', '2025-04-15 14:16:24.823894', '2026-04-09 00:00:00.000000', 'À faire', 'Moyenne', 'Projet', 4, 4, 7),
(26, 'Écrire tests unitaires', 'Couvrir les services avec xUnit', '2025-04-15 14:18:26.385372', '2025-05-22 00:00:00.000000', 'Annulée', 'Haute', 'Perso', 6, 4, 9),
(27, 'la vie est belle', 'Faire du web', '2025-04-15 14:19:34.124818', NULL, 'Terminée', 'Moyenne', 'Travail', 6, 4, 9),
(28, 'Réserver le lieu', 'Choisir et réserver l’espace de conférence', '2025-04-15 14:20:42.668328', NULL, 'Terminée', 'Haute', 'Travail', 6, 4, 10);

-- --------------------------------------------------------

--
-- Structure de la table `utilisateurs`
--

CREATE TABLE `utilisateurs` (
  `Id` int(11) NOT NULL,
  `Nom` longtext DEFAULT NULL,
  `Prenom` longtext DEFAULT NULL,
  `Email` longtext NOT NULL,
  `MotDePasse` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `utilisateurs`
--

INSERT INTO `utilisateurs` (`Id`, `Nom`, `Prenom`, `Email`, `MotDePasse`) VALUES
(1, 'a', 'a', 'a', 'a'),
(2, 'meriem', 'yanis', 'meriem@meriem.com', '123456'),
(3, 'dragnir', 'Natsu', 'Natsu@gmail.com', 'Natsu'),
(4, 'Merveil', 'Alice', 'alice@mail.com', 'Alice'),
(5, 'Boucher', 'Bob', 'bob@mail.com', 'Bob45'),
(6, 'Chateau', 'Charlie', 'charlie@mail.com', 'Charlie'),
(7, 'Exellance', 'Diana', 'diana@mail.com', 'Diana');

-- --------------------------------------------------------

--
-- Structure de la table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20250410114025_MigrationInitialCreation', '8.0.13'),
('20250410114620_MigrationInitialCreation2', '8.0.13'),
('20250410114743_MigrationInitialCreation3', '8.0.13'),
('20250410183537_MigrationSecondeVersion', '8.0.13'),
('20250413131030_MigrationAddProject', '8.0.13');

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `commentaires`
--
ALTER TABLE `commentaires`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Commentaires_AuteurId` (`AuteurId`),
  ADD KEY `IX_Commentaires_TacheId` (`TacheId`);

--
-- Index pour la table `etiquettes`
--
ALTER TABLE `etiquettes`
  ADD PRIMARY KEY (`Id`);

--
-- Index pour la table `projets`
--
ALTER TABLE `projets`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Projets_CreateurId` (`CreateurId`);

--
-- Index pour la table `soustaches`
--
ALTER TABLE `soustaches`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_SousTaches_TacheId` (`TacheId`);

--
-- Index pour la table `tacheetiquette`
--
ALTER TABLE `tacheetiquette`
  ADD PRIMARY KEY (`EtiquetteId`,`TacheId`),
  ADD KEY `IX_TacheEtiquette_TacheId` (`TacheId`);

--
-- Index pour la table `taches`
--
ALTER TABLE `taches`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Taches_AuteurId` (`AuteurId`),
  ADD KEY `IX_Taches_RealisateurId` (`RealisateurId`),
  ADD KEY `IX_Taches_ProjetId` (`ProjetId`);

--
-- Index pour la table `utilisateurs`
--
ALTER TABLE `utilisateurs`
  ADD PRIMARY KEY (`Id`);

--
-- Index pour la table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `commentaires`
--
ALTER TABLE `commentaires`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=72;

--
-- AUTO_INCREMENT pour la table `etiquettes`
--
ALTER TABLE `etiquettes`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=39;

--
-- AUTO_INCREMENT pour la table `projets`
--
ALTER TABLE `projets`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT pour la table `soustaches`
--
ALTER TABLE `soustaches`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT pour la table `taches`
--
ALTER TABLE `taches`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT pour la table `utilisateurs`
--
ALTER TABLE `utilisateurs`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `commentaires`
--
ALTER TABLE `commentaires`
  ADD CONSTRAINT `FK_Commentaires_Taches_TacheId` FOREIGN KEY (`TacheId`) REFERENCES `taches` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Commentaires_Utilisateurs_AuteurId` FOREIGN KEY (`AuteurId`) REFERENCES `utilisateurs` (`Id`) ON DELETE SET NULL;

--
-- Contraintes pour la table `projets`
--
ALTER TABLE `projets`
  ADD CONSTRAINT `FK_Projets_Utilisateurs_CreateurId` FOREIGN KEY (`CreateurId`) REFERENCES `utilisateurs` (`Id`) ON DELETE SET NULL;

--
-- Contraintes pour la table `soustaches`
--
ALTER TABLE `soustaches`
  ADD CONSTRAINT `FK_SousTaches_Taches_TacheId` FOREIGN KEY (`TacheId`) REFERENCES `taches` (`Id`) ON DELETE CASCADE;

--
-- Contraintes pour la table `tacheetiquette`
--
ALTER TABLE `tacheetiquette`
  ADD CONSTRAINT `FK_TacheEtiquette_Etiquettes_EtiquetteId` FOREIGN KEY (`EtiquetteId`) REFERENCES `etiquettes` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_TacheEtiquette_Taches_TacheId` FOREIGN KEY (`TacheId`) REFERENCES `taches` (`Id`) ON DELETE CASCADE;

--
-- Contraintes pour la table `taches`
--
ALTER TABLE `taches`
  ADD CONSTRAINT `FK_Taches_Projets_ProjetId` FOREIGN KEY (`ProjetId`) REFERENCES `projets` (`Id`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_Taches_Utilisateurs_AuteurId` FOREIGN KEY (`AuteurId`) REFERENCES `utilisateurs` (`Id`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_Taches_Utilisateurs_RealisateurId` FOREIGN KEY (`RealisateurId`) REFERENCES `utilisateurs` (`Id`) ON DELETE SET NULL;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
