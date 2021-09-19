DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `QEH_CipherGet`(
	IN userId INT,
    IN cipherId INT
    )
BEGIN
	SELECT	c.idcipher
			,c.iduser
			,c.createdatetime
			,c.serialnumber
			,c.startpoint
			,c.cipherstring
            ,c.maxencryptionlength
	FROM cipher c
    WHERE c.iduser = userId
    AND c.idcipher = cipherId;
END$$
DELIMITER ;
