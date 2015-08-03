using TGameLibrary.Enums;

namespace TGameLibrary
{
    public class GameState
    {
        public GameStates Current;

        public GameState()
        {
            Current = GameStates.Menu;
        }
    }
}
