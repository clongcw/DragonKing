using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonKing.Communication
{
    public struct CanData
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public UInt32 TimeStamp;

        /// <summary>
        /// 帧类型
        /// </summary>
        public FrameFlag FrameFlag;

        /// <summary>
        /// 帧格式
        /// </summary>
        public FrameMode FrameMode;

        /// <summary>
        /// 帧ID
        /// </summary>
        public UInt32 ID;

        /// <summary>
        /// 帧数据长度
        /// </summary>
        public byte Length;

        /// <summary>
        /// 帧数据
        /// </summary>
        public byte[] Data;

        public void Init()
        {
            Data = new byte[8];
        }

        public CanData Clone()
        {
            CanData cloneData = new CanData();
            cloneData.Init();
            cloneData.TimeStamp = TimeStamp;
            cloneData.FrameFlag = FrameFlag;
            cloneData.FrameMode = FrameMode;
            cloneData.ID = ID;
            cloneData.Length = Length;
            Data.CopyTo(cloneData.Data, 0);
            return cloneData;
        }
    }

    //1.ZLGCAN系列接口卡信息的数据类型。
    public struct VCI_BOARD_INFO
    {
        public UInt16 hw_Version;
        public UInt16 fw_Version;
        public UInt16 dr_Version;
        public UInt16 in_Version;
        public UInt16 irq_Num;
        public byte can_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] str_Serial_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] str_hw_Type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Reserved;
    }

    //2.定义CAN信息帧的数据类型。
    public struct VCI_CAN_OBJ
    {
        public UInt32 ID;
        public UInt32 TimeStamp;
        public byte TimeFlag;
        public byte SendType;
        public byte RemoteFlag;//是否是远程帧
        public byte ExternFlag;//是否是扩展帧
        public byte DataLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Data;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Reserved;

        public void Init()
        {
            Data = new byte[8];
            Reserved = new byte[3];
        }
    }

    //3.定义CAN控制器状态的数据类型。
    public struct VCI_CAN_STATUS
    {
        public byte ErrInterrupt;
        public byte regMode;
        public byte regStatus;
        public byte regALCapture;
        public byte regECCapture;
        public byte regEWLimit;
        public byte regRECounter;
        public byte regTECounter;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved;
    }

    //4.定义错误信息的数据类型。
    public struct VCI_ERR_INFO
    {
        public UInt32 ErrCode;
        public byte Passive_ErrData1;
        public byte Passive_ErrData2;
        public byte Passive_ErrData3;
        public byte ArLost_ErrData;
    }

    //5.定义初始化CAN的数据类型
    public struct VCI_INIT_CONFIG
    {
        public UInt32 AccCode;
        public UInt32 AccMask;
        public UInt32 Reserved;
        public byte Filter;
        public byte Timing0;
        public byte Timing1;
        public byte Mode;
    }

    public struct CHGDESIPANDPORT
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] szpwd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] szdesip;
        public Int32 desport;

        public void Init()
        {
            szpwd = new byte[10];
            szdesip = new byte[20];
        }
    }

    ///////// new add struct for filter /////////
    //typedef struct _VCI_FILTER_RECORD{
    //    DWORD ExtFrame;	//是否为扩展帧
    //    DWORD Start;
    //    DWORD End;
    //}VCI_FILTER_RECORD,*PVCI_FILTER_RECORD;
    public struct VCI_FILTER_RECORD
    {
        public UInt32 ExtFrame;
        public UInt32 Start;
        public UInt32 End;
    }

    public struct FrameId
    {
        public byte Dir;
        public byte Group;
        public byte DevId;
        public byte FrameType;
    }
}
