using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fujita.Communication;
using Fujita.Misc;

namespace LineCameraSheetSystem
{
    enum ESignalStatus
    {
        Shutdown = 0x0, 
        InspectStop = 0x1,
        InspectStart = 0x2,
        InspectSuspend = 0x4,
        NG = 0x10,
        InspectNotStart = 0x20,
        Error  =0x40,
        LengthMeasError = 0x80,
    }

    enum ESignalControl
    {
        Red = 0,
        Green,
        Yellow,
        Blue,
        Buzzer,//V1333 表示灯（中央）としてそのまま使用
        External1,
        External2,
        External3, //V1057 NG表裏修正 yuasa 20190118：外部３追加
        External4, //V1057 NG表裏修正 yuasa 20190118：外部４追加
        GCustomBuzzer, //v1326
        BuzzerBothSide, //V1333 表示灯（両端）として追加
        PowerOffRelay ,//v1338 yuasa PC電源ボタン接続用リレーとして追加
    }

    class clsSignalControl
    {
        private static clsSignalControl _singleton = new clsSignalControl();

        public static clsSignalControl GetInstance()
        {
            return _singleton;
        }
        
        clsDioDelayController _dioDelayCtrl = new clsDioDelayController();

        clsDinCountController _dinCountCtrl = new clsDinCountController();

        bool _bInitialize = false;
        CommunicationDIO _dio = null;
        ESignalStatus _eSysStat = ESignalStatus.Shutdown;
        int[] _iaDioMap;
        public int DipMap(ESignalControl sig)
        {
            return _iaDioMap[(int)sig];
        }

        public clsSignalControl()
        {

        }

        public bool Initialize(CommunicationDIO dio)
        {
            if (dio == null)
                return false;
            _dio = dio;
            _iaDioMap = new int[Enum.GetValues(typeof(ESignalControl)).Length];
            _bInitialize = true;
            //
            _dioDelayCtrl.Initialize(dio);
            //ｼｮｯﾄ数監視
            _dinCountCtrl.Initialize(dio);

            return true;
        }
        public void Clear()
        {
            _dinCountCtrl.Clear();
        }
        public bool Start()
        {
            if (_bInitialize != true)
                return false;
            //
            _dioDelayCtrl.Start();
            //
            _dinCountCtrl.Start();

            return true;
        }

        public void Terminate()
        {
            if (!_bInitialize)
                return;

            // DIOをすべてOFF状態にする
            foreach (ESignalControl e in Enum.GetValues(typeof(ESignalControl)))
            {
                //v1338 PowerOffRelayのみ何もしない。パソコンが落ちるタイミングでDOを落とす考えのため。
                if ((SystemParam.GetInstance().PowerOffButtonEnable == true) && (e == ESignalControl.PowerOffRelay))
                {
                    //_dio.OUT1(_iaDioMap[(int)e], true);
                }
                else
                {
                    _dio.OUT1(_iaDioMap[(int)e], false);
                }
            }

            _dioDelayCtrl.Terminate();

            _dinCountCtrl.Terminate();

            //_iNGBuzzerCount = 0; //V1333 削除
            _eSysStat = ESignalStatus.Shutdown;
            _dio = null;
            _bInitialize = false;
        }

        public bool IsInspectStart
        {
            get { return (_eSysStat & ESignalStatus.InspectStart) != 0; }
        }

        public bool IsInspectStop
        {
            get { return (_eSysStat & ESignalStatus.InspectStop) != 0; }
        }

        public bool IsInspectSuspend
        {
            get { return (_eSysStat & ESignalStatus.InspectSuspend) != 0; }
        }

        public bool IsNG
        {
            get { return (_eSysStat & ESignalStatus.NG) != 0; }
        }

        public bool IsInspectNotStart
        {
            get { return (_eSysStat & ESignalStatus.InspectNotStart) != 0; }
        }

        public bool IsError
        {
            get { return (_eSysStat & ESignalStatus.Error) != 0; }
        }

        public bool IsLengthMeasError
        {
            get { return (_eSysStat & ESignalStatus.LengthMeasError) != 0; }
        }

        public bool Load(string sPath, string sSection = "")
        {
            if (!_bInitialize)
                return false;
                
            IniFileAccess ifa = new IniFileAccess();

            _iaDioMap[(int)ESignalControl.Red] = SystemParam.GetInstance().OutPointRed;
            _iaDioMap[(int)ESignalControl.Green] = SystemParam.GetInstance().OutPointGreen;
            _iaDioMap[(int)ESignalControl.Yellow] = SystemParam.GetInstance().OutPointYellow;
            _iaDioMap[(int)ESignalControl.Blue] = SystemParam.GetInstance().OutPointBlue;
            _iaDioMap[(int)ESignalControl.Buzzer] = SystemParam.GetInstance().OutPointBuzzer;
            _iaDioMap[(int)ESignalControl.External1] = SystemParam.GetInstance().OutPointExternal1;
            _iaDioMap[(int)ESignalControl.External2] = SystemParam.GetInstance().OutPointExternal2;
            _iaDioMap[(int)ESignalControl.External3] = SystemParam.GetInstance().OutPointExternal3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            _iaDioMap[(int)ESignalControl.External4] = SystemParam.GetInstance().OutPointExternal4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            _iaDioMap[(int)ESignalControl.GCustomBuzzer] = SystemParam.GetInstance().OutPointGCustomBuzzer; //v1326
            _iaDioMap[(int)ESignalControl.BuzzerBothSide] = SystemParam.GetInstance().OutPointBuzzerBothSide;//V1333
            _iaDioMap[(int)ESignalControl.PowerOffRelay] = SystemParam.GetInstance().PowerOffRelay;//v1338

            // DIOをすべてOFF状態にする                  tắt tất cả DIO
            foreach (ESignalControl e in Enum.GetValues(typeof(ESignalControl)))
            {
                if((SystemParam.GetInstance().PowerOffButtonEnable == true) && (e == ESignalControl.PowerOffRelay))//v1338 PowerOffRelayのみオンにする
                {
                    _dio.OUT1(_iaDioMap[(int)e], true);
                }
                else
                {
                    _dio.OUT1(_iaDioMap[(int)e], false);
                }
            }

            _dinCountCtrl.Load(sPath, sSection);

            return true;
        }


        // setting màu cho đèn cảnh báoTháp tín hiệu chiếu sáng màu vàng
        public bool SetInspectStatus(ESignalStatus stat)
        {
            if (!_bInitialize)
                return false;

            if ( stat != ESignalStatus.InspectStart
                && stat != ESignalStatus.InspectStop
                && stat != ESignalStatus.InspectSuspend
                && stat != ESignalStatus.Shutdown)
                return false;

            reset(ESignalStatus.InspectStart);
            reset(ESignalStatus.InspectStop);
            reset(ESignalStatus.InspectSuspend);
            set(stat);

            if (IsInspectStart)
            {
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Blue], true);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Yellow], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Green], false);
            }
            else if (IsInspectStop)
            {
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Blue], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Yellow], true);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Green], false);
            }
            else if (IsInspectSuspend)
            {
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Blue], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Yellow], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Green], true);
            }
            return true;
        }

        /// <summary>
        /// SetNGが呼び出された数分カウントされる
        /// </summary>
     
        //int _iNGBuzzerCount = 0; //V1333 削除

        //V1333 lstZoneを追加。他ブザー鳴動をスレッドプールではなく、他DIOと同様にcmdに変更。
        public bool SetNG(int FrontReverseSide, AppData.ZoneID zoneId, List<AppData.ZoneID> lstZones, bool isBothSide,bool isCenter) //V1057 NG表裏修正 yuasa 20190118：引数を追加

        {
            //System.Diagnostics.Debug.WriteLine(Recipe.GetInstance().InspParam[0].Kando[5].Threshold);
            //V1333 削除
            // スレッドプールで時間指定のブザー信号を鳴らす
            //_dio.OUT1(_iaDioMap[(int)ESignalControl.Red], true);
            //_dio.OUT1(_iaDioMap[(int)ESignalControl.Buzzer], true);

            //_iNGBuzzerCount++;

            //if (SystemParam.GetInstance().BuzzerTimer != 0)
            //{
            //	// ブザーだけは3秒後に停止させる
            //	System.Threading.ThreadPool.QueueUserWorkItem((ms) =>
            //		{
            //			System.Threading.Thread.Sleep((int)ms);
            //			_iNGBuzzerCount--;
            //			if (!IsInspectNotStart && !IsError && _iNGBuzzerCount <= 0 && !IsLengthMeasError)
            //			{
            //				_dio.OUT1(_iaDioMap[(int)ESignalControl.Buzzer], false);
            //			}
            //		}, (object)SystemParam.GetInstance().BuzzerTimer);
            //}

            if (!_bInitialize)
                return false;

            int extShot1 = SystemParam.GetInstance().ExternalShot1;
            int extShot2 = SystemParam.GetInstance().ExternalShot2;
            int extShot3 = SystemParam.GetInstance().ExternalShot3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            int extShot4 = SystemParam.GetInstance().ExternalShot4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            if (Recipe.GetInstance().ExternalEnable == true)
            {
                extShot1 = Recipe.GetInstance().ExternalShot1;
                extShot2 = Recipe.GetInstance().ExternalShot2;
                extShot3 = Recipe.GetInstance().ExternalShot3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
                extShot4 = Recipe.GetInstance().ExternalShot4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            }

            set(ESignalStatus.NG);
            bool FrontReverseDivide = SystemParam.GetInstance().ExternalFrontReverseDivide; //V1057 NG表裏修正 yuasa 20190118：設定読み込み

            //表示灯（中央）と表示灯（両端）が異なったら個別にコマンド出力
            //→設定表示追加を基本とするので、両端で変えるべきなのでif文削除
            //if(SystemParam.GetInstance().OutPointBuzzer != SystemParam.GetInstance().OutPointBuzzerBothSide)
            //{

            //両端のパトライト処理
            //foreach (AppData.ZoneID zone in lstZones)//V1333 foreachでzone毎に出力する
            //{
            //    if (isZoneBothSide(zone) == true)
            //    {
            //        procPatLiteCmdAdd(true);
            //        break;
            //    }
            //}
            if(isBothSide == true)
            {
                bool isTop = FrontReverseSide == 0 | FrontReverseSide == 2;
                procPatLiteCmdAdd(true, isTop);
            }


            //中央のパトライト処理
            //foreach (AppData.ZoneID zone in lstZones)//V1333 foreachでzone毎に出力する
            //{
            //    if (isZoneBothSide(zone) == false)
            //    {
            //        procPatLiteCmdAdd(false);
            //        break;
            //    }
            //}
            if (isCenter == true)
            {
                bool isTop = FrontReverseSide == 0 | FrontReverseSide == 2;
                procPatLiteCmdAdd(false, isTop);
            }

            //}
            //else
            //{
            //    procPatLiteCmdAdd(true);//引数は両端（true）だが出力先が同じなので問題なし。
            //}

            //外部１非常停止用
            foreach (AppData.ZoneID zone in lstZones)//V1333 foreachでzone毎に出力する
            {
                if (SystemParam.GetInstance().ExternalEnable1 && ((FrontReverseSide == 0 || FrontReverseSide == 2) || !FrontReverseDivide)) //V1057 NG表裏修正 yuasa 20190118：条件追加
                {
                    bool isEnableZone = SystemParam.GetInstance().Extarnal1Zone[(int)zone];
                    if (isEnableZone == true || false == SystemParam.GetInstance().SystemSettingFormZoneDisp)//SystemSettingFormZoneDispがfalseの場合は従来どおりゾーンに関係なく出力
                    {
                        if (extShot1 > 0)
                        {
                            clsDinCountController.Command cmd = new clsDinCountController.Command(extShot1, procExt1);
                            _dinCountCtrl.AddCommand(cmd);
                        }
                        else
                        {
                            procExt1(this, new EventArgs());
                        }
                        //1個でも該当すれば抜ける
                        break;
                    }
                }
            }

            //外部２非常停止用
            foreach (AppData.ZoneID zone in lstZones)//V1333 foreachでzone毎に出力する
            {
                if (SystemParam.GetInstance().ExternalEnable2 && ((FrontReverseSide == 0 || FrontReverseSide == 2) || !FrontReverseDivide)) //V1057 NG表裏修正 yuasa 20190118：条件追加
                {
                    bool isEnableZone = SystemParam.GetInstance().Extarnal2Zone[(int)zone];
                    if (isEnableZone == true || false == SystemParam.GetInstance().SystemSettingFormZoneDisp)//SystemSettingFormZoneDispがfalseの場合は従来どおりゾーンに関係なく出力
                    {
                        if (extShot2 > 0)
                        {
                            clsDinCountController.Command cmd = new clsDinCountController.Command(extShot2, procExt2);
                            _dinCountCtrl.AddCommand(cmd);
                        }
                        else
                        {
                            procExt2(this, new EventArgs());
                        }
                        //1個でも該当すれば抜ける
                        break;
                    }
                }
            }

            //外部３非常停止用
            foreach (AppData.ZoneID zone in lstZones)//V1333 foreachでzone毎に出力する
            {
                if (SystemParam.GetInstance().ExternalEnable3 && (FrontReverseSide == 1 || FrontReverseSide == 2) && FrontReverseDivide) //V1057 NG表裏修正 yuasa 20190118：外部３追加
                {
                    bool isEnableZone = SystemParam.GetInstance().Extarnal3Zone[(int)zone];
                    if (isEnableZone == true || false == SystemParam.GetInstance().SystemSettingFormZoneDisp)//SystemSettingFormZoneDispがfalseの場合は従来どおりゾーンに関係なく出力
                    {
                        if (extShot3 > 0)
                        {
                            clsDinCountController.Command cmd = new clsDinCountController.Command(extShot3, procExt3);
                            _dinCountCtrl.AddCommand(cmd);
                        }
                        else
                        {
                            procExt3(this, new EventArgs());
                        }
                        //1個でも該当すれば抜ける
                        break;
                    }
                }
            }

            //外部４非常停止用
            foreach (AppData.ZoneID zone in lstZones)//V1333 foreachでzone毎に出力する
            {
                if (SystemParam.GetInstance().ExternalEnable4 && (FrontReverseSide == 1 || FrontReverseSide == 2) && FrontReverseDivide) //V1057 NG表裏修正 yuasa 20190118：外部４追加
                {
                    bool isEnableZone = SystemParam.GetInstance().Extarnal4Zone[(int)zone];
                    if (isEnableZone == true || false == SystemParam.GetInstance().SystemSettingFormZoneDisp)//SystemSettingFormZoneDispがfalseの場合は従来どおりゾーンに関係なく出力
                    {
                        if (extShot4 > 0)
                        {
                            clsDinCountController.Command cmd = new clsDinCountController.Command(extShot4, procExt4);
                            _dinCountCtrl.AddCommand(cmd);
                        }
                        else
                        {
                            procExt4(this, new EventArgs());
                        }
                        //1個でも該当すれば抜ける
                        break;
                    }
                }
            }
            return true;
        }

        public void AddFlipperNGCmd()//v1326 岐阜カスタムの場合のコマンド生成メソッド これがフリッパ検知にコールされる。
        {
            if(true == SystemParam.GetInstance().GCustomEnable)
            {
                int patLiteDelay = SystemParam.GetInstance().CommonPatLiteDelaySecond;
                int patLiteOnTime = SystemParam.GetInstance().CommonPatLiteOnTimeSecond;

                if (true == Recipe.GetInstance().PatLiteEnable && SystemStatus.GetInstance().NowState == SystemStatus.State.Inspection)
                {
                    patLiteDelay = Recipe.GetInstance().PatLiteDelay;
                    patLiteOnTime = Recipe.GetInstance().PatLiteOnTime;
                }

                clsDioDelayController.Command cmdPatLite;
                //秒をミリ秒に変換
                clsDioDelayController.GetCommandObject(patLiteDelay * 1000, clsDioDelayController.ECmdType.Active, _iaDioMap[(int)ESignalControl.GCustomBuzzer],
                    (patLiteOnTime == 0) ? false : true, patLiteOnTime * 1000, out cmdPatLite);
                _dioDelayCtrl.AddCommand(cmdPatLite);
            }
        }

        public void ResetPatLite()
        {
            _dio.OUT1(_iaDioMap[(int)ESignalControl.GCustomBuzzer], false);
        }

        private void procExt1(object sender, EventArgs e)
        {
            int extDelay = SystemParam.GetInstance().ExternalDelayTime1;
            int extReset = SystemParam.GetInstance().ExternalResetTime1;
            if (Recipe.GetInstance().ExternalEnable == true)
            {
                extDelay = Recipe.GetInstance().ExternalDelayTime1;
                extReset = Recipe.GetInstance().ExternalResetTime1;
            }

            clsDioDelayController.Command cmdEx1;
            clsDioDelayController.GetCommandObject(extDelay, clsDioDelayController.ECmdType.Active, _iaDioMap[(int)ESignalControl.External1],
                (extReset == 0) ? false : true, extReset, out cmdEx1);
            _dioDelayCtrl.AddCommand(cmdEx1);
        }
        private void procExt2(object sender, EventArgs e)
        {
            int extDelay = SystemParam.GetInstance().ExternalDelayTime2;
            int extReset = SystemParam.GetInstance().ExternalResetTime2;
            if (Recipe.GetInstance().ExternalEnable == true)
            {
                extDelay = Recipe.GetInstance().ExternalDelayTime2;
                extReset = Recipe.GetInstance().ExternalResetTime2;
            }

            clsDioDelayController.Command cmdEx2;
            clsDioDelayController.GetCommandObject(extDelay, clsDioDelayController.ECmdType.Active, _iaDioMap[(int)ESignalControl.External2],
                (extReset == 0) ? false : true, extReset, out cmdEx2);
            _dioDelayCtrl.AddCommand(cmdEx2);
        }
        private void procExt3(object sender, EventArgs e) //V1057 NG表裏修正 yuasa 20190118：外部３追加
        {
            int extDelay = SystemParam.GetInstance().ExternalDelayTime3;
            int extReset = SystemParam.GetInstance().ExternalResetTime3;
            if (Recipe.GetInstance().ExternalEnable == true)
            {
                extDelay = Recipe.GetInstance().ExternalDelayTime3;
                extReset = Recipe.GetInstance().ExternalResetTime3;
            }

            clsDioDelayController.Command cmdEx3;
            clsDioDelayController.GetCommandObject(extDelay, clsDioDelayController.ECmdType.Active, _iaDioMap[(int)ESignalControl.External3],
                (extReset == 0) ? false : true, extReset, out cmdEx3);
            _dioDelayCtrl.AddCommand(cmdEx3);
        }
        private void procExt4(object sender, EventArgs e) //V1057 NG表裏修正 yuasa 20190118：外部４追加
        {
            int extDelay = SystemParam.GetInstance().ExternalDelayTime4;
            int extReset = SystemParam.GetInstance().ExternalResetTime4;
            if (Recipe.GetInstance().ExternalEnable == true)
            {
                extDelay = Recipe.GetInstance().ExternalDelayTime4;
                extReset = Recipe.GetInstance().ExternalResetTime4;
            }

            clsDioDelayController.Command cmdEx4;
            clsDioDelayController.GetCommandObject(extDelay, clsDioDelayController.ECmdType.Active, _iaDioMap[(int)ESignalControl.External4],
                (extReset == 0) ? false : true, extReset, out cmdEx4);
            _dioDelayCtrl.AddCommand(cmdEx4);
        }
        /// <summary>パトライト用コマンド登録メソッド</summary>//V1333
        private void procPatLiteCmdAdd(bool isBothSide,bool isTopSide)
        {
            clsDioDelayController.Command cmd;
            int DioIndex = 0;
            int DelayTime = 0;
            int ResetTime = 0;

            if (isBothSide)
            {
                //両サイド
                if(isTopSide == true)
                {
                    DioIndex = _iaDioMap[(int)ESignalControl.BuzzerBothSide];
                    DelayTime = SystemParam.GetInstance().PatLiteDelayTimeBothSide;
                    ResetTime = SystemParam.GetInstance().PatLiteResetTimeBothSide;
                }
                else
                {
                    DioIndex = _iaDioMap[(int)ESignalControl.BuzzerBothSide];
                    DelayTime = SystemParam.GetInstance().PatLiteDelayTimeBothSideUra;
                    ResetTime = SystemParam.GetInstance().PatLiteResetTimeBothSideUra;
                }
            }
            else
            {
                //中央
                if (isTopSide == true)
                {
                    DioIndex = _iaDioMap[(int)ESignalControl.Buzzer];
                    DelayTime = SystemParam.GetInstance().PatLiteDelayTimeCenter;
                    ResetTime = SystemParam.GetInstance().PatLiteResetTimeCenter;
                }
                else
                {
                    DioIndex = _iaDioMap[(int)ESignalControl.Buzzer];
                    DelayTime = SystemParam.GetInstance().PatLiteDelayTimeCenterUra;
                    ResetTime = SystemParam.GetInstance().PatLiteResetTimeCenterUra;
                }
            }

            clsDioDelayController.GetCommandObject(DelayTime, clsDioDelayController.ECmdType.Active, DioIndex,
                    (ResetTime == 0) ? false : true, ResetTime, out cmd);
            _dioDelayCtrl.AddCommand(cmd);
        }

        public bool ResetNG()
        {
            if (!_bInitialize)
                return false;

            reset(ESignalStatus.NG);

            int extReset1 = SystemParam.GetInstance().ExternalResetTime1;
            int extReset2 = SystemParam.GetInstance().ExternalResetTime2;
            int extReset3 = SystemParam.GetInstance().ExternalResetTime3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            int extReset4 = SystemParam.GetInstance().ExternalResetTime4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            if (Recipe.GetInstance().ExternalEnable == true)
            {
                extReset1 = Recipe.GetInstance().ExternalResetTime1;
                extReset2 = Recipe.GetInstance().ExternalResetTime2;
                extReset3 = Recipe.GetInstance().ExternalResetTime3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
                extReset4 = Recipe.GetInstance().ExternalResetTime4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            }
            if (extReset1 == 0)
            {
                _dio.OUT1(_iaDioMap[(int)ESignalControl.External1], false);
            }

            if (extReset2 == 0)
            {
                _dio.OUT1(_iaDioMap[(int)ESignalControl.External2], false);
            }

            if (extReset3 == 0) //V1057 NG表裏修正 yuasa 20190118：外部３追加
            {
                _dio.OUT1(_iaDioMap[(int)ESignalControl.External3], false);
            }

            if (extReset4 == 0) //V1057 NG表裏修正 yuasa 20190118：外部４追加
            {
                _dio.OUT1(_iaDioMap[(int)ESignalControl.External4], false);
            }

            if (!IsInspectNotStart && !IsError && !IsLengthMeasError )
            {
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Red], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Buzzer], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.BuzzerBothSide], false);//V1333
            }

            //_iNGBuzzerCount = 0; //V1333 削除

            return false;

        }

        public bool ResetExternalOutput(bool extOutReset)
        {
            if (!_bInitialize)
                return false;

            reset(ESignalStatus.NG);

            if (extOutReset)
            {
                int resetTime = SystemParam.GetInstance().ExternalResetTime1;
                if (Recipe.GetInstance().ExternalEnable)
                    resetTime = Recipe.GetInstance().ExternalResetTime1;
                _dioDelayCtrl.DeleteExtOut1ActiveCommand(resetTime);
            }

            if (!IsInspectNotStart && !IsError && !IsLengthMeasError)
            {
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Red], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Buzzer], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.BuzzerBothSide], false);//V1333
            }

            //_iNGBuzzerCount = 0; //V1333 削除

            return true;
        }

        public bool ResetInspectNotStart()
        {
            if (!_bInitialize)
                return false;
            
            reset(ESignalStatus.InspectNotStart);

            if (!IsNG && !IsError && !IsLengthMeasError )
            {
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Red], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Buzzer], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.BuzzerBothSide], false);//V1333
            }

            return true;
        }

        public bool SetError()
        {
            if (!_bInitialize)
                return false;

            set(ESignalStatus.Error);

            _dio.OUT1(_iaDioMap[(int)ESignalControl.Red], true);
            _dio.OUT1(_iaDioMap[(int)ESignalControl.Buzzer], true);
            _dio.OUT1(_iaDioMap[(int)ESignalControl.BuzzerBothSide], true);//V1333
            LogingDllWrap.LogingDll.Loging_SetLogString("SetError() ESignalControl.Buzzer=TRUE");

            return true;
        }

        public bool ResetError()
        {
            if (!_bInitialize)
                return false;

            reset(ESignalStatus.Error);

            if (!IsNG && !IsInspectNotStart && !IsLengthMeasError )
            {
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Red], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Buzzer], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.BuzzerBothSide], false);//V1333
            }
            return true;
        }

        public bool SetLengthMeasError()
        {
            if (!_bInitialize)
                return false;

            set(ESignalStatus.LengthMeasError);

            _dio.OUT1(_iaDioMap[(int)ESignalControl.Red], true);
            _dio.OUT1(_iaDioMap[(int)ESignalControl.Buzzer], true);
            _dio.OUT1(_iaDioMap[(int)ESignalControl.BuzzerBothSide], true);//V1333
            LogingDllWrap.LogingDll.Loging_SetLogString("SetLengthMeasError() ESignalControl.Buzzer=TRUE");

            return true;
        }

        public bool ResetLengthMeasError()
        {
            if (!_bInitialize)
                return false;

            reset(ESignalStatus.LengthMeasError);

            if (!IsNG && !IsInspectNotStart && !IsError)
            {
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Red], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.Buzzer], false);
                _dio.OUT1(_iaDioMap[(int)ESignalControl.BuzzerBothSide], false);//V1333
            }
            return true;
        }


        private void set(ESignalStatus stat)
        {
            _eSysStat |= stat;
        }

        private void reset(ESignalStatus stat)
        {
            _eSysStat &= ~stat;
        }

        //v1326 岐阜カスタム
        public int GetPatLiteTimerNum()
        {
            return _dioDelayCtrl.GetPatLiteTimerNum();
        }
        /// <summary>引数のZoneが両サイドがどうか判定する</summary>V1333 追加
        private bool isZoneBothSide(AppData.ZoneID zone)
        {
            if (zone == AppData.ZoneID.Z1 || (int)zone == Recipe.GetInstance().Partition)
                return true;
            return false;
        }
    }
}
