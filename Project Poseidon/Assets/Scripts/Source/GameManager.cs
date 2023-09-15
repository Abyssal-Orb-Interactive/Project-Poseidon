using System;
using Source.Battle_Field;
using Source.Graphics;
using UnityEngine;
using Grid = Source.Battle_Field.Grid;

namespace Source
{
    public class GameManager : MonoBehaviour
    {
        private Grid _grid;
        public event Action TurnEnded;

        [SerializeField] private GridVisualizer _gridVisualizer;

        private void Start()
        {
            _grid = new Grid(GridGenerator.CreateGrid());
            _gridVisualizer.Initialize(_grid);
            
            var opener = new Opener();
            _grid.OpenCells(opener);
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
