USE grassroots;

CREATE TABLE `gr_causenote` (
  `CauseNoteID` int(11) NOT NULL AUTO_INCREMENT,
  `CauseID` int(11) NOT NULL,
  `UserProfileID` int(11) NOT NULL,
  `Text` varchar(8000) NOT NULL DEFAULT '''''''''',
  `EntryDate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`CauseNoteID`),
  KEY `CauseNote_Cause_FK` (`CauseID`),
  KEY `CauseNote_UserProfile_FK` (`UserProfileID`),
  CONSTRAINT `CauseNote_Cause_FK` FOREIGN KEY (`CauseID`) REFERENCES `gr_cause` (`CauseID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `CauseNote_UserProfile_FK` FOREIGN KEY (`UserProfileID`) REFERENCES `gr_userprofile` (`UserProfileID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


