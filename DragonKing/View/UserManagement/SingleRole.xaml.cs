using Panuon.WPF.UI;
using System.Windows;

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
