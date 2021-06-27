CREATE TABLE `quantumencrypt`.`cipher` (
  `idcipher` INT NOT NULL AUTO_INCREMENT,
  `iduser` INT NOT NULL,
  `serialnumber` VARCHAR(75) NOT NULL,
  `startpoint` INT NULL,
  `cipherstring` MEDIUMTEXT NULL,
  PRIMARY KEY (`idcipher`),
  UNIQUE INDEX `iduser_UNIQUE` (`iduser` ASC) VISIBLE,
  UNIQUE INDEX `serialnumber_UNIQUE` (`serialnumber` ASC) VISIBLE,
  UNIQUE INDEX `idcipher_UNIQUE` (`idcipher` ASC) VISIBLE);