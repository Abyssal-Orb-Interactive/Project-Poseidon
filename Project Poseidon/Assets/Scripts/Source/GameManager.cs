using System;
using System.Linq;
using Source.Battle_Field;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Grid = Source.Battle_Field.Grid;

namespace Source
{
    public class GameManager : MonoBehaviour
    {
        private Grid _grid;
        public event Action TurnEnded;

        private void Start()
        {
            _grid = new Grid(GridGenerator.CreateGrid());
            var opener = new Opener();
            _grid.OpenCell(opener);

            foreach (var cell in _grid.Cells)
            {
                Debug.Log(cell.GetStatus());
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                EndTurn();
            }
        }

        private void EndTurn()
        {
            TurnEnded?.Invoke();
        }
    }
}
