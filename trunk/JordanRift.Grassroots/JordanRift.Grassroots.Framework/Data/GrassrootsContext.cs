//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Data.Entity;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Framework.Data
{
    public class GrassrootsContext : DbContext
    {
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignDonor> CampaignDonors { get; set; }
        public DbSet<CauseTemplate> CauseTemplates { get; set; }
        public DbSet<Cause> Causes { get; set; }
		public DbSet<CauseNote> CauseNotes { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationSetting> OrganizationSettings { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new CampaignConfiguration());
            modelBuilder.Configurations.Add(new CampaignDonorConfiguration());
            modelBuilder.Configurations.Add(new CauseConfiguration());
			modelBuilder.Configurations.Add( new CauseNoteConfiguration() );
            modelBuilder.Configurations.Add(new CauseTemplateConfiguration());
            modelBuilder.Configurations.Add(new OrganizationConfiguration());
            modelBuilder.Configurations.Add(new OrganizationSettingConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
        }
    }
}
