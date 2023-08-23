using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace DragonKing.Model
{
    public class ChannelModel : ObservableObject
    {
        private int _id = 0;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                SetProperty(ref _id, value);
            }
        }

        public List<WellModel> WellList { get; } = new List<WellModel>();

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

        private bool _isLoaded = false;

        public bool IsLoaded
        {
            get { return _isLoaded; }
            set
            {
                _isLoaded = value;
                SetProperty(ref _isLoaded, value);
            }
        }

        public ChannelModel(int id)
        {
            Id = id;
            var index = 0;
            var startName = 'A';
            for (int i = 0; i < 6; i++)
            {
                var name = ((char)(startName + i)).ToString();
                WellList.Add(new WellModel() { Id = index, WellType = (int)WellType.Reagent, Name = name });
                index++;
            }

            WellList.Add(new WellModel() { Id = index, WellType = (int)WellType.Ext, Name = DragonKing.LocationName.Extraction });
            index++;

            for (int i = 0; i < 3; i++)
            {
                var name = DragonKing.LocationName.GetTipName(i);
                WellList.Add(new WellModel() { Id = index, WellType = (int)WellType.Tip, Name = name });
                index++;
            }
            WellList.Add(new WellModel() { Id = index, WellType = (int)WellType.IC, Name = DragonKing.LocationName.LocIC });
            index++;
            WellList.Add(new WellModel() { Id = index, WellType = (int)WellType.Waste, Name = DragonKing.LocationName.Waste });
            index++;
            for (int i = 0; i < 6; i++)
            {
                var name = DragonKing.LocationName.GetPcrName(i);
                WellList.Add(new WellModel() { Id = index, WellType = (int)WellType.Temp, Name = name });
                index++;
            }
            WellList.Add(new WellModel() { Id = index, WellType = (int)WellType.Smp, Name = DragonKing.LocationName.Sample });

        }
    }
}
