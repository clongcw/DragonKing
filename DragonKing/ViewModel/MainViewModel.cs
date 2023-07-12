using CommunityToolkit.Mvvm.ComponentModel;
using DragonKing.Database.Interface;

namespace DragonKing.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public MainViewModel(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }
    }
}
