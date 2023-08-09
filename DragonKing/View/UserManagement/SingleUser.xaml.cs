using Panuon.WPF.UI;
using System.Windows;

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
