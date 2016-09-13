#region File Description
//-----------------------------------------------------------------------------
// AnimatedSprite.cs
//
// Written by Thomas
// Last Updated: 2016-09-13
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace TGGameLibrary
{
    public class Quadtree
    {
        #region Properties
        private const int MAX_OBJECTS = 10;
        private const int MAX_LEVELS = 5;

        private int _level;
        private List<ICollidable> _objects;
        private Rectangle _bounds;
        private Quadtree[] _nodes;
        #endregion

        #region Initialisation
        public Quadtree(int level, Rectangle bounds)
        {
            _level = level;
            _objects = new List<ICollidable>();
            _bounds = bounds;
            _nodes = new Quadtree[4];
        }
        #endregion

        #region Public Methods
        public void Add(ICollidable sprite)
        {
            if (_nodes[0] != null)
            {
                int index = indexOf(sprite);

                if (index != -1)
                {
                    _nodes[index].Add(sprite);
                    return;
                }
            }

            _objects.Add(sprite);

            if (_objects.Count > MAX_OBJECTS && _level < MAX_LEVELS)
            {
                if (_nodes[0] == null)
                {
                    split();
                }

                int i = 0;
                while (i < _objects.Count)
                {
                    int index = indexOf(_objects[i]);
                    if (index != -1)
                    {
                        _nodes[index].Add(_objects[i]);
                        _objects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        public void AddRange(IEnumerable<ICollidable> range)
        {
            foreach (ICollidable collidable in range)
            {
                Add(collidable);
            }
        }

        public List<ICollidable> SameNodeAs(ICollidable sprite)
        {
            List<ICollidable> result = new List<ICollidable>();
            int index = indexOf(sprite);
            if (index != -1 && _nodes[0] != null)
            {
                result.AddRange(_nodes[index].SameNodeAs(sprite));
            }

            result.AddRange(_objects);

            return result;
        }

        public void Clear()
        {
            _objects.Clear();
            for (int i = 0; i < _nodes.Length; i++)
            {
                if (null != _nodes[i])
                {
                    _nodes[i].Clear();
                    _nodes[i] = null;
                }
            }
        }
        #endregion

        #region Private Methods
        private void split()
        {
            int divWidth = _bounds.Width / 2;
            int divHeight = _bounds.Height / 2;

            _nodes[0] = new Quadtree(_level + 1, new Rectangle(_bounds.X + divWidth, _bounds.Y, divWidth, divHeight));
            _nodes[1] = new Quadtree(_level + 1, new Rectangle(_bounds.X, _bounds.Y, divWidth, divHeight));
            _nodes[2] = new Quadtree(_level + 1, new Rectangle(_bounds.X, _bounds.Y + divHeight, divWidth, divHeight));
            _nodes[3] = new Quadtree(_level + 1, new Rectangle(_bounds.X + divWidth, _bounds.Y + divHeight, divWidth, divHeight));
        }

        private int indexOf(ICollidable sprite)
        {
            int index = -1;
            double verticalMidpoint = _bounds.X + (_bounds.Width / 2);
            double horizontalMidpoint = _bounds.Y + (_bounds.Height / 2);

            // Object can completely fit within the top quadrants
            bool topQuadrant = (sprite.Footprint.Y < horizontalMidpoint && sprite.Footprint.Y + sprite.Footprint.Height < horizontalMidpoint);
            // Object can completely fit within the bottom quadrants
            bool bottomQuadrant = (sprite.Footprint.Y > horizontalMidpoint);

            // Object can completely fit within the left quadrants
            if (sprite.Footprint.X < verticalMidpoint && sprite.Footprint.X + sprite.Footprint.Width < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            // Object can completely fit within the right quadrants
            else if (sprite.Footprint.X > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }

            return index;
        }
        #endregion
    }
}
