using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonKing.Database.EntityModel;
using DragonKing.Database.Interface;
using DragonKing.UI.Utils;
using DragonKing.View;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DragonKing.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        #region Property
        [ObservableProperty]
        private object _content;
        [ObservableProperty]
        private object _currentDateTime;
        [ObservableProperty]
        private Stopwatch _stopwatch;
        [ObservableProperty]
        private DispatcherTimer _timer;
        [ObservableProperty]
        private User _user;
        #endregion

        public MainViewModel(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;

            Content = App.Current._host.Services.GetService<GMView>();
            User = Const.User;

            #region 刷新时间
            _stopwatch = new Stopwatch();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); // 每秒触发一次
            _timer.Tick += (s, e) =>
            {
                CurrentDateTime = DateTime.Now.ToString("yyyy年M月d日 HH:mm:ss dddd", new CultureInfo("zh-CN"));
            };
            _stopwatch.Start();
            _timer.Start();
            #endregion
        }

        [RelayCommand]
        public void SelectionChanged(object listboxitem)
        {
            var viewname = string.Empty;

            if (listboxitem as ListBoxItem != null)
            {
                var textBlock = WPFUtil.FindVisualChild<TextBlock>(listboxitem as ListBoxItem);
                if (textBlock != null)
                {
                    viewname = textBlock.Text;
                }
            }

            switch (viewname)
            {
                case "主页":
                    Content = App.Current._host.Services.GetService<GMView>();
                    break;
                case "结果查询":
                    Content = App.Current._host.Services.GetService<ResultView>();
                    break;
                case "Office":
                    Content = App.Current._host.Services.GetService<OfficeView>();
                    break;
                case "线程":
                    Content = App.Current._host.Services.GetService<TaskView>();
                    break;
                case "设置":
                    Content = App.Current._host.Services.GetService<SettingsView>();
                    break;
                default:
                    break;
            }
        }
    }
}
