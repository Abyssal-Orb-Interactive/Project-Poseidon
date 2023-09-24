using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Ships
{
    [CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ship")]
    public class Ship : ScriptableObject
    {
        [SerializeField] private int _size = 1;
        [SerializeField] private int _maxNumberOfShipsOfThisType;
        public int Size { 
            get
        {
            if (_size <= 0) throw new MissingFieldException("Size of ship must be positive");
            return _size;
        }}

        [SerializeField] private GameObject _shipPrefab;

        public GameObject ShipPrefab
        {
            get
            {
                if (_shipPrefab == null) throw new MissingFieldException("Ship must have prefab");
                return _shipPrefab;
            }
        }

        public int MaxNumberOfShipsOfThisType
        {
            get
            {
                if (_maxNumberOfShipsOfThisType < 0) throw new InvalidOperationException("A player cannot have a negative number of ships.");
                return _maxNumberOfShipsOfThisType;
            }
        }
    }
}
