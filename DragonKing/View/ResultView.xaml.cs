using DragonKing.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Controls;

namespace DragonKing.View
{
    /// <summary>
    /// ResultView.xaml 的交互逻辑
    /// </summary>
    public partial class ResultView : UserControl
    {
        public ResultView()
        {
            InitializeComponent();
            this.DataContext = App.Current._host.Services.GetService<ResultViewModel>();

        }
    }
}
