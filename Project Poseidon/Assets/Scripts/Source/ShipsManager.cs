using System;
using Source.Ships;
using Source.Turn_State_Machine;
using UnityEngine;

namespace Source
{
    public class ShipsManager : MonoBehaviour
    {
        [SerializeField] private ShipsPack _shipsPack;
        [SerializeField] private Fleet _fleet;
        [SerializeField] private Visualizer _visualizer;
        
        
        
        public void PlaceShips(PlayersManager manager)
        {
            ShipFabric.Initialize(_shipsPack);
            var firstPlayerShipPlacer = new ShipPlacer(manager.GetPlayerByID(Players.First).GetBattlefield().GetGrid(), _fleet);
            var secondPlayerShipPlacer = new ShipPlacer(manager.GetPlayerByID(Players.Second).GetBattlefield().GetGrid(), _fleet);
            firstPlayerShipPlacer.TryPlaceShips();
            secondPlayerShipPlacer.TryPlaceShips();
            
            _visualizer.VisualizeShips(Players.First, firstPlayerShipPlacer);
            _visualizer.VisualizeShips(Players.Second, secondPlayerShipPlacer);
        }
    }
}
