using System;
using UnityEngine;

namespace Source.Ships
{
    [CreateAssetMenu(fileName = "ShipsPack", menuName = "ScriptableObjects/ShipsPack")]
    public class ShipsPack : ScriptableObject
    {
        [SerializeField] private Ship _torpedoBoat;

        [SerializeField] private Ship _destroyer;

        [SerializeField] private Ship _cruiser;

        [SerializeField] private Ship _battleship;

        private void OnValidate()
        {
            if (_torpedoBoat == null) throw new InvalidOperationException("Torpedo boat must have data");
            if (_destroyer == null) throw new InvalidOperationException("Destroyer must have data");
            if (_cruiser == null) throw new InvalidOperationException("Cruiser boat must have data");
            if (_battleship == null) throw new InvalidOperationException("Battleship boat must have data");
        }


        public Ship TorpedoBoat => _torpedoBoat;
        public Ship Destroyer => _destroyer;
        public Ship Cruiser => _cruiser;
        public Ship Battleship => _battleship;
    }
}
