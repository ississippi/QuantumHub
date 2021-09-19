DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `QEH_CipherSerialNumberGet`(
	IN userId INT
    )
BEGIN
	SELECT	c.userId
			,c.createdatetime
			,c.serialnumber
            ,c.activated
	FROM cipher c
    WHERE c.iduser = userId;
END$$
DELIMITER ;
