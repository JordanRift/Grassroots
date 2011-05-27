CREATE TABLE `gr_region` (
  `RegionID` int(11) NOT NULL AUTO_INCREMENT,
  `CauseTemplateID` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(200) NOT NULL,
  PRIMARY KEY (`RegionID`),
  KEY `FK_Region_CauseTemplate` (`CauseTemplateID`),
  CONSTRAINT `FK_Region_CauseTemplate` FOREIGN KEY (`CauseTemplateID`) REFERENCES `gr_causetemplate` (`CauseTemplateID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

