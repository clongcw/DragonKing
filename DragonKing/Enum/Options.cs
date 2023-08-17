using DragonKing.Converter;
using System.ComponentModel;

namespace DragonKing.Enum
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Options : int
    {
        [Description("横向数据源")]
        横向数据源 = 0,
        [Description("纵向数据源")]
        纵向数据源 = 1
    }
}
