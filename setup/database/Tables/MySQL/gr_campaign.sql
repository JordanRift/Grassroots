CREATE TABLE `gr_campaign` (
  `CampaignID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `UserProfileID` int(11) NOT NULL,
  `CauseTemplateID` int(11) NOT NULL,
  `CauseID` int(11) DEFAULT NULL,
  `Title` varchar(100) NOT NULL DEFAULT '''''',
  `Description` varchar(8000) NOT NULL DEFAULT '''''''''',
  `ImagePath` varchar(100) NOT NULL DEFAULT '''''',
  `StartDate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `EndDate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `GoalAmount` decimal(11,2) NOT NULL DEFAULT '0.00',
  `UrlSlug` varchar(30) NOT NULL DEFAULT '''''',
  `CampaignType` int(11) DEFAULT '-1',
  `IsGeneralFund` bit(1) DEFAULT b'0'
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

