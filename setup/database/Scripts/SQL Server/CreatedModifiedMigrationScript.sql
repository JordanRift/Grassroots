
alter table gr_Campaign
add CreatedOn datetime not null default '1/1/1900';
alter table gr_Campaign
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_CampaignDonor
add CreatedOn datetime not null default '1/1/1900';
alter table gr_CampaignDonor
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_Cause
add CreatedOn datetime not null default '1/1/1900';
alter table gr_Cause
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_CauseNote
add CreatedOn datetime not null default '1/1/1900';
alter table gr_CauseNote
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_CauseTemplate
add CreatedOn datetime not null default '1/1/1900';
alter table gr_CauseTemplate
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_Organization
add CreatedOn datetime not null default '1/1/1900';
alter table gr_Organization
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_OrganizationSetting
add CreatedOn datetime not null default '1/1/1900';
alter table gr_OrganizationSetting
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_Recipient
add CreatedOn datetime not null default '1/1/1900';
alter table gr_Recipient
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_Region
add CreatedOn datetime not null default '1/1/1900';
alter table gr_Region
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_Role
add CreatedOn datetime not null default '1/1/1900';
alter table gr_Role
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_User
add CreatedOn datetime not null default '1/1/1900';
alter table gr_User
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_UserProfile
add CreatedOn datetime not null default '1/1/1900';
alter table gr_UserProfile
add LastModifiedOn datetime not null default '1/1/1900';