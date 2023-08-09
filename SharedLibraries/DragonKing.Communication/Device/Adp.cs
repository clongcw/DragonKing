using DragonKing.Log.Interface;
using DragonKing.Log.Service;
using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace DragonKing.Communication.Device
{
    public class Adp : CommonAdp
    {
        private const float MAX_HIGH_SPEED = 1000.0f;
        private const float MIN_HIGH_SPEED = 2.5f;
        private const float MAX_CUTOFF_SPEED = 200.0f;
        private const float MIN_CUTOFF_SPEED = 2.5f;
        private Port _port;
        private ILog _log = LogService.Instance;
        private AutoResetEvent _ackEvent;
        private int _errCode = 0;
        private bool _bSended;
        private bool _reportState = false;
        private int _currentPressure = 0;

        private bool _bReportTipState = false;
        private bool _bReportPressure = false;

        public delegate void DelegateRefreshReceiveMsg(CanData msg);
        public DelegateRefreshReceiveMsg OnReceiveMsg = null;
        public delegate void DelegateReceiveStatus(byte status);
        public DelegateReceiveStatus OnReceiveStatus = null;

        public Adp(Port port, int sequence)
        {
            AdpInfo = new AdpInfo();
            AdpInfo.Name = "Adp" + sequence.ToString();
            AdpInfo.NodeId = sequence;
            AdpInfo.Enabled = true;
            _ackEvent = new AutoResetEvent(false);
            _port = port;
            _ackEvent = new AutoResetEvent(false);
            _port = port;
            _port.DataReceived += _port_DataReceived;
            PressureInitial = 0;
        }

        void _port_DataReceived(object sender, CanDataReceivedEventArgs e)
        {
            if ((e.CanData.FrameFlag == FrameFlag.Stanard))
            {
                CanData recObj = e.CanData.Clone();
                UInt16 deviceId = Common.ParseDeviceId(recObj.ID);
                if ((deviceId < Common.MIN_DEVICE_ID) || (deviceId > Common.MAX_DEVICE_ID))
                {
                    //_log.DebugFormat("ADP反馈信息，ID为：{0}", deviceId);
                    FrameId frameId = new FrameId();
                    AdpConst.AnalysisFramID(recObj.ID, ref frameId);
                    if (frameId.DevId == AdpInfo.NodeId)
                    {

                        if (frameId.Group == AdpConst.GROUP_GUIDE)
                        {
                            byte msgGuide = byte.Parse(AdpConst.GROUP_ADP.ToString() + frameId.DevId.ToString(), NumberStyles.HexNumber);
                            _port.Send(AdpConst.ID_GUIDE, 2, new byte[] { msgGuide, msgGuide });
                        }
                        if ((recObj.Length == 0))
                        {
                            //_log.Debug("确认");
                            //_ackEvents.Set();
                        }
                        if (recObj.Length > 1)
                        {
                            if (_bSended)
                            {
                                _bSended = false;
                                if (frameId.FrameType == AdpConst.FRAME_ACTION)
                                {
                                    if (recObj.Data[1] == 0x60)
                                    {
                                        if (OnReceiveStatus != null)
                                        {
                                            OnReceiveStatus(recObj.Data[0]);
                                        }
                                        //_log.DebugFormat("返回命令执行状态信息ADP{0}:{1}", frameId.DevId, e.CanData.Data[0]); 
                                        _errCode = recObj.Data[0] - 0x20;
                                        _ackEvent.Set();

                                    }
                                }
                                else if (frameId.FrameType == AdpConst.FRAME_REPORT)
                                {
                                    _errCode = recObj.Data[0] - 0x20;
                                    if (recObj.Length > 2)
                                    {
                                        if (_bReportTipState)
                                        {
                                            byte tipState = (byte)(recObj.Data[2] - 0x30);
                                            if (tipState == AdpConst.TIP_EXIST)
                                            {
                                                _reportState = true;
                                            }
                                            else
                                            {
                                                _reportState = false;
                                            }
                                        }
                                        else if (_bReportPressure)
                                        {
                                            byte[] bytPrs = new byte[recObj.Length - 2];
                                            Array.Copy(recObj.Data, 2, bytPrs, 0, bytPrs.Length);
                                            _currentPressure = int.Parse(ASCIIEncoding.ASCII.GetString(bytPrs));
                                        }
                                    }
                                    _ackEvent.Set();
                                }
                            }
                        }
                    }

                }

            }
        }

        public override int AbsMove(double pos, byte mode)
        {
            throw new NotImplementedException();
        }

        public override int Aspirate(double pos, byte mode)
        {
            throw new NotImplementedException();
        }

        public override int Dispense(double pos, byte mode)
        {
            throw new NotImplementedException();
        }

        public override int EjectTip(char mode)
        {
            throw new NotImplementedException();
        }

        public override int Init()
        {
            return OnInit();
        }

        private int OnInit()
        {
            int res = ResultCode.S_SUCCESSED;
            if (AdpInfo.Enabled)
            {
                res = ExecuteCommandByFrame1(AdpConst.CMD_INIT, null);
            }
            AdpInfo.IsInitialized = res == ResultCode.S_SUCCESSED ? true : false;
            if (AdpInfo.IsInitialized)
            {
                _log.Debug($"ADP{AdpInfo.NodeId}初始化成功");
            }
            else
            {
                _log.Warning($"ADP{AdpInfo.NodeId}初始化失败，错误代码{res}");
            }


            //读取Adp初始压力值，用于判断堵塞
            if (res == ResultCode.S_SUCCESSED)
            {
                int pressure = 0;
                res = ReportPressure(ref pressure);
                if (res == ResultCode.S_SUCCESSED)
                {
                    PressureInitial = pressure;
                    _log.Debug("ADP{0}初始压力为{1}！", AdpInfo.NodeId, PressureInitial);
                }
            }
            return res;
        }

        /// <summary>
        /// 以帧类型1执行命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="data">命令参数</param>
        /// <returns>命令执行结果，超时返回false</returns>
        private int ExecuteCommandByFrame1(byte cmd, byte[] data)
        {
            return ExecuteCommand(cmd, data, AdpConst.FRAME_ACTION, true);
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="data"></param>
        /// <param name="frameType"></param>
        /// <param name="isAddR"></param>
        /// <returns>resultCode 0成功，其他失败</returns>
        private int ExecuteCommand(byte cmd, byte[] data, byte frameType, bool isAddR)
        {
            UInt32 sendId = AdpConst.GenerateFramID(AdpConst.DIR_DOWN, AdpConst.GROUP_ADP, (byte)AdpInfo.NodeId, frameType);
            byte[] msg;
            if (data != null)
            {
                int length = 2 + data.Length;
                if (!isAddR)
                {
                    length = 1 + data.Length;
                }

                msg = new byte[length];
                msg[0] = cmd;
                data.CopyTo(msg, 1);
                if (isAddR)
                {
                    msg[1 + data.Length] = AdpConst.CMD_EXECUTE;
                }
            }
            else
            {
                if (isAddR)
                {
                    msg = new byte[] { cmd, AdpConst.CMD_EXECUTE };
                }
                else
                {
                    msg = new byte[] { cmd };
                }
            }
            int res = SendMessage(sendId, msg, AdpConst.TIMEOUT_CMD);
            return res;
        }

        private int SendMessage(UInt32 frameId, byte[] msg, int timeOut)
        {
            _errCode = ResultCode.S_SUCCESSED;
            _bSended = true;
            int res = _port.Send(frameId, (byte)msg.Length, msg);
            if (res == ResultCode.S_OK)
            {
                if (timeOut > 0)
                {
                    bool ackRes = _ackEvent.WaitOne(timeOut);
                    //_log.DebugFormat("Adp{0}执行命令{1}结果{2}",AdpInfo.Sequence,msg[0], ackRes);
                    if (!ackRes)
                    {
                        _bSended = false;
                        _errCode = ResultCode.ERR_TIMEOUT;
                    }
                }
            }
            else
            {
                _bSended = false;
                _errCode = ResultCode.ERR_DISCONNECTED;
            }
            return _errCode;
        }

        public override int LiquidDetection(char mode)
        {
            throw new NotImplementedException();
        }

        public override int ReportPressure(ref int pressure)
        {
            throw new NotImplementedException();
        }

        public override int ReportTipState(ref bool state)
        {
            throw new NotImplementedException();
        }

        public override int Reset()
        {
            throw new NotImplementedException();
        }

        public override int SetCutOffSpeed(float speed, byte mode)
        {
            throw new NotImplementedException();
        }

        public override int SetMaxSpeed(float speed, byte mode)
        {
            throw new NotImplementedException();
        }

        public override int SetPLLD(byte mode, uint paramValue)
        {
            throw new NotImplementedException();
        }

        public override int SetStartSpeed(float speed, byte mode)
        {
            throw new NotImplementedException();
        }


    }
}
