using System.Collections.Generic;
using Source.Battle_Field;
using Source.Graphics;
using Source.Graphics.Markers;
using Source.Graphics.UI;
using Source.Ships;
using Source.Turn_State_Machine;
using UnityEngine;

namespace Source
{
    public class Visualizer : MonoBehaviour
    {
        [SerializeField] private GridVisualizer[] _gridVisualizers;
        [SerializeField] private ShipVisualizer[] _shipVisualizers;
        [SerializeField] private MarkersVisualizer[] _markersVisualizers;
        [SerializeField] private MarkersPack _markersPack;
        [SerializeField] private TimerLine _timerLine;

        public void VisualizeShips(Players player, ShipPlacer placer)
        {
            _shipVisualizers[(int) player].AddShips(placer.GetAllShips());
            _shipVisualizers[(int) player].Visualize();
        }

        public void VisualizeMarker(Vector2Int coord, OpenType type, PlayersManager manager)
        {
            _markersVisualizers[(int)manager.GetNextPlayerID()].AddMarker(coord, type);
            _markersVisualizers[(int)manager.GetNextPlayerID()].Visualize();
        }

        public void VisualizeHitOnShip(Vector2Int coord, PlayersManager manager)
        {
            _shipVisualizers[(int)manager.GetNextPlayerID()].VisualizeHit(coord);
        }

        public void InitializeGridVisualizers(PlayersManager manager, Players playerID)
        {
            var player = manager.GetPlayerByID(playerID);
            var grid = player.GetBattlefield().GetGrid();
            grid.ShipExplosion += () => VisualizeExplosionMarkers(player.GetBattlefield(), playerID);
            _gridVisualizers[(int)playerID].Initialize(grid);
            _gridVisualizers[(int)playerID].Visualize();
            MarkerFabric.Initialize(_markersPack);
        }

        private void VisualizeExplosionMarkers(Battlefield battlefield,Players playerID)
        {
            
            var grid = battlefield.GetGrid();
            var opensTypeIdentifier = battlefield.GetOpensTypeIdentifier();
            
            var opener = grid.GetExplosion();
            var coords = (IReadOnlyList<Vector2Int>)opener.GetOpenInformation();
            if(grid.TryOpenCells((IOpener)opener)) _markersVisualizers[(int)playerID].AddMarkers(coords, new List<OpenType>(opensTypeIdentifier.GetTypes(coords, (IOpener)opener)));
            _markersVisualizers[(int) playerID].Visualize();
        }

        public void InitializeTimerLine(TimeToTurnTracker tracker)
        {
            _timerLine.Initialize(tracker);
        }

        public void MakeShipsVisible(int playerID)
        {
            _shipVisualizers[playerID].MakeVisible(); 
        }

        public void MakeShipsInvisible(int playerID)
        {
            _shipVisualizers[playerID].MakeInvisible(); 
        }

        public void ClearShipsVisuals(int playerID)
        {
            _shipVisualizers[playerID].ClearVisual();
        }

        public void DeleteShipVisual(IReadonlyLogicalRepresentation ship, int playerID)
        {
            foreach (var coord in ship.SegmentsCoords)
            {
                _shipVisualizers[playerID].DeleteSegmentVisual(coord);
            }
        }
    }
}