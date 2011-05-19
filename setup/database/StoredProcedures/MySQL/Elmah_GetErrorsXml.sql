DROP PROCEDURE IF EXISTS grassroots.Elmah_GetErrorsXml;
CREATE PROCEDURE grassroots.`Elmah_GetErrorsXml`(
	IN  `pApplication` NVARCHAR(60),
	IN  `pPageIndex`	 INT,
	IN  `pPageSize`	 INT,
	OUT `pTotalCount` INT
)
BEGIN
	SELECT COUNT(*) INTO `pTotalCount` FROM `Elmah_Error` WHERE `Application`= Application;
	
	SET @index = pPageIndex * (pPageSize + 1);
	SET @count = pPageSize;
	PREPARE STMT FROM 'SELECT * FROM `elmah_error` WHERE `Application`=Application ORDER BY `TimeUtc` DESC, `Sequence` DESC LIMIT ?,?';
	EXECUTE STMT USING @index, @count;

END;
