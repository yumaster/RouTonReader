using System.Runtime.InteropServices;
using System.Text;

namespace JingLunReader
{
    class ReaderApi
    {
        /// <summary>
        /// 动态链接库路径
        /// </summary>
        public const string dllPath = "Sdtapi.dll";

        #region 端口函数
        /// <summary>
        /// 端口初始化函数
        /// 本函数用于打开串口或USB并检测读卡设备是否就绪。
        /// </summary>
        /// <param name="iPort">设置串口、USB（公安部标准驱动）、USB-HID（免驱动）、USB-CCID接口</param>
        /// <returns>1-正确，其他-错误</returns>
        [DllImport(dllPath)]
        public static extern int InitComm(int iPort);
        /// <summary>
        /// 端口关闭接口
        /// 本函数用于关闭已打开的端口，一般在调用InitComm成功并完成读卡任务后调用
        /// </summary>
        /// <returns>1-正确，其他-错误</returns>
        [DllImport(dllPath)]
        public static extern int CloseComm();
        /// <summary>
        /// 关闭天线接口
        /// 本函数用于关闭天线场强。使用前必须端口初始化（InitComm）成功，关闭天线后，调用卡认证接口或者找卡命令，天线场强将自动打开
        /// </summary>
        /// <returns>1-正确，0-错误，-1-端口未打开</returns>
        [DllImport(dllPath)]
        public static extern int Routon_ShutDownAntenna();
        /// <summary>
        /// 获取当前接入的HID接口iDR210数量
        /// 本函数用于获取当前接入的HID接口iDR210的数量。使用前必须端口初始化（InitComm）成功。
        /// </summary>
        /// <returns>设备数量</returns>
        [DllImport(dllPath)]
        public static extern int GetHIDCount();

        /// <summary>
        /// 设定当前操作的HID接口iDR210
        /// 本函数用于设定当前操作的HID接口iDR210。使用前必须端口初始化（InitComm）成功。
        /// </summary>
        /// <param name="index"></param>
        /// <returns>true-成功，false-失败</returns>
        [DllImport(dllPath)]
        public static extern bool HIDSelect(int index);
        #endregion

        #region 读二代证相关函数
        /// <summary>
        /// 卡认证接口
        /// 本函数用于发现身份证卡并选择卡
        /// </summary>
        /// <returns>1-正确，0-错误</returns>
        [DllImport(dllPath)]
        public static extern int Authenticate();

        /// <summary>
        /// 读卡信息接口
        /// 本函数用于读取卡中基本信息，包括文字信息与图像信息。
        /// 文字信息已经分段解析，输出格式为单字节,且每一字段信息已经被表示为字符串。
        /// 图象信息被解码后存为文件photo.bmp（在当前工作目录下）
        /// </summary>
        /// <param name="pMsg">无符号字符指针，指向读到的文本信息。需要在调用时分配内存，字节数不小于192。
        /// 函数调用成功后，各字段的文本信息已经转换为单字节形式，并表示为字符串格式</param>
        /// <param name="len">整数， 返回总字符长度，可以给空值</param>
        /// <returns>1-正确，0-错误</returns>
        [DllImport(dllPath)]
        public static extern int ReadBaseMsg(byte[] pMsg, int[] len);

        /// <summary>
        /// 读追加地址信息
        /// 本函数用于读取卡中追加地址信息，输出格式为单字节字符串格式。
        /// </summary>
        /// <param name="pMsg"></param>
        /// <param name="num"></param>
        /// <returns>1-正确，0-错误</returns>
        [DllImport(dllPath)]
        public static extern int ReadNewAppMsg(byte[] pMsg, int[] num);

        /// <summary>
        /// 读卡体管理号
        /// 本函数用于读取身份证卡的管理号
        /// 返回1正确，0错误
        /// </summary>
        /// <param name="pMsg"></param>
        /// <returns>1-正确，0-错误</returns>
        [DllImport(dllPath)]
        public static extern int ReadIINSNDN(byte[] pMsg);

        /// <summary>
        /// 读模块序列号
        /// 本函数用于读取验证安全控制模块（SAM_V）的序列号
        /// </summary>
        /// <param name="pcSAMID"></param>
        /// <returns>1-正确，0-协议包读写错误，-1-通讯失败，-3-接收错误协议包，-4-读取包错误（base64串口设备），-5,-6,-8-读取超时</returns>
        [DllImport(dllPath)]
        public static extern int GetSAMIDToStr(byte[] pcSAMID);

        /// <summary>
        /// 判断身份证是否在设备上
        /// 本函数用于用于判断身份证是否在机具上。
        /// </summary>
        /// <returns>1有身份证，0无身份证</returns>
        [DllImport(dllPath)]
        public static extern int CardOn();

        /// <summary>
        /// 判断设备是否支持指纹信息读取
        /// 本函数用于判断iDR210是否支持指纹信息读取。
        /// </summary>
        /// <returns>1支持，-1设备不支持，-2模块不支持</returns>
        [DllImport(dllPath)]
        public static extern int IsFingerPrintDevice();

        /// <summary>
        /// 读卡指纹及卡信息接口
        /// 本函数用于读取指纹信息及卡中基本信息，包括文字信息与图像信息。
        /// 文字信息已经分段解析，输出格式为单字节,且每一字段信息已经被表示为字符串。
        /// 图象信息被解码后存为文件photo.bmp（在当前工作目录下）。
        /// </summary>
        /// <param name="pMsg"></param>
        /// <param name="len"></param>
        /// <param name="pucFPMsg"></param>
        /// <param name="puiFPMsgLen"></param>
        /// <returns>1正确，0错误</returns>
        [DllImport(dllPath)]
        public static extern int ReadBaseFPMsg(byte[] pMsg, int[] len, byte[] pucFPMsg, int[] puiFPMsgLen);
        #endregion

        #region Type A卡相关函数
        /// <summary>
        /// 找IC卡
        /// 本函数用于寻卡。
        /// </summary>
        /// <returns>1 M1-S50卡; 2 CPU卡; 3 M1-S70卡; 4 Mifare UltraLight卡; 0 未找到卡; 其他 不明卡错误码</returns>
        [DllImport(dllPath)]
        public static extern int Routon_IC_FindCard();

        /// <summary>
        /// 读IC卡序列号高级函数
        /// 本函数用于读取IC卡的序列号，自动完成找卡、选卡等过程
        /// </summary>
        /// <param name="SN"></param>
        /// <returns>1 正确，0错误</returns>
        [DllImport(dllPath)]
        public static extern int Routon_IC_HL_ReadCardSN(byte[] SN);

        /// <summary>
        /// 读IC卡区块高级函数
        /// 本函数用于读取IC卡指定扇区的数据内容，在调用本函数前需要先找卡Routon_IC_FindCard()。
        /// </summary>
        /// <param name="SID">扇区号，0-15之间（对M1S50卡）</param>
        /// <param name="BID">块号，0-3之间</param>
        /// <param name="KeyType">密钥类型，两种：0x60 keyA，0x61 keyB</param>
        /// <param name="Key">密钥</param>
        /// <param name="data">读取到的数据内容，需要在调用时分配内存，字节数不小于16。</param>
        /// <returns>1正确，0读卡错误，-1参数错误，-3秘钥或卡类型错误</returns>
        [DllImport(dllPath)]
        public static extern int Routon_IC_HL_ReadCard(int SID, int BID, int KeyType, byte[] Key, byte[] data);

        /// <summary>
        /// 写IC卡区块高级函数
        /// 本函数用于向IC卡指定扇区写入数据内容，在调用本函数前需要先找卡Routon_IC_FindCard()
        /// </summary>
        /// <param name="SID">扇区号，0-15之间（对M1S50卡）</param>
        /// <param name="BID">块号，0-3之间</param>
        /// <param name="KeyType">密钥类型，两种：0x60 keyA，0x61 keyB</param>
        /// <param name="Key">密钥</param>
        /// <param name="data">准备写入的数据内容，字节数为16</param>
        /// <returns></returns>
        [DllImport(dllPath)]
        public static extern int Routon_IC_HL_WriteCard(int SID, int BID, int KeyType, byte[] Key, byte[] data);
        #endregion

        /// <summary>
        /// 控制蜂鸣器和指示灯
        /// 本函数用于控制iDR210 USB-HID 设备的LED指示灯和蜂鸣器。
        /// BeepON和LEDON为布尔类型，值为True时，对应的蜂鸣器和指示灯打开；duration为打开持续的时间，单位为毫秒。
        /// </summary>
        /// <param name="BeepON"></param>
        /// <param name="LEDON"></param>
        /// <param name="duration"></param>
        /// <returns>1正确，0错误</returns>

        [DllImport(dllPath)]
        public static extern int HID_BeepLED(bool BeepON, bool LEDON, int duration);

        /// <summary>
        /// 蜂鸣器开关
        /// </summary>
        /// <param name="isMute">true为关闭，false为打开</param>
        /// <returns>0失败，1成功</returns>
        [DllImport(dllPath)]
        public static extern int Routon_Mute(bool isMute);

        /// <summary>
        /// 本函数用户判断当前证件类型是身份证，还是外国人居留证。注意，该函数需在调用Authenticate函数后再调用。
        /// </summary>
        /// <returns>100中国身份证，101外国人居留证，102港澳台居住证，其他-其他错误</returns>
        [DllImport(dllPath)]
        public static extern int Routon_DecideIDCardType();




        /// <summary>
        /// 读取身份证信息
        /// </summary>
        /// <returns></returns>
        public static byte[] ReadIDCardInfo()
        {
            Routon_Mute(true);//关闭蜂鸣器
            int init = InitComm(1001);//初始化
            //HID_BeepLED(true, true, 100);//蜂鸣器及LED灯，持续时间
            if (init != 1)
                return new byte[0];
            int search = Authenticate();//卡认证
            if (search != 1)
            {
                return new byte[0];
            }
            byte[] pMsg = new byte[192];
            int readBaseMsg = ReadBaseMsg(pMsg, null);
            if (readBaseMsg != 1)
                return new byte[0];
            CloseComm();
            return pMsg;
        }
        /// <summary>
        /// 获取身份证内码
        /// </summary>
        /// <returns></returns>
        internal static byte[] ReadIDCardSn()
        {
            int init = InitComm(1001);
            if (init != 1)
            {
                return new byte[0];
            }
            byte[] pMsg = new byte[16];
            int readBaseMsg = ReadIINSNDN(pMsg);
            if (readBaseMsg != 1)
            {
                return new byte[0];
            }
            CloseComm();
            return pMsg;
        }

        /// <summary>
        /// 读IC卡的卡片内码
        /// </summary>
        /// <returns></returns>
        public static byte[] ReadICCardSn()
        {
            int init = InitComm(1001);
            if (init != 1)
            {
                return new byte[0];
            }
            byte[] pMsg = new byte[16];
            int readBaseMsg = Routon_IC_HL_ReadCardSN(pMsg);
            if (readBaseMsg != 1)
            {
                return new byte[0];
            }
            return pMsg;
        }
    }
}
