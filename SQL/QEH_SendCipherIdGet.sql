DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `QEH_SendCipherIdGet`(
	IN sendId int
    )
BEGIN
-- Returns the Cipher which matches the CipherId in the Send Request
SELECT	c.idcipher
			,c.iduser
			,c.createdatetime
			,c.serialnumber
			,c.startpoint
			,c.cipherstring
            ,c.maxencryptionlength
	FROM cipher c
    JOIN cipher_send s ON s.idcipher = c.idcipher
    WHERE s.idcipher_send = sendId;
END$$
DELIMITER ;
