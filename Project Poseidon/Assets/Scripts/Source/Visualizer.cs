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
        [SerializeField] private MarkersVisualizer _markersVisualizer;
        [SerializeField] private MarkersPack _markersPack;
        [SerializeField] private TimerLine _timerLine;

        public void VisualizeShips(Players player, ShipPlacer placer)
        {
            _shipVisualizers[(int) player].AddShips(placer.GetAllShips());
            _shipVisualizers[(int) player].Visualize();
        }

        public void VisualizeMarker(Vector2Int coord, OpenType type)
        {
            _markersVisualizer.AddMarker(coord, type);
            _markersVisualizer.Visualize();
        }

        public void VisualizeHitOnShip(Vector2Int coord, PlayersManager manager)
        {
            _shipVisualizers[(int)manager.GetCurrentPlayerID()].VisualizeHit(coord);
        }
        
        public void InitializeGridVisualizers(PlayersManager manager, Players playerID)
        {
            InitializeAndVisualizeGrid(manager.GetPlayerByID(playerID).GetBattlefield(), _gridVisualizers[(int) playerID]);
        }
        private void InitializeAndVisualizeGrid(Battlefield battlefield, GridVisualizer gridVisualizer)
        {
            var grid = battlefield.GetGrid();
            grid.ShipExplosion += () => VisualizeExplosionMarkers(battlefield);
            gridVisualizer.Initialize(grid);
            gridVisualizer.Visualize();
            MarkerFabric.Initialize(_markersPack);
        }
        
        private void VisualizeExplosionMarkers(Battlefield battlefield)
        {
            var grid = battlefield.GetGrid();
            var opensTypeIdentifier = battlefield.GetOpensTypeIdentifier();
            
            var opener = grid.GetExplosion();
            var coords = (IReadOnlyList<Vector2Int>)opener.GetOpenInformation();
            if(grid.TryOpenCells((IOpener)opener)) _markersVisualizer.AddMarkers(coords, new List<OpenType>(opensTypeIdentifier.GetTypes(coords, (IOpener)opener)));
            _markersVisualizer.Visualize();
        }

        public void InitializeTimerLine(TimeToTurnTracker tracker)
        {
            _timerLine.Initialize(tracker);
        }
    }
}