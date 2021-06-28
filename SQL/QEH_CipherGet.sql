DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `QEH_CipherGet`(
	IN userId INT,
    IN serialNo VARCHAR(75)
    )
BEGIN
	SELECT	c.idcipher
			,c.iduser
			,c.createdatetime
			,c.serialnumber
			,c.startpoint
			,c.cipherstring
	FROM cipher c
    WHERE c.iduser = userId
    AND c.serialnumber = serialNo;
END$$
DELIMITER ;
SELECT * FROM quantumencrypt.cipher_send;