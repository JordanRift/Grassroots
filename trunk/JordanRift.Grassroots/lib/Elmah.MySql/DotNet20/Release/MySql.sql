-- ELMAH DDL script for MySql

/* ------------------------------------------------------------------------ 
        TABLES
   ------------------------------------------------------------------------ */

CREATE TABLE Elmah_Error
(
	`ErrorId`		CHAR(36)			NOT NULL,
	`Application`	NVARCHAR(60)		NOT NULL,
	`Host`			NVARCHAR(50)		NOT NULL,
	`Type`			NVARCHAR(100)		NOT NULL,
	`Source`		NVARCHAR(60)		NOT NULL,
	`Message`		NVARCHAR(500)		NOT NULL,
	`User`			NVARCHAR(50)		NOT NULL,
	`StatusCode`	INT					NOT NULL,
	`TimeUtc`		DATETIME			NOT NULL,

	/* Sequence cannot be auto incremented, otherwise we get the following error.
	   Incorrect table definition; there can be only one auto column and it must be defined as a key :-(
	   triggers to the solution :-)
	*/
	`Sequence`		INT					NOT NULL, -- this will be generated via trigger


	`AllXml`		TEXT				NOT NULL,
	PRIMARY KEY(`ErrorId`)
);

-- function to get the new sequence number
DELIMITER $$

-- DROP FUNCTION IF EXISTS `Emlah_Error_NewSequenceNumber` $$
CREATE FUNCTION  `Emlah_Error_NewSequenceNumber`() RETURNS INT
    NO SQL
BEGIN
      DECLARE newSequence INT;
      SELECT IFNULL(MAX(`Sequence`) +1 , 1) INTO newSequence FROM `elmah_error`;
      RETURN(newSequence);
END $$
DELIMITER ;


-- trigger to make sure we get our sequence number in the table
-- DROP TRIGGER IF EXISTS `trgElmah_Error_AutoIncrementSequence`;
CREATE TRIGGER `trgElmah_Error_AutoIncrementSequence`
BEFORE INSERT on `Elmah_Error`
FOR EACH ROW SET NEW.`Sequence` = Emlah_Error_NewSequenceNumber();

-- can't put UUID() as default value,
-- mysql 5 requires default values to be constant
-- but i how do i get the last inserted guid in mysql?
-- so better send guid from the one who inserts it - the app itself (C#)
--
-- CREATE TRIGGER `trgElmah_Error_AutoGUID`
-- BEFORE INSERT ON `Elmah_Error`
-- FOR EACH ROW SET NEW.ErrorId = UUID();

/* ------------------------------------------------------------------------ 
        STORED PROCEDURES                                                      
   ------------------------------------------------------------------------ */
DELIMITER $$
-- DROP PROCEDURE IF EXISTS `Elmah_GetErrorXml` $$
CREATE PROCEDURE Elmah_GetErrorXml
(
	IN `pApplication` NVARCHAR(60),
	IN `pErrorId`	 CHAR(36)
)
BEGIN
	SELECT `AllXml`
	FROM `Elmah_Error`
	WHERE 
		`ErrorId` = pErrorId AND `Application` = pApplication;
END $$

-- DROP PROCEDURE IF EXISTS `Elmah_GetErrorsXml` $$
CREATE PROCEDURE `Elmah_GetErrorsXml`
(
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

END $$

-- DROP PROCEDURE IF EXISTS `Elmah_LogError` $$
CREATE PROCEDURE `Elmah_LogError`
(
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
END $$

DELIMITER ;