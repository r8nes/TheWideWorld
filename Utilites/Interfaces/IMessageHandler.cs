namespace TheWideWorld.Utilites.Interfaces
{
    public interface IMessageHandler
    {
        public void Write(string message = "");
        public string Read();
        /// <summary>
        /// Used to clear the screen.
        /// </summary>
        public void Clear();
    }
}
