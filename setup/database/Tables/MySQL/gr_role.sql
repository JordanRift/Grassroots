USE grassroots;

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
