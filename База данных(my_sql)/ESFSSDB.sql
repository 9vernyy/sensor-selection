CREATE DATABASE  IF NOT EXISTS `esfssdb` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `esfssdb`;
-- MySQL dump 10.13  Distrib 5.7.9, for Win64 (x86_64)
--
-- Host: localhost    Database: esfssdb
-- ------------------------------------------------------
-- Server version	5.7.11-enterprise-commercial-advanced-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `producers`
--

DROP TABLE IF EXISTS `producers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `producers` (
  `prodId` int(11) NOT NULL AUTO_INCREMENT,
  `prodName` varchar(45) NOT NULL,
  `prodLocation` varchar(45) NOT NULL,
  `prodEmail` varchar(45) NOT NULL,
  `prodLogin` varchar(45) NOT NULL,
  `prodPass` varchar(45) NOT NULL,
  PRIMARY KEY (`prodId`),
  UNIQUE KEY `prodId_UNIQUE` (`prodId`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `producers`
--

LOCK TABLES `producers` WRITE;
/*!40000 ALTER TABLE `producers` DISABLE KEYS */;
/*!40000 ALTER TABLE `producers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sensors`
--

DROP TABLE IF EXISTS `sensors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sensors` (
  `sensId` int(11) NOT NULL AUTO_INCREMENT,
  `prodId` int(11) NOT NULL,
  `sensName` varchar(45) NOT NULL,
  `sensCateg` varchar(45) NOT NULL,
  `sensType` varchar(45) NOT NULL,
  `measRange` varchar(45) NOT NULL,
  `outSignal` varchar(45) NOT NULL,
  `accuracy` varchar(45) NOT NULL,
  `bodyMat` varchar(45) NOT NULL,
  `applying` varchar(45) NOT NULL,
  `cost` varchar(45) NOT NULL,
  `quality` varchar(45) NOT NULL,
  `prodName` varchar(45) NOT NULL,
  `additInf` longtext,
  `curr` varchar(45) NOT NULL,
  `measRangeSym` varchar(45) NOT NULL,
  `outSignalSym` varchar(45) NOT NULL,
  `sensClass` varchar(45) NOT NULL,
  `image` longblob,
  `date` varchar(45) NOT NULL,
  PRIMARY KEY (`sensId`),
  UNIQUE KEY `sensId_UNIQUE` (`sensId`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sensors`
--

LOCK TABLES `sensors` WRITE;
/*!40000 ALTER TABLE `sensors` DISABLE KEYS */;
/*!40000 ALTER TABLE `sensors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `userId` int(11) NOT NULL AUTO_INCREMENT,
  `userName` varchar(45) NOT NULL,
  `userSurname` varchar(45) NOT NULL,
  `userEmail` varchar(45) NOT NULL,
  `userLogin` varchar(45) NOT NULL,
  `userPass` varchar(45) NOT NULL,
  PRIMARY KEY (`userId`),
  UNIQUE KEY `userId_UNIQUE` (`userId`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-05-20  1:39:50
