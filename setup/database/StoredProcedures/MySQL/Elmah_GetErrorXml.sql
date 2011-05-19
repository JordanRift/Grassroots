DROP PROCEDURE IF EXISTS grassroots.Elmah_GetErrorXml;
CREATE PROCEDURE grassroots.`Elmah_GetErrorXml`(
	IN `pApplication` NVARCHAR(60),
	IN `pErrorId`	 CHAR(36)
)
BEGIN
	SELECT `AllXml`
	FROM `Elmah_Error`
	WHERE 
		`ErrorId` = pErrorId AND `Application` = pApplication;
END;
