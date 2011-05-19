CREATE TABLE `elmah_error` (
  `ErrorId` char(36) NOT NULL,
  `Application` varchar(60) NOT NULL,
  `Host` varchar(50) NOT NULL,
  `Type` varchar(100) NOT NULL,
  `Source` varchar(60) NOT NULL,
  `Message` varchar(500) NOT NULL,
  `User` varchar(50) NOT NULL,
  `StatusCode` int(10) NOT NULL,
  `TimeUtc` datetime NOT NULL,
  `Sequence` int(10) NOT NULL AUTO_INCREMENT,
  `AllXml` text NOT NULL,
  PRIMARY KEY (`Sequence`),
  UNIQUE KEY `IX_ErrorId` (`ErrorId`(8)),
  KEY `IX_ELMAH_Error_App_Time_Seql` (`Application`(10),`TimeUtc`,`Sequence`),
  KEY `IX_ErrorId_App` (`ErrorId`(8),`Application`(10))
) ENGINE=MyISAM DEFAULT CHARSET=utf8 CHECKSUM=1 DELAY_KEY_WRITE=1 ROW_FORMAT=DYNAMIC;