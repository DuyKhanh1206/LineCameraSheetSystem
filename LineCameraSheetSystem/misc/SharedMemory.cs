using System;
using System.IO;
using System.Threading;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Fujita.Misc
{
    /// <summary>
    /// 共有メモリオブジェクト
    /// </summary>
    /// <typeparam name="T">オブジェクトの型</typeparam>
    public class SharedMemoryObject<T> : RealProxy, IDisposable
    {
        SharedMemoryData data;
        object instance;
        bool processing;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        private SharedMemoryObject(string name, int size)
            : base(typeof(T))
        {
            data = SharedMemoryData.Create(name, size);
            data.SetData(instance = Activator.CreateInstance<T>());
            data.EventEnabled = true;
            data.Changed += new EventHandler(data_Changed);
            processing = false;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        private SharedMemoryObject(string name)
            : base(typeof(T))
        {
            data = SharedMemoryData.Get(name);
            instance = data.GetData();
            data.EventEnabled = true;
            data.Changed += new EventHandler(data_Changed);
            processing = false;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SharedMemoryObject()
        {
            Dispose();
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            if (data != null)
            {
                data.Dispose();
                data = null;
            }
        }

        /// <summary>
        /// 作成
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static SharedMemoryObject<T> Create(string name, int size)
        {
            return new SharedMemoryObject<T>(name, size);
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SharedMemoryObject<T> Get(string name)
        {
            return new SharedMemoryObject<T>(name);
        }

        /// <summary>
        /// オブジェクトの取得
        /// </summary>
        /// <returns></returns>
        public T GetObject()
        {
            return (T)(this.GetTransparentProxy());
        }

        /// <summary>
        /// 存在確認
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Exists(string name)
        {
            return SharedMemoryData.Exists(name);
        }

        /// <summary>
        /// 暗黙的キャスト
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static implicit operator T(SharedMemoryObject<T> obj)
        {
            return obj.GetObject();
        }


        /// <summary>
        /// メッセージ処理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage mm = (IMethodCallMessage)msg;
            IMessage retmsg = null;

            processing = true;

            switch (mm.MethodName)
            {
                case "FieldGetter":
                    retmsg = FieldGetter(mm, instance);
                    break;

                case "FieldSetter":
                    retmsg = FieldSetter(mm, instance);
                    break;

                default:
                    MethodInfo mi = (MethodInfo)mm.MethodBase;
                    ParameterInfo[] pi = mm.MethodBase.GetParameters();

                    object[] args = new object[pi.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        args[i] = pi[i].IsOut ? null : mm.Args[i];
                    }

                    object ret = mi.Invoke(instance, args);

                    retmsg = new ReturnMessage(ret, args, args.Length, mm.LogicalCallContext, mm);
                    break;
            }

            data.SetData(instance);
            processing = false;

            return retmsg;
        }

        private IMethodReturnMessage FieldGetter(IMethodCallMessage mm, object instance)
        {
            string typeName = mm.Args[0] as string;
            Type type = Type.GetType(typeName);

            string fieldName = mm.Args[1] as string;
            FieldInfo fi = type.GetField(fieldName);

            object value = fi.GetValue(instance);
            object[] outArgs = new object[] {
            null,
            null,
            Convert.ChangeType(value, fi.FieldType)
            };
            return new ReturnMessage(typeof(void), outArgs, outArgs.Length, mm.LogicalCallContext, mm);
        }

        private IMethodReturnMessage FieldSetter(IMethodCallMessage mm, object instance)
        {
            string typeName = mm.Args[0] as string;
            Type type = Type.GetType(typeName);

            string fieldName = mm.Args[1] as string;
            FieldInfo fi = type.GetField(fieldName);

            fi.SetValue(instance, mm.Args[2]);

            return new ReturnMessage(typeof(void), null, 0, mm.LogicalCallContext, mm);
        }

        void data_Changed(object sender, EventArgs e)
        {
            while (processing) Thread.Sleep(10);
            instance = data.GetData();
        }
    }


    /// <summary>
    /// 共有メモリデータクラス
    /// </summary>
    public class SharedMemoryData : IDisposable
    {
        const string EVENT_SUFFIX = "__Set";
        SharedMemoryStream stream;
        EventWaitHandle set;
        Thread checker;
        bool eventEnabled;

        public event EventHandler Changed;

        /// <summary>
        /// コンストラクタ(作成)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        private SharedMemoryData(string name, int size)
        {
            stream = SharedMemoryStream.Create(name, size);
            initialize(name);
        }

        /// <summary>
        /// コンストラクタ(取得)
        /// </summary>
        /// <param name="name"></param>
        private SharedMemoryData(string name)
        {
            stream = SharedMemoryStream.Open(name);
            initialize(name);
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SharedMemoryData()
        {
            Dispose();
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            EventEnabled = false;

            checker = null;
            set = null;

            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
        }

        /// <summary>
        /// 作成
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static SharedMemoryData Create(string name, int size)
        {
            return new SharedMemoryData(name, size);
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SharedMemoryData Get(string name)
        {
            return new SharedMemoryData(name);
        }

        /// <summary>
        /// 存在確認
        /// </summary>
        /// <returns></returns>
        public static bool Exists(string name)
        {
            return SharedMemoryStream.Exists(name);
        }

        /// <summary>
        /// イベントの有効化/無効化
        /// </summary>
        public bool EventEnabled
        {
            get
            {
                return eventEnabled;
            }
            set
            {
                if (eventEnabled != value)
                {
                    if (value)
                    {
                        if (checker == null)
                        {
                            checker = new Thread(new ThreadStart(Checker));
                            checker.Name = "SharedMemory";
                            checker.IsBackground = true;
                        }
                        if (!checker.IsAlive)
                        {
                            checker.Start();
                        }
                    }
                    else
                    {
                        if (checker != null)
                        {
                            if (checker.IsAlive)
                            {
                                checker.Abort();
                                checker.Join();
                            }
                        }
                    }
                    eventEnabled = value;
                }
            }
        }

        /// <summary>
        /// ストリーム
        /// </summary>
        public SharedMemoryStream Stream
        {
            get
            {
                return stream;
            }
        }

        /// <summary>
        /// バイト配列の取得
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return stream.ToArray();
        }

        /// <summary>
        /// データの取得
        /// </summary>
        /// <returns></returns>
        public object GetData()
        {
            object obj = null;

            stream.WaitPermission();
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                obj = formatter.Deserialize(stream);
            }
            finally
            {
                stream.ReleasePermission();
            }
            return obj;
        }

        /// <summary>
        /// データの取得
        /// </summary>
        /// <param name="data"></param>
        public void GetData(ref object data)
        {
            data = GetData();
        }

        /// <summary>
        /// データの設定
        /// </summary>
        /// <param name="data"></param>
        public void SetData(object data)
        {
            stream.WaitPermission();
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                stream.SetLength(0);
                formatter.Serialize(stream, data);

                if (eventEnabled) setUpdated();
            }
            finally
            {
                stream.ReleasePermission();
            }
        }

        private void initialize(string name)
        {
            set = new EventWaitHandle(false, EventResetMode.ManualReset, name + EVENT_SUFFIX);
            checker = null;
            eventEnabled = false;
        }

        private void setUpdated()
        {
            new Thread(new ThreadStart(() =>
            {
                set.Set();
                Thread.Sleep(25);
                set.Reset();
            })).Start();
        }

        void Checker()
        {
            try
            {
                byte[] bp = stream.ToArray();
                while (true)
                {
                    set.WaitOne();
                    byte[] bn = stream.ToArray();
                    if (!bn.Equals(bp))
                    {
                        if (Changed != null) Changed.Invoke(this, EventArgs.Empty);
                        bn = bp;
                    }
                }
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// 共有メモリストリームクラス
    /// </summary>
    public class SharedMemoryStream : Stream
    {
        const string MUTEX_SUFFIX = "__Mutex";
        private static readonly int RESERVED_SIZE = Marshal.SizeOf(typeof(int));

        private SharedMemory shared;
        private Mutex mutex;
        private int refcount;
        private int position;

        /// <summary>
        /// コンストラクタ
        /// (共有メモリの作成)
        /// </summary>
        /// <param name="name">共有名</param>
        /// <param name="maxLength">最大サイズ</param>
        /// <param name="protect">保護</param>
        private SharedMemoryStream(string name, int maxLength)
        {
            try
            {
                shared = SharedMemory.Create(name, maxLength);
                initialize(name);
            }
            catch
            {
                Close();
                throw;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// (共有メモリの参照)
        /// </summary>
        /// <param name="name"></param>
        private SharedMemoryStream(string name, SharedMemoryAccessMode desiredAccess)
        {
            try
            {
                shared = SharedMemory.Open(name, desiredAccess);
                initialize(name);
            }
            catch
            {
                Close();
                throw;
            }
        }

        private void initialize(string name)
        {
            position = 0;
            mutex = new Mutex(false, name + MUTEX_SUFFIX);
            refcount = 0;
        }

        /// <summary>
        /// 共有メモリストリームの作成
        /// </summary>
        /// <param name="name">共有名</param>
        /// <param name="maxLength">最大サイズ</param>
        /// <returns>ストリーム</returns>
        public static SharedMemoryStream Create(string name, int maxLength)
        {
            return new SharedMemoryStream(name, maxLength);
        }

        /// <summary>
        /// 共有メモリストリームの取得
        /// </summary>
        /// <param name="name">共有名</param>
        /// <param name="desiredAccess">要求するメモリアクセス</param>
        /// <returns>ストリーム</returns>
        public static SharedMemoryStream Open(string name, SharedMemoryAccessMode desiredAccess)
        {
            return new SharedMemoryStream(name, desiredAccess);
        }

        /// <summary>
        /// 共有メモリストリームの取得
        /// </summary>
        /// <param name="name">共有名</param>
        /// <returns>ストリーム</returns>
        public static SharedMemoryStream Open(string name)
        {
            return new SharedMemoryStream(name, SharedMemoryAccessMode.AllAccess);
        }

        /// <summary>
        /// 存在確認
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Exists(string name)
        {
            return SharedMemory.Exists(name);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (mutex != null)
                {
                    mutex.Close();
                    mutex = null;
                }

                if (shared != null)
                {
                    shared.Close();
                    shared = null;
                }
            }
            catch
            {
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 共有メモリ名
        /// </summary>
        public string Name { get { return shared.Name; } }

        /// <summary>
        /// 最大長さ
        /// </summary>
        public long MaxLength { get { return shared.Size; } }

        /// <summary>
        /// クローズ
        /// </summary>
        public override void Close()
        {
            Dispose(true);
        }

        /// <summary>
        /// 読み込み可
        /// </summary>
        public override bool CanRead { get { return true; } }

        /// <summary>
        /// シーク可
        /// </summary>
        public override bool CanSeek { get { return true; } }

        /// <summary>
        /// 書き込み可
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return shared.AccessMode != SharedMemoryAccessMode.Read;
            }
        }

        /// <summary>
        /// データのフラッシュ
        /// </summary>
        public override void Flush()
        {
            // 何もしない
        }

        /// <summary>
        /// バッファの取得
        /// </summary>
        /// <returns></returns>
        public byte[] GetBuffer()
        {
            if (isNull()) throw new ObjectDisposedException(Name); // 既に破棄されている

            WaitPermission();
            byte[] b;
            try
            {
                b = new byte[MaxLength];
                for (int i = 0; i < b.Length; i++) b[i] = shared.ReadByte(RESERVED_SIZE + i);
            }
            finally
            {
                ReleasePermission();
            }
            return b;
        }

        /// <summary>
        /// ストリームの長さ
        /// </summary>
        public override long Length
        {
            get
            {
                if (isNull()) throw new ObjectDisposedException(Name); // 既に破棄されている

                WaitPermission();
                long len = 0;
                try
                {
                    len = (long)getLength();
                }
                finally
                {
                    ReleasePermission();
                }
                return len;
            }
        }

        /// <summary>
        /// ストリームポインタの位置
        /// </summary>
        public override long Position
        {
            get
            {
                if (isNull()) throw new ObjectDisposedException(Name); // 既に破棄されている

                return position;
            }
            set
            {
                if (isNull()) throw new ObjectDisposedException(Name); // 既に破棄されている
                if (value > MaxLength) throw new IOException(); // 最大長より後ろには設定できない

                position = (int)value;
                return;
            }
        }

        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="buffer">バッファ</param>
        /// <param name="offset">オフセット</param>
        /// <param name="count">バイト数</param>
        /// <returns>バイト数</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (isNull()) throw new ObjectDisposedException(Name); // 既に破棄されている
            if (buffer == null) throw new ArgumentNullException(); // バッファがNULL
            if (offset < 0 || count < 0) throw new ArgumentOutOfRangeException(); // オフセットかカウントが負
            if (buffer.Length < offset + count) throw new ArgumentException(); // オフセット＋カウントがバッファ長より大きい

            int n = 0;
            WaitPermission();
            try
            {
                int len = getLength();
                for (int i = 0; i < count; i++)
                {
                    if (position >= len) break;
                    buffer[offset + i] = shared.ReadByte(RESERVED_SIZE + position);
                    position++;
                    n++;
                }
            }
            finally
            {
                ReleasePermission();
            }
            return n;
        }

        /// <summary>
        /// シーク
        /// </summary>
        /// <param name="offset">オフセット</param>
        /// <param name="origin">基準</param>
        /// <returns>新しいオフセット</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;

                case SeekOrigin.Current:
                    Position += offset;
                    break;

                case SeekOrigin.End:
                    Position = Length - 1 + offset;
                    break;
            }
            return Position;
        }

        /// <summary>
        /// 長さの設定
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            if (isNull()) throw new ObjectDisposedException(Name); // 既に破棄されている
            if (value > (long)MaxLength) throw new IOException(); // 最大長より大きい値には設定できない

            WaitPermission();
            try
            {
                setLength((int)value);
            }
            finally
            {
                ReleasePermission();
            }
        }

        /// <summary>
        /// 配列の取得
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            if (isNull()) throw new ObjectDisposedException(Name); // 既に破棄されている

            WaitPermission();
            byte[] b;
            try
            {
                b = new byte[getLength()];
                for (int i = 0; i < b.Length; i++) b[i] = shared.ReadByte(RESERVED_SIZE + i);
            }
            finally
            {
                ReleasePermission();
            }
            return b;
        }

        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="buffer">バッファ</param>
        /// <param name="offset">オフセット</param>
        /// <param name="count">バイト数</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (isNull()) throw new ObjectDisposedException(Name); // 既に破棄されている
            if (buffer == null) throw new ArgumentNullException(); // バッファがNULL
            if (offset < 0 || count < 0) throw new ArgumentOutOfRangeException(); // オフセットかカウントが負
            if (buffer.Length < offset + count) throw new ArgumentException(); // オフセット＋カウントがバッファ長より大きい

            WaitPermission();
            try
            {
                for (int i = 0; i < count; i++)
                {
                    if (position >= MaxLength) break;
                    shared.WriteByte(RESERVED_SIZE + position, buffer[offset + i]);
                    position++;
                }
                if (position > getLength())
                {
                    setLength(position);
                }
            }
            finally
            {
                ReleasePermission();
            }
        }

        /// <summary>
        /// アクセス権の取得
        /// </summary>
        internal void WaitPermission()
        {
            if (refcount == 0)
            {
                mutex.WaitOne();
            }
            refcount++;
        }

        /// <summary>
        /// アクセス権の解放
        /// </summary>
        /// <returns></returns>
        internal void ReleasePermission()
        {
            refcount--;
            if (refcount <= 0)
            {
                mutex.ReleaseMutex();
                refcount = 0;
            }
        }

        private bool isNull()
        {
            return shared == null || shared.View == IntPtr.Zero;
        }

        private int getLength()
        {
            return shared.ReadInt32();
        }

        private void setLength(int len)
        {
            shared.WriteInt32(len);
        }
    }

    /// <summary>
    /// 低レベル共有メモリアクセスクラス
    /// </summary>
    public class SharedMemory : IDisposable
    {
        const uint PAGE_READWRITE = 0x4;
        const int ERROR_ALREADY_EXISTS = 183;

        private static readonly IntPtr NULL = IntPtr.Zero;
        private static readonly IntPtr INVALID_HANDLE_VALUE = (IntPtr)(-1);

        /// <summary>
        /// コンストラクタ
        /// (共有メモリの作成)
        /// </summary>
        /// <param name="name">共有名</param>
        /// <param name="size">最大サイズ</param>
        private SharedMemory(string name, int size)
        {
            try
            {
                /// ハンドル取得
                Handle = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, size, name);
                int code = Marshal.GetLastWin32Error();
                if (Handle == NULL || code == ERROR_ALREADY_EXISTS) throw new Win32Exception(code);

                /// マッピング
                View = MapViewOfFile(Handle, (uint)SharedMemoryAccessMode.AllAccess, 0, 0, 0);
                if (View == NULL) throw new Win32Exception(Marshal.GetLastWin32Error());

                /// プロパティの設定
                Name = name;
                Size = size;
                AccessMode = SharedMemoryAccessMode.AllAccess;
            }
            catch
            {
                Close();
                throw;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// (共有メモリの参照)
        /// </summary>
        /// <param name="name"></param>
        private SharedMemory(string name, SharedMemoryAccessMode desiredAccess)
        {
            try
            {
                /// ハンドル取得
                Handle = OpenFileMapping((uint)desiredAccess, false, name);
                if (Handle == NULL) throw new Win32Exception(Marshal.GetLastWin32Error());

                /// マッピング
                View = MapViewOfFile(Handle, (uint)desiredAccess, 0, 0, 0);
                if (View == NULL) throw new Win32Exception(Marshal.GetLastWin32Error());

                /// プロパティの設定
                Name = name;
                Size = GetSizeFromView(View);
                AccessMode = desiredAccess;
            }
            catch
            {
                Close();
                throw;
            }
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SharedMemory()
        {
            Dispose();
        }

        /// <summary>
        /// 共有メモリの作成
        /// </summary>
        /// <param name="name">共有名</param>
        /// <param name="size">最大サイズ</param>
        /// <returns>共有メモリ</returns>
        public static SharedMemory Create(string name, int size)
        {
            return new SharedMemory(name, size);
        }

        /// <summary>
        /// 共有メモリの取得
        /// </summary>
        /// <param name="name">共有名</param>
        /// <param name="desiredAccess">要求するメモリアクセス</param>
        /// <returns>共有メモリ</returns>
        public static SharedMemory Open(string name, SharedMemoryAccessMode desiredAccess)
        {
            return new SharedMemory(name, desiredAccess);
        }

        /// <summary>
        /// 共有メモリの取得
        /// </summary>
        /// <param name="name">共有名</param>
        /// <returns>共有メモリ</returns>
        public static SharedMemory Open(string name)
        {
            return new SharedMemory(name, SharedMemoryAccessMode.AllAccess);
        }

        /// <summary>
        /// 存在確認
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Exists(string name)
        {
            try
            {
                bool b = false;
                IntPtr h = OpenFileMapping((uint)SharedMemoryAccessMode.Read, false, name);
                if (h != NULL)
                {
                    CloseHandle(h);
                    b = true;
                }
                return b;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (View != NULL)
                {
                    UnmapViewOfFile(View);
                    View = NULL;
                }

                if (Handle != NULL)
                {
                    CloseHandle(Handle);
                    Handle = NULL;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// ビューからサイズを取得
        /// </summary>
        /// <param name="view">ビュー</param>
        /// <returns>サイズ</returns>
        protected int GetSizeFromView(IntPtr view)
        {
            MEMORY_BASIC_INFORMATION mbi = new MEMORY_BASIC_INFORMATION();
            VirtualQuery(view, ref mbi, Marshal.SizeOf(mbi));
            return mbi.RegionSize;
        }

        /// <summary>
        /// 共有メモリ名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 共有メモリハンドル
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// 共有メモリのマッピングビューポインタ
        /// </summary>
        public IntPtr View { get; private set; }

        /// <summary>
        /// アクセスモード
        /// </summary>
        public SharedMemoryAccessMode AccessMode { get; private set; }

        /// <summary>
        /// サイズ
        /// </summary>
        public long Size { get; private set; }

        /// <summary>
        /// クローズ
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        public byte ReadByte() { return Marshal.ReadByte(View); }
        public byte ReadByte(int ofs) { return Marshal.ReadByte(View, ofs); }

        public Int16 ReadInt16() { return Marshal.ReadInt16(View); }
        public Int16 ReadInt16(int ofs) { return Marshal.ReadInt16(View, ofs); }

        public Int32 ReadInt32() { return Marshal.ReadInt32(View); }
        public Int32 ReadInt32(int ofs) { return Marshal.ReadInt32(View, ofs); }

        public Int64 ReadInt64() { return Marshal.ReadInt64(View); }
        public Int64 ReadInt64(int ofs) { return Marshal.ReadInt64(View, ofs); }

        public IntPtr ReadIntPtr() { return Marshal.ReadIntPtr(View); }
        public IntPtr ReadIntPtr(int ofs) { return Marshal.ReadIntPtr(View, ofs); }

        public object ReadStructure(Type t) { return Marshal.PtrToStructure(View, t); }
        public object ReadStructure(int ofs, Type t) { return Marshal.PtrToStructure(Offset(ofs), t); }

        public void WriteByte(byte val) { Marshal.WriteByte(View, val); }
        public void WriteByte(int ofs, byte val) { Marshal.WriteByte(View, ofs, val); }

        public void WriteInt16(Int16 val) { Marshal.WriteInt16(View, val); }
        public void WriteInt16(int ofs, Int16 val) { Marshal.WriteInt16(View, ofs, val); }

        public void WriteInt32(Int32 val) { Marshal.WriteInt32(View, val); }
        public void WriteInt32(int ofs, Int32 val) { Marshal.WriteInt32(View, ofs, val); }

        public void WriteInt64(Int64 val) { Marshal.WriteInt64(View, val); }
        public void WriteInt64(int ofs, Int64 val) { Marshal.WriteInt64(View, ofs, val); }

        public void WriteStructure(object o) { Marshal.StructureToPtr(o, View, false); }
        public void WriteStructure(int ofs, object o) { Marshal.StructureToPtr(o, Offset(ofs), false); }

        public void Copy(byte[] src, int idx, int count)
        {
            Marshal.Copy(src, idx, View, count);
        }

        public void Copy(byte[] src, int idx, int count, int ofs)
        {
            Marshal.Copy(src, idx, Offset(ofs), count);
        }

        public IntPtr Offset(int ofs)
        {
            return (IntPtr)(View.ToInt64() + ofs);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFileMapping(
        IntPtr hFile, // ファイルのハンドル
        IntPtr lpAttributes, // セキュリティ
        uint flProtect, // 保護
        uint dwMaximumSizeHigh, // サイズを表す上位 DWORD
        int dwMaximumSizeLow, // サイズを表す下位 DWORD
        string lpName // オブジェクト名
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenFileMapping(
        uint dwDesiredAccess, // アクセスモード
        [MarshalAs(UnmanagedType.Bool)]
bool bInheritHandle, // 継承フラグ
        string lpName // オブジェクト名
        );


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr MapViewOfFile(
        IntPtr hFileMappingObject, // ファイルマッピングオブジェクトのハンドル
        uint dwDesiredAccess, // アクセスモード
        uint dwFileOffsetHigh, // オフセットの上位 DWORD
        uint dwFileOffsetLow, // オフセットの下位 DWORD
        uint dwNumberOfBytesToMap // マップ対象のバイト数
        );

        [DllImport("kernel32.dll")]
        static extern bool UnmapViewOfFile(
        IntPtr lpBaseAddress // 開始アドレス
        );

        [DllImport("kernel32.dll")]
        static extern bool CloseHandle(
        IntPtr hObject // オブジェクトのハンドル
        );

        [DllImport("kernel32.dll")]
        static extern uint VirtualQuery(
        IntPtr lpAddress, // 領域のアドレス
        ref MEMORY_BASIC_INFORMATION lpBuffer, // 情報バッファのアドレス
        int dwLength // バッファのサイズ
        );

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public int RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }
    }

    /// <summary>
    /// 共有メモリのアクセスモード
    /// </summary>
    public enum SharedMemoryAccessMode : uint
    {
        Write = 0x0002,
        Read = 0x0004,
        AllAccess = 0x001F
    }
}