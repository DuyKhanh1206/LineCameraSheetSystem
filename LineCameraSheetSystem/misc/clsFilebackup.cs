using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;

namespace Fujita.Misc
{
    static class StringExtensions
    {
#if STRING_EXTENSION
        #region　Left メソッド

        /// -----------------------------------------------------------------------------------
        /// <summary>
        ///     文字列の左端から指定された文字数分の文字列を返します。</summary>
        /// <param name="stTarget">
        ///     取り出す元になる文字列。</param>
        /// <param name="iLength">
        ///     取り出す文字数。</param>
        /// <returns>
        ///     左端から指定された文字数分の文字列。
        ///     文字数を超えた場合は、文字列全体が返されます。</returns>
        /// -----------------------------------------------------------------------------------
        public static string Left(this string stTarget, int iLength)
        {
            if (iLength <= stTarget.Length)
            {
                return stTarget.Substring(0, iLength);
            }

            return stTarget;
        }

        #endregion

        #region　Mid メソッド (+1)

        /// -----------------------------------------------------------------------------------
        /// <summary>
        ///     文字列の指定された位置以降のすべての文字列を返します。</summary>
        /// <param name="stTarget">
        ///     取り出す元になる文字列。</param>
        /// <param name="iStart">
        ///     取り出しを開始する位置。</param>
        /// <returns>
        ///     指定された位置以降のすべての文字列。</returns>
        /// -----------------------------------------------------------------------------------
        public static string Mid(this string stTarget, int iStart)
        {
            if (iStart <= stTarget.Length)
            {
                return stTarget.Substring(iStart - 1);
            }

            return string.Empty;
        }

        /// -----------------------------------------------------------------------------------
        /// <summary>
        ///     文字列の指定された位置から、指定された文字数分の文字列を返します。</summary>
        /// <param name="stTarget">
        ///     取り出す元になる文字列。</param>
        /// <param name="iStart">
        ///     取り出しを開始する位置。</param>
        /// <param name="iLength">
        ///     取り出す文字数。</param>
        /// <returns>
        ///     指定された位置から指定された文字数分の文字列。
        ///     文字数を超えた場合は、指定された位置からすべての文字列が返されます。</returns>
        /// -----------------------------------------------------------------------------------
        public static string Mid(this string stTarget, int iStart, int iLength)
        {
            if (iStart <= stTarget.Length)
            {
                if (iStart + iLength - 1 <= stTarget.Length)
                {
                    return stTarget.Substring(iStart - 1, iLength);
                }

                return stTarget.Substring(iStart - 1);
            }

            return string.Empty;
        }

        #endregion

        #region　Right メソッド (+1)

        /// -----------------------------------------------------------------------------------
        /// <summary>
        ///     文字列の右端から指定された文字数分の文字列を返します。</summary>
        /// <param name="stTarget">
        ///     取り出す元になる文字列。</param>
        /// <param name="iLength">
        ///     取り出す文字数。</param>
        /// <returns>
        ///     右端から指定された文字数分の文字列。
        ///     文字数を超えた場合は、文字列全体が返されます。</returns>
        /// -----------------------------------------------------------------------------------
        public static string Right(this string stTarget, int iLength)
        {
            if (iLength <= stTarget.Length)
            {
                return stTarget.Substring(stTarget.Length - iLength);
            }

            return stTarget;
        }

        #endregion
#endif
        public static string GetFolderPath(this string s)
        {
            if (s.Right(1) != "\\")
                return s + "\\";
            else
                return s;
        }

        public static string GetFileName(this string s)
        {
            int iLastIndex = s.LastIndexOf('\\');
            if (iLastIndex != -1)
            {
                return s.Substring(iLastIndex + 1);
            }
            return "";
        }

        public static string GetFileExtension(this string s)
        {
            int iLastIndex = s.LastIndexOf('.');
            if (iLastIndex != -1)
            {
                return s.Substring(iLastIndex + 1);
            }
            return "";
        }

        public static string GetDrivePath(this string s)
        {
            Regex reg = new Regex(@"^[A-Za-z]:\\");
            Match mtc = reg.Match(s);
            if (!mtc.Success)
                return "";
            return mtc.Value;
        }
    }

    class clsFilebackup
    {
        public static bool Backup(string source, string dest, EFileChoiceType fct = EFileChoiceType.NewUpdate, ESyncFileType sft = ESyncFileType.Delete, ESyncDirectoryType sdt = ESyncDirectoryType.Delete)
        {
            string sReason;
            clsFilebackup backup = new clsFilebackup();
            backup.AddTarget("dummy", source, dest, out sReason);
            backup.SetFileChoiceType("dummy", fct);
            backup.SetSyncFileType("dummy", sft);
            backup.SetSyncDirectoryType("dummy", sdt);
            return backup.DoBackup();
        }

        public enum EFileChoiceType
        {
            NewUpdate,
            Source,
        }

        public enum ESyncFileType
        {
            Save,
            Delete,
        }

        public enum ESyncDirectoryType
        {
            Save,
            Delete,
        }

        class clsTarget
        {
            public string Name { get; set; }
            public bool Enable { get; set; }
            public EFileChoiceType FileChoiceType { get; set; }
            public ESyncDirectoryType SyncDirectoryType { get; set; }
            public ESyncFileType SyncFileType { get; set; }
            public string SourceFolders { get; set; }
            public string DestinationFolders { get; set; }
            public bool IncludeSubFolder { get; set; }

            public DateTime LastUpdate { get; set; }
            public bool LastResult { get; set; }
            public string LastResultMessage { get; set; }

            public clsTarget()
            {
                Enable = true;
                FileChoiceType = EFileChoiceType.NewUpdate;
                SyncDirectoryType = ESyncDirectoryType.Delete;
                SyncFileType = ESyncFileType.Delete;
                IncludeSubFolder = true;
            }
        }

        private bool checkParams(clsTarget cls, out string sReason)
        {
            sReason = "";

            if (cls.SourceFolders == "" || !System.IO.Directory.Exists(cls.SourceFolders))
            {
                sReason = "バックアップ元フォルダが不正";
                cls.LastResult = false;
                cls.LastResultMessage = sReason;
                return false;
            }

            if (!CheckDestFolder(cls.DestinationFolders, out sReason))
            {
                cls.LastResult = false;
                cls.LastResultMessage = sReason;
                return false;
            }

            return true;
        }

        private string[] getFileNames(string[] saFilesPaths)
        {
            int iCnt = 0;
            for (int i = 0; i < saFilesPaths.Length; i++)
            {
                if (saFilesPaths[i].GetFileName() != "")
                    iCnt++;
            }
            if (iCnt == 0)
                return new string[0];

            string[] saRet = new string[iCnt];

            iCnt = 0;
            for (int i = 0; i < saFilesPaths.Length ; i++)
            {
                string sFileName = saFilesPaths[i].GetFileName();
                if ( sFileName != "")
                {
                    saRet[iCnt] = sFileName;
                    iCnt++;
                }
            }
            return saRet;
        }

        private void syncFiles(string sSrcFolderPath, string sDestFolderPath, EFileChoiceType fcType, ESyncFileType sfType )
        {
            try
            {
                string[] saSrcFiles = Directory.GetFiles(sSrcFolderPath);
                string[] saDestFiles = Directory.GetFiles(sDestFolderPath);
                string[] saFileNames = getFileNames(saDestFiles);
                bool[] baCopied = new bool[saFileNames.Length];

                for (int i = 0; i < saSrcFiles.Length; i++)
                {
                    string sFileName = saSrcFiles[i].GetFileName();
                    int iFind = Array.IndexOf(saFileNames, sFileName);
                    if (iFind == -1)
                    {
                        // 無条件コピー
                        File.Copy(saSrcFiles[i], sDestFolderPath + sFileName);
                        if (OnCopy != null)
                        {
                            OnCopy(this, new CopyEventArgs(saSrcFiles[i], sDestFolderPath + sFileName));
                        }
                    }
                    else
                    {
                        // 存在する場合、条件に合わせて上書きをする
                        if (fcType == EFileChoiceType.Source)
                        {
                            File.Delete(sDestFolderPath + sFileName);
                            File.Copy(saSrcFiles[i], sDestFolderPath + sFileName);
                            if (OnCopy != null)
                            {
                                OnCopy(this, new CopyEventArgs(saSrcFiles[i], sDestFolderPath + sFileName));
                            }
                        }
                        else
                        {
                            DateTime dtSrcTime = File.GetLastWriteTime(saSrcFiles[i]);
                            DateTime dtDestTime = File.GetLastWriteTime(sDestFolderPath + sFileName);

                            if (dtSrcTime > dtDestTime)
                            {
                                File.Delete(sDestFolderPath + sFileName);
                                File.Copy(saSrcFiles[i], sDestFolderPath + sFileName);
                                if (OnCopy != null)
                                {
                                    OnCopy(this, new CopyEventArgs(saSrcFiles[i], sDestFolderPath + sFileName));
                                }
                            }
                        }
                        baCopied[iFind] = true;
                    }
                }

                if (sfType == ESyncFileType.Delete)
                {
                    // 元に存在しないファイルが、先にあった場合削除する
                    for (int i = 0; i < saFileNames.Length; i++)
                    {
                        if (!baCopied[i])
                        {
                            File.Delete(sDestFolderPath + saFileNames[i]);
                            if (OnDelete != null)
                            {
                                OnDelete(this, new DeleteEventArgs(sDestFolderPath + saFileNames[i], false));
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private bool recursiveBackup(string sSource, string sDest, EFileChoiceType fcType, ESyncFileType sfType, ESyncDirectoryType sdType)
        {
            string sSrcFolderPath = sSource.GetFolderPath();
            string sDestFolderPath = sDest.GetFolderPath();

            // フォルダが存在しているかどうか
            if (!Directory.Exists(sSrcFolderPath) 
                || !Directory.Exists(sDestFolderPath))
                return false;

            syncFiles(sSrcFolderPath, sDestFolderPath, fcType, sfType );

            string[] saSrcDirs = Directory.GetDirectories(sSrcFolderPath);
            string[] saDestDirs = Directory.GetDirectories(sDestFolderPath);
            string[] saDestDirNames = getFileNames(saDestDirs);
            bool[] baExistDirs = new bool[saDestDirNames.Length];

            for (int i = 0; i < saSrcDirs.Length; i++)
            {
                string sDir = saSrcDirs[i].GetFileName();
                int iFind = Array.IndexOf(saDestDirNames, sDir);
                if (iFind == -1)
                {
                    Directory.CreateDirectory(sDestFolderPath + sDir);
                }
                else
                {
                    baExistDirs[iFind] = true;
                }
                recursiveBackup(sSrcFolderPath + sDir, sDestFolderPath + sDir, fcType, sfType, sdType);
            }

            if (sdType == ESyncDirectoryType.Delete)
            {
                // 同期から外れたフォルダを削除する
                for (int i = 0; i < saDestDirNames.Length; i++)
                {
                    if (!baExistDirs[i])
                    {
                        DeleteDirectory(sDestFolderPath + saDestDirNames[i]);
                        if (OnDelete != null)
                        {
                            OnDelete( this, new DeleteEventArgs( sDestFolderPath + saDestDirNames[i], true ));
                        }
                    }
                }
            }

            return true;
        }

        private bool backup( string sSource, string sDest, EFileChoiceType fcType, ESyncFileType sfType )
        {
            string sSrcFolderPath = sSource.GetFolderPath();
            string sDestFolderPath = sDest.GetFolderPath();

            syncFiles(sSrcFolderPath, sDestFolderPath, fcType, sfType);

            return true;
        }

        List<clsTarget> _lstTarget = new List<clsTarget>();
        public bool DoBackup()
        {
            DateTime start = DateTime.Now;
            if (OnStart != null)
            {
                OnStart(this, new StartEndEventArgs( start, new DateTime(0), new TimeSpan(0)));
            }

            string sReason;
            for (int i = 0; i < _lstTarget.Count; i++)
            {
                if (!_lstTarget[i].Enable)
                    continue;

                if (!checkParams(_lstTarget[i], out sReason))
                {
                    if (OnError != null)
                    {
                        OnError( this, new ErrorEventArgs(new Exception(sReason )));
                    }

                    continue;
                }

                if (!_lstTarget[i].IncludeSubFolder)
                {
                    backup(_lstTarget[i].SourceFolders, _lstTarget[i].DestinationFolders, _lstTarget[i].FileChoiceType, _lstTarget[i].SyncFileType);
                }
                else
                {
                    recursiveBackup(_lstTarget[i].SourceFolders, _lstTarget[i].DestinationFolders, _lstTarget[i].FileChoiceType, _lstTarget[i].SyncFileType, _lstTarget[i].SyncDirectoryType);
                }
                _lstTarget[i].LastUpdate = DateTime.Now;
            }

            DateTime end = DateTime.Now;
            if (OnEnd != null)
            {
                OnEnd(this, new StartEndEventArgs( start, end, end - start ));
            }

            return true;
        }

        public bool AddTarget(string sName, string sSourceFolder, string sDestFolder, out string sReason )
        {
            sReason = "";
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target != null)
                return false;

            if (!CheckDestFolder(sDestFolder, out sReason ))
            {
                return false;
            }

            target = new clsTarget();
            target.Name = sName;
            target.SourceFolders = sSourceFolder + sSourceFolder.DirectoryMark();
            target.DestinationFolders = sDestFolder + sDestFolder.DirectoryMark(); ;
            _lstTarget.Add(target);
            return true;

        }

        public bool DeleteTarget(string sName)
        {
            return (0 < _lstTarget.RemoveAll(x => x.Name == sName));
        }

        public bool SetEnable(string sName, bool bEnable)
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);

            if (target == null)
                return false;

            target.Enable = bEnable;

            return true;
        }

        public bool GetEnable(string sName, ref bool bEnable )
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            bEnable = target.Enable;
            return true;
        }

        public bool SetSourceFolder(string sName, string sSourcePath)
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            target.SourceFolders = sSourcePath;
            return true;
        }

        public bool GetSourceFolder(string sName, ref string sSourcePath)
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            sSourcePath = target.SourceFolders;
            return true;
        }

        public static  bool isDriveRoot(string sPath)
        {
            Regex reg = new Regex(@"^[A-Za-z]:\\$");
            return reg.IsMatch(sPath);
        }

        public static bool CheckDestFolder(string sDestPath, out string sReason)
        {
            sReason = "";
            string sTargetPath = sDestPath.Trim().GetFolderPath();

            if (sTargetPath == "")
            {
                sReason = "パスが空です";
                return false;
            }

            if (isDriveRoot(sTargetPath))
            {
                sReason = "ルートフォルダは指定できません";
                return false;
            }

            string sDrive = sTargetPath.GetDrivePath().ToLower();
            string sSystemDrive = Environment.GetFolderPath(Environment.SpecialFolder.System).GetDrivePath().ToLower();

            if (sSystemDrive == sDrive)
            {
                sReason = "システムドライブは設定出来ません";
                return false;
            }

            bool bExist = false;
            foreach (var di in DriveInfo.GetDrives())
            {
                if (di.Name.ToLower() == sDrive)
                {
                    if (di.DriveType == DriveType.CDRom)
                    {
                        sReason = "指定先は光学メディアです";
                        return false;
                    }
                    bExist = true;
                }
            }

            if (!bExist)
            {
                sReason = "指定ドライブが存在しません";
                return false;
            }

            return true;

        }

        public bool checkDestFolder( string sDestPath, out string sReason )
        {
            return clsFilebackup.CheckDestFolder(sDestPath, out sReason);
        }

        public bool SetDestFolder(string sName, string sDestPath)
        {
            string sReason;
            if (!checkDestFolder(sDestPath, out sReason))
                return false;

            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            target.DestinationFolders = sDestPath;
            return true;
        }

        public bool GetDestFolder(string sName, ref string sDestPath)
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            sDestPath = target.DestinationFolders;
            return true;
        }

        public bool SetIncludeSubDirectory(string sName, bool bInclude )
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            target.IncludeSubFolder = bInclude;
            return true;
        }

        public bool GetIncludeSubDirectory(string sName, ref bool bInclude)
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            bInclude = target.IncludeSubFolder;
            return true;
        }

        public bool SetFileChoiceType(string sName, EFileChoiceType fcType)
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            target.FileChoiceType = fcType;
            return true;
        }

        public bool GetFileChoiceType(string sName, ref  EFileChoiceType fcType)
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            fcType = target.FileChoiceType;
            return true;
        }

        public bool SetSyncFileType(string sName, ESyncFileType sfType)
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            target.SyncFileType = sfType;
            return true;
        }

        public bool GetSyncFileType(string sName, ref  ESyncFileType sfType)
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            sfType = target.SyncFileType;
            return true;
        }

        public bool SetSyncDirectoryType(string sName, ESyncDirectoryType sdType)
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            target.SyncDirectoryType = sdType;
            return true;
        }

        public bool GetSyncDirectoryType(string sName, ref  ESyncDirectoryType sdType)
        {
            clsTarget target = _lstTarget.Find(x => x.Name == sName);
            if (target == null)
                return false;
            sdType = target.SyncDirectoryType;
            return true;
        }

        public bool Save(string sPath)
        {
            SetIniString("global", "Listnum", _lstTarget.Count.ToString(), sPath);

            for (int i = 0; i < _lstTarget.Count; i++)
            {
                string sSection = "List" + (i+1).ToString();
                SetIniString(sSection, "Name", _lstTarget[i].Name, sPath);
                SetIniString(sSection, "Enable", _lstTarget[i].Enable.ToString(), sPath);
                SetIniString(sSection, "FileChoiceType", _lstTarget[i].FileChoiceType.ToString(), sPath);
                SetIniString(sSection, "SyncDirectoryType", _lstTarget[i].SyncDirectoryType.ToString(), sPath);
                SetIniString(sSection, "SyncFileType", _lstTarget[i].SyncFileType.ToString(), sPath);
                SetIniString(sSection, "SourceFolder", _lstTarget[i].SourceFolders, sPath);
                SetIniString(sSection, "DestinationFolder", _lstTarget[i].DestinationFolders, sPath);
                SetIniString(sSection, "IncludeSubFolder", _lstTarget[i].IncludeSubFolder.ToString(), sPath);
                SetIniString(sSection, "LastUpdate", _lstTarget[i].ToString(), sPath);
            }

            return true;
        }

        public bool Load(string sPath)
        {
            DeleteAll();
            int Count = int.Parse(GetIniString("global", "ListNum", sPath, "0"));
            for (int i = 0; i < Count; i++)
            {
                string sSection = "List" + (i + 1).ToString();

                clsTarget target = new clsTarget();
                target.Name = GetIniString(sSection, "Name", sPath, "" );
                target.Enable = bool.Parse(GetIniString(sSection, "Enable", sPath, false.ToString() ));
                target.FileChoiceType = (EFileChoiceType)Enum.Parse( typeof(EFileChoiceType), GetIniString(sSection, "FileChoiceType", sPath, EFileChoiceType.NewUpdate.ToString() ));
                target.SyncDirectoryType = (ESyncDirectoryType)Enum.Parse(typeof(ESyncDirectoryType), GetIniString(sSection, "SyncDirectoryType", sPath, ESyncFileType.Save.ToString()));
                target.SyncFileType = (ESyncFileType)Enum.Parse( typeof(ESyncFileType), GetIniString(sSection, "SyncFileType", sPath, ESyncFileType.Save.ToString()));
                target.SourceFolders =  GetIniString(sSection, "SourceFolder", sPath, "");
                target.DestinationFolders = GetIniString(sSection, "DestinationFolder", sPath, "");
                target.IncludeSubFolder = bool.Parse( GetIniString(sSection, "IncludeSubFolder", sPath, true.ToString() ));
                target.LastUpdate  = DateTime.Parse( GetIniString(sSection, "LastUpdate", sPath, DateTime.Now.ToString()));

                _lstTarget.Add( target );
            }

            return true;
        }

        public void DeleteAll()
        {
            _lstTarget.Clear();
        }

        public class CopyEventArgs : EventArgs
        {
            public string SourceFilePath { get; private set; }
            public string DestinationFilePath { get; private set; }

            public CopyEventArgs(string sSrc, string sDest)
            {
                SourceFilePath = sSrc;
                DestinationFilePath = sDest;
            }
        }

        public class DeleteEventArgs : EventArgs
        {
            public string DeleteFilePath { get; private set; }
            public bool Directory { get; private set; }

            public DeleteEventArgs(string sDel, bool bDirectory )
            {
                DeleteFilePath = sDel;
                Directory = bDirectory;
            }
        }

        public class StartEndEventArgs : EventArgs
        {
            public DateTime StartTime { get; private set; }
            public DateTime EndTime { get; private set; }
            public TimeSpan ElapseTime { get; private set; }

            public StartEndEventArgs(DateTime st, DateTime ed, TimeSpan sp)
            {
                StartTime = st;
                EndTime = ed;
                ElapseTime = sp;
            }
        }

        public event Action<object, StartEndEventArgs> OnStart;
        public event Action<object, StartEndEventArgs> OnEnd;
        public event Action<object, CopyEventArgs> OnCopy;
        public event Action<object, DeleteEventArgs> OnDelete;
        public event Action<object, ErrorEventArgs> OnError;

        /// <summary>
        /// フォルダを根こそぎ削除する（ReadOnlyでも削除）
        /// </summary>
        /// <param name="dir">削除するフォルダ</param>
        private static void DeleteDirectory(string dir)
        {
            //DirectoryInfoオブジェクトの作成
            DirectoryInfo di = new DirectoryInfo(dir);

            //フォルダ以下のすべてのファイル、フォルダの属性を削除
            RemoveReadonlyAttribute(di);

            //フォルダを根こそぎ削除
            di.Delete(true);
        }

        private static void RemoveReadonlyAttribute(DirectoryInfo dirInfo)
        {
            //基のフォルダの属性を変更
            if ((dirInfo.Attributes & FileAttributes.ReadOnly) ==
                FileAttributes.ReadOnly)
                dirInfo.Attributes = FileAttributes.Normal;
            //フォルダ内のすべてのファイルの属性を変更
            foreach (FileInfo fi in dirInfo.GetFiles())
                if ((fi.Attributes & FileAttributes.ReadOnly) ==
                    FileAttributes.ReadOnly)
                    fi.Attributes = FileAttributes.Normal;
            //サブフォルダの属性を回帰的に変更
            foreach (DirectoryInfo di in dirInfo.GetDirectories())
                RemoveReadonlyAttribute(di);
        }

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(
            string lpAppName, string lpKeyName, string lpDefault,
            char[] lpReturnedString, int nSize, string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        public static extern int WritePrivateProfileString(
            string lpAppName, string lpKeyName,
            string lpString, string lpFileName);

        //INIファイルから文字列を読込む
        public string GetIniString(string SectionName, string KeyName, string FileName, string Defaults)
        {
            char[] buf = new char[1024];
            string Result = "";
            int ret = GetPrivateProfileString(SectionName, KeyName, Defaults, buf, buf.Length, FileName);
            Result = new string(buf, 0, ret);
            return Result;
        }

        //INIファイルに文字列を書込む
        public int SetIniString(string SectionName, string KeyName, string SetValue, string FileName)
        {
            return WritePrivateProfileString(SectionName, KeyName, SetValue, FileName);
        }
    }
}
