DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `QEH_CiphersGet`(
	IN userId int)
BEGIN
	SELECT	c.idcipher
			,c.iduser
			,c.createdatetime
			,c.serialnumber
			,c.startpoint
			,c.cipherstring
            ,maxencryptionlength
	FROM cipher c
    WHERE c.iduser = userId
	ORDER BY c.createdatetime DESC;
END$$
DELIMITER ;
