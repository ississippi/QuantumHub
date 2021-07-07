DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `QEH_AcceptDenyStatusSave`(
	IN idCipher INT,
    IN status varchar(16)
)
BEGIN
	UPDATE cipher_send s
    SET acceptdenystatus = status, acceptdenystatusdatetime = Now()
	WHERE s.idcipher_send = idCipher;
END$$
DELIMITER ;
