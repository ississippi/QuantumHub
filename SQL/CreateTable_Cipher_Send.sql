CREATE TABLE `quantumencrypt`.`cipher_send` (
  `idcipher_send` INT NOT NULL AUTO_INCREMENT,
  `idsender` INT NOT NULL,
  `idrecipient` INT NOT NULL,
  `idcipher` INT NOT NULL,
  `createdate` DATETIME NOT NULL,
  PRIMARY KEY (`idcipher_send`));