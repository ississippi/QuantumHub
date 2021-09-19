CREATE TABLE `cipher_send` (
  `idcipher_send` int NOT NULL AUTO_INCREMENT,
  `acceptdenystatus` varchar(16) DEFAULT NULL,
  `idsender` int NOT NULL,
  `idrecipient` int NOT NULL,
  `idcipher` int NOT NULL,
  `startpoint` int DEFAULT NULL,
  `createdate` datetime NOT NULL,
  `acceptdenystatusdatetime` datetime DEFAULT NULL,
  PRIMARY KEY (`idcipher_send`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
