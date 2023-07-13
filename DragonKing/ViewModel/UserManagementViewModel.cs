using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonKing.Database.EntityModel;
using DragonKing.Database.Interface;
using DragonKing.Log.Interface;
using System;
using System.Collections.ObjectModel;

namespace DragonKing.ViewModel
{
    public partial class UserManagementViewModel : ObservableObject
    {
        private readonly ILog _log;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        [ObservableProperty]
        private User _user;
        [ObservableProperty]
        private ObservableCollection<User> _userList = new ObservableCollection<User>();
        [ObservableProperty]
        private ObservableCollection<Role> _roleList = new ObservableCollection<Role>();
        [ObservableProperty]
        private ObservableCollection<string> _roleNames = new ObservableCollection<string>();

        public UserManagementViewModel(ILog log, IUserService userService, IRoleService roleService)
        {
            _log = log;
            _roleService = roleService;
            _userService = userService;

            GetAllUsers();
            GetAllRoles();
        }

        private void GetAllUsers()
        {
            if (_userService != null)
            {
                UserList.Clear();
                var query = _userService.GetUsers();

                foreach (var item in query)
                {
                    item.UpdateUserCommand = UpdateUserCommand;
                    item.DeleteUserCommand = DeleteUserCommand;
                    UserList.Add(item);
                }

            }
        }

        private void GetAllRoles()
        {
            if (_userService != null)
            {
                RoleList.Clear();
                var query = _roleService.GetRoles();

                foreach (var item in query)
                {
                    item.UpdateRoleCommand = UpdateRoleCommand;
                    item.DeleteRoleCommand = DeleteRoleCommand;
                    RoleList.Add(item);
                }
                RoleNames.Clear();
                foreach (var item in RoleList)
                {
                    RoleNames.Add(item.RoleName);
                }

            }
        }

        [RelayCommand]
        public void UpdateUser(object command)
        {

        }

        [RelayCommand]
        public void DeleteUser(object command)
        {

        }

        [RelayCommand]
        public void UpdateRole(object command)
        {

        }

        [RelayCommand]
        public void DeleteRole(object command)
        {

        }
    }
}
