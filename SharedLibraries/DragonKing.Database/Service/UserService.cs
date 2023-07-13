using DragonKing.Database.DbContext;
using DragonKing.Database.EntityModel;
using DragonKing.Database.Interface;
using System;
using System.Collections.Generic;

namespace DragonKing.Database.Service
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _context;

        public UserService(UserDbContext dbContext)
        {
            _context = dbContext;
        }

        public bool AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool CheckPassword(string name)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUserByName(string name)
        {
            return _context.Db.Queryable<User>()
                            .Where(p => p.Name == name)
                            .Includes(t => t.Role, s => s.Privileges)
                            .First();

        }

        public List<User> GetUsers()
        {
            return _context.UserDb.Context.Queryable<User>()
                .Includes(t => t.Role, t => t.Privileges)
                .ToList();
        }

        public void RemoverUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePassword(User user, string strPassword, bool isReset)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(User user)
        {
            return _context.UserDb.Context.UpdateNav(user)
                .Include(s => s.Role)
                .ThenInclude(s => s.Privileges)
                .ExecuteCommand();

        }

        public bool UpdateUser()
        {
            throw new NotImplementedException();
        }
    }
}
