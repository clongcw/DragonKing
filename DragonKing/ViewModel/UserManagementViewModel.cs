using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DragonKing.Database.EntityModel;
using DragonKing.Database.Interface;
using DragonKing.Log.Interface;
using DragonKing.View.UserManagement;
using NetTaste;
using Panuon.WPF.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

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
        private User _currentUser;
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
            if (_roleService != null)
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
        public void AddUser(object command)
        {
            CurrentUser = new User();
            CurrentUser.RoleNames = RoleNames;
            CurrentUser.Role = new Role();
            SingleUser singleUser = new SingleUser()
            {
                DataContext = CurrentUser,
                WindowStyle = System.Windows.WindowStyle.None,
            };
            var result = singleUser.ShowDialog();
            if (result == true)
            {
                CurrentUser.RoleId = _roleService.GetRoleByRoleName(CurrentUser.Role.RoleName).Id;
                CurrentUser.Role = _roleService.GetRoleById(CurrentUser.RoleId);
                CurrentUser.Password = CurrentUser.Password;
                CurrentUser.Role.Users.Add(CurrentUser);
                _userService.AddUser(CurrentUser);


                GetAllUsers();
            }
        }

        [RelayCommand]
        public void UpdateUser(object command)
        {
            if (SelectedUser != null)
            {
                SelectedUser = _userService.GetUserByName(SelectedUser.Name);
                SelectedUser.RoleNames = RoleNames;

                if (SelectedUser.Role == null)
                {
                    SelectedUser.Role = new Role();
                    SelectedUser.Role.RoleName = "普通用户";
                }
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
            if (UserList.Count > 1)
            {
                _userService.RemoverUser(SelectedUser);
                GetAllRoles();
                GetAllUsers();
            }
            else
            {
                MessageBoxX.Show(Application.Current.MainWindow, "仅剩一位用户，禁止删除！", "提示", MessageBoxButton.OK, MessageBoxIcon.Warning, DefaultButton.YesOK, 5);
            }
        }

        [RelayCommand]
        public void AddRole(object command)
        {
            Role role = new Role();
            role.Privileges = new List<Privilege>();
            role.Privileges.Add(new Privilege { Name = "Lis", });
            role.Privileges.Add(new Privilege { Name = "Report", });
            role.Privileges.Add(new Privilege { Name = "Setting", });
            SingleRole singleRole = new SingleRole()
            {
                DataContext = role,
                WindowStyle = System.Windows.WindowStyle.None,
            };
            var result = singleRole.ShowDialog();
            if (result == true)
            {
                _roleService.AddRole(role);
                GetAllRoles();
            }
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
            _roleService.RemoverRole(SelectedRole);
            GetAllRoles();
            GetAllUsers();
        }
    }
}
