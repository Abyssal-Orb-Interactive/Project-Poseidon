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

        public Ship TorpedoBoat => _torpedoBoat;
        public Ship Destroyer => _destroyer;
        public Ship Cruiser => _cruiser;
        public Ship Battleship => _battleship;
    }
}
