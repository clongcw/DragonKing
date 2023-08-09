using System;
using System.Globalization;

namespace DragonKing.Converter
{
    public class CNToENVisualConverter : ValueConverterBase<CNToENVisualConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string CN = (string)value;
            if (CN == "Visible")
            {
                return "显示";
            }
            else if (CN == "Collapsed")
            {
                return "不显示";
            }
            else
            {
                return "不显示";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string CN = (string)value;
            if (CN == "显示")
            {
                return "Visible";
            }
            else if (CN == "不显示")
            {
                return "Collapsed";
            }
            else
            {
                return "Collapsed";
            }
        }
    }
}
