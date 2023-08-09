using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonKing.Communication;
using DragonKing.Communication.Device;
using DragonKing.View;
using System.Diagnostics;

namespace DragonKing.ViewModel
{
    public partial class TestViewModel : ObservableObject
    {
        #region Property
        [ObservableProperty]
        private string? _message;
        [ObservableProperty]
        private bool _isActive;
        [ObservableProperty]
        private GcCanPort _gcPort;
        [ObservableProperty]
        private Adp _adp;
        #endregion



        #region Command
        [RelayCommand]
        public void ShowHello()
        {
            IsActive = true;
            Message = "测试的不错";
            Trace.Write(Message);
        }

        [RelayCommand]
        public void ShowOffice()
        {
            var office = new DocumentEditorDemo();
            office.Show();

        }
        #endregion



        public TestViewModel()
        {
            GcPort = new GcCanPort(101);
            Adp = new Adp(GcPort, 1);
            //Adp.Init();
        }
    }
}
