CREATE TABLE `cipher` (
  `idcipher` int NOT NULL AUTO_INCREMENT,
  `iduser` int NOT NULL,
  `serialnumber` varchar(75) NOT NULL,
  `startpoint` int NOT NULL,
  `cipherstring` mediumtext NOT NULL,
  `createdatetime` datetime NOT NULL,
  `maxencryptionlength` int DEFAULT NULL,
  PRIMARY KEY (`idcipher`),
  UNIQUE KEY `idcipher_UNIQUE` (`idcipher`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
