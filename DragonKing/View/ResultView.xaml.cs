﻿using DragonKing.UI.Utils;
using DragonKing.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
