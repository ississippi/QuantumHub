DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `QEH_CipherSave`(
	IN userId INT
    ,IN serialNumber VARCHAR(75)
    ,IN startPoint INT
    ,IN cipherString MEDIUMTEXT
    ,IN maxEncryptionLength INT
)
BEGIN
IF (SELECT serialnumber FROM cipher c WHERE c.iduser = userId AND c.serialNumber = serialNumber)
THEN
	SELECT -1;
ELSE 
	INSERT INTO cipher
		(iduser
		,createdatetime
		,serialnumber
		,startpoint
		,cipherstring
        ,maxencryptionlength)
	VALUES
		(userId
        ,Now()
        ,serialNumber
        ,startpoint
        ,cipherString
        ,maxEncryptionLength
        );
	SELECT last_insert_id() AS idcipher;
END IF;
END$$
DELIMITER ;
