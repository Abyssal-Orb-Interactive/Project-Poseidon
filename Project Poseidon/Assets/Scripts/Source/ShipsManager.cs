using System;
using System.Collections.Generic;
using System.Linq;
using Source.Battle_Field;
using Source.Ships;
using Source.Turn_State_Machine;
using UnityEngine;
using Grid = Source.Battle_Field.Grid;

namespace Source
{
    public class ShipsManager : MonoBehaviour
    {
        private const int NUM_OF_SHIP_TYPES = 4;
        private const float HIEGHT_OF_GHOST = 1.2f;
        
        [SerializeField] private float _cellSize = 10f;
        [SerializeField] private float _fieldOffset = 4.5f;
        
        [SerializeField] private ShipsPack _shipsPack;
        [SerializeField] private Fleet _fleet;
        [SerializeField] private Visualizer _visualizer;
        [SerializeField] private List<int> _numsOfShips = new (NUM_OF_SHIP_TYPES);

        private GameObject _shipGhost;
        private int _shipGhostSize;
        private List<GameObject> _shipGhostSegments = new ();
        private List<Vector2Int> _shipGhostSegmentsCoords = new ();
        private Ship currentShip;
        private Ship _ghostShipType;
        private Orientation _shipGhostOrientation = Orientation.Horizontal;
        private int _playerIndexOffset;
        private Vector2Int _currentMouseCoord;
        private Grid _currentGrid;

        private readonly List<ShipPlacer> _shipPlacers = new ();

        private PlayersManager _players;
        private PlayerActions _actions;
        private event Action ContinueDesired;
        
        public void Initialize(PlayersManager manager)
        {  
            ShipFabric.Initialize(_shipsPack);
            _actions = new PlayerActions();

            _players = manager;

            var firstPlayerShipPlacer = new ShipPlacer(manager.GetPlayerByID(Players.First).GetBattlefield().GetGrid(), _fleet);
            var secondPlayerShipPlacer = new ShipPlacer(manager.GetPlayerByID(Players.Second).GetBattlefield().GetGrid(), _fleet);
            
            _shipPlacers.Add(firstPlayerShipPlacer);
            _shipPlacers.Add(secondPlayerShipPlacer);

            _actions.Base.Shoot.performed += _ => OnPlaceDesired();
            _actions.Base.Delete.performed += _ => OnDelete();
            
            _actions.Enable();
            
            CalculateShipsLimits();
        }

        private void CalculateShipsLimits()
        {
            foreach (var ship in _fleet.Ships)
            {
                switch (ship.Type)
                {
                    case ShipType.TorpedoBoat:
                        _numsOfShips[(int)ShipType.TorpedoBoat]++;
                        break;
                    case ShipType.Destroyer:
                        _numsOfShips[(int)ShipType.Destroyer]++;
                        break;
                    case ShipType.Cruiser:
                        _numsOfShips[(int)ShipType.Cruiser]++;
                        break;
                    case ShipType.Battleship:
                        _numsOfShips[(int)ShipType.Battleship]++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void UpdateGhostPosition(Vector2Int mousePosition)
        {
            _currentMouseCoord = mousePosition;
            
            if(_shipGhost == null) return;

            _currentGrid = _players.GetPlayerByID((Players)_playerIndexOffset).GetBattlefield().GetGrid();
            
            if (IsCursorOverGrid(_currentMouseCoord, _currentGrid) || IsShipGhostOverGrid(_currentMouseCoord, _currentGrid)) return;
            
            _shipGhost.transform.position = new Vector3(_currentMouseCoord.x * _cellSize + _fieldOffset, HIEGHT_OF_GHOST, _currentMouseCoord.y * _cellSize + _fieldOffset);
            
            VisualizeShip(_shipGhost);
        }

        private void OnPlaceDesired()
        {
            if (_shipGhost == null || IsCursorOverGrid(_currentMouseCoord, _currentGrid)) return;
            
            var result = _shipPlacers[_playerIndexOffset].TryPlaceShip(_shipGhostOrientation, _ghostShipType, _shipGhostSegmentsCoords.First());
            if (!result) return;
            
            _numsOfShips[(int)_ghostShipType.Type]--;
            ClearGhost();
            
            _visualizer.VisualizeShips((Players) _playerIndexOffset, _shipPlacers[_playerIndexOffset]);
            _visualizer.MakeShipsVisible(_playerIndexOffset);
        }

        private bool IsCursorOverGrid(Vector2Int mousePosition, ReadonlyGrid grid)
        {
            return mousePosition.x > grid.MaxCoord.x || mousePosition.y > grid.MaxCoord.y ||
                   mousePosition.x < grid.MinCoord.x || mousePosition.y < grid.MinCoord.y;
        }

        private bool IsShipGhostOverGrid(Vector2Int mousePosition, ReadonlyGrid grid)
        {
            var result = _shipGhostOrientation switch
            {
                Orientation.Horizontal => mousePosition.x + _shipGhostSize - 1 > grid.MaxCoord.x,
                Orientation.Vertical => mousePosition.y + _shipGhostSize - 1 > grid.MaxCoord.y,
                Orientation.HorizontalReversed => mousePosition.x - _shipGhostSize < grid.MinCoord.x,
                Orientation.VerticalReversed => mousePosition.y - _shipGhostSize < grid.MinCoord.y,
                _ => throw new ArgumentOutOfRangeException()
            };

            return result;
        }
        public void Rotate()
        {
            _shipGhostOrientation = _shipGhostOrientation == Orientation.VerticalReversed ? Orientation.Horizontal : _shipGhostOrientation + 1;
            RecalculateAllCoords();
        }
        
        public void PlaceShips()
        {
            _shipPlacers[_playerIndexOffset].DisposeAllShips();
            _visualizer.ClearShipsVisuals(_playerIndexOffset);
            
            var result = _shipPlacers[_playerIndexOffset].TryPlaceShips();

            while (!result)
            {
                result = _shipPlacers[_playerIndexOffset].TryPlaceShips(); 
            }
            
            for (var i = 0; i < _numsOfShips.Capacity; i++)
            {
                _numsOfShips[i] = 0;
            }
            
            _visualizer.VisualizeShips((Players) _playerIndexOffset, _shipPlacers[_playerIndexOffset]);
            _visualizer.MakeShipsVisible(_playerIndexOffset);
        }
        
        private void VisualizeShip(GameObject prefab)
        {
            RecalculateAllCoords();
            foreach (var coord in _shipGhostSegmentsCoords)
            {
                if(coord == _shipGhostSegmentsCoords.First()) continue;
                
                var position = new Vector3(coord.x * _cellSize + _fieldOffset, HIEGHT_OF_GHOST, coord.y * _cellSize + _fieldOffset);
                var shipSegment = Instantiate(prefab, position, Quaternion.identity);
                _shipGhostSegments.Add(shipSegment);
            }
        }
        
        private void RecalculateAllCoords()
        {
            _shipGhostSegmentsCoords?.Clear();
            foreach (var segment in _shipGhostSegments)
            { 
                if(segment == _shipGhost) continue;
                Destroy(segment);
            }
            
            CalculateSegmentsCoords();
        }
        
        private void CalculateSegmentsCoords()
        {
            Vector3 position;
            
            position = _shipGhost != null ? _shipGhost.transform.position : Vector3.zero;
            _shipGhostSegmentsCoords.Add(new Vector2Int((int)((position.x - _fieldOffset) / _cellSize), (int)((position.z - _fieldOffset) / _cellSize)));
            
            var previousCoord = _shipGhostSegmentsCoords.Last();
            
            var (dx, dy) = GetRelativePosition(1);
            for (var i = 1; i < _shipGhostSize; i++)
            {
                _shipGhostSegmentsCoords.Add(new Vector2Int(previousCoord.x + dx, previousCoord.y + dy));
                previousCoord = _shipGhostSegmentsCoords.Last();
            }
        }
        
        private (int, int) GetRelativePosition(int index)
        {
            int dx, dy;
            switch (_shipGhostOrientation)
            {
                case Orientation.Horizontal:
                    dx = index;
                    dy = 0;
                    break;
                case Orientation.Vertical:
                    dx = 0;
                    dy = index;
                    break;
                case Orientation.HorizontalReversed:
                    dx = -index;
                    dy = 0;
                    break;
                case Orientation.VerticalReversed:
                    dx = 0;
                    dy = -index;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Orientation));
            }
            return (dx, dy);
        }

        private void SetGhost(GameObject prefab)
        {
            _shipGhost = Instantiate(prefab);
            _shipGhost.layer = 7;
            _shipGhostSegments.Add(_shipGhost);
        }

        public void PlaceTorpedoBoat()
        {
            if (_numsOfShips[(int)ShipType.TorpedoBoat] == 0) return;
            
            if (_shipGhost != null)
            {
                ClearGhost();
            }
            SetGhost(_shipsPack.TorpedoBoat.ShipPrefab);
            _shipGhostSize = 1;
            _shipGhostOrientation = Orientation.Horizontal;
            _ghostShipType = _shipsPack.TorpedoBoat;
        }
        
        public void PlaceDestroyer()
        {
            if( _numsOfShips[(int)ShipType.Destroyer] == 0) return;
            
            if (_shipGhost != null)
            {
                ClearGhost();
            }
            
            SetGhost(_shipsPack.Destroyer.ShipPrefab);
            _shipGhostSize = 2;
            _ghostShipType = _shipsPack.Destroyer;
            _shipGhostOrientation = Orientation.Horizontal;
        }
        
        public void PlaceCruiser()
        {
            if( _numsOfShips[(int)ShipType.Cruiser] == 0) return;
            
            if (_shipGhost != null)
            {
                ClearGhost();
            }
            
            SetGhost(_shipsPack.Cruiser.ShipPrefab);
            _shipGhostSize = 3;
            _ghostShipType = _shipsPack.Cruiser;
            _shipGhostOrientation = Orientation.Horizontal;
        }
        
        public void PlaceBattleship()
        {
            if( _numsOfShips[(int)ShipType.Battleship] == 0) return;
            
            if (_shipGhost != null)
            {
                ClearGhost();
            }
            
            SetGhost(_shipsPack.Battleship.ShipPrefab);
            _shipGhostSize = 4;
            _ghostShipType = _shipsPack.Battleship;
            _shipGhostOrientation = Orientation.Horizontal;
        }

        private void ClearGhost()
        {
            foreach (var segment in _shipGhostSegments)
            {
                Destroy(segment);
            }
            _shipGhostSegmentsCoords.Clear();
            _shipGhostSegments.Clear();
            _shipGhostOrientation = Orientation.Horizontal;
            _shipGhostSize = 0;
        }

        public void OnContinue()
        {
            if (_numsOfShips.Exists(num => num != 0))
            {
                return;
            }

            if (_playerIndexOffset + 1 >= 3)
            {
                _actions.Dispose();
                return;
            }
            
            _playerIndexOffset++;
            CalculateShipsLimits();
            
            ContinueDesired?.Invoke();
        }

        private void OnDelete()
        {
            if (_shipGhost != null)
            {
                ClearGhost();
            }
            else
            {
                var shipsToDelete = new List<IReadonlyLogicalRepresentation>();
                foreach (var ship in _shipPlacers[_playerIndexOffset].GetAllShips())
                {
                    if (!ship.SegmentsCoords.Contains(_currentMouseCoord)) continue;
                    
                    _numsOfShips[ship.HealthPoints - 1]++;
                    _players.GetPlayerByID((Players)_playerIndexOffset).GetBattlefield().GetGrid().DeleteShip(ship);
                    _visualizer.DeleteShipVisual(ship, _playerIndexOffset);
                    shipsToDelete.Add(ship);
                }

                foreach (var ship in shipsToDelete)
                {
                    _shipPlacers[_playerIndexOffset].DeleteShip(ship);
                }
            }
        }

        public void SubscribeToContinueDesired(Action action)
        {
            ContinueDesired += action;
        }
        
        public void UnSubscribeToContinueDesired(Action action)
        {
            ContinueDesired -= action;
        }
    }
}
