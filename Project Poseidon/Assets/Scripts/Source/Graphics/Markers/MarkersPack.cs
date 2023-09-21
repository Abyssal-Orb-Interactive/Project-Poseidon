using UnityEngine;

namespace Source.Graphics.Markers
{
    [CreateAssetMenu(fileName = "Marker's Pack", menuName = "ScriptableObjects/MarkerPack")]
    public class MarkersPack : ScriptableObject
    {
        [SerializeField] private GameObject _missMarker;
        [SerializeField] private GameObject _hitMarker;
        [SerializeField] private GameObject _shipExplosionMarker;

        public Object MissMarker => _missMarker;
        public Object HitMarker => _hitMarker;
        public Object ShipExplosionMarker => _shipExplosionMarker;
    }
}
