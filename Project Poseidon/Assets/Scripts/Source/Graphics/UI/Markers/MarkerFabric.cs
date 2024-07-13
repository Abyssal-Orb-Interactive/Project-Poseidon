using System;
using Source.Battle_Field;
using Source.Graphics.UI.Markers;

namespace Source.Graphics.Markers
{
    public static class MarkerFabric
    {
        private static MarkersPack _pack;

        public static void Initialize(MarkersPack pack)
        {
            _pack = pack ? pack : throw new ArgumentNullException(nameof(pack));
        }
        
        public static Marker Create(OpenType openType, MarkersPack pack = null)
        {
            pack ??= _pack;
            
            return openType switch
            {
                OpenType.Miss => new MissMarker(pack.MissMarker),
                OpenType.Hit => new HitMarker(pack.HitMarker),
                OpenType.ShipExplosion => new ShipExplosionMarker(pack.ShipExplosionMarker),
                _ => throw new ArgumentOutOfRangeException(nameof(openType), openType, null)
            };
        }
    }
}
