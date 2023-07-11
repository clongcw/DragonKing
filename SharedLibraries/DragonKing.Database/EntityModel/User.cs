using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DragonKing.Database.EntityModel
{
    public class User
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int RoleId { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(RoleId))]
        public Role Role { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        [SugarColumn(IsNullable = true)]
        public bool Enable { get; set; }

        public bool IsAdmin { get; set; } = false;
        [SugarColumn(IsNullable = true)]
        public int Sequence { get; set; }
        [SugarColumn(IsNullable = true)]
        public string GroupName { get; set; }
        [SugarColumn(IsNullable = true)]
        public int LoginTimes { get; set; }

        public bool Enabled { get; set; } = true;

        public DateTime CreateDate { get; set; }
        [SugarColumn(IsNullable = true)]
        public string Creator { get; set; }
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime LastLoginTime { get; set; }
        [SugarColumn(IsNullable = true)]
        public int WrongPwdTimes { get; set; }
        [SugarColumn(IsNullable = true)]
        public bool IsDefault { get; set; }

        [SugarColumn(IsIgnore = true)]
        public ICommand UpdateUserCommand { get; set; }
        [SugarColumn(IsIgnore = true)]
        public ICommand DeleteUserCommand { get; set; }
        [SugarColumn(IsIgnore = true)]
        public ObservableCollection<string> RoleNames { get; set; }
    }
}
