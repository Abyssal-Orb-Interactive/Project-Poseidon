using System;
using System.Collections.Generic;
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

        private readonly List<ShipPlacer> _shipPlacers = new ();

        public void Initialize(PlayersManager manager)
        {  
            ShipFabric.Initialize(_shipsPack);

            var firstPlayerShipPlacer = new ShipPlacer(manager.GetPlayerByID(Players.First).GetBattlefield().GetGrid(), _fleet);
            var secondPlayerShipPlacer = new ShipPlacer(manager.GetPlayerByID(Players.Second).GetBattlefield().GetGrid(), _fleet);
            
            _shipPlacers.Add(firstPlayerShipPlacer);
            _shipPlacers.Add(secondPlayerShipPlacer);
        }

        public void Rotate()
        {
            
        }
        
        public void PlaceShips(int playersID)
        {
            _shipPlacers[playersID].TryPlaceShips();
            
            _visualizer.VisualizeShips(Players.First, _shipPlacers[playersID]);
        }

        public void PlaceShip(int playersID)
        {
            var result = _shipPlacers[playersID].TryPlaceShip(Orientation.Horizontal, _shipsPack.TorpedoBoat);
            
            while (!result)
            {
                result = _shipPlacers[playersID].TryPlaceShip(Orientation.Horizontal, _shipsPack.TorpedoBoat);
            }
        }
    }
}
