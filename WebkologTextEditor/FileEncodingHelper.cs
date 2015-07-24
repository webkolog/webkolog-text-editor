using System;
using System.IO;
using System.Text;

public class FileEncodingHelper
{
    public static Encoding DetectEncoding(string filename)
    {
        using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
        {
            if (fs.Length >= 3 && fs.ReadByte() == 0xEF && fs.ReadByte() == 0xBB && fs.ReadByte() == 0xBF)
            {
                return Encoding.UTF8;
            }
            // Daha fazla BOM kontrolü eklenebilir (UTF-16 vb.)
            // ANSI için kesin bir tespit yöntemi olmasa da, BOM olmaması ANSI olma ihtimalini artırır.
            return Encoding.Default; // Varsayılan olarak ANSI kabul edilebilir.
        }
    }
}