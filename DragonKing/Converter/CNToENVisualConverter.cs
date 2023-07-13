using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DragonKing.Converter
{
    public class CNToENVisualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
