CREATE TABLE `gr_recipient` (
  `RecipientID` int(11) NOT NULL AUTO_INCREMENT,
  `CauseID` int(11) NOT NULL,
  `FirstName` varchar(30) NOT NULL DEFAULT '''''',
  `LastName` varchar(30) NOT NULL DEFAULT '''''',
  `Birthdate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`RecipientID`),
  KEY `FK_Recipient_Cause` (`CauseID`),
  CONSTRAINT `FK_Recipient_Cause` FOREIGN KEY (`CauseID`) REFERENCES `gr_cause` (`CauseID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

