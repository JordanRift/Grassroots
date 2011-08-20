CREATE TABLE `gr_organization` (
  `OrganizationID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL DEFAULT '''''',
  `Tagline` varchar(150) NOT NULL DEFAULT '''''',
  `SummaryHtml` varchar(1000) DEFAULT '''''''''',
  `DescriptionHtml` varchar(8000) DEFAULT '''''''''',
  `ContactPhone` varchar(20) NOT NULL DEFAULT '''''',
  `ContactEmail` varchar(30) NOT NULL DEFAULT '''''',
  `YtdGoal` decimal(18,2) DEFAULT '0.00',
  `FiscalYearStartMonth` int(11) DEFAULT '1',
  `FiscalYearStartDay` int(11) DEFAULT '1',
  `PaymentGatewayType` int(11) NOT NULL DEFAULT '-1',
  `PaymentGatewayApiUrl` varchar(100) DEFAULT '''''',
  `PaymentGatewayApiKey` varchar(100) NOT NULL DEFAULT '''''',
  `PaymentGatewayApiSecret` varchar(100) NOT NULL DEFAULT '''''',
  `FacebookPageUrl` varchar(200) NOT NULL DEFAULT '''''''''',
  `VideoEmbedHtml` varchar(1000) NOT NULL DEFAULT '''''',
  `TwitterName` varchar(50) DEFAULT '''''''''',
  `BlogRssUrl` varchar(250) DEFAULT '''''',
  `ThemeName` varchar(50) NOT NULL DEFAULT '''''',
  PRIMARY KEY (`OrganizationID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

