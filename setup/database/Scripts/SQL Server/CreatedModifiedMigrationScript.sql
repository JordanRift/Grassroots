
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
add IsSystemRole bit not null default 0;
alter table gr_Role
add CreatedOn datetime not null default '1/1/1900';
alter table gr_Role
add LastModifiedOn datetime not null default '1/1/1900';
go

update gr_Role
set IsSystemRole = 1
where name in ( 'Root', 'Administrator' )
go

alter table gr_UserProfile drop constraint FK_gr_UserProfile_gr_Role

alter table gr_UserProfile add constraint FK_gr_UserProfile_gr_Role
	foreign key (RoleID) references gr_Role(RoleID) on delete set null
go

alter table gr_User
add CreatedOn datetime not null default '1/1/1900';
alter table gr_User
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_UserProfile
add CreatedOn datetime not null default '1/1/1900';
alter table gr_UserProfile
add LastModifiedOn datetime not null default '1/1/1900';

alter table gr_CampaignDonor alter column Email varchar(100) not null
alter table gr_User alter column Username varchar(100) not null
alter table gr_UserProfile alter column Email varchar(100) not null
go