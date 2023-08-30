using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Source
{
    public class GameManager : MonoBehaviour
    {
        public event Action TurnEnded;

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
