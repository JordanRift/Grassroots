USE grassroots;

CREATE TABLE `gr_userprofile` (
  `UserProfileID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `RoleID` int(11) DEFAULT NULL,
  `FacebookID` varchar(50) DEFAULT NULL,
  `FirstName` varchar(30) NOT NULL DEFAULT '''''',
  `LastName` varchar(30) NOT NULL DEFAULT '''''',
  `Birthdate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `Gender` varchar(20) NOT NULL DEFAULT '''''',
  `Email` varchar(50) NOT NULL DEFAULT '''''',
  `PrimaryPhone` varchar(30) NOT NULL DEFAULT '''''',
  `AddressLine1` varchar(200) NOT NULL DEFAULT '''''',
  `AddressLine2` varchar(200) DEFAULT NULL,
  `City` varchar(100) NOT NULL DEFAULT '''''',
  `State` varchar(100) NOT NULL DEFAULT '''''',
  `ZipCode` varchar(20) NOT NULL DEFAULT '''''',
  `Consent` bit(1) DEFAULT b'0',
  `Active` bit(1) DEFAULT b'0',
  `IsActivated` bit(1) DEFAULT b'0',
  `ImagePath` varchar(100) NOT NULL DEFAULT '''''',
  PRIMARY KEY (`UserProfileID`),
  KEY `UserProfile_Organization_FK` (`OrganizationID`),
  KEY `UserProfile_Role_FK` (`RoleID`),
  KEY `email_index` (`Email`),
  CONSTRAINT `UserProfile_Organization_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `UserProfile_Role_FK` FOREIGN KEY (`RoleID`) REFERENCES `gr_role` (`RoleID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;


insert into `gr_userprofile`(`UserProfileID`,`OrganizationID`,`RoleID`,`FacebookID`,`FirstName`,`LastName`,`Birthdate`,`Gender`,`Email`,`PrimaryPhone`,`AddressLine1`,`AddressLine2`,`City`,`State`,`ZipCode`,`Consent`,`Active`,`IsActivated`,`ImagePath`) values (6,1,null,'1420411494','System','Administrator','1981-01-01 00:00:00','male','info@jordanrift.com','602-555-4567','1234 My St','Suite 201','Gilbert','AZ','85296',1,0,0,'https://graph.facebook.com/jasonoffutt/picture');
