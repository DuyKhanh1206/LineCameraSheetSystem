using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace Fujita.Misc
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class AliasNameAttribute : Attribute
    {
        public string AliasName { get; private set; }

        public AliasNameAttribute(string aliasName)
        {
            AliasName = aliasName;
        }
    }

    public static class EnumExt
    {
        // どうしてもメソッドチェインを崩したくない人用
        public static T ThrowIf<T>(this T value, Func<T, bool> predicate, Exception exception)
        {
            if (predicate(value)) throw exception;
            else return value;
        }

        public static string ToAliasName(this Enum value)
        {
            return value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(AliasNameAttribute), false)
                .Cast<AliasNameAttribute>()
                .FirstOrDefault()
                .ThrowIf(a => a == null, new ArgumentException("属性が設定されていません。"))
                .AliasName;
        }
    }

    public static class StringExtension
    {
        public static string DirectoryMark(this string str)
        {
            string ext = "";
            if (str.Length > 0)
            {
                ext = str[str.Length - 1] != '\\' ? "\\" : "";
            }
            return ext;
        }

        public static string FullPathToDirPath(this string str)
        {
            // 空の場合そのまま
            if (str == "" )
                return str;

            // ディレクトリの場合そのまま
            if (str[str.Length - 1] == '\\')
                return str;

            return str.Left(str.LastIndexOf('\\') + 1);
        }

        /// <summary>
        /// ひとつ上の階層に上がる
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ParentDirectory(this string str)
        {
            if (str == "")
                return str;

            if (str.LastIndexOf('\\') != str.Length - 1)
            {
                return str.Left(str.LastIndexOf('\\') + 1);
            }
            else
            {
                return str.Left(str.LastIndexOf('\\', 0, str.Length - 1) + 1);
            }
        }

        /// <summary>
        /// ファイル名を返す
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FileName(this string str)
        {
            if (str == "" || str == null )
                return "";

            return str.Right(str.LastIndexOf('\\') + 1);
        }

        public static string Left(this string s, int iLength)
        {
            return s.Substring(0, Math.Min(iLength, s.Length));
        }

        public static string Right(this string s, int iLength)
        {
            if (iLength >= s.Length)
                return s;

            return s.Substring(s.Length - iLength, iLength);
        }

        public static string Mid(this string s, int iPosition, int iLength)
        {
            if (iPosition >= s.Length || iLength < 0)
                return "";
            if (iPosition + iLength > s.Length)
                iLength = s.Length - iPosition;
            return s.Substring(iPosition, iLength);
        }
    }

    public static class DoubleExt
    {
        public static double ToDeg(this double dRad)
        {
            return dRad / Math.PI * 180;

        }

        public static double ToRad(this double dDeg)
        {
            return dDeg / 180 * Math.PI;
        }

        public static double ToPercent(this double d)
        {
            return d * 100.0;
        }

        public static double ToMinor(this double d)
        {
            return d / 100.0;
        }
    }

    public static class CharExt
    {
        public static bool IsNumber(this char c)
        {
            return char.IsNumber(c);
        }

        public static bool IsNumberPeriod(this char c)
        {
            return (IsNumber(c) || c == '.');
        }
    }

    public static class ListExt
    {
        public const string LIST_DOUBLE_SERIAL_DATA_EXT = ".ldb";

        public static bool SaveBin(this List<double> c, string sPath, string sName )
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_DOUBLE_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;
            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        for (int i = 0; i < c.Count; i++)
                        {
                            bw.Write(c[i]);
                        }
                        bw.Close();
                    }
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public static bool Save(this List<double> c, string sPath, string sName)
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_DOUBLE_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
                    {
                        for (int i = 0; i < c.Count; i++)
                        {
                            sw.WriteLine(c[i]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public static bool LoadBin(this List<double> c, string sPath, string sName )
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_DOUBLE_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;
            if (!File.Exists(sFilePath))
                return false;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] bytDatas = br.ReadBytes((int)fs.Length);
                        for (int i = 0; i < bytDatas.Length; i += sizeof(double))
                        {
                            c.Add(BitConverter.ToDouble(bytDatas, i));
                        }
                        br.Close();
                    }
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public static bool Load(this List<double> c, string sPath, string sName)
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_DOUBLE_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;
            if (!File.Exists(sFilePath))
                return false;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.ASCII))
                    {
                        while (sr.Peek() >= 0)
                        {
                            string sData = sr.ReadLine();
                            c.Add(double.Parse(sData));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                c.Clear();
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public const string LIST_INT_SERIAL_DATA_EXT = ".lnb";

        public static bool SaveBin(this List<int> c, string sPath, string sName)
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_INT_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;
            try
            {
                FileStream fs = new FileStream(sFilePath, FileMode.OpenOrCreate);
                BinaryWriter bw = new BinaryWriter(fs);
                for (int i = 0; i < c.Count; i++)
                {
                    bw.Write(c[i]);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool Save(this List<int> c, string sPath, string sName)
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_INT_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
                    {
                        for (int i = 0; i < c.Count; i++)
                        {
                            sw.WriteLine(c[i]);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool LoadBin(this List<int> c, string sPath, string sName)
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_INT_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;
            if (!File.Exists(sFilePath))
                return false;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] bytDatas = br.ReadBytes((int)fs.Length);
                        for (int i = 0; i < bytDatas.Length; i += sizeof(double))
                        {
                            c.Add(BitConverter.ToInt32(bytDatas, i));
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool Load(this List<int> c, string sPath, string sName)
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_INT_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;
            if (!File.Exists(sFilePath))
                return false;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.ASCII))
                    {
                        while (sr.Peek() >= 0)
                        {
                            string sData = sr.ReadLine();
                            c.Add(int.Parse(sData));
                        }
                    }
                }
            }
            catch (Exception)
            {
                c.Clear();
                return false;
            }
            return true;
        }

        public const string LIST_RECT1_DOUBLE_SERIAL_DATA_EXT = ".lr1";
        public static bool SaveBin( this List<CRectangle1> c, string sPath, string sName )
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_RECT1_DOUBLE_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        for (int i = 0; i < c.Count; i++)
                        {
                            bw.Write(c[i].X);
                            bw.Write(c[i].Y);
                            bw.Write(c[i].Width);
                            bw.Write(c[i].Height);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool Save(this List<CRectangle1> c, string sPath, string sName)
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_RECT1_DOUBLE_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
                    {
                        for (int i = 0; i < c.Count; i++)
                        {
                            sw.WriteLine(c[i].ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool LoadBin( this List<CRectangle1> c, string sPath, string sName )
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_RECT1_DOUBLE_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;

            if (!File.Exists(sFilePath))
                return false;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] bytDatas = br.ReadBytes((int)fs.Length);
                        for (int i = 0; i < bytDatas.Length; i += sizeof(double) * 4)
                        {
                            c.Add(new CRectangle1(
                                BitConverter.ToDouble(bytDatas, i + sizeof(double) * 0),
                                BitConverter.ToDouble(bytDatas, i + sizeof(double) * 1),
                                BitConverter.ToDouble(bytDatas, i + sizeof(double) * 2),
                                BitConverter.ToDouble(bytDatas, i + sizeof(double) * 3)));
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static bool Load(this List<CRectangle1> c, string sPath, string sName)
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_RECT1_DOUBLE_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;
            if (!File.Exists(sFilePath))
                return false;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.ASCII))
                    {
                        while (sr.Peek() >= 0)
                        {
                            string sData = sr.ReadLine();
                            string[] sDiv = sData.Split(new char[] { ',' }, StringSplitOptions.None);
                            if (sDiv.Length != 4)
                                throw new Exception("フォーマットエラー");
                            c.Add(new CRectangle1(double.Parse(sDiv[0].Trim()), double.Parse(sDiv[1].Trim()), 
                                double.Parse(sDiv[2].Trim()), double.Parse(sDiv[3].Trim())));
                        }
                    }
                }
            }
            catch (Exception)
            {
                c.Clear();
                return false;
            }
            return true;
        }

        public const string LIST_RECT2_DOUBLE_SERIAL_DATA_EXT = ".lr2";
        public static bool SaveBin(this List<CRectangle2> c, string sPath, string sName)
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_RECT2_DOUBLE_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        for (int i = 0; i < c.Count; i++)
                        {
                            bw.Write(c[i].Row);
                            bw.Write(c[i].Col);
                            bw.Write(c[i].Phi);
                            bw.Write(c[i].Length1);
                            bw.Write(c[i].Length2);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool Save(this List<CRectangle2> c, string sPath, string sName)
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_RECT2_DOUBLE_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
                    {
                        for (int i = 0; i < c.Count; i++)
                        {
                            sw.WriteLine(c[i].ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool LoadBin(this List<CRectangle2> c, string sPath, string sName)
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_RECT2_DOUBLE_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;

            if (!File.Exists(sFilePath))
                return false;

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] bytDatas = br.ReadBytes((int)fs.Length);
                        for (int i = 0; i < bytDatas.Length; i += sizeof(double) * 4)
                        {
                            c.Add(new CRectangle2(
                                BitConverter.ToDouble(bytDatas, i + sizeof(double) * 0),
                                BitConverter.ToDouble(bytDatas, i + sizeof(double) * 1),
                                BitConverter.ToDouble(bytDatas, i + sizeof(double) * 2),
                                BitConverter.ToDouble(bytDatas, i + sizeof(double) * 3),
                                BitConverter.ToDouble(bytDatas, i + sizeof(double) * 4)));
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool Load(this List<CRectangle2> c, string sPath, string sName)
        {
            if (sName.LastIndexOf('.') == -1)
                sName += LIST_RECT2_DOUBLE_SERIAL_DATA_EXT;
            string sFilePath = sPath + sPath.DirectoryMark() + sName;
            if (!File.Exists(sFilePath))
                return false;

            c.Clear();

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.ASCII))
                    {
                        while (sr.Peek() >= 0)
                        {
                            string sData = sr.ReadLine();
                            string[] sDiv = sData.Split(new char[] { ',' }, StringSplitOptions.None);
                            if (sDiv.Length != 5)
                                throw new Exception("フォーマットエラー");
                            c.Add(new CRectangle2(
                                double.Parse(sDiv[0].Trim()), 
                                double.Parse(sDiv[1].Trim()), 
                                double.Parse(sDiv[2].Trim()), 
                                double.Parse(sDiv[3].Trim()), 
                                double.Parse(sDiv[4].Trim())));
                        }
                    }
                }
            }
            catch (Exception)
            {
                c.Clear();
                return false;
            }
            return true;
        }
    }

    public struct PointD
    {
        double _x;
        double _y; 
        public PointD( double x, double y )
        {
            _x = x;
            _y = y;
        }

        public static PointD operator +(PointD pt1, PointD pt2)
        {
            return new PointD(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }

        public static PointD operator -(PointD pt1, PointD pt2)
        {
            return new PointD(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        public override bool  Equals(object obj)
        {
            if (obj is PointD)
            {
                return (X == ((PointD)obj).X && Y == ((PointD)obj).Y);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)(X+Y);
        }

        public double X{
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
    }

    public class CRectangle1
    {
        double _x;
        double _y;
        double _width;
        double _height;

        public CRectangle1()
        {
            _x = 0.0;
            _y = 0.0;
            _width = 0.0;
            _height = 0.0;
        }

        public bool IsEmpty()
        {
            return (_x == 0.0 && _y == 0.0 && _width == 0.0 && _height == 0.0);
        }

        public CRectangle1(double x, double y, double width, double height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public void SetLTRB(double col1, double row1, double col2, double row2)
        {
            _x = col1;
            _y = row1;
            _width = col2 - col1;
            _height = row2 - row1;
        }

        public void SetRect(double x, double y, double width, double height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public void SetRect(CRectangle1 rc)
        {
            if (rc == null)
                return;
            _x = rc._x;
            _y = rc._y;
            _width = rc._width;
            _height = rc._height;
        }

        public CRectangle1(CRectangle1 rc)
        {
            _x = rc.X;
            _y = rc.Y;
            _width = rc.Width;
            _height = rc.Height;
        }

        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
        public double Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        public double Left
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public double Top
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        public double Right
        {
            get
            {
                return _x + _width;
            }
            set
            {
                _width = value - _x;
            }
        }

        public double Bottom
        {
            get
            {
                return _y + _height;
            }
            set
            {
                _height = value - _y;
            }
        }

        public double Col1
        {
            get{ return Left;}
            set{ Left = value;}
        }

        public double Row1
        {
            get { return Top; }
            set { Top = value; }
        }

        public double Col2
        {
            get { return Right; }
            set { Right = value; }
        }

        public double Row2
        {
            get { return Bottom; }
            set { Bottom = value; }
        }

        public double CenterX()
        {
            return _x + _width / 2.0;
        }

        public double CenterY()
        {
            return _y + _height / 2.0;
        }

        public PointD Center()
        {
            return new PointD(CenterX(), CenterY());
        }

        public bool PtInRect(PointD ptd)
        {
            if (Col1 > ptd.X || ptd.X > Col2)
                return false;
            if (Row1 > ptd.Y || ptd.Y > Row2)
                return false;

            return true;
        }

        public bool RectCollision(CRectangle1 rect)
        {
            if (Col2 < rect.Col1 || Row2 < rect.Row1)
                return false;
            if (rect.Col2 < Col1 || rect.Row2 < Row1)
                return false;
            return true;
        }

        public override string ToString()
        {
            return _x.ToString() + "," + _y.ToString() + "," + _width.ToString() + "," + _height.ToString();
        }
    }

    public class CRectangle2
    {
        double _row;
        double _col;
        double _phi;
        double _len1;
        double _len2;

        public CRectangle2(double row, double col, double phi, double len1, double len2)
        {
            Set(row, col, phi, len1, len2);
        }

        public void Set(double row, double col, double phi, double len1, double len2)
        {
            _row = row;
            _col = col;
            _phi = phi;
            _len1 = len1;
            _len2 = len2;
        }

        public double Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public double Col 
        { 
            get { return _col; }
            set { _col = value; }
        }

        public double Phi
        {
            get { return _phi; }
            set { _phi = value; }
        }

        public double Length1
        {
            get { return _len1; }
            set { _len1 = value; }
        }

        public double Length2
        {
            get { return _len2; }
            set { _len2 = value; }
        }

        public override string ToString()
        {
            return _row.ToString() + "," + _col.ToString() + "," + _phi.ToString() + "," + _len1.ToString() + "," + _len2.ToString();
        }
    }

    public class GeometricalLogic
    {
        public static double Length2Point(PointD pt1, PointD pt2)
        {
            return Math.Sqrt( Math.Pow(( pt2.X - pt1.X ), 2 ) + Math.Pow((pt2.Y - pt1.Y ), 2 ));
        }

        public static PointD Center2Point(PointD pt1, PointD pt2)
        {
            return new PointD( ( pt1.X + pt2.X ) / 2.0, ( pt1.Y + pt2.Y ) / 2.0 );
        }

        public static double Angle2Point(PointD pt1, PointD pt2)
        {
            return Math.Atan2((pt2.Y - pt1.Y), (pt2.Y - pt1.X));
        }

        public static double Angle2Point(PointD pt1, PointD pt2, out PointD cent)
        {
            cent = Center2Point(pt1, pt2);
            return Angle2Point(pt1, pt2);
        }

        public static double Angle3Point(PointD center, PointD pt1, PointD pt2 )
        {
            PointD vec1 = new PointD( (pt1.X - center.X), (pt1.Y - center.Y));
            PointD vec2 = new PointD( (pt2.X - center.X), (pt2.Y - center.Y));

            return Math.Acos( ( vec1.X * vec2.X + vec1.Y * vec2.Y ) / 
                ( Math.Sqrt( Math.Pow( vec1.X, 2) + Math.Pow( vec1.Y, 2 )) + Math.Sqrt( Math.Pow( vec2.X, 2) + Math.Pow( vec2.Y, 2 ))));
        }
    }


    public class CalibrationFunc
    {
        public static double PixToMicron(double dPix, double dRes)
        {
            return dPix * dRes;
        }

        public static double MicronToPix( double dMicron, double dRes )
        {
            return dMicron / dRes;
        }

        public static double SquarePixToSquareMicro( double dHorzPix, double dVertPix, double dResX, double dResY )
        {
            return dHorzPix * dResX * dVertPix * dResY;
        }

        public static double SquarePixToSquareMili( double dHorzPix, double dVertPix, double dResX, double dResY )
        {
            return SquarePixToSquareMicro( dHorzPix, dVertPix, dResX, dResY ) / 1000000.0;
        }

        public static double SquareMicronToSquarePix( double dHorz, double dVert, double dResX, double dResY )
        {
            return ( dHorz * dVert ) / ( dResX * dResY );
        }
    }

    public class MiscFunc
    {
        /// <summary>
        /// 文字列がパスで使用禁止のコードを使用していないかどうか
        /// </summary>
        /// <param name="sTest">テストする文字列</param>
        /// <returns>
        /// true 使用禁止文字が含まれている
        /// false 使用禁止文字が含まれていない
        /// </returns>
        public static bool IsFileNameInvalidString(string sTest)
        {
            char [] code = Path.GetInvalidFileNameChars();
            foreach (char c in code)
            {
                if (sTest.Contains(c))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 文字がパスで仕様金のコードかどうか
        /// </summary>
        /// <param name="c">テストする文字</param>
        /// <returns>
        /// true 使用禁止文字
        /// false 使用禁止文字では無い
        /// </returns>
        public static bool IsFileNameInvalidChar(char c)
        {
            char[] code = Path.GetInvalidFileNameChars();
            if (code.Contains(c))
                return true;
            return false;
        }

        /// <summary>
        /// ファイル名に使用してはいけない文字列を返す
        /// </summary>
        /// <returns></returns>
        public static string GetFileNameInvalidString()
        {
            return new string(Path.GetInvalidFileNameChars());
        }


        public static bool IsPathInvalidString(string sTest)
        {
            char[] code = Path.GetInvalidPathChars();
            foreach (char c in code)
            {
                if (sTest.Contains(c))
                    return true;
            }
            return false;
        }

        public static bool IsPathInvalidChar(char c)
        {
            char[] code = Path.GetInvalidPathChars();
            if (code.Contains(c))
                return true;
            return false;
        }

        public static string GetPathInvalidString()
        {
            return new string(Path.GetInvalidPathChars());
        }

        public static string GetMD5Hash(string sTest)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(sTest);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = md5.ComputeHash(data);
            System.Text.StringBuilder result = new StringBuilder();
            foreach (byte b in bs)
            {
                result.Append(b.ToString("X"));
            }
            return result.ToString();
        }

        public static void ListAdd<T>(List<T> source, List<T> renew)
        {
            if (source.Count == renew.Count)
            {
                for (int i = 0; i < source.Count; i++)
                    source[i] = renew[i];
            }
            else if (source.Count > renew.Count)
            {
                for (int i = 0; i < renew.Count; i++)
                    source[i] = renew[i];
                source.RemoveRange(renew.Count, source.Count - renew.Count);
            }
            else
            {
                for (int i = 0; i < source.Count; i++)
                    source[i] = renew[i];

                for (int i = source.Count; i < renew.Count; i++)
                {
                    source.Add(renew[i]);
                }
            }
        }

        public static string FullPathtoDirPath(string sPath)
        {
            int index = sPath.LastIndexOf('\\');
            if (index == -1)
                return sPath;

            return sPath.Substring(0, index + 1 );
        }

        /// ----------------------------------------------------------------------------
        /// <summary>
        ///     指定したディレクトリをすべて削除します。</summary>
        /// <param name="stDirPath">
        ///     削除するディレクトリのパス。</param>
        /// ----------------------------------------------------------------------------
        public static void DeleteDirectory(string stDirPath)
        {
            DeleteDirectory(new System.IO.DirectoryInfo(stDirPath));
        }

        /// ----------------------------------------------------------------------------
        /// <summary>
        ///     指定したディレクトリをすべて削除します。</summary>
        /// <param name="hDirectoryInfo">
        ///     削除するディレクトリの DirectoryInfo。</param>
        /// ----------------------------------------------------------------------------
        public static void DeleteDirectory(System.IO.DirectoryInfo hDirectoryInfo)
        {
            // すべてのファイルの読み取り専用属性を解除する
            foreach (System.IO.FileInfo cFileInfo in hDirectoryInfo.GetFiles())
            {
                if ((cFileInfo.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                {
                    cFileInfo.Attributes = System.IO.FileAttributes.Normal;
                }
            }

            // サブディレクトリ内の読み取り専用属性を解除する (再帰)
            foreach (System.IO.DirectoryInfo hDirInfo in hDirectoryInfo.GetDirectories())
            {
                DeleteDirectory(hDirInfo);
            }

            // このディレクトリの読み取り専用属性を解除する
            if ((hDirectoryInfo.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
            {
                hDirectoryInfo.Attributes = System.IO.FileAttributes.Directory;
            }

            // このディレクトリを削除する
            hDirectoryInfo.Delete(true);
        }

        public static void DivideRectangle(CRectangle1 rcSrc, out List<CRectangle1> lstRects, int iDivHorz, int iDivVert, double dOverlapHorz, double dOverlapVert)
        {
            lstRects = new List<CRectangle1>();

            // 分割サイズ指定が来た場合
            double dDivR = (rcSrc.Row2 - rcSrc.Row1) / (double)iDivVert, dDivC = (rcSrc.Col2 - rcSrc.Col1) / (double)iDivHorz;

            // 左上基準
            for (int v = 0; v < iDivVert; v++)
            {
                for (int h = 0; h < iDivHorz; h++)
                {
                    lstRects.Add( new CRectangle1(
                        rcSrc.Col1 + dDivC * h - dOverlapHorz,
                        rcSrc.Row1 + dDivR * v - dOverlapVert,
                        dDivC + 2 * dOverlapHorz,
                        dDivR + 2 * dOverlapVert));
                }
            }
        }
    }

    /// <summary>
    /// コントロールが持つ入力制限
    /// </summary>
    public class ControlEvent
    {
        public static void OnKeyPress_NumericOnly(object sender, KeyPressEventArgs e)
        {
            if (!e.KeyChar.IsNumber() && e.KeyChar != '\b')
                e.Handled = true;
        }
        public static void OnKeyPress_NumericPeriod(object sender, KeyPressEventArgs e)
        {
            if (!e.KeyChar.IsNumberPeriod() && e.KeyChar != '\b')
                e.Handled = true;
        }
    }
}
