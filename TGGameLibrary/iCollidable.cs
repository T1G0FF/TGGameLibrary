#region File Description
//-----------------------------------------------------------------------------
// iCollidable.cs
//
// Written by Thomas
// Last Updated: 2016-09-13
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace TGGameLibrary
{
    public interface ICollidable
    {
        Rectangle Footprint { get; }
    }
}
