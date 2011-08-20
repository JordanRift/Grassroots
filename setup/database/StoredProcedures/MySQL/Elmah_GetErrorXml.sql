DROP PROCEDURE IF EXISTS grassroots.elmah_GetErrorXml;
CREATE PROCEDURE grassroots.`elmah_GetErrorXml`(
  IN Id CHAR(36),
  IN App VARCHAR(60)
)
    READS SQL DATA
BEGIN
    SELECT  `AllXml`
    FROM    `elmah_error`
    WHERE   `ErrorId` = Id AND `Application` = App;
END;
