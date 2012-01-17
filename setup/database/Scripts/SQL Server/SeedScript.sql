BEGIN TRANSACTION
DECLARE @OrgID INT;
DECLARE @RoleID INT;
DECLARE @UserProfileID INT;
DECLARE @CauseTemplateID INT;

INSERT INTO [dbo].[gr_Organization]
           ([Name]
           ,[Tagline]
           ,[SummaryHtml]
           ,[DescriptionHtml]
           ,[ContactPhone]
           ,[ContactEmail]
           ,[YtdGoal]
           ,[FiscalYearStartMonth]
           ,[FiscalYearStartDay]
           ,[PaymentGatewayType]
           ,[PaymentGatewayApiUrl]
           ,[PaymentGatewayApiKey]
           ,[PaymentGatewayApiSecret]
           ,[FacebookPageUrl]
           ,[VideoEmbedHtml]
           ,[TwitterName]
           ,[BlogRssUrl]
           ,[ThemeName]
           ,[PaymentGatewayArbApiUrl]
           ,[CreatedOn]
           ,[LastModifiedOn])
     VALUES
           ('Grassroots'
           ,'Social giving. Done right.'
           ,'<h2>Welcome to Grassroots!</h2>'
           ,'<p>Lorem ipsum description.</p>'
           ,'602-123-4567'
           ,'some-email@gmail.com'
           ,NULL
           ,1
           ,1
           ,1
           ,'https://test.authorize.net/gateway/transact.dll'
           ,'2E3jsfH7L5F'
           ,'979cxZC5g8dDRf9b'
           ,'http://www.facebook.com'
           ,'<iframe src="https://player.vimeo.com/video/15065967?title=0&amp;byline=0&amp;portrait=0&amp;color=8ccc8e" width="450" height="253" frameborder="0"></iframe>'
           ,'grassrootsproj'
           ,null
           ,'grassroots-theme'
           ,'https://apitest.authorize.net/xml/v1/request.api'
           ,GETDATE()
           ,GETDATE());

SET @OrgID = SCOPE_IDENTITY();

INSERT INTO [dbo].[gr_Role]
           ([OrganizationID]
           ,[Name]
           ,[Description]
           ,[IsSystemRole]
           ,[CreatedOn]
           ,[LastModifiedOn])
     VALUES
           (@OrgID
           ,'Root'
           ,'Super User'
           ,1
           ,GETDATE()
           ,GETDATE());

SET @RoleID = SCOPE_IDENTITY();

INSERT INTO [dbo].[gr_Role]
           ([OrganizationID]
           ,[Name]
           ,[Description]
           ,[IsSystemRole]
           ,[CreatedOn]
           ,[LastModifiedOn])
     VALUES
           (@OrgID
           ,'Administrator'
           ,'System Administrator'
           ,1
           ,GETDATE()
           ,GETDATE());
           
INSERT INTO [dbo].[gr_UserProfile]
           ([OrganizationID]
           ,[RoleID]
           ,[FacebookID]
           ,[FirstName]
           ,[LastName]
           ,[Birthdate]
           ,[Gender]
           ,[Email]
           ,[PrimaryPhone]
           ,[AddressLine1]
           ,[AddressLine2]
           ,[City]
           ,[State]
           ,[ZipCode]
           ,[Consent]
           ,[Active]
           ,[IsActivated]
           ,[ImagePath]
           ,[ActivationHash]
           ,[ActivationPin]
           ,[LastActivationAttempt]
           ,[CreatedOn]
           ,[LastModifiedOn])
     VALUES
           (@OrgID
           ,@RoleID
           ,NULL
           ,'System'
           ,'Admin'
           ,'1/1/1970'
           ,'male'
           ,'admin@gmail.com'
           ,'602-123-4567'
           ,'123 Some Place'
           ,NULL
           ,'Any Town'
           ,'AZ'
           ,'85310'
           ,1
           ,1
           ,1
           ,NULL
           ,NULL
           ,NULL
           ,'1/1/1900'
           ,GETDATE()
           ,GETDATE());

SET @UserProfileID = SCOPE_IDENTITY();

INSERT INTO [dbo].[gr_User]
           ([Username]
           ,[Password]
           ,[UserProfileID]
           ,[IsActive]
           ,[IsAuthorized]
           ,[ForcePasswordChange]
           ,[RegisterDate]
           ,[LastLoggedIn]
           ,[FailedLoginAttempts]
           ,[CreatedOn]
           ,[LastModifiedOn])
     VALUES
           ('admin@gmail.com'
           ,'9mdZfZTsYyk9wOqMpJOun0XcMtqGJJXVUg=='
           ,@UserProfileID
           ,1
           ,1
           ,0
           ,GETDATE()
           ,GETDATE()
           ,0
           ,GETDATE()
           ,GETDATE());

INSERT INTO [dbo].[gr_CauseTemplate]
           ([OrganizationID]
           ,[Name]
           ,[ActionVerb]
           ,[GoalName]
           ,[CallToAction]
           ,[Active]
           ,[AmountIsConfigurable]
           ,[DefaultAmount]
           ,[TimespanIsConfigurable]
           ,[DefaultTimespanInDays]
           ,[Summary]
           ,[VideoEmbedHtml]
           ,[DescriptionHtml]
           ,[ImagePath]
           ,[BeforeImagePath]
           ,[AfterImagePath]
           ,[InstructionsOpenHtml]
           ,[InstructionsClosedHtml]
           ,[StatisticsHtml]
           ,[CutOffDate]
           ,[CreatedOn]
           ,[LastModifiedOn])
     VALUES
           (@OrgID
           ,'Cat Farm'
           ,'Farmed'
           ,'Cats'
           ,'These cats don''t herd themselves!'
           ,1
           ,1
           ,1000
           ,0
           ,90
           ,'Let''s farm some kitties!'
           ,'<iframe src="https://player.vimeo.com/video/15065967?title=0&amp;byline=0&amp;portrait=0&amp;color=8ccc8e" width="450" height="253" frameborder="0"></iframe>'
           ,'<p>This is how we plan ot farm cats.</p>'
           ,''
           ,NULL
           ,NULL
           ,'<p>Click the button to fund a cat farm!</p>'
           ,'<p><strong>Uh oh!</strong> No more cat farms for you...</p>'
           ,'<p>Coming soon... facts on the challenges of farming cats.</p>'
           ,NULL
           ,GETDATE()
           ,GETDATE());

SET @CauseTemplateID = SCOPE_IDENTITY();

INSERT INTO [dbo].[gr_Region]
           ([Name]
           ,[Description]
           ,[CreatedOn]
           ,[LastModifiedOn])
     VALUES
           ('Rocky Point'
           ,'Rocky Point, Mexico'
           ,GETDATE()
           ,GETDATE());

INSERT INTO [dbo].[gr_Campaign]
           ([OrganizationID]
           ,[UserProfileID]
           ,[CauseTemplateID]
           ,[CauseID]
           ,[Title]
           ,[Description]
           ,[ImagePath]
           ,[StartDate]
           ,[EndDate]
           ,[GoalAmount]
           ,[UrlSlug]
           ,[CampaignType]
           ,[IsGeneralFund]
           ,[CreatedOn]
           ,[LastModifiedOn])
     VALUES
           (@OrgID
           ,@UserProfileID
           ,@CauseTemplateID
           ,NULL
           ,'General Fund'
           ,'This is the general fund.'
           ,''
           ,GETDATE()
           ,DATEADD(DAY, 90, GETDATE())
           ,1000
           ,'general'
           ,0
           ,1
           ,GETDATE()
           ,GETDATE())
COMMIT
GO