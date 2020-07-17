-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jul 17, 2020 at 10:50 PM
-- Server version: 10.4.13-MariaDB
-- PHP Version: 7.4.8

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `discordswearbot`
--

-- --------------------------------------------------------

--
-- Table structure for table `dictionary`
--

CREATE TABLE `dictionary` (
  `text` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `dictionary`
--

INSERT INTO `dictionary` (`text`) VALUES
('fuck'),
('shit'),
('piss'),
('nigger'),
('motherfucker'),
('ass'),
('tits'),
('titties'),
('cunt'),
('faggot'),
('wetback'),
('gay'),
('bitch'),
('dick'),
('asshole'),
('fgt'),
('fucking'),
('hell'),
('dipshit'),
('niggers'),
('goddamn'),
('fucker');

-- --------------------------------------------------------

--
-- Table structure for table `server_stats`
--

CREATE TABLE `server_stats` (
  `name` varchar(256) NOT NULL,
  `value` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `server_stats`
--

INSERT INTO `server_stats` (`name`, `value`) VALUES
('total', 2695);

-- --------------------------------------------------------

--
-- Table structure for table `user_stats`
--

CREATE TABLE `user_stats` (
  `userid` varchar(64) NOT NULL,
  `count` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
