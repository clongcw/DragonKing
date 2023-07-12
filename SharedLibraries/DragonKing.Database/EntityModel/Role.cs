using SqlSugar;
using System.Collections.Generic;
using System.Windows.Input;

namespace DragonKing.Database.EntityModel
{
    public class Role
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string RoleName { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(Privilege.RoleId))]
        public List<Privilege> Privileges { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(User.RoleId))]
        public List<User> Users { get; set; }

        [SugarColumn(IsIgnore = true)]
        public ICommand UpdateRoleCommand { get; set; }
        [SugarColumn(IsIgnore = true)]
        public ICommand DeleteRoleCommand { get; set; }
    }
}
