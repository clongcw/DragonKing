using CommunityToolkit.Mvvm.ComponentModel;
using DragonKing.Database.EntityModel;
using DragonKing.Database.Interface;
using System.Collections.ObjectModel;

namespace DragonKing.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        #region Property
        [ObservableProperty]
        private ObservableCollection<User> _users;
        #endregion

        public MainViewModel(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;

            _users = new ObservableCollection<User>();
            _users.Add(new User { Name = "66", Password = "66", RoleId = 66 });
            _users.Add(new User { Name = "66", Password = "66", RoleId = 66 });
            _users.Add(new User { Name = "66", Password = "66", RoleId = 66 });
            _users.Add(new User { Name = "66", Password = "66", RoleId = 66 });
        }
    }
}
