DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `QEH_CipherSendRequestsGet`(
	IN recipientUserId INT,
    IN statusRequested varchar(16)
    )
BEGIN
	SELECT	s.idcipher_send
			,s.acceptdenystatus
			,s.idsender
            ,s.idrecipient
			,s.idcipher
			,s.startpoint
			,s.createdate
            ,s.acceptdenystatusdatetime
	FROM cipher_send s
    WHERE s.idrecipient = recipientUserId
    AND s.acceptdenystatus = statusRequested
	ORDER BY s.createdate DESC;
END$$
DELIMITER ;
