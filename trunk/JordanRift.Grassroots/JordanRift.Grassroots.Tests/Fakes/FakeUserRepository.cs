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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;

namespace JordanRift.Grassroots.Tests.Fakes
{
    [Export(typeof(IUserRepository))]
    public class FakeUserRepository : IUserRepository
    {
        private static List<User> users;
        public PriorityType Priority { get; set; }

        static FakeUserRepository()
        {
            SetUp();
        }

        private static void SetUp()
        {
            users = new List<User>();

            for (int i = 0; i < 5; i++)
            {
                var user = EntityHelpers.GetValidUser();
                user.Username += i.ToString();
                users.Add(user);
            }
        }

        public FakeUserRepository()
        {
            Priority = PriorityType.High;
        }

        public static void Reset()
        {
            SetUp();
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

        public void Dispose()
        {
        }
    }
}
