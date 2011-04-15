//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

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

        public virtual void Save()
        {
            ObjectContext.SaveChanges();
        }
    }
}
