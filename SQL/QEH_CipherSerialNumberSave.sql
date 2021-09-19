DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `QEH_CipherSerialNumberSave`(
	IN userId INT
    ,IN serialNumber VARCHAR(75)
    ,IN activated CHAR(1)
)
BEGIN
IF (SELECT serialnumber FROM cipher c WHERE c.iduser = userId AND c.serialNumber = serialNumber)
THEN
	SELECT -1;
ELSE 
	INSERT INTO cipher_segment_serial_number
		(userId
		,createdatetime
		,serialnumber
		,activated
        )
	VALUES
		(userId
        ,Now()
        ,serialNumber
        ,activated
        );
END IF;
END$$
DELIMITER ;
