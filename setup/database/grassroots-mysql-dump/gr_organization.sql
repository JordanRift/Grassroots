USE grassroots;

CREATE TABLE `gr_organization` (
  `OrganizationID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL DEFAULT '''''',
  `Tagline` varchar(150) NOT NULL DEFAULT '''''',
  `Summary` varchar(1000) DEFAULT '''''',
  `DescriptionHtml` varchar(2000) DEFAULT '''''',
  `ContactPhone` varchar(20) NOT NULL DEFAULT '''''',
  `ContactEmail` varchar(30) NOT NULL DEFAULT '''''',
  `YtdGoal` decimal(18,2) NOT NULL DEFAULT '0.00',
  `FiscalYearStartMonth` int(11) NOT NULL DEFAULT '1',
  `FiscalYearStartDay` int(11) NOT NULL DEFAULT '1',
  `PaymentGatewayType` int(11) NOT NULL DEFAULT '-1',
  `PaymentGatewayApiUrl` varchar(100) DEFAULT '''''',
  `PaymentGatewayApiKey` varchar(100) NOT NULL DEFAULT '''''',
  `PaymentGatewayApiSecret` varchar(100) NOT NULL DEFAULT '''''',
  `FacebookPageUrl` varchar(150) NOT NULL DEFAULT '''''',
  `VideoEmbedHtml` varchar(1000) NOT NULL DEFAULT '''''',
  `TwitterName` varchar(50) NOT NULL DEFAULT '''''',
  `BlogRssUrl` varchar(250) DEFAULT '''''',
  `ThemeName` varchar(50) NOT NULL DEFAULT '''''',
  PRIMARY KEY (`OrganizationID`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;


insert into `gr_organization`(`OrganizationID`,`Name`,`Tagline`,`Summary`,`DescriptionHtml`,`ContactPhone`,`ContactEmail`,`YtdGoal`,`FiscalYearStartMonth`,`FiscalYearStartDay`,`PaymentGatewayType`,`PaymentGatewayApiUrl`,`PaymentGatewayApiKey`,`PaymentGatewayApiSecret`,`FacebookPageUrl`,`VideoEmbedHtml`,`TwitterName`,`BlogRssUrl`,`ThemeName`) values (1,'One Mission','If they can, I can.','One Mission is super duper cool and extra spiffy. You should join our cause.','<h2>Howdy howdy howdy from our MySQL database</h2>','602-555-4567','info@jordanrift.com',2000.00,1,1,1,'https://test.authorize.net/gateway/transact.dll','2E3jsfH7L5F','979cxZC5g8dDRf9b','http://www.facebook.com','<iframe src="http://player.vimeo.com/video/15065967?title=0&amp;byline=0&amp;portrait=0&amp;color=8ccc8e" width="450" height="253" frameborder="0"></iframe>','@grassrootsproj','http://thebarrios.blogspot.com/feeds/posts/default?alt=rss','grassroots-theme');
