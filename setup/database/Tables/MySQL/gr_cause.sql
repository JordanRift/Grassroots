CREATE TABLE `gr_cause` (
  `CauseID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `CauseTemplateID` int(11) NOT NULL,
  `RegionID` int(11) DEFAULT NULL,
  `Name` varchar(100) NOT NULL DEFAULT '''''',
  `Active` bit(1) DEFAULT b'0',
  `Summary` varchar(500) NOT NULL DEFAULT '''''',
  `VideoEmbedHtml` varchar(200) NOT NULL DEFAULT '''''',
  `DescriptionHtml` varchar(8000) NOT NULL DEFAULT '''''''''',
  `ImagePath` varchar(200) NOT NULL DEFAULT '''''''''',
  `HoursVolunteered` int(11) DEFAULT '0',
  `BeforeImagePath` varchar(200) DEFAULT NULL,
  `AfterImagePath` varchar(200) DEFAULT NULL,
  `Latitude` decimal(18,15) DEFAULT NULL,
  `ReferenceNumber` varchar(50) DEFAULT NULL,
  `Longitude` decimal(18,15) DEFAULT NULL,
  `IsCompleted` bit(1) NOT NULL DEFAULT b'0',
  `DateCompleted` datetime DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`CauseID`),
  KEY `IX_ReferenceNumber` (`ReferenceNumber`),
  KEY `Cause_CauseTemplate_FK` (`CauseTemplateID`),
  KEY `Cause_Organization_FK` (`OrganizationID`),
  KEY `Cause_Region_FK` (`RegionID`),
  CONSTRAINT `Cause_CauseTemplate_FK` FOREIGN KEY (`CauseTemplateID`) REFERENCES `gr_causetemplate` (`CauseTemplateID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `Cause_Organization_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `Cause_Region_FK` FOREIGN KEY (`RegionID`) REFERENCES `gr_region` (`RegionID`) ON DELETE SET NULL ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

