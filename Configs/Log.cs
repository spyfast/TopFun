using System;
using System.Collections.Generic;

namespace TopFun.Configs
{
    public class Log
    {
        public static Queue<string> Logs = new Queue<string>();

        public static void Push(string message)
        {
            Logs.Enqueue($"[{DateTime.Now.ToShortTimeString()}]: {message}");
        }
    }
}
