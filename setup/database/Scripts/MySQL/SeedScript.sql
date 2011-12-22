START TRANSACTION;

SET @OrgID = 0;
SET @RoleID = 0;
SET @UserProfileID = 0;
SET @CauseTemplateID = 0;

INSERT INTO `gr_organization`
(
`Name`,
`Tagline`,
`SummaryHtml`,
`DescriptionHtml`,
`ContactPhone`,
`ContactEmail`,
`YtdGoal`,
`FiscalYearStartMonth`,
`FiscalYearStartDay`,
`PaymentGatewayType`,
`PaymentGatewayApiUrl`,
`PaymentGatewayArbApiUrl`,
`PaymentGatewayApiKey`,
`PaymentGatewayApiSecret`,
`FacebookPageUrl`,
`VideoEmbedHtml`,
`TwitterName`,
`BlogRssUrl`,
`ThemeName`,
`CreatedOn`,
`LastModifiedOn`)
VALUES
(
'Grassroots',
'Social giving. Done right.',
'<h2>Welcome to Grassroots!</h2>',
'<p>Lorem ipsum description.</p>',
'602-123-4567',
'some-email@gmail.com',
NULL,
1,
1,
1,
'https://test.authorize.net/gateway/transact.dll',
'https://apitest.authorize.net/xml/v1/request.api',
'2E3jsfH7L5F',
'979cxZC5g8dDRf9b',
'http://www.facebook.com',
'<iframe src="https://player.vimeo.com/video/15065967?title=0&amp;byline=0&amp;portrait=0&amp;color=8ccc8e" width="450" height="253" frameborder="0"></iframe>',
'grassrootsproj',
NULL,
'grassroots-theme',
CURDATE(),
CURDATE()
);

SET @OrgID = LAST_INSERT_ID();

INSERT INTO `gr_role`
(
`OrganizationID`,
`Name`,
`Description`,
`CreatedOn`,
`LastModifiedOn`)
VALUES
(
@OrgID,
'Root',
'Super User',
CURDATE(),
CURDATE()
);

SET @RoleID = LAST_INSERT_ID();

INSERT INTO `gr_role`
(
`OrganizationID`,
`Name`,
`Description`,
`CreatedOn`,
`LastModifiedOn`)
VALUES
(
@OrgID,
'Administrator',
'System Administrator',
CURDATE(),
CURDATE()
);

INSERT INTO `gr_userprofile`
(
`OrganizationID`,
`RoleID`,
`FacebookID`,
`FirstName`,
`LastName`,
`Birthdate`,
`Gender`,
`Email`,
`PrimaryPhone`,
`AddressLine1`,
`AddressLine2`,
`City`,
`State`,
`ZipCode`,
`Consent`,
`Active`,
`IsActivated`,
`ImagePath`,
`ActivationHash`,
`ActivationPin`,
`LastActivationAttempt`,
`CreatedOn`,
`LastModifiedOn`)
VALUES
(
@OrgID,
@RoleID,
NULL,
'System',
'Admin',
'1970-01-01 00:00:00',
'male',
'admin@gmail.com',
'602-123-4567',
'123 Some Place',
NULL,
'Any Town',
'AZ',
'85310',
b'1',
b'1',
b'1',
NULL,
NULL,
NULL,
'1900-01-01 00:00:00',
CURDATE(),
CURDATE()
);

SET @UserProfileID = LAST_INSERT_ID();

INSERT INTO `gr_user`
(
`Username`,
`Password`,
`UserProfileID`,
`IsActive`,
`IsAuthorized`,
`ForcePasswordChange`,
`RegisterDate`,
`LastLoggedIn`,
`FailedLoginAttempts`,
`CreatedOn`,
`LastModifiedOn`)
VALUES
(
'admin@gmail.com',
'9mdZfZTsYyk9wOqMpJOun0XcMtqGJJXVUg==',
@UserProfileID,
b'1',
b'1',
b'0',
CURDATE(),
CURDATE(),
0,
CURDATE(),
CURDATE()
);

INSERT INTO `gr_causetemplate`
(
`OrganizationID`,
`Name`,
`ActionVerb`,
`GoalName`,
`CallToAction`,
`Active`,
`AmountIsConfigurable`,
`DefaultAmount`,
`TimespanIsConfigurable`,
`DefaultTimespanInDays`,
`CutOffDate`,
`Summary`,
`VideoEmbedHtml`,
`DescriptionHtml`,
`ImagePath`,
`BeforeImagePath`,
`AfterImagePath`,
`InstructionsOpenHtml`,
`InstructionsClosedHtml`,
`StatisticsHtml`,
`CreatedOn`,
`LastModifiedOn`)
VALUES
(
@OrgID,
'Cat Farm',
'Farmed',
'Cats',
'These cats don''t herd themselves!',
b'1',
b'1',
1000,
b'0',
90,
NULL,
'Let''s farm some kitties!',
'<iframe src="https://player.vimeo.com/video/15065967?title=0&amp;byline=0&amp;portrait=0&amp;color=8ccc8e" width="450" height="253" frameborder="0"></iframe>',
'<p>This is how we plan ot farm cats.</p>',
'',
NULL,
NULL,
'<p>Click the button to fund a cat farm!</p>',
'<p><strong>Uh oh!</strong> No more cat farms for you...</p>',
'<p>Coming soon... facts on the challenges of farming cats.</p>',
CURDATE(),
CURDATE()
);

SET @CauseTemplateID = LAST_INSERT_ID();

INSERT INTO `gr_region`
(
`Name`,
`Description`,
`CreatedOn`,
`LastModifiedOn`)
VALUES
(
'Rocky Point',
'Rocky Point, Mexico',
CURDATE(),
CURDATE()
);

INSERT INTO `grassroots`.`gr_campaign`
(
`OrganizationID`,
`UserProfileID`,
`CauseTemplateID`,
`CauseID`,
`Title`,
`Description`,
`ImagePath`,
`StartDate`,
`EndDate`,
`GoalAmount`,
`UrlSlug`,
`CampaignType`,
`IsGeneralFund`,
`CreatedOn`,
`LastModifiedOn`)
VALUES
(
@OrgID,
@UserProfileID,
@CauseTemplateID,
NULL,
'General Fund',
'This is the general fund.',
'',
CURDATE(),
ADDDATE(CURDATE(), 90),
1000,
'general',
0,
b'1',
CURDATE(),
CURDATE()
);

COMMIT;