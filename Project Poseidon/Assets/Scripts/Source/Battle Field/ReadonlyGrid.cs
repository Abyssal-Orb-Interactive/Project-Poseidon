using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public abstract class ReadonlyGrid : IDisposable
    {
        public abstract IReadonlyOpener GetExplosion();
        public abstract bool HasShip(Vector2Int coord);

        public abstract IEnumerable<IReadonlyCell> GetCells();
        public  abstract IEnumerable<Vector2Int> GetCoords();
        public abstract Vector2Int MaxCoord { get; protected set; }
        public abstract Vector2Int MinCoord { get; protected set; }
        public abstract void Dispose();
    }
}