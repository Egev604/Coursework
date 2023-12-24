-- MySqlBackup.NET 2.3.6
-- Dump Time: 2023-05-21 16:11:16
-- --------------------------------------
-- Server version 5.7.38 MySQL Community Server (GPL)


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of employees
-- 

DROP TABLE IF EXISTS `employees`;
CREATE TABLE IF NOT EXISTS `employees` (
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

-- 
-- Dumping data for table employees
-- 

/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
INSERT INTO `employees`(`id_employees`,`position`,`surname`,`name`,`patronymic`,`dateofbirth`,`phone`) VALUES(2,'пользователь','Иванов','Иван','Иванович','2023-02-07 00:00:00',845964586),(3,'пользователь','Смирновв','Иван','Иванович','2000-02-02 00:00:00',583493478);
/*!40000 ALTER TABLE `employees` ENABLE KEYS */;

-- 
-- Definition of rate
-- 

DROP TABLE IF EXISTS `rate`;
CREATE TABLE IF NOT EXISTS `rate` (
  `id_rate` int(11) NOT NULL AUTO_INCREMENT,
  `name_rate` varchar(45) NOT NULL,
  `price_onemin` double NOT NULL,
  PRIMARY KEY (`id_rate`),
  UNIQUE KEY `id_rate_UNIQUE` (`id_rate`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table rate
-- 

/*!40000 ALTER TABLE `rate` DISABLE KEYS */;
INSERT INTO `rate`(`id_rate`,`name_rate`,`price_onemin`) VALUES(2,'VIP',2.4),(3,'zxc',2),(4,'Обычный',1);
/*!40000 ALTER TABLE `rate` ENABLE KEYS */;

-- 
-- Definition of pc
-- 

DROP TABLE IF EXISTS `pc`;
CREATE TABLE IF NOT EXISTS `pc` (
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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table pc
-- 

/*!40000 ALTER TABLE `pc` DISABLE KEYS */;
INSERT INTO `pc`(`id_pc`,`number_pc`,`lastdate`,`cost`,`id_rate`) VALUES(5,1,'2023-02-14 00:00:00',0,3),(6,2,'2023-02-14 00:00:00',0,2),(7,3,'2023-05-10 00:00:00',1,4);
/*!40000 ALTER TABLE `pc` ENABLE KEYS */;

-- 
-- Definition of rent
-- 

DROP TABLE IF EXISTS `rent`;
CREATE TABLE IF NOT EXISTS `rent` (
  `id_rent` int(11) NOT NULL AUTO_INCREMENT,
  `idclients` int(11) NOT NULL,
  `out_data_rent` varchar(45) DEFAULT NULL,
  `time` varchar(45) DEFAULT NULL,
  `idpc` int(11) NOT NULL,
  `idemployees` int(11) NOT NULL,
  `price` decimal(10,0) DEFAULT NULL,
  PRIMARY KEY (`id_rent`),
  UNIQUE KEY `id_rent_UNIQUE` (`id_rent`),
  KEY `clients_idx` (`idclients`),
  KEY `empl_idx` (`idemployees`),
  KEY `pc_idx` (`idpc`),
  CONSTRAINT `clients` FOREIGN KEY (`idclients`) REFERENCES `clients` (`id_clients`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `empl` FOREIGN KEY (`idemployees`) REFERENCES `employees` (`id_employees`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `pc` FOREIGN KEY (`idpc`) REFERENCES `pc` (`id_pc`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table rent
-- 

/*!40000 ALTER TABLE `rent` DISABLE KEYS */;
INSERT INTO `rent`(`id_rent`,`idclients`,`out_data_rent`,`time`,`idpc`,`idemployees`,`price`) VALUES(55,10,'21.05.2023 16:36:11','50',7,2,90);
/*!40000 ALTER TABLE `rent` ENABLE KEYS */;

-- 
-- Definition of sale_minuts
-- 

DROP TABLE IF EXISTS `sale_minuts`;
CREATE TABLE IF NOT EXISTS `sale_minuts` (
  `id_sale` int(11) NOT NULL AUTO_INCREMENT,
  `minuts` int(11) NOT NULL,
  `sale` int(11) NOT NULL,
  PRIMARY KEY (`id_sale`),
  UNIQUE KEY `id_sale_UNIQUE` (`id_sale`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table sale_minuts
-- 

/*!40000 ALTER TABLE `sale_minuts` DISABLE KEYS */;
INSERT INTO `sale_minuts`(`id_sale`,`minuts`,`sale`) VALUES(1,0,0),(28,1000,10);
/*!40000 ALTER TABLE `sale_minuts` ENABLE KEYS */;

-- 
-- Definition of clients
-- 

DROP TABLE IF EXISTS `clients`;
CREATE TABLE IF NOT EXISTS `clients` (
  `id_clients` int(11) NOT NULL AUTO_INCREMENT,
  `surname` varchar(45) NOT NULL,
  `name` varchar(45) NOT NULL,
  `patronymic` varchar(45) DEFAULT NULL,
  `dateofbirth` date NOT NULL,
  `minutes` int(11) NOT NULL,
  `idsale` int(11) DEFAULT '1',
  PRIMARY KEY (`id_clients`),
  UNIQUE KEY `id_Clients_UNIQUE` (`id_clients`),
  KEY `dsa_idx` (`idsale`),
  CONSTRAINT `dsa` FOREIGN KEY (`idsale`) REFERENCES `sale_minuts` (`id_sale`) ON DELETE SET NULL ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table clients
-- 

/*!40000 ALTER TABLE `clients` DISABLE KEYS */;
INSERT INTO `clients`(`id_clients`,`surname`,`name`,`patronymic`,`dateofbirth`,`minutes`,`idsale`) VALUES(10,'Лебедев','Артем','Сергеевич','2023-05-09 00:00:00',2541,28),(11,'Перистов','Дмитрий','Сергеевич','2023-05-09 00:00:00',1241,28);
/*!40000 ALTER TABLE `clients` ENABLE KEYS */;

-- 
-- Definition of save_rent
-- 

DROP TABLE IF EXISTS `save_rent`;
CREATE TABLE IF NOT EXISTS `save_rent` (
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
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table save_rent
-- 

/*!40000 ALTER TABLE `save_rent` DISABLE KEYS */;
INSERT INTO `save_rent`(`id_save_rent`,`client`,`out_data_rent`,`time`,`number_pc`,`staff`,`price`,`status`,`id_rent`) VALUES(1,'Сергеевичч','21.04.2023 16:15:54','40',2,'Смирновв',96,'Удален',15),(2,'Сергеевичч','21.04.2023 16:33:47','50',1,'Смирновв',100,'Удален',16),(3,'Иванов','21.04.2023 16:54:18','50',2,'Иванов',120,'Удален',17),(4,'Сергеевичч','21.04.2023 16:34:58','30',1,'Смирновв',60,'Удален',18),(5,'Сергеевичч','21.04.2023 16:47:00','40',1,'Смирновв',80,'Удален',19),(6,'Сергеевичч','21.04.2023 16:47:59','40',2,'Смирновв',96,'Удален',20),(7,'Сергеевичч','21.04.2023 16:11:14','1',2,'Иванов',2,'Оплачен',21),(8,'Сергеевичч','21.04.2023 17:14:57','50',1,'Иванов',100,'Оплачен',22),(9,'Сергеевичч','30.04.2023 13:33:54','60',2,'Иванов',144,'Оплачен',23),(10,'Перистов','30.04.2023 13:42:27','50',1,'Иванов',100,'Удален',24),(11,'Перистов','30.04.2023 14:32:42','100',2,'Иванов',240,'Удален',25),(12,'Перистов','30.04.2023 13:16:08','1',1,'Иванов',2,'Оплачен',26),(13,'Перистов','30.04.2023 13:16:08','1',1,'Иванов',2,'Оплачен',26),(14,'Перистов','30.04.2023 13:16:08','1',1,'Иванов',2,'Оплачен',26),(15,'Перистов','30.04.2023 13:16:08','1',1,'Иванов',2,'Оплачен',26),(16,'Перистов','30.04.2023 13:16:08','1',1,'Иванов',2,'Оплачен',26),(17,'Перистов','30.04.2023 13:16:08','1',1,'Иванов',2,'Оплачен',26),(18,'Перистов','30.04.2023 13:16:08','1',1,'Иванов',2,'Оплачен',26),(19,'Перистов','30.04.2023 13:28:10','1',2,'Смирновв',2,'Оплачен',27),(20,'Перистов','30.04.2023 13:28:10','1',2,'Смирновв',2,'Оплачен',27),(21,'Перистов','30.04.2023 13:28:10','1',2,'Смирновв',2,'Оплачен',27),(22,'Перистов','30.04.2023 14:22:01','50',1,'Иванов',100,'Оплачен',28),(23,'Перистов','30.04.2023 13:33:09','1',2,'Иванов',2,'Оплачен',29),(24,'Перистов','30.04.2023 13:33:09','1',2,'Иванов',2,'Оплачен',29),(25,'Перистов','30.04.2023 14:22:01','50',1,'Иванов',100,'Оплачен',28),(26,'Перистов','03.05.2023','100',1,'Иванов',0,'Оплачен',30),(27,'Перистов','03.05.2023','100',1,'Иванов',0,'Оплачен',31),(28,'Перистов','03.05.2023','150',1,'Иванов',0,'Оплачен',32),(29,'Перистов','03.05.2023','100',1,'Смирновв',0,'Оплачен',33),(30,'Перистов','03.05.2023 13:07:28','40',1,'Иванов',64,'Оплачен',34),(31,'Перистов','03.05.2023','40',2,'Иванов',77,'Оплачен',36),(32,'Перистов','03.05.2023 13:07:28','40',1,'Иванов',64,'Оплачен',34),(33,'Перистов','07.05.2023 16:31:14','89',1,'Иванов',142,'Оплачен',37),(34,'Перистов','07.05.2023 17:34:20','60',2,'Смирновв',115,'Оплачен',38),(35,'Перистов','07.05.2023 16:34:50','40',1,'Иванов',64,'Оплачен',39),(36,'Перистов','07.05.2023 17:34:20','60',2,'Смирновв',115,'Оплачен',38),(37,'Перистов','09.05.2023 19:32:21','60',1,'Иванов',96,'Оплачен',39),(38,'Лебедев','10.05.2023 8:42:40','40',1,'Иванов',72,'Оплачен',40),(39,'Перистов','10.05.2023 8:44:12','30',1,'Иванов',54,'Оплачен',41),(40,'Лебедев','10.05.2023 8:45:19','40',1,'Иванов',72,'Оплачен',42),(41,'Лебедев','10.05.2023 8:48:06','50',1,'Иванов',90,'Оплачен',43),(42,'Лебедев','10.05.2023 8:49:42','30',1,'Иванов',54,'Оплачен',44),(43,'Лебедев','10.05.2023 8:53:21','50',1,'Иванов',90,'Оплачен',45),(44,'Лебедев','10.05.2023 8:54:44','70',1,'Иванов',126,'Оплачен',46),(45,'Лебедев','10.05.2023 8:55:45','70',1,'Иванов',126,'Оплачен',47),(46,'Лебедев','10.05.2023 8:56:51','60',1,'Иванов',108,'Оплачен',48),(47,'Лебедев','10.05.2023 8:57:38','780',1,'Иванов',1404,'Оплачен',49),(48,'Лебедев','10.05.2023 9:00:35','60',1,'Иванов',108,'Оплачен',50),(49,'Лебедев','10.05.2023 9:01:30','50',1,'Иванов',90,'Оплачен',51),(50,'Лебедев','10.05.2023 9:03:14','60',1,'Иванов',108,'Оплачен',52),(51,'Лебедев','10.05.2023 10:13:24','50',1,'Иванов',25,'Оплачен',53),(52,'Лебедев','10.05.2023 10:43:57','80',2,'Иванов',48,'Удален',54),(53,'Лебедев','10.05.2023 10:13:24','50',1,'Иванов',25,'Оплачен',53),(54,'Лебедев','21.05.2023 16:36:11','50',1,'Иванов',90,'Оплачен',55),(55,'Перистов','21.05.2023 16:57:20','60',2,'Иванов',130,'Удален',56);
/*!40000 ALTER TABLE `save_rent` ENABLE KEYS */;

-- 
-- Definition of users
-- 

DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
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

-- 
-- Dumping data for table users
-- 

/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users`(`id_users`,`login`,`passwd`,`user_name`,`data_create`,`role`) VALUES(1,'admin','admin','admin','2023-02-07 00:00:00','admin'),(2,'user','user','user','2023-02-07 00:00:00','user');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-21 16:11:16
-- Total time: 0:0:0:0:56 (d:h:m:s:ms)
