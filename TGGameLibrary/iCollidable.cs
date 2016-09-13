using System;
using Microsoft.Xna.Framework;

namespace TGGameLibrary
{
    public interface ICollidable
    {
        Rectangle Footprint { get; }
    }
}
