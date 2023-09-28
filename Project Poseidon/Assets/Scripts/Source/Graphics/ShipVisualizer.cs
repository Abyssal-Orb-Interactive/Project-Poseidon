using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Ships
{
    public class ShipVisualizer : MonoBehaviour
    {
        [SerializeField] private Transform _shipHolder;

        private void OnValidate()
        {
            _shipHolder ??= GetComponent<Transform>();
        }
        
        public void VisualizeShips(IEnumerable<IReadonlyLogicalRepresentation> ships)
        {
            foreach (var ship in ships)
            {
                var prefab = ship.GetGraphicsRepresentation();
                foreach (var coord in ship.SegmentsCoords)
                {
                    Instantiate(prefab, new Vector3(coord.x * 10f + 5f, 0, coord.y * 10f + 5f), Quaternion.identity, _shipHolder);
                }
            }
        }
    }
}
