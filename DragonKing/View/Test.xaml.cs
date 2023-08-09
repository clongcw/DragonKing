using DragonKing.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Controls;

namespace DragonKing.View
{
    /// <summary>
    /// Test.xaml 的交互逻辑
    /// </summary>
    public partial class Test : UserControl
    {
        public Test()
        {
            InitializeComponent();
            this.DataContext = App.Current._host.Services.GetService<TestViewModel>();
        }
    }
}
