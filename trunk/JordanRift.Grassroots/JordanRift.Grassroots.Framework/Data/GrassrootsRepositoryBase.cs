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

using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Framework.Data
{
    public abstract class GrassrootsRepositoryBase
    {
        private GrassrootsContext objectContext;

        protected GrassrootsContext ObjectContext
        {
            get
            {
                if (UnitOfWorkScope.CurrentObjectContext != null)
                {
                    return UnitOfWorkScope.CurrentObjectContext;
                }
                
                if (objectContext == null)
                {
                    objectContext = new GrassrootsContext();
                }

                return objectContext;
            }
        }

        public PriorityType Priority { get; set; }

        public virtual void Save()
        {
            try
            {
                ObjectContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var e in error.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", e.PropertyName, e.ErrorMessage);
                    }
                }

                throw;
            }
        }
    }
}
