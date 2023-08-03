using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonKing.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DragonKing.ViewModel
{
    public partial class ResultViewModel : ObservableObject
    {
        #region Property
        [ObservableProperty]
        private List<RecordListItemModel> _recordList = new();
        [ObservableProperty]
        private ObservableCollection<RecordListItemModel> _recordListDisplay = new ObservableCollection<RecordListItemModel>();
        [ObservableProperty]
        private string _currentPage = "1";
        [ObservableProperty]
        private double _TotalPages = 0;

        public const double PageSize = 10;
        #endregion

        public ResultViewModel()
        {
            InitRecords();
        }

        private void InitRecords()
        {
            for (int i = 0; i < 101; i++)
            {
                RecordList.Add(new RecordListItemModel() { Id = i, Channel = i.ToString(), ProjectId = i + 2 });
            }
            TotalPages = Math.Ceiling(RecordList.Count / PageSize);
            GoToPage();
        }

        [RelayCommand]
        public void GoToPage()
        {
            var p = Convert.ToInt32(CurrentPage);
            RecordListDisplay = new ObservableCollection<RecordListItemModel>(RecordList.Skip((int)(PageSize * (p - 1))).Take((int)PageSize));

        }
    }
}
