USE grassroots;

CREATE TABLE `gr_campaigndonor` (
  `CampaignDonorID` int(11) NOT NULL AUTO_INCREMENT,
  `CampaignID` int(11) NOT NULL,
  `UserProfileID` int(11) DEFAULT NULL,
  `FirstName` varchar(50) NOT NULL DEFAULT '''''',
  `LastName` varchar(50) NOT NULL DEFAULT '''''',
  `Comments` varchar(500) NOT NULL DEFAULT '''''',
  `Amount` decimal(11,2) NOT NULL DEFAULT '0.00',
  `Email` varchar(30) NOT NULL DEFAULT '''''',
  `PrimaryPhone` varchar(30) NOT NULL DEFAULT '''''',
  `AddressLine1` varchar(200) NOT NULL DEFAULT '''''',
  `AddressLine2` varchar(200) DEFAULT NULL,
  `City` varchar(50) NOT NULL DEFAULT '''''',
  `State` varchar(25) NOT NULL DEFAULT '''''',
  `ZipCode` varchar(20) NOT NULL DEFAULT '''''',
  `DonationDate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `Approved` bit(1) DEFAULT b'0',
  `ReferenceID` varchar(50) DEFAULT NULL,
  `Notes` varchar(8000) DEFAULT NULL,
  PRIMARY KEY (`CampaignDonorID`),
  KEY `CampaignDonor_Campaign_FK` (`CampaignID`),
  KEY `CampaignDonor_UserProfile_FK` (`UserProfileID`),
  CONSTRAINT `CampaignDonor_Campaign_FK` FOREIGN KEY (`CampaignID`) REFERENCES `gr_campaign` (`CampaignID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `CampaignDonor_UserProfile_FK` FOREIGN KEY (`UserProfileID`) REFERENCES `gr_userprofile` (`UserProfileID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


