using TheWideWorld.Adventures;

namespace TheWideWorld.Game.Interfaces
{
    public interface IGameService
    {
        bool StartTheGame(Adventure adventure = null);
    }
}