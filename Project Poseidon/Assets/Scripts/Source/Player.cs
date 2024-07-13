using Source.Battle_Field;
using UnityEngine;
using Grid = UnityEngine.Grid;

namespace Source
{
    public class Player
    {
        private readonly Battlefield _battlefield;

        public Player(Vector2Int battlefieldOffset, Transform cameraTarget)
        {
            _battlefield = new Battlefield(battlefieldOffset.x, battlefieldOffset.y, cameraTarget);
        }

        public Battlefield GetBattlefield()
        {
            return _battlefield;
        }
    }
}
