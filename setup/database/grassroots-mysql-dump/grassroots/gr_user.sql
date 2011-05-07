USE grassroots;

CREATE TABLE `gr_user` (
  `UserID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) NOT NULL DEFAULT '''''',
  `Password` varchar(2000) NOT NULL DEFAULT '''''',
  `UserProfileID` int(11) NOT NULL,
  `IsActive` bit(1) DEFAULT b'0',
  `IsAuthorized` bit(1) DEFAULT b'0',
  `ForcePasswordChange` bit(1) DEFAULT b'0',
  `RegisterDate` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  `LastLoggedIn` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
  PRIMARY KEY (`UserID`),
  KEY `User_UserProfile_FK` (`UserProfileID`),
  CONSTRAINT `User_UserProfile_FK` FOREIGN KEY (`UserProfileID`) REFERENCES `gr_userprofile` (`UserProfileID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;


insert into `gr_user`(`UserID`,`Username`,`Password`,`UserProfileID`,`IsActive`,`IsAuthorized`,`ForcePasswordChange`,`RegisterDate`,`LastLoggedIn`) values (7,'info@jordanrift.com','8c1/DJeUxC8U+RhmN2HjKV5Jqyzh1j4NUtM=',6,0,0,0,'2011-05-04 09:39:13','2011-05-04 09:39:13');
