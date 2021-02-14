using System;
using System.Collections.Generic;
using System.Text;
using TheWideWorld.Utilites.Interfaces;

namespace TheWideWorld.Utilites
{
    class ConsoleMessageHandler : IMessageHandler
    {
        public void Write(string message = "", bool withLine = true)
        {
            if (withLine)
            {
                Console.WriteLine(message);
            }
            else {
                Console.Write(message);
            }
        }

        public string Read()
        {
            return Console.ReadLine();
        }

        public void Clear() {
            Console.Clear();
        }
    }
}
