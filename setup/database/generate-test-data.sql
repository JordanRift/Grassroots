
use MyCampaign
go

declare @orgID int

insert into myc_Organization
values (
	'Test Organization',
	'123-456-7890',
	'test@yahoo.com'
)

set @orgID = scope_identity()

insert into myc_OrganizationSetting
values (
	@orgID,
	'MyC.IsFixedGoalAmount',
	'false',
	4 -- Boolean DataType
)

insert into myc_OrganizationSetting
values (
	@orgID,
	'MyC.GoalAmount',
	'4000',
	3 -- Decimal DataType
)

insert into myc_OrganizationSetting
values (
	@orgID,
	'MyC.OrganizationDomain',
	'www.onemission.us',
	1 -- String DataType
)

insert into myc_OrganizationSetting
values (
	@orgID,
	'MyC.OrganizationDonationEmailBody',
	'#FirstName# #LastName# donated',
	1 -- String DataType
)

insert into myc_OrganizationSetting
values (
	@orgID,
	'MyC.OrganizationDonationEmailSubject',
	'Donation from #domain#',
	1 -- String DataType
)

insert into myc_OrganizationSetting
values (
	@orgID,
	'MyC.OrganizationDonationToEmailAddress',
	'jason@jordanrift.com',
	1 -- String DataType
)

insert into myc_OrganizationSetting
values (
	@orgID,
	'MyC.OrganizationCampaignLength',
	'90',
	2 -- Int DataType
)