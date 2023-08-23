using CommunityToolkit.Mvvm.ComponentModel;
using DragonKing.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace DragonKing.ViewModel
{
    public partial class GMViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ChannelCtrlModel> _channelCtrls = new ObservableCollection<ChannelCtrlModel>();
        [ObservableProperty]
        private ObservableCollection<PcrStateModel> _pcrStates = new ObservableCollection<PcrStateModel>();
        [ObservableProperty]
        private TipBoxHModel _tipBoxHModel0 = new TipBoxHModel();
        [ObservableProperty]
        private TipBoxHModel _tipBoxHModel1 = new TipBoxHModel();
        #region Property
        #endregion

        public GMViewModel()
        {
            for (int i = 0; i < 4; i++)
            {
                ChannelCtrls.Add(new ChannelCtrlModel() { MpIndex = i });
                PcrStates.Add(new PcrStateModel() { MpIndex = i });
            }
        }
    }
}
