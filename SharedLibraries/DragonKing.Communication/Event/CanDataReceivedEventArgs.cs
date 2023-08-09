using System;

namespace DragonKing.Communication
{
    public class CanDataReceivedEventArgs : EventArgs
    {
        public readonly CanData CanData;
        public CanDataReceivedEventArgs(CanData canData)
        {
            CanData = canData;
        }
    }
}
