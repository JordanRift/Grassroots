//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Linq;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Framework.Data
{
    public class UserRepository : GrassrootsRepositoryBase, IUserRepository
    {
        public User GetUserByName(string name)
        {
            return ObjectContext.Users.FirstOrDefault(u => u.Username.ToLower() == name.ToLower());
        }

        public void Add(User user)
        {
            ObjectContext.Users.Add(user);
        }

        public void Delete(User user)
        {
            ObjectContext.Users.Remove(user);
        }

        void IUserRepository.Save()
        {
            base.Save();
        }

        //public IQueryable<User> FindUsersByEmail(string email)
        //{
            
        //}

        //public IQueryable<User> FindUsersByName(string username)
        //{
        //    return ObjectContext.Users.Where(u => u.Username.ToLower() == username.ToLower());
        //}

        public IQueryable<User> FindAllUsers()
        {
            return ObjectContext.Users.AsQueryable();
        }
    }
}
