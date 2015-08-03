using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGameLibrary
{
    class GameState
    {
        public GameStates Current = GameStates.Menu;

        public enum GameStates { Menu, Loading, Playing, Paused };
    }
}
