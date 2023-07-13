using DragonKing.Database.DbContext;
using DragonKing.Database.EntityModel;
using DragonKing.Database.Interface;
using System;
using System.Collections.Generic;

namespace DragonKing.Database.Service
{
    public class RoleService : IRoleService
    {
        private readonly UserDbContext _context;

        public RoleService(UserDbContext dbContext)
        {
            _context = dbContext;
        }

        public bool AddRole(Role role)
        {
            throw new NotImplementedException();
        }

        public Role GetRoleById(int id)
        {
            return _context.RoleDb.Context.Queryable<Role>()
                    .Where(s => s.Id == id)
                    .Includes(p => p.Privileges, s => s.Role)
                    .Includes(p => p.Users)
                    .First();
        }

        public Role GetRoleByRoleName(string rolename)
        {
            return _context.RoleDb.Context.Queryable<Role>()
                    .Where(s => s.RoleName == rolename)
                    .Includes(p => p.Privileges, s => s.Role)
                    .Includes(p => p.Users)
                    .First();
        }

        public List<Role> GetRoles()
        {
            return _context.RoleDb.Context.Queryable<Role>()
                .Includes(t => t.Privileges)
                .Includes(t => t.Users)
                .ToList();
        }

        public void RemoverRole(Role role)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRole(Role role)
        {
            bool result = _context.RoleDb.Context.UpdateNav(role)
                .Include(s => s.Privileges)
                .Include(s => s.Users)
                .ExecuteCommand();
            return result;
        }
    }
}
