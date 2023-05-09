-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : localhost
-- Généré le : mar. 09 mai 2023 à 15:13
-- Version du serveur : 8.0.31
-- Version de PHP : 7.4.33

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `appinfo`
--

-- --------------------------------------------------------

--
-- Structure de la table `archive`
--

CREATE TABLE `archive` (
  `id` int NOT NULL,
  `title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `source` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `urlPicture` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `archive`
--

INSERT INTO `archive` (`id`, `title`, `source`, `url`, `urlPicture`) VALUES
(3, 'ANALYSE. L’Italie est en colère et la France n’est pas exemplaire', 'La Voix du Nord', 'https://www.lavoixdunord.fr/1324287/article/2023-05-05/analyse-l-italie-est-en-colere-et-la-france-n-est-pas-exemplaire', 'https://www.bing.com/th?id=OVFT.19WYeFWPrhRcFXJUUoUoUS&pid=News'),
(4, 'Paraguayo Cubas, 3e à la présidentielle, interpellé', 'Le Matin via MSN.com', 'https://www.msn.com/fr-ch/actualite/others/paraguayo-cubas-3e-%C3%A0-la-pr%C3%A9sidentielle-interpell%C3%A9/ar-AA1aNEcW', 'https://www.bing.com/th?id=OVFT.iY2ceFIvidhq-DKADW_koy&pid=News'),
(5, 'Étymologie et signification du prénom Charle', 'Magic Maman', 'https://www.magicmaman.com/prenom/charle,2006200,1190724.asp', 'https://hearhear.org/wp-content/uploads/2019/09/no-image-icon-300x300.png');

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `archive`
--
ALTER TABLE `archive`
  ADD PRIMARY KEY (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
