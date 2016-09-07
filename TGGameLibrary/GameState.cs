using TGGameLibrary.Enums;

namespace TGGameLibrary
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
