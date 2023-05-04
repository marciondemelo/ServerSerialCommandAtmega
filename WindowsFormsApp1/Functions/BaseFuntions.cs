using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using WindowsFormsApp1.models;

namespace WindowsFormsApp1.Functions
{
    public class BaseFuntions
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hwnd, int Msg, IntPtr wParam, IntPtr lParam);

        public const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        public const int APPCOMMAND_VOLUME_UP = 0xA0000;
        public const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        public const int WM_APPCOMMAND = 0x319;

        public List<ComandoModel> comandosA()
        {
            return ReadFileConfiguration.Comandos("a");
        }

        public List<ComandoModel> comandosB()
        {
            return ReadFileConfiguration.Comandos("b");
        }

        public List<ComandoModel> comandosC()
        {
            return ReadFileConfiguration.Comandos("c");
        }

        public List<ComandoModel> comandosD()
        {
            return ReadFileConfiguration.Comandos("d");
        }

    }
}
