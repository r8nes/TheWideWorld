namespace TheWideWorld.Utilites.Interfaces
{
    public interface IMessageHandler
    {
        public void Write(string message = "", bool withLine = true);
        public string Read();
        public void WriteRead(string message);
        /// <summary>
        /// Used to clear the screen.
        /// </summary>
        public void Clear();
    }
}
