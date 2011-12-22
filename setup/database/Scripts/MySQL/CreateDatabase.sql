START TRANSACTION;
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';
CREATE TABLE IF NOT EXISTS `elmah_error` (
  `ErrorId` CHAR(36) NOT NULL ,
  `Application` VARCHAR(60) NOT NULL ,
  `Host` VARCHAR(50) NOT NULL ,
  `Type` VARCHAR(100) NOT NULL ,
  `Source` VARCHAR(60) NOT NULL ,
  `Message` VARCHAR(500) NOT NULL ,
  `User` VARCHAR(50) NOT NULL ,
  `StatusCode` INT(10) NOT NULL ,
  `TimeUtc` DATETIME NOT NULL ,
  `Sequence` INT(10) NOT NULL AUTO_INCREMENT ,
  `AllXml` TEXT NOT NULL ,
  PRIMARY KEY (`Sequence`) ,
  UNIQUE INDEX `IX_ErrorId` (`ErrorId`(8) ASC) ,
  INDEX `IX_ELMAH_Error_App_Time_Seql` (`Application`(10) ASC, `TimeUtc` DESC, `Sequence` DESC) ,
  INDEX `IX_ErrorId_App` (`ErrorId`(8) ASC, `Application`(10) ASC) )
ENGINE = MyISAM
DEFAULT CHARACTER SET = utf8
CHECKSUM = 1
DELAY_KEY_WRITE = 1
ROW_FORMAT = DYNAMIC;

DELIMITER //

CREATE PROCEDURE `elmah_GetErrorXml` (
  IN Id CHAR(36),
  IN App VARCHAR(60)
)
NOT DETERMINISTIC
READS SQL DATA
BEGIN
    SELECT  `AllXml`
    FROM    `elmah_error`
    WHERE   `ErrorId` = Id AND `Application` = App;
END//

CREATE PROCEDURE `elmah_GetErrorsXml` (
  IN App VARCHAR(60),
  IN PageIndex INT(10),
  IN PageSize INT(10),
  OUT TotalCount INT(10)
)
NOT DETERMINISTIC
READS SQL DATA
BEGIN
    
    SELECT  count(*) INTO TotalCount
    FROM    `elmah_error`
    WHERE   `Application` = App;

    SET @index = PageIndex * PageSize;
    SET @count = PageSize;
    SET @app = App;
    PREPARE STMT FROM '
    SELECT
        `ErrorId`,
        `Application`,
        `Host`,
        `Type`,
        `Source`,
        `Message`,
        `User`,
        `StatusCode`,
        CONCAT(`TimeUtc`, '' Z'') AS `TimeUtc`
    FROM
        `elmah_error` error
    WHERE
        `Application` = ?
    ORDER BY
        `TimeUtc` DESC,
        `Sequence` DESC
    LIMIT
        ?, ?';
    EXECUTE STMT USING @app, @index, @count;

END//

CREATE PROCEDURE `elmah_LogError` (
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
NOT DETERMINISTIC
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
END//

DELIMITER ;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

CREATE TABLE `gr_organization` (
  `OrganizationID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL DEFAULT '',
  `Tagline` varchar(150) NOT NULL DEFAULT '',
  `SummaryHtml` varchar(1000) DEFAULT '',
  `DescriptionHtml` varchar(8000) DEFAULT '',
  `ContactPhone` varchar(20) NOT NULL DEFAULT '',
  `ContactEmail` varchar(30) NOT NULL DEFAULT '',
  `YtdGoal` decimal(18,2) DEFAULT '0.00',
  `FiscalYearStartMonth` int(11) DEFAULT '1',
  `FiscalYearStartDay` int(11) DEFAULT '1',
  `PaymentGatewayType` int(11) NOT NULL DEFAULT '-1',
  `PaymentGatewayApiUrl` varchar(100) DEFAULT '',
  `PaymentGatewayArbApiUrl` varchar(100) DEFAULT '',
  `PaymentGatewayApiKey` varchar(100) NOT NULL DEFAULT '',
  `PaymentGatewayApiSecret` varchar(100) NOT NULL DEFAULT '',
  `FacebookPageUrl` varchar(200) NOT NULL DEFAULT '',
  `VideoEmbedHtml` varchar(1000) NOT NULL DEFAULT '',
  `TwitterName` varchar(50) DEFAULT '',
  `BlogRssUrl` varchar(250) DEFAULT '',
  `ThemeName` varchar(50) NOT NULL DEFAULT '',
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`OrganizationID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

CREATE TABLE `gr_role` (
  `RoleID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL DEFAULT '',
  `Description` varchar(500) NOT NULL DEFAULT '',
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`RoleID`),
  KEY `Role_Organization_FK` (`OrganizationID`),
  KEY `name_index` (`Name`),
  CONSTRAINT `Role_Organization_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

CREATE TABLE `gr_organizationsetting` (
  `OrganizationSettingID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL DEFAULT '''''',
  `Value` varchar(8000) NOT NULL DEFAULT '''''''''',
  `DataType` int(11) NOT NULL DEFAULT '0',
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`OrganizationSettingID`),
  KEY `Organization_OrganizationSetting_FK` (`OrganizationID`),
  KEY `name_index` (`Name`),
  CONSTRAINT `Organization_OrganizationSetting_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `gr_userprofile` (
  `UserProfileID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `RoleID` int(11) DEFAULT NULL,
  `FacebookID` varchar(50) DEFAULT NULL,
  `FirstName` varchar(30) NOT NULL DEFAULT '',
  `LastName` varchar(30) NOT NULL DEFAULT '',
  `Birthdate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `Gender` varchar(20) NOT NULL DEFAULT '',
  `Email` varchar(50) NOT NULL DEFAULT '',
  `PrimaryPhone` varchar(30) DEFAULT '',
  `AddressLine1` varchar(200) DEFAULT '',
  `AddressLine2` varchar(200) DEFAULT NULL,
  `City` varchar(100) DEFAULT '',
  `State` varchar(100) DEFAULT '',
  `ZipCode` varchar(20) NOT NULL DEFAULT '',
  `Consent` bit(1) DEFAULT b'0',
  `Active` bit(1) DEFAULT b'0',
  `IsActivated` bit(1) DEFAULT b'0',
  `ImagePath` varchar(100) DEFAULT '',
  `ActivationHash` varchar(500) DEFAULT NULL,
  `ActivationPin` varchar(12) DEFAULT NULL,
  `LastActivationAttempt` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`UserProfileID`),
  KEY `email_index` (`Email`),
  KEY `IX_FacebookID` (`FacebookID`),
  KEY `userprofile_activationhash` (`ActivationHash`(255)),
  KEY `UserProfile_Organization_FK` (`OrganizationID`),
  KEY `UserProfile_Role_FK` (`RoleID`),
  CONSTRAINT `UserProfile_Organization_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `UserProfile_Role_FK` FOREIGN KEY (`RoleID`) REFERENCES `gr_role` (`RoleID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8;

CREATE TABLE `gr_user` (
  `UserID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) NOT NULL DEFAULT '',
  `Password` varchar(2000) NOT NULL DEFAULT '',
  `UserProfileID` int(11) NOT NULL,
  `IsActive` bit(1) DEFAULT b'0',
  `IsAuthorized` bit(1) DEFAULT b'0',
  `ForcePasswordChange` bit(1) DEFAULT b'0',
  `RegisterDate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastLoggedIn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `FailedLoginAttempts` int(11) NOT NULL DEFAULT '0',
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`UserID`),
  KEY `User_UserProfile_FK` (`UserProfileID`),
  KEY `username_index` (`Username`),
  CONSTRAINT `User_UserProfile_FK` FOREIGN KEY (`UserProfileID`) REFERENCES `gr_userprofile` (`UserProfileID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;

CREATE TABLE `gr_region` (
  `RegionID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(200) NOT NULL,
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`RegionID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `gr_causetemplate` (
  `CauseTemplateID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL DEFAULT '',
  `ActionVerb` varchar(50) NOT NULL DEFAULT '',
  `GoalName` varchar(50) NOT NULL DEFAULT '',
  `CallToAction` varchar(300) DEFAULT '',
  `Active` bit(1) DEFAULT b'0',
  `AmountIsConfigurable` bit(1) DEFAULT b'0',
  `DefaultAmount` decimal(7,2) NOT NULL DEFAULT '0.00',
  `TimespanIsConfigurable` bit(1) DEFAULT b'0',
  `DefaultTimespanInDays` int(11) NOT NULL DEFAULT '0',
  `CutOffDate` DATETIME DEFAULT NULL,
  `Summary` varchar(500) NOT NULL DEFAULT '',
  `VideoEmbedHtml` varchar(200) NOT NULL DEFAULT '',
  `DescriptionHtml` varchar(8000) NOT NULL DEFAULT '',
  `ImagePath` varchar(200) NOT NULL DEFAULT '',
  `BeforeImagePath` varchar(200) DEFAULT NULL,
  `AfterImagePath` varchar(200) DEFAULT NULL,
  `InstructionsOpenHtml` varchar(8000) NOT NULL DEFAULT '',
  `InstructionsClosedHtml` varchar(20) NOT NULL DEFAULT '',
  `StatisticsHtml` varchar(2000) DEFAULT NULL,
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`CauseTemplateID`),
  KEY `CauseTemplate_Organization_FK` (`OrganizationID`),
  CONSTRAINT `CauseTemplate_Organization_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8;

CREATE TABLE `gr_cause` (
  `CauseID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `CauseTemplateID` int(11) NOT NULL,
  `RegionID` int(11) DEFAULT NULL,
  `Name` varchar(100) NOT NULL DEFAULT '',
  `Active` bit(1) DEFAULT b'0',
  `Summary` varchar(1000) DEFAULT '',
  `VideoEmbedHtml` varchar(200) NOT NULL DEFAULT '',
  `DescriptionHtml` varchar(8000) DEFAULT '',
  `ImagePath` varchar(200) NOT NULL DEFAULT '',
  `HoursVolunteered` int(11) DEFAULT '0',
  `BeforeImagePath` varchar(200) DEFAULT NULL,
  `AfterImagePath` varchar(200) DEFAULT NULL,
  `Latitude` decimal(18,15) DEFAULT NULL,
  `ReferenceNumber` varchar(50) DEFAULT NULL,
  `Longitude` decimal(18,15) DEFAULT NULL,
  `IsCompleted` bit(1) NOT NULL DEFAULT b'0',
  `DateCompleted` datetime DEFAULT '1900-01-01 00:00:00',
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`CauseID`),
  KEY `IX_ReferenceNumber` (`ReferenceNumber`),
  KEY `Cause_CauseTemplate_FK` (`CauseTemplateID`),
  KEY `Cause_Organization_FK` (`OrganizationID`),
  KEY `Cause_Region_FK` (`RegionID`),
  CONSTRAINT `Cause_CauseTemplate_FK` FOREIGN KEY (`CauseTemplateID`) REFERENCES `gr_causetemplate` (`CauseTemplateID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `Cause_Organization_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `Cause_Region_FK` FOREIGN KEY (`RegionID`) REFERENCES `gr_region` (`RegionID`) ON DELETE SET NULL ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `gr_recipient` (
  `RecipientID` int(11) NOT NULL AUTO_INCREMENT,
  `CauseID` int(11) NOT NULL,
  `FirstName` varchar(30) NOT NULL DEFAULT '''''',
  `LastName` varchar(30) NOT NULL DEFAULT '''''',
  `Birthdate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`RecipientID`),
  KEY `FK_Recipient_Cause` (`CauseID`),
  CONSTRAINT `FK_Recipient_Cause` FOREIGN KEY (`CauseID`) REFERENCES `gr_cause` (`CauseID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `gr_causenote` (
  `CauseNoteID` int(11) NOT NULL AUTO_INCREMENT,
  `CauseID` int(11) NOT NULL,
  `UserProfileID` int(11) NOT NULL,
  `Text` varchar(8000) NOT NULL DEFAULT '',
  `EntryDate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`CauseNoteID`),
  KEY `CauseNote_Cause_FK` (`CauseID`),
  KEY `CauseNote_UserProfile_FK` (`UserProfileID`),
  CONSTRAINT `CauseNote_Cause_FK` FOREIGN KEY (`CauseID`) REFERENCES `gr_cause` (`CauseID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `CauseNote_UserProfile_FK` FOREIGN KEY (`UserProfileID`) REFERENCES `gr_userprofile` (`UserProfileID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `gr_campaign` (
  `CampaignID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `UserProfileID` int(11) NOT NULL,
  `CauseTemplateID` int(11) NOT NULL,
  `CauseID` int(11) DEFAULT NULL,
  `Title` varchar(100) NOT NULL DEFAULT '',
  `Description` varchar(8000) NOT NULL DEFAULT '',
  `ImagePath` varchar(100) NOT NULL DEFAULT '',
  `StartDate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `EndDate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `GoalAmount` decimal(11,2) NOT NULL DEFAULT '0.00',
  `UrlSlug` varchar(30) NOT NULL DEFAULT '',
  `CampaignType` int(11) DEFAULT '-1',
  `IsGeneralFund` bit(1) DEFAULT b'0',
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`CampaignID`),
  KEY `urlslug_index` (`UrlSlug`),
  KEY `Campaign_CauseTemplate_FK` (`CauseTemplateID`),
  KEY `Campaign_Cause_FK` (`CauseID`),
  KEY `Campaign_Organization_FK` (`OrganizationID`),
  KEY `Campaign_UserProfile_FK` (`UserProfileID`),
  CONSTRAINT `Campaign_CauseTemplate_FK` FOREIGN KEY (`CauseTemplateID`) REFERENCES `gr_causetemplate` (`CauseTemplateID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `Campaign_Cause_FK` FOREIGN KEY (`CauseID`) REFERENCES `gr_cause` (`CauseID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `Campaign_Organization_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `Campaign_UserProfile_FK` FOREIGN KEY (`UserProfileID`) REFERENCES `gr_userprofile` (`UserProfileID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

CREATE TABLE `gr_campaigndonor` (
  `CampaignDonorID` int(11) NOT NULL AUTO_INCREMENT,
  `CampaignID` int(11) NOT NULL,
  `UserProfileID` int(11) DEFAULT NULL,
  `FirstName` varchar(50) NOT NULL DEFAULT '',
  `LastName` varchar(50) NOT NULL DEFAULT '',
  `Comments` varchar(500) NOT NULL DEFAULT '',
  `Amount` decimal(11,2) NOT NULL DEFAULT '0.00',
  `Email` varchar(30) NOT NULL DEFAULT '',
  `PrimaryPhone` varchar(30) NOT NULL DEFAULT '',
  `AddressLine1` varchar(200) NOT NULL DEFAULT '',
  `AddressLine2` varchar(200) DEFAULT NULL,
  `City` varchar(50) NOT NULL DEFAULT '',
  `State` varchar(25) NOT NULL DEFAULT '',
  `ZipCode` varchar(20) NOT NULL DEFAULT '',
  `DonationDate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `Approved` bit(1) DEFAULT b'0',
  `ReferenceID` varchar(50) DEFAULT NULL,
  `Notes` varchar(8000) DEFAULT NULL,
  `IsAnonymous` bit(1) DEFAULT b'0',
  `DisplayName` varchar(50) DEFAULT NULL,
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`CampaignDonorID`),
  KEY `CampaignDonor_Campaign_FK` (`CampaignID`),
  KEY `CampaignDonor_UserProfile_FK` (`UserProfileID`),
  CONSTRAINT `CampaignDonor_Campaign_FK` FOREIGN KEY (`CampaignID`) REFERENCES `gr_campaign` (`CampaignID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `CampaignDonor_UserProfile_FK` FOREIGN KEY (`UserProfileID`) REFERENCES `gr_userprofile` (`UserProfileID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
COMMIT;