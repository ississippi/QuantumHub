USE `quantumencrypt`;
DROP procedure IF EXISTS `QEH_CipherSend`;

DELIMITER $$
USE `quantumencrypt`$$
CREATE PROCEDURE `QEH_CipherSend` (
	IN userId INT
	,IN recipientUserId INT
    ,IN cipherId INT
    ,IN startPoint INT
)
BEGIN
	INSERT INTO cipher_send
		(idsender
		,idrecipient
		,idcipher
		,startpoint
        ,createdate
        )
	VALUES
		(userId
        ,recipientUserId
        ,cipherId
        ,startpoint
        ,Now()
        );
END$$

DELIMITER ;

