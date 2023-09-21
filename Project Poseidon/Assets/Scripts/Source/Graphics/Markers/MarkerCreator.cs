using System;
using Source.Battle_Field;

namespace Source.Graphics.Markers
{
    public static class MarkerCreator
    {
        private static MarkersPack _pack;

        public static void Initialize(MarkersPack pack)
        {
            _pack = pack;
        }
        
        public static Marker Create(TypeOfOpens type)
        {
            if (_pack == null) throw new InvalidOperationException("Before create markers, you must initialize markers pack");
            
            return type switch
            {
                TypeOfOpens.Miss => new MissMarker(_pack.MissMarker),
                TypeOfOpens.Hit => new HitMarker(_pack.HitMarker),
                TypeOfOpens.ShipExplosion => new ShipExplosionMarker(_pack.ShipExplosionMarker),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static Marker CreateFromPack(TypeOfOpens type, MarkersPack pack)
        {
            return type switch
            {
                TypeOfOpens.Miss => new MissMarker(pack.MissMarker),
                TypeOfOpens.Hit => new HitMarker(pack.HitMarker),
                TypeOfOpens.ShipExplosion => new ShipExplosionMarker(pack.ShipExplosionMarker),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
