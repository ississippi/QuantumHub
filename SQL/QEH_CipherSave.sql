USE `quantumencrypt`;
DROP procedure IF EXISTS `QEH_CipherSave`;

DELIMITER $$
USE `quantumencrypt`$$
CREATE PROCEDURE `QEH_CipherSave` (
	IN userId INT
    ,IN serialNumber VARCHAR(75)
    ,IN startPoint INT
    ,IN cipherString MEDIUMTEXT
)
BEGIN
	INSERT INTO cipher
		(iduser
		,createdatetime
		,serialnumber
		,startpoint
		,cipherstring)
	VALUES
		(userId
        ,Now()
        ,serialNumber
        ,startpoint
        ,cipherString
        );
END$$

DELIMITER ;

