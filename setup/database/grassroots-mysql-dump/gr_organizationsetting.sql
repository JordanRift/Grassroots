USE grassroots;

CREATE TABLE `gr_organizationsetting` (
  `OrganizationSettingID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL DEFAULT '''''',
  `Value` varchar(8000) NOT NULL DEFAULT '''''''''',
  `DataType` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`OrganizationSettingID`),
  KEY `Organization_OrganizationSetting_FK` (`OrganizationID`),
  KEY `name_index` (`Name`),
  CONSTRAINT `Organization_OrganizationSetting_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


