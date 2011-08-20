USE grassroots;

CREATE TABLE `gr_role` (
  `RoleID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL DEFAULT '''''',
  `Description` varchar(500) NOT NULL DEFAULT '''''',
  PRIMARY KEY (`RoleID`),
  KEY `Role_Organization_FK` (`OrganizationID`),
  KEY `name_index` (`Name`),
  CONSTRAINT `Role_Organization_FK` FOREIGN KEY (`OrganizationID`) REFERENCES `gr_organization` (`OrganizationID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;


insert into `gr_role`(`RoleID`,`OrganizationID`,`Name`,`Description`) values (1,1,'Administrator','System Administrator');
