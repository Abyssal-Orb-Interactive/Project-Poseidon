using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Ships
{
    [CreateAssetMenu(fileName = "Ship", menuName = "Ships/Create Ship")]
    public class Ship : ScriptableObject
    {
        [SerializeField] private int _size = 1;
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
    }
}
