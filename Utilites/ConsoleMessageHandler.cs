using System;
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

        public void WriteRead(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
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
