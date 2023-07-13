using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonKing.Database.EntityModel;
using DragonKing.Database.Interface;
using DragonKing.Log.Interface;
using DragonKing.Utils;
using DragonKing.View;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Panuon.WPF.UI;
using System;
using System.IO;
using System.Windows;

namespace DragonKing.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        #region Property
        [ObservableProperty]
        private string _username;
        [ObservableProperty]
        private string _password;
        [ObservableProperty]
        private bool _enabled;
        [ObservableProperty]
        private User _currentUser;
        #endregion


        private readonly ILog _log;

        private readonly IUserService _userService;

        public LoginViewModel(ILog log, IUserService userService)
        {
            _log = log;
            _userService = userService;

            string jsonPath = Environment.CurrentDirectory + @"\Configuration";
            string jsonName = @"user.json";
            string wholeName = Path.Combine(jsonPath, jsonName);

            #region json文件不存在就创建
            if (!Directory.Exists(jsonPath))
            {
                // 创建文件夹并设置文件夹的访问权限为可读可写
                Directory.CreateDirectory(jsonPath).Attributes = FileAttributes.Normal;
            }

            if (!File.Exists(wholeName))
            {
                // 创建文件
                using (FileStream fs = File.Create(wholeName)) ;
                User user = _userService.GetUserByName("Admin");
                JsonUtil.WriteJsonFile(wholeName, JsonConvert.SerializeObject(user));
            }
            #endregion


            string userstring = JsonUtil.ReadJsonFile(wholeName);

            CurrentUser = JsonConvert.DeserializeObject<User>(userstring);

            if (CurrentUser.Enabled)
            {
                Username = CurrentUser.Name;
                Password = CurrentUser.Password;
                Enabled = CurrentUser.Enabled;
            }
        }

        #region 登录
        [RelayCommand]
        public void SignIn()
        {
            User user = _userService.GetUserByName(Username);



            if (user != null && user.Password == Password)
            {
                #region 记住登录信息
                //将当前的配置序列化为json字符串
                CurrentUser.Enabled = Enabled;
                CurrentUser.Enabled = Enabled;
                CurrentUser.Name = Username;
                CurrentUser.Password = Password;
                var content = JsonConvert.SerializeObject(CurrentUser);
                JsonUtil.WriteJsonFile(Environment.CurrentDirectory + @"\Configuration\user.json", content);
                #endregion

                var mainView = App.Current._host.Services.GetRequiredService<MainView>();
                mainView.DataContext = App.Current._host.Services.GetRequiredService<MainViewModel>();
                mainView!.Show();

                App.Current._host.Services.GetRequiredService<LoginView>().Close();

                _log.Debug($"此次登录的用户名：{Username}，密码：{Password}，登录时间：{DateTime.Now}");
            }
            else
            {
                MessageBoxX.Show(Application.Current.MainWindow, "用户名或密码错误！", "提示", MessageBoxButton.OK, MessageBoxIcon.Error, DefaultButton.YesOK, 5);
            }



        }
        #endregion
    }
}
