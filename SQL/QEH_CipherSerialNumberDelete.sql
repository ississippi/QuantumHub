DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `QEH_CipherSerialNumberDelete`(
	IN userId INT
    )
BEGIN
	DELETE
	FROM cipher_segment_serial_number s
    WHERE s.userId = userId;
END$$
DELIMITER ;
