using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DragonKing.Model
{
    public class ChannelCtrlModel : ObservableObject
    {
        #region Property
        private int _groupId = 0;
        private int _mpIndex = 0;

        public int MpIndex
        {
            get { return _groupId; }
            set
            {
                _groupId = value;
                GroupName = ((char)('A' + _groupId)).ToString();
                SetProperty(ref _mpIndex, value);
            }
        }

        private string _groupName = "A";
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                _groupName = value;
                SetProperty(ref _groupName, value);
            }
        }

        private bool _isEmpty = true;

        public bool IsEmpty
        {
            get { return _isEmpty; }
            set
            {
                _isEmpty = value;
                SetProperty(ref _isEmpty, value);
            }
        }

        private string _currentProcess = "未开始";

        public string CurrentProcess
        {
            get { return _currentProcess; }
            set
            {
                _currentProcess = value;
                SetProperty(ref _currentProcess, value);
            }
        }

        private int _leftTime = 0;

        public int LeftTime
        {
            get { return _leftTime; }
            set
            {
                _leftTime = value;
                if (_leftTime < 0)
                {
                    _leftTime = 120;
                }
                SetProperty(ref _leftTime, value);
            }
        }

        private bool _isRunning = false;

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                if (value != _isRunning)
                {
                    if (!_isRunning)
                    {
                        _timerRefresh.Start();
                    }
                    else
                    {
                        _timerRefresh.Stop();
                    }
                    _isRunning = value;
                    SetProperty(ref _isRunning, value);
                }
            }
        }

        private DispatcherTimer _timerRefresh = new DispatcherTimer();

        private void _timerRefresh_Tick(object sender, EventArgs e)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                LeftTime--;
            });
        }

        public ObservableCollection<ChannelModel> ChannelModels { get; } = new ObservableCollection<ChannelModel>();
        #endregion

        public ChannelCtrlModel()
        {
            for (int i = 0; i < 4; i++)
            {
                ChannelModels.Add(new ChannelModel(i));
            }

            _timerRefresh.Interval = TimeSpan.FromSeconds(1);
            _timerRefresh.Tick += _timerRefresh_Tick;
        }
    }
}
