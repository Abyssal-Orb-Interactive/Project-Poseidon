using System;

namespace Source.Ships
{
    public static class ShipFabric
    {
        private static ShipsPack _pack;

        public static void Initialize(ShipsPack pack)
        {
            _pack = pack;
        }
        
        public static ShipLogicalRepresentation Create(ShipType type)
        {
            if (_pack == null) throw new InvalidOperationException("ShipCreator must be initialized with the desired ship pack before it can be used.");
            
            return type switch
            {
                ShipType.TorpedoBoat => new ShipLogicalRepresentation(_pack.TorpedoBoat),
                ShipType.Destroyer => new ShipLogicalRepresentation(_pack.Destroyer),
                ShipType.Cruiser => new ShipLogicalRepresentation(_pack.Cruiser),
                ShipType.Battleship => new ShipLogicalRepresentation(_pack.Battleship),
                _ => throw new InvalidOperationException("There is no such ship type in this pack.")
            };
        }
    }
}
