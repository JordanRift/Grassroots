USE grassroots;

CREATE TABLE `gr_cause` (
  `CauseID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `CauseTemplateID` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL DEFAULT '''''',
  `Active` bit(1) DEFAULT b'0',
  `Summary` varchar(500) NOT NULL DEFAULT '''''',
  `DescriptionHtml` varchar(2000) NOT NULL DEFAULT '''''',
  `ImagePath` varchar(100) NOT NULL DEFAULT '''''',
  PRIMARY KEY (`CauseID`),
  KEY `Cause_CauseTemplate_FK` (`CauseTemplateID`),
  KEY `Cause_Organization_FK` (`OrganizationID`),
  CONSTRAINT `Cause_CauseTemplate_FK` FOREIGN KEY (`CauseTemplateID`) REFERENCES `gr_causetemplate` (`CauseTemplateID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `Cause_Organization_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


