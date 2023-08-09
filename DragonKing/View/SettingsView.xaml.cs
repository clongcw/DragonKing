using DragonKing.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Controls;

namespace DragonKing.View
{
    /// <summary>
    /// SettingsView.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            this.DataContext = App.Current._host.Services.GetService<SettingsViewModel>();

        }


    }
}
