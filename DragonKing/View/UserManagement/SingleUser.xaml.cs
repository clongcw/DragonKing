﻿using Panuon.WPF.UI;
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
using System.Windows.Shapes;

namespace DragonKing.View.UserManagement
{
    /// <summary>
    /// SingleUser.xaml 的交互逻辑
    /// </summary>
    public partial class SingleUser : WindowX
    {
        public SingleUser()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirme_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}