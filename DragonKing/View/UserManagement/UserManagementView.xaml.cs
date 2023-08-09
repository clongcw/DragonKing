using DragonKing.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Controls;

namespace DragonKing.View.UserManagement
{
    /// <summary>
    /// UserManagementView.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagementView : UserControl
    {
        public UserManagementView()
        {
            InitializeComponent();
            this.DataContext = App.Current._host.Services.GetService<UserManagementViewModel>();
        }
    }
}
