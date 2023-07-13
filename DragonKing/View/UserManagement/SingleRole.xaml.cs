using Panuon.WPF.UI;
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
    /// SingleRole.xaml 的交互逻辑
    /// </summary>
    public partial class SingleRole : WindowX
    {
        public SingleRole()
        {
            InitializeComponent();

            string[] ports = new string[] { "不显示", "显示" };
            visuallist.ItemsSource = ports;
            sunFunvisuallist.ItemsSource = ports;
            visuallistbutton.ItemsSource = ports;
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
