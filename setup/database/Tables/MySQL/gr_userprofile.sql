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
  `PrimaryPhone` varchar(30) DEFAULT '''''''''',
  `AddressLine1` varchar(200) DEFAULT '''''''''',
  `AddressLine2` varchar(200) DEFAULT NULL,
  `City` varchar(100) DEFAULT '''''''''',
  `State` varchar(100) DEFAULT '''''''''',
  `ZipCode` varchar(20) NOT NULL DEFAULT '''''',
  `Consent` bit(1) DEFAULT b'0',
  `Active` bit(1) DEFAULT b'0',
  `IsActivated` bit(1) DEFAULT b'0',
  `ImagePath` varchar(100) DEFAULT '''''''''',
  `ActivationHash` varchar(500) DEFAULT NULL,
  `LastActivationAttempt` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`UserProfileID`),
  KEY `email_index` (`Email`),
  KEY `IX_FacebookID` (`FacebookID`),
  KEY `UserProfile_Organization_FK` (`OrganizationID`),
  KEY `UserProfile_Role_FK` (`RoleID`),
  KEY `userprofile_activationhash` (`ActivationHash`(255)),
  CONSTRAINT `UserProfile_Organization_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `UserProfile_Role_FK` FOREIGN KEY (`RoleID`) REFERENCES `gr_role` (`RoleID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8;

