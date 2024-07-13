using System;
using Source.Battle_Field;
using UnityEngine;
using Grid = Source.Battle_Field.Grid;

namespace Source
{
    public class Battlefield : IDisposable
    {
        private readonly Grid _grid;
        private readonly OpensTypeIdentifier _typeIdentifier;
        private readonly Transform _cameraTarget;

        public Battlefield(int xOffset, int yOffset, Transform cameraTarget)
        {
            _grid = new Grid(GridFabric.CreateGrid(xOffset, yOffset));
            _typeIdentifier = new OpensTypeIdentifier(_grid);
            _cameraTarget = cameraTarget;
        }

        public Grid GetGrid()
        {
            return _grid;
        }

        public Transform GetCameraTarget()
        {
            return _cameraTarget;
        }

        public bool TryRegisterShoot(IOpener shooter)
        {
            return _grid.TryOpenCells(shooter);
        }

        public OpenType GetTypeOfShoot(Vector2Int coord, IOpener shooter)
        {
            return _typeIdentifier.GetType(coord, shooter);
        }

        public OpensTypeIdentifier GetOpensTypeIdentifier()
        {
            return _typeIdentifier;
        }

        public void Dispose()
        {
            _grid.Dispose();
            _typeIdentifier.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}