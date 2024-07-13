using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Ships
{
    [CreateAssetMenu(fileName = "Fleet", menuName = "ScriptableObjects/Fleet")]
    public class Fleet : ScriptableObject
    {
        private const int FLEET_POWER = 20;
        
        [SerializeField] private List<Ship> _ships;

        private void OnValidate()
        {
            var currentFleetPower = _ships.Sum(ship => ship.ShipPower);
            if (currentFleetPower > FLEET_POWER) throw new InvalidOperationException("Your fleet too powerful");
        }

        public IEnumerable<Ship> Ships => _ships;
    }
}
