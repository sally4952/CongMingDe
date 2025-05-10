using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CongMingDe
{
    internal static class VersionHelper
    {
        [SecurityCritical]
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern bool RtlGetVersion(ref OsVersionInfoEx versionInfo);

        [DllImport("kernel32.dll")]
        public static extern bool GetVersionEx([In, Out] OsVersionInfoEx versionInfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct OsVersionInfoEx
        {
            //定义：https://docs.microsoft.com/zh-cn/windows-hardware/drivers/ddi/wdm/ns-wdm-_osversioninfoexw
            internal int OSVersionInfoSize;     //对象结构的大小
            internal int MajorVersion;          //主要版本号
            internal int MinorVersion;          //次要版本号
            internal int BuildNumber;           //内部版本号
            internal int PlatformId;            //平台标识符
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string CSDVersion;         //补丁版本字符串，如Service Pack 3
            internal ushort ServicePackMajor;   //补丁版本的主要版本号，如Service Pack 3主要版本号为3
            internal ushort ServicePackMinor;   //补丁版本的次要版本号，如Service Pack 3次要版本号为0
            internal short SuiteMask;           //可用的产品套件，如VER_SUITE_SINGLEUSERTS表示支持远程桌面
            internal byte ProductType;          //产品类型，如VER_NT_WORKSTATION表示Windows 2000 或更高版本专业版
            internal byte Reserved;             //保留供将来使用
        }

        public static Version GetVersion()
        {
            var osVersionInfo = new OsVersionInfoEx { OSVersionInfoSize = Marshal.SizeOf(typeof(OsVersionInfoEx)) };
            if (!RtlGetVersion(ref osVersionInfo))
            {
                osVersionInfo = new OsVersionInfoEx
                {
                    OSVersionInfoSize = Marshal.SizeOf(typeof(OsVersionInfoEx))
                };
                GetVersionEx(osVersionInfo);
            }

            var version = new Version(
                osVersionInfo.MajorVersion,
                osVersionInfo.MinorVersion,
                osVersionInfo.BuildNumber);

            return version;
        }

        public static string GetOSVersion()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                var sCPUSerialNumber = "";
                foreach (var mo in searcher.Get())
                {

                    sCPUSerialNumber = mo["Name"].ToString().Trim();
                }
                return sCPUSerialNumber.Split('|').First();//分割字符串
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
