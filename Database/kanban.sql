-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: kanban
-- ------------------------------------------------------
-- Server version	8.0.40

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
-- Table structure for table `tablero`
--

DROP TABLE IF EXISTS `tablero`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tablero` (
  `id_tablero` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `titulo` varchar(20) NOT NULL,
  `descripcion` varchar(100) DEFAULT NULL,
  `color` varchar(45) DEFAULT '#ffffff',
  PRIMARY KEY (`id_tablero`),
  KEY `fk_usuario_1_idx` (`id_usuario`),
  CONSTRAINT `fk_usuario_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=105 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tablero`
--

LOCK TABLES `tablero` WRITE;
/*!40000 ALTER TABLE `tablero` DISABLE KEYS */;
INSERT INTO `tablero` VALUES (77,17,'sgvsgsg',NULL,'#ffffff'),(78,17,'efasesdfgsdf',NULL,'#d86f6f'),(92,24,'Ideas','cdv xdv','#d07c7c'),(93,24,'Tablero 1',NULL,'#b86f6f'),(97,28,'Tareas admin',NULL,'#ee9b9b'),(98,28,'Mas tareas',NULL,'#ffffff'),(99,24,'Proyectos 2025',NULL,'#d6f3ff'),(100,24,'Prueba',NULL,'#a59eff'),(102,24,'Ejemplo',NULL,'#ffffff'),(104,24,'hola','b','#ffffff');
/*!40000 ALTER TABLE `tablero` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tarea`
--

DROP TABLE IF EXISTS `tarea`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tarea` (
  `id_tarea` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int DEFAULT NULL,
  `titulo` varchar(45) NOT NULL,
  `descripcion` varchar(200) DEFAULT NULL,
  `color` varchar(30) DEFAULT '#ffffff',
  `fecha_modificacion` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `id_tablero` int DEFAULT NULL,
  `estado` int NOT NULL,
  PRIMARY KEY (`id_tarea`),
  KEY `fk_tarea_tablero_idx` (`id_tablero`),
  CONSTRAINT `fk_tarea_tablero` FOREIGN KEY (`id_tablero`) REFERENCES `tablero` (`id_tablero`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tarea`
--

LOCK TABLES `tarea` WRITE;
/*!40000 ALTER TABLE `tarea` DISABLE KEYS */;
INSERT INTO `tarea` VALUES (42,24,'fadsfd',NULL,'#ffffff','2025-02-22 23:01:29',92,1),(43,28,'a',NULL,'#ffffff','2025-02-23 22:53:35',93,1),(44,24,'vsxgsxdf',NULL,'#ffffff','2025-02-23 20:15:57',97,1),(45,0,'jgjh',NULL,'#ffffff','2025-02-23 23:27:51',98,1),(46,28,'hola',NULL,'#ffffff','2025-02-23 20:21:54',99,1),(47,24,'tarea','Descripcion de ejemplo\r\n\r\n','#ffffff','2025-02-23 20:26:04',99,2),(48,24,'soy una tarea','descripcion de ejemplo','#ffffff','2025-02-23 20:26:33',99,5),(50,24,'ejemplo',NULL,'#000000','2025-02-23 20:21:30',99,3),(51,28,'otra',NULL,'#ffffff','2025-02-23 20:21:48',99,2),(52,24,'v',NULL,'#ffffff','2025-02-23 23:27:47',98,1);
/*!40000 ALTER TABLE `tarea` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuario` (
  `id_usuario` int NOT NULL AUTO_INCREMENT,
  `nombre_usuario` varchar(20) NOT NULL,
  `contrasenia` varchar(90) NOT NULL,
  `email` varchar(45) NOT NULL,
  `rol` int NOT NULL,
  PRIMARY KEY (`id_usuario`),
  UNIQUE KEY `nombre_usuario_UNIQUE` (`nombre_usuario`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (17,'elultimo10','AQAAAAIAAYagAAAAEOt6Qseay3lQ50R05qzvsP/JEqXFW7qTP3PMyfnTdPht3Q+h7y4gT/hXZfG3WaNgVQ==','siuuuuuuuuu@yahoo.com.ar',1),(20,'123456','AQAAAAIAAYagAAAAEKab2N22Z8jXIVCc4FhZ+FXzkLpRJncd2tCHo2rhZuN7ulADW8gM0D5eoiGZnwSQHg==','siuuuuuuuuu@yahoo.com.ar',2),(24,'elAdmin','AQAAAAIAAYagAAAAEAq9zebV/XyutMc79WSs5o77LiQqS50xnb/1ffsjbziB2OPoF0/tGvOywe5BFZ8ISg==','siuuuuuuuuu@yahoo.com.ar',1),(28,'pruebas123','AQAAAAIAAYagAAAAEMjgEJfdkoJZGWL9WNLEnhdKuopNswsVbuG/sVJKvoHYx+nM6Z87raFNH+SSiwzCsg==','siuu@yahoo.com.ar',2),(30,'borrar','AQAAAAIAAYagAAAAEDhmdLHFN24KcrZs39agco2dXYO/Z2tyhkZ2r/K4b54uJXsLKLo7I7S9aWi9wS9bbw==','siuu@yahoo.com.ar',1);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'kanban'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-02-23 23:29:31
