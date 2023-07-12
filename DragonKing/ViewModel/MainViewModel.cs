using CommunityToolkit.Mvvm.ComponentModel;
using DragonKing.Database.DbContext;
using DragonKing.Database.Interface;
using DragonKing.Database.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
