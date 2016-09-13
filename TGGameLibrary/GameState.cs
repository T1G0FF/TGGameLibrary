#region File Description
//-----------------------------------------------------------------------------
// GameState.cs
//
// Written by Thomas
// Last Updated: 2016-09-13
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using TGGameLibrary.Enums;
#endregion

namespace TGGameLibrary
{
    public class GameState
    {
        #region Properties
        public GameStates Current;
        #endregion

        #region Initialisation
        public GameState()
        {
            Current = GameStates.Menu;
        }
        #endregion
    }
}
