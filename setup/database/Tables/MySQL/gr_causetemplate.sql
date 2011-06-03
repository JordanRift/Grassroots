CREATE TABLE `gr_causetemplate` (
  `CauseTemplateID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL DEFAULT '''''',
  `ActionVerb` varchar(50) NOT NULL DEFAULT '''''',
  `GoalName` varchar(50) NOT NULL DEFAULT '''''',
  `Active` bit(1) DEFAULT b'0',
  `AmountIsConfigurable` bit(1) DEFAULT b'0',
  `DefaultAmount` decimal(7,2) NOT NULL DEFAULT '0.00',
  `TimespanIsConfigurable` bit(1) DEFAULT b'0',
  `DefaultTimespanInDays` int(11) NOT NULL DEFAULT '0',
  `Summary` varchar(500) NOT NULL DEFAULT '''''',
  `VideoEmbedHtml` varchar(200) NOT NULL DEFAULT '''''',
  `DescriptionHtml` varchar(8000) NOT NULL DEFAULT '''''''''',
  `ImagePath` varchar(50) NOT NULL DEFAULT '''''',
  `BeforeImagePath` varchar(100) DEFAULT NULL,
  `AfterImagePath` varchar(100) DEFAULT NULL,
  `InstructionsOpenHtml` varchar(8000) NOT NULL DEFAULT '''''',
  `InstructionsClosedHtml` varchar(20) NOT NULL DEFAULT '''''',
  PRIMARY KEY (`CauseTemplateID`),
  KEY `CauseTemplate_Organization_FK` (`OrganizationID`),
  CONSTRAINT `CauseTemplate_Organization_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8;

