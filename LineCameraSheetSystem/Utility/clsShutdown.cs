using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineCameraSheetSystem
{
    /// <summary>  tất cả các hàm trong Dll này đều phục vụ cho việc kiểm tra trước khi tắt máy trong hàm  AdjustToken() </summary>
    public class clsShutdown
    {

        public enum ExitWindows : uint
        {
            EWX_LOGOFF = 0x00,
            EWX_SHUTDOWN = 0x01,
            EWX_REBOOT = 0x02,
            EWX_POWEROFF = 0x08,
            EWX_RESTARTAPPS = 0x40,
            EWX_FORCE = 0x04,
            EWX_FORCEIFHUNG = 0x10,
        }
        /// <summary>
        /// leenhjt tắt máy tính thừ thư viện user32.dll của máy
        /// </summary>
        /// <param name="uFlags"> Cờ tắt máy. nguyên nhân do enum ExitWindows.EWX_POWEROFF hoặc bất kỳ </param>
        /// <param name="dwReason"> kiểu int và = 0 </param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        public static extern bool ExitWindowsEx(ExitWindows uFlags,
            int dwReason);

        /// <summary>
        /// trả về vị trí con trỏ của tiến trình hiện tại
        /// </summary>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        /// <summary> trả về thông tin quyền truy cập liên quan đến quản lý truy cập và quyền truy cập </summary>  
        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle,
            uint DesiredAccess,
            out IntPtr TokenHandle);


        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true,
            CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern bool LookupPrivilegeValue(string lpSystemName,
            string lpName,
            out long lpLuid);

        [System.Runtime.InteropServices.StructLayout(
           System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
        private struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            public long Luid;
            public int Attributes;
        }

        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
            bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES NewState,
            int BufferLength,
            IntPtr PreviousState,
            IntPtr ReturnLength);

        //シャットダウンするためのセキュリティ特権を有効にする            kích hoạt quền bảo mật để tắt       (tắt máy)

        public static void AdjustToken()
        {
            const uint TOKEN_ADJUST_PRIVILEGES = 0x20;
            const uint TOKEN_QUERY = 0x8;
            const int SE_PRIVILEGE_ENABLED = 0x2;
            const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return;

            IntPtr procHandle = GetCurrentProcess();

            //トークンを取得する        Nhận được mã thông báo     
            IntPtr tokenHandle;
            OpenProcessToken(procHandle,
                TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out tokenHandle);
            //LUIDを取得する         Nhận LUID
            TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES();
            tp.Attributes = SE_PRIVILEGE_ENABLED;
            tp.PrivilegeCount = 1;
            LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, out tp.Luid);
            //特権を有効にする              kích hoạt đặc quyền
            AdjustTokenPrivileges(
                tokenHandle, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
        }

        public void Shutdown() //tắt máy?
        {
            //シャットダウンする 
            AdjustToken();
            ExitWindowsEx(ExitWindows.EWX_POWEROFF, 0);// lệnh tắt máy
        }
        public void Reboot()
        {
            AdjustToken();
            ExitWindowsEx(ExitWindows.EWX_REBOOT, 0);// lệnh tắt máy
        }
    }
}
