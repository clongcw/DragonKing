using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonKing.Database.EntityModel;
using DragonKing.Database.Interface;
using DragonKing.Log.Interface;
using DragonKing.View.UserManagement;
using System;
using System.Collections.Generic;
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
        [ObservableProperty]
        private User _selectedUser;
        [ObservableProperty]
        private Role _selectedRole;

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
            if (SelectedUser != null)
            {
                SelectedUser = _userService.GetUserByName(SelectedUser.Name);
                SelectedUser.RoleNames = RoleNames;
                string rolename = SelectedUser.Role.RoleName;
                int roleid = _roleService.GetRoleByRoleName(rolename).Id;
                SingleUser singleUser = new SingleUser()
                {
                    DataContext = SelectedUser,
                    WindowStyle = System.Windows.WindowStyle.None,
                };
                var result = singleUser.ShowDialog();
                if (result == true)
                {
                    //第一步，先修改用户
                    SelectedUser.RoleId = _roleService.GetRoleByRoleName(SelectedUser.Role.RoleName).Id;
                    SelectedUser.Role = _roleService.GetRoleById(SelectedUser.RoleId);
                    SelectedUser.Password = SelectedUser.Password;
                    _userService.UpdateUser(SelectedUser);

                    //第二步，再修改角色
                    Role role = _roleService.GetRoleById(roleid);
                    role.RoleName = rolename;//因为刚刚界面选择的时候改变了role的id对应的rolename，这里把rolename变回来
                    
                    _roleService.UpdateRole(role);

                    GetAllUsers();
                    GetAllRoles();
                }
            }
        }

        [RelayCommand]
        public void DeleteUser(object command)
        {

        }

        [RelayCommand]
        public void UpdateRole(object command)
        {
            SelectedRole = _roleService.GetRoleById(SelectedRole.Id);

            SingleRole singleRole = new SingleRole()
            {
                DataContext = SelectedRole,
                WindowStyle = System.Windows.WindowStyle.None,
            };
            var result = singleRole.ShowDialog();
            if (result == true)
            {
                _roleService.UpdateRole(SelectedRole);
                GetAllRoles();
            }
        }

        [RelayCommand]
        public void DeleteRole(object command)
        {

        }
    }
}
