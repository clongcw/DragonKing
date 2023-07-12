using DragonKing.Database.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonKing.Database.Interface
{
    public interface IRoleService
    {
        List<Role> GetRoles();
        bool AddRole(Role role);
        void RemoverRole(Role role);
        bool UpdateRole(Role role);
        Role GetRoleById(int id);
        Role GetRoleByRoleName(string rolename);

    }
}
