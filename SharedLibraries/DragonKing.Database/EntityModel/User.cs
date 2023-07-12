using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DragonKing.Database.EntityModel
{
    public class User
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        [JsonIgnore]
        public int RoleId { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(RoleId))]
        [JsonIgnore]
        public Role Role { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public bool IsAdmin { get; set; } = false;
        [SugarColumn(IsNullable = true)]
        [JsonIgnore]
        public int Sequence { get; set; }
        [SugarColumn(IsNullable = true)]
        [JsonIgnore]
        public string GroupName { get; set; }
        [SugarColumn(IsNullable = true)]
        [JsonIgnore]
        public int LoginTimes { get; set; }

        public bool Enabled { get; set; } = true;
        [JsonIgnore]
        public DateTime CreateDate { get; set; }
        [SugarColumn(IsNullable = true)]
        [JsonIgnore]
        public string Creator { get; set; }
        [SugarColumn(IsNullable = true)]
        [JsonIgnore]
        public string Remark { get; set; }
        [SugarColumn(IsNullable = true)]
        [JsonIgnore]
        public DateTime LastLoginTime { get; set; }
        [SugarColumn(IsNullable = true)]
        [JsonIgnore]
        public int WrongPwdTimes { get; set; }
        [SugarColumn(IsNullable = true)]
        [JsonIgnore]
        public bool IsDefault { get; set; }

        [SugarColumn(IsIgnore = true)]
        [JsonIgnore]
        public ICommand UpdateUserCommand { get; set; }
        [SugarColumn(IsIgnore = true)]
        [JsonIgnore]
        public ICommand DeleteUserCommand { get; set; }
        [SugarColumn(IsIgnore = true)]
        [JsonIgnore]
        public ObservableCollection<string> RoleNames { get; set; }
    }
}
