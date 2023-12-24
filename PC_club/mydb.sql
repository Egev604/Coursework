CREATE DATABASE  IF NOT EXISTS `mydb` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `mydb`;
-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: mydb
-- ------------------------------------------------------
-- Server version	5.7.38

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `clients`
--

DROP TABLE IF EXISTS `clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clients` (
  `id_clients` int(11) NOT NULL AUTO_INCREMENT,
  `surname` varchar(45) NOT NULL,
  `name` varchar(45) NOT NULL,
  `patronymic` varchar(45) DEFAULT NULL,
  `dateofbirth` date NOT NULL,
  `minutes` int(11) NOT NULL,
  PRIMARY KEY (`id_clients`),
  UNIQUE KEY `id_Clients_UNIQUE` (`id_clients`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clients`
--

LOCK TABLES `clients` WRITE;
/*!40000 ALTER TABLE `clients` DISABLE KEYS */;
INSERT INTO `clients` VALUES (2,'Сергеевичч','Дмитрий','Смирнов','2023-02-01',461),(3,'Иванов','Иван','Иванович','2023-02-14',50);
/*!40000 ALTER TABLE `clients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employees`
--

DROP TABLE IF EXISTS `employees`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employees` (
  `id_employees` int(11) NOT NULL AUTO_INCREMENT,
  `position` varchar(45) NOT NULL,
  `surname` varchar(45) NOT NULL,
  `name` varchar(45) NOT NULL,
  `patronymic` varchar(45) DEFAULT NULL,
  `dateofbirth` date NOT NULL,
  `phone` int(11) NOT NULL,
  PRIMARY KEY (`id_employees`),
  UNIQUE KEY `id_employees_UNIQUE` (`id_employees`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employees`
--

LOCK TABLES `employees` WRITE;
/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
INSERT INTO `employees` VALUES (2,'пользователь','Иванов','Иван','Иванович','2023-02-07',845964586),(3,'пользователь','Смирновв','Иван','Иванович','2000-02-02',583493478);
/*!40000 ALTER TABLE `employees` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pc`
--

DROP TABLE IF EXISTS `pc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pc` (
  `id_pc` int(11) NOT NULL AUTO_INCREMENT,
  `number_pc` int(11) NOT NULL,
  `lastdate` datetime NOT NULL,
  `cost` tinyint(1) NOT NULL,
  `id_rate` int(11) NOT NULL,
  PRIMARY KEY (`id_pc`),
  UNIQUE KEY `id_PC_UNIQUE` (`id_pc`),
  UNIQUE KEY `number_pc_UNIQUE` (`number_pc`),
  KEY `w_idx` (`id_rate`),
  CONSTRAINT `w` FOREIGN KEY (`id_rate`) REFERENCES `rate` (`id_rate`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pc`
--

LOCK TABLES `pc` WRITE;
/*!40000 ALTER TABLE `pc` DISABLE KEYS */;
INSERT INTO `pc` VALUES (5,1,'2023-02-14 00:00:00',1,3),(6,2,'2023-02-14 00:00:00',0,2);
/*!40000 ALTER TABLE `pc` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rate`
--

DROP TABLE IF EXISTS `rate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rate` (
  `id_rate` int(11) NOT NULL AUTO_INCREMENT,
  `name_rate` varchar(45) NOT NULL,
  `price_onemin` double NOT NULL,
  PRIMARY KEY (`id_rate`),
  UNIQUE KEY `id_rate_UNIQUE` (`id_rate`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rate`
--

LOCK TABLES `rate` WRITE;
/*!40000 ALTER TABLE `rate` DISABLE KEYS */;
INSERT INTO `rate` VALUES (2,'VIP',2.4),(3,'zxc',2);
/*!40000 ALTER TABLE `rate` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rent`
--

DROP TABLE IF EXISTS `rent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rent` (
  `id_rent` int(11) NOT NULL AUTO_INCREMENT,
  `idclients` int(11) NOT NULL,
  `out_data_rent` varchar(45) NOT NULL,
  `time` varchar(45) NOT NULL,
  `idpc` int(11) NOT NULL,
  `idemployees` int(11) NOT NULL,
  `price` decimal(10,0) NOT NULL,
  PRIMARY KEY (`id_rent`),
  UNIQUE KEY `id_rent_UNIQUE` (`id_rent`),
  KEY `clients_idx` (`idclients`),
  KEY `empl_idx` (`idemployees`),
  KEY `pc_idx` (`idpc`),
  CONSTRAINT `clients` FOREIGN KEY (`idclients`) REFERENCES `clients` (`id_clients`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `empl` FOREIGN KEY (`idemployees`) REFERENCES `employees` (`id_employees`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `pc` FOREIGN KEY (`idpc`) REFERENCES `pc` (`id_pc`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rent`
--

LOCK TABLES `rent` WRITE;
/*!40000 ALTER TABLE `rent` DISABLE KEYS */;
INSERT INTO `rent` VALUES (22,2,'21.04.2023 17:14:57','50',5,2,100);
/*!40000 ALTER TABLE `rent` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `save_rent`
--

DROP TABLE IF EXISTS `save_rent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `save_rent` (
  `id_save_rent` int(11) NOT NULL AUTO_INCREMENT,
  `client` varchar(200) NOT NULL,
  `out_data_rent` varchar(45) NOT NULL,
  `time` varchar(45) NOT NULL,
  `number_pc` int(11) NOT NULL,
  `staff` varchar(200) NOT NULL,
  `price` decimal(10,0) NOT NULL,
  `status` varchar(45) DEFAULT NULL,
  `id_rent` int(11) NOT NULL,
  PRIMARY KEY (`id_save_rent`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `save_rent`
--

LOCK TABLES `save_rent` WRITE;
/*!40000 ALTER TABLE `save_rent` DISABLE KEYS */;
INSERT INTO `save_rent` VALUES (1,'Сергеевичч','21.04.2023 16:15:54','40',2,'Смирновв',96,'Удален',15),(2,'Сергеевичч','21.04.2023 16:33:47','50',1,'Смирновв',100,'Удален',16),(3,'Иванов','21.04.2023 16:54:18','50',2,'Иванов',120,'Удален',17),(4,'Сергеевичч','21.04.2023 16:34:58','30',1,'Смирновв',60,'Удален',18),(5,'Сергеевичч','21.04.2023 16:47:00','40',1,'Смирновв',80,'Удален',19),(6,'Сергеевичч','21.04.2023 16:47:59','40',2,'Смирновв',96,'Удален',20),(7,'Сергеевичч','21.04.2023 16:11:14','1',2,'Иванов',2,'Оплачен',21),(8,'Сергеевичч','21.04.2023 17:14:57','50',1,'Иванов',100,'Оплачен',22);
/*!40000 ALTER TABLE `save_rent` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id_users` int(11) NOT NULL AUTO_INCREMENT,
  `login` varchar(45) NOT NULL,
  `passwd` varchar(100) NOT NULL,
  `user_name` varchar(45) NOT NULL,
  `data_create` date NOT NULL,
  `role` varchar(45) NOT NULL,
  PRIMARY KEY (`id_users`),
  UNIQUE KEY `id_users_UNIQUE` (`id_users`),
  UNIQUE KEY `login_UNIQUE` (`login`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin','admin','admin','2023-02-07','admin'),(2,'user','user','user','2023-02-07','user');
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

-- Dump completed on 2023-04-21 18:25:29
