using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonKing.Model
{
    public class PcrStateModel : ObservableObject
    {
        private int _groupId = 0;
        private int _mpIndex = 0;

        public int MpIndex
        {
            get { return _groupId; }
            set
            {
                _groupId = value;
                SetProperty(ref _mpIndex, value);
            }
        }
        private int _totalCycle;
        public int TotalCycle
        {
            get => _totalCycle;
            set => SetProperty(ref _totalCycle, value);
        }

        private int _currentCycle;

        public int CurrentCycle
        {
            get => _currentCycle;
            set => SetProperty(ref _currentCycle, value);
        }

        public delegate void DelegateRefreshCycle(int total, int current);

        //public DelegateRefreshCycle OnRefreshCycle = null;

        public void RefreshCycle(int total, int current)
        {
            TotalCycle = total;
            CurrentCycle = current;


        }
    }
}
