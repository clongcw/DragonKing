using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonKing.Model
{
    public class TipModel : ObservableObject
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                SetProperty(ref _id, value);
            }
        }

        private bool _isUsed;

        public bool IsUsed
        {
            get { return _isUsed; }
            set
            {
                if (_isUsed != value)
                {
                    _isUsed = value;
                    SetProperty(ref _isUsed, value );
                }
            }
        }

        private bool _isValid;
        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    SetProperty(ref _isValid, value );
                }
            }
        }

        public TipModel(int id)
        {
            Id = id;
            IsUsed = false;
            IsValid = true;
        }
    }
}
