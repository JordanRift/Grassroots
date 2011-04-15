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
    public interface IUserRepository
    {
        IQueryable<User> FindAllUsers();
        User GetUserByName(string name);
        void Add(User user);
        void Delete(User user);
        void Save();
        //IQueryable<User> FindUsersByEmail(string email);
        //IQueryable<User> FindUsersByName(string username);
    }
}
