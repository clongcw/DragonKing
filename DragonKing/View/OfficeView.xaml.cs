using DragonKing.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Controls;

namespace DragonKing.View
{
    /// <summary>
    /// OfficeView.xaml 的交互逻辑
    /// </summary>
    public partial class OfficeView : UserControl
    {
        public OfficeView()
        {
            InitializeComponent();
            this.DataContext = App.Current._host.Services.GetService<OfficeViewModel>();


        }
    }
}
