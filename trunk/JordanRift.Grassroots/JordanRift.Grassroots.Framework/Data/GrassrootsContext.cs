//
// Grassroots is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Grassroots is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Grassroots.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using JordanRift.Grassroots.Framework.Entities;
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
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new CampaignConfiguration());
            modelBuilder.Configurations.Add(new CampaignDonorConfiguration());
            modelBuilder.Configurations.Add(new CauseConfiguration());
            modelBuilder.Configurations.Add(new CauseNoteConfiguration());
            modelBuilder.Configurations.Add(new CauseTemplateConfiguration());
            modelBuilder.Configurations.Add(new OrganizationConfiguration());
            modelBuilder.Configurations.Add(new OrganizationSettingConfiguration());
            modelBuilder.Configurations.Add(new RecipientConfiguration());
            modelBuilder.Configurations.Add(new RegionConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
        }

        public override int SaveChanges()
        {
            var changedEntities = ChangeTracker.Entries();

            foreach (var changedEntity in changedEntities)
            {
                var model = (Model) changedEntity.Entity;

                switch (changedEntity.State)
                {
                    case EntityState.Added:
                        model.OnBeforeInsert();
                        break;
                    case EntityState.Modified:
                        model.OnBeforeUpdate();
                        break;
                }
            }

            return base.SaveChanges();
        }
    }
}
