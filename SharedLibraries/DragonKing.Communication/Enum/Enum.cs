using System.ComponentModel;

namespace DragonKing.Communication
{
    /// <summary>
    /// 波特率
    /// </summary>
    public enum BaudRate
    {
        BAUD_10K,
        BAUD_20K,
        BAUD_50K,
        BAUD_100K,
        BAUD_125K,
        BAUD_250K,
        BAUD_500K,
        BAUD_800K,
        BAUD_1000K
    }

    /// <summary>
    /// 帧类型
    /// </summary>
    public enum FrameFlag
    {
        Stanard, //标准帧
        Extern   //扩展帧
    }

    /// <summary>
    /// 帧格式
    /// </summary>
    public enum FrameMode
    {
        Data,  //数据帧
        Remote //远程帧
    }

    /// <summary>
    /// 液体类型
    /// </summary>
    public enum LiquidType
    {
        [Description("水或试剂")]
        Water = 0x01,
        [Description("血清")]
        Serum,
        [Description("其它")]
        Other,
    }

    public enum GroupId
    {
        Pipette900_ChannelA = 0,//排枪 900tip头
        Pipette900_ChannelB = 1,
        Pipette900_ChannelC = 2,
        Pipette900_ChannelD = 3,

        Pipette175_ChannelA = 4,//排枪 175tip头
        Pipette175_ChannelB = 5,
        Pipette175_ChannelC = 6,
        Pipette175_ChannelD = 7,

        Adp175_ChannelA = 10,//Adp 175tip头
        Adp175_ChannelB = 11,
        Adp175_ChannelC = 12,
        Adp175_ChannelD = 13,
        TipBox175 = 14,
        Adp900_ChannelA = 15,//Adp 900tip头
        Adp900_ChannelB = 16,
        Adp900_ChannelC = 17,
        Adp900_ChannelD = 18,




        Scanner_ChannelA = 30,//扫码枪A通道
        Scanner_ChannelB = 31,
        Scanner_ChannelC = 32,
        Scanner_ChannelD = 33,

        ReagentScanner_ChannelA = 40,//试剂扫码枪A通道
        ReagentScanner_ChannelB = 41,
        ReagentScanner_ChannelC = 42,
        ReagentScanner_ChannelD = 43,

        SealingYCollection = 50,//热封Y收集位置
        SealingYSealing = 51,//热封Y热封位置
        SealingYHome = 52,
        SealingZSealing = 53,
        SealingZHome = 54,
        //Adp900_ChannelA = 14,//Adp 900tip头
        //Adp900_ChannelB = 15,
        //Adp900_ChannelC = 16,
        //Adp900_ChannelD = 17,
    }
    //GroupId格式
    //channel+Tip+单个模块

    /// <summary>
    /// 通道
    /// </summary>
    public enum Channels
    {
        A,
        B,
        C,
        D,
        Tip,
        Pcr1,
        Pcr2,
        Pcr3,
        Pcr4
    }

    /// <summary>
    /// 通道
    /// </summary>
    public enum HeatingBlocks
    {
        A,
        B,
    }
    /// <summary>
    /// Tip头类型
    /// </summary>
    public enum Tips
    {
        Tip900,
        Tip175 = 2,
    }

    /// <summary>
    /// 模块
    /// </summary>
    public enum Module
    {
        Adp = 10,//adp
        Pipette,//排枪
        Scanner,//扫码枪
        ReagentScanner,//试剂扫码枪
    }


    /// <summary>
    /// 项目类型
    /// </summary>
    public enum ProjectType
    {
        [Description("定性")]
        QualitativePCR,
        [Description("定量")]
        RealtimePCR,
        [Description("溶解曲线")]
        Other,
    }
    /// <summary>
    /// 样本类型
    /// </summary>
    public enum SampleType
    {
        [Description("标准品")]
        Standard,
        [Description("阴性对照")]
        Negative,
        [Description("阳性对照")]
        Positive,
        [Description("空白对照")]
        NoTemplateControl,
        [Description("未知样本")]
        Unknown,
    }
    public enum RetryModule
    {
        LiqHa,
        HandleX,
        HandleZ,
        Scanner

    }
    public enum RetryFlag
    {
        /// <summary>
        /// 终止
        /// </summary>
        Abort,
        /// <summary>
        /// 重试
        /// </summary>
        Retry,
        /// <summary>
        /// 忽略
        /// </summary>
        Ignore,
    }

    public enum ReporterType
    {
        FAM = 1,
        HEX,
        ROX,
        Cy5,
        VIC,
        Quasar705
    }

    public enum Gender
    {
        Male,
        Female
    }
    public enum ThresholdMethod
    {
        [Description("自动阈值")]
        AutoMean,
        [Description("手动阈值")]
        Manual
    }

    /// <summary>
    /// 检测结果
    /// </summary>
    public enum ResultType
    {
        [Description("阴性")]
        Negative,
        [Description("阳性")]
        Positive
    }
}
