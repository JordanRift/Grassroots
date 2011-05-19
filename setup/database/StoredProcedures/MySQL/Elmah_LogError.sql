DROP PROCEDURE IF EXISTS grassroots.Elmah_LogError;
CREATE PROCEDURE grassroots.`Elmah_LogError`(
	IN `pErrorId`		CHAR(36),
	IN `pApplication`	NVARCHAR(60),
	IN `pHost`			NVARCHAR(50),
	IN `pType`			NVARCHAR(100),
	IN `pSource`		NVARCHAR(60),
	IN `pMessage`		NVARCHAR(500),
	IN `pUser`			NVARCHAR(50),
	IN `pStatusCode`	INT,
	IN `pTimeUtc`		DATETIME,
	IN `pAllXml`		TEXT
)
BEGIN
	INSERT INTO `Elmah_Error` (
		`ErrorId`,	
		`Application`,
		`Host`,		
		`Type`,		
		`Source`,		
		`Message`,	
		`User`,		
		`StatusCode`,	
		`TimeUtc`,	
		`Sequence`,
		`AllXml`		
	) VALUES
	(
		pErrorId,
		pApplication,
		pHost,
		pType,
		pSource,
		pMessage,
		pUser,
		pStatusCode,
		pTimeUtc,
		p0,        -- since sequence is not null, we have to pass some dummy value, this will be updated by trigger on insert
		pAllXml
	);
END;
