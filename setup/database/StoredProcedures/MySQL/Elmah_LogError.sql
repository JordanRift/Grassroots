DROP PROCEDURE IF EXISTS grassroots.elmah_LogError;
CREATE PROCEDURE grassroots.`elmah_LogError`(
    IN ErrorId CHAR(36), 
    IN Application varchar(60), 
    IN Host VARCHAR(30), 
    IN Type VARCHAR(100), 
    IN Source VARCHAR(60), 
    IN Message VARCHAR(500), 
    IN User VARCHAR(50), 
    IN AllXml TEXT, 
    IN StatusCode INT(10), 
    IN TimeUtc DATETIME
)
    MODIFIES SQL DATA
BEGIN
    INSERT INTO `elmah_error` (
        `ErrorId`, 
        `Application`, 
        `Host`, 
        `Type`, 
        `Source`, 
        `Message`, 
        `User`, 
        `AllXml`, 
        `StatusCode`, 
        `TimeUtc`
    ) VALUES (
        ErrorId, 
        Application, 
        Host, 
        Type, 
        Source, 
        Message, 
        User, 
        AllXml, 
        StatusCode, 
        TimeUtc
    );
END;
