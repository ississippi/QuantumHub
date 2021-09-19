DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `QEH_CipherSend`(
	IN userId INT
	,IN recipientUserId INT
    ,IN cipherId INT
    ,IN startPoint INT
)
BEGIN
	INSERT INTO cipher_send
		(idsender
        ,acceptdenystatus
		,idrecipient
		,idcipher
		,startpoint
        ,createdate
        )
	VALUES
		(userId
        ,'pending'
        ,recipientUserId
        ,cipherId
        ,startpoint
        ,Now()
        );
	SELECT last_insert_id() AS id_ciphersend;
END$$
DELIMITER ;
