//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;

namespace JordanRift.Grassroots.Tests.Fakes
{
    [Obsolete("This class will be obsolete in future versions in favor of using Rhino Mocks. See DonateControllerTests for example of new pattern.")]
    public class FakeUserRepository : IUserRepository
    {
        private static List<User> users;

        public void SetUpRepository()
        {
            users = new List<User>();

            for (int i = 0; i < 5; i++)
            {
                var user = EntityHelpers.GetValidUser();
                user.Username += i.ToString();
                users.Add(user);
            }
        }

        public User GetUserByName(string name)
        {
            return users.FirstOrDefault(u => u.Username.ToLower() == name.ToLower());
        }

        public void Add(User user)
        {
            users.Add(user);
        }

        public void Delete(User user)
        {
            users.Remove(user);
        }

        public void Save()
        {
        }

        //public IQueryable<User> FindUsersByEmail(string email)
        //{
        //    throw new NotImplementedException();
        //}

        //public IQueryable<User> FindUsersByName(string username)
        //{
        //    throw new NotImplementedException();
        //}

        public IQueryable<User> FindAllUsers()
        {
            return users.AsQueryable();
        }
    }
}
