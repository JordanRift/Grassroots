CREATE TABLE `elmah_error` (
  `ErrorId` char(36) NOT NULL,
  `Application` varchar(60) NOT NULL,
  `Host` varchar(50) NOT NULL,
  `Type` varchar(100) NOT NULL,
  `Source` varchar(60) NOT NULL,
  `Message` varchar(500) NOT NULL,
  `User` varchar(50) NOT NULL,
  `StatusCode` int(11) NOT NULL,
  `TimeUtc` datetime NOT NULL,
  `Sequence` int(11) NOT NULL,
  `AllXml` text NOT NULL,
  PRIMARY KEY (`ErrorId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

