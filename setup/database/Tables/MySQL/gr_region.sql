CREATE TABLE `gr_region` (
  `RegionID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(200) NOT NULL,
  `CreatedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastModifiedOn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`RegionID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

