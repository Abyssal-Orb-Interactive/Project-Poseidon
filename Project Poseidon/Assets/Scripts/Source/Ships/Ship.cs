using System;
using UnityEngine;

namespace Source.Ships
{
    [CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ship")]
    public class Ship : ScriptableObject
    {
        [SerializeField] private int _size = 1;
        [SerializeField] private int _shipPower = 1;
        [SerializeField] private ShipType _type;
        [SerializeField] private GameObject _shipPrefab;

        private void OnValidate()
        {
            if (_size <= 0) throw new MissingFieldException("Size of ship must be positive");
            if (_shipPrefab == null) throw new MissingFieldException("Ship must have prefab");
            if (_shipPower < 0) throw new InvalidOperationException("A ship cannot have negative power.");
        }

        public int Size => _size;

        public GameObject ShipPrefab => _shipPrefab;

        public int ShipPower => _shipPower;

        public ShipType Type => _type;
    }
}
