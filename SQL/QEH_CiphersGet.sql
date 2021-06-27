-------------------------------------------------------------------------------
-- QEH_CiphersGet
-- Author: Gary Smith
-- Created: 2021-06-27
-- Description: Retrieve one or more cipher rows.
-- Revision History
-------------------------------------------------------------------------------
USE `quantumencrypt`;
DROP procedure IF EXISTS `QEH_CiphersGet`;

DELIMITER $$
USE `quantumencrypt`$$
CREATE PROCEDURE `QEH_CiphersGet` ()
BEGIN
	SELECT	c.idcipher
			,c.iduser
			,c.createdatetime
			,c.serialnumber
			,c.startpoint
			,c.cipherstring
	FROM cipher c
	ORDER BY c.createdate DESC;
END$$

DELIMITER ;


GO

