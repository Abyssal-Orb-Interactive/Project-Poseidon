using System;
using System.Collections.Generic;
using System.Linq;
using Source.Turn_State_Machine;

namespace Source
{
    public class PlayersManager
    {
        private IList<Player> _players;
        private Players _currentPlayer;

        public PlayersManager(IList<Player> players)
        {
            _players = players;
            _currentPlayer = 0;
        }

        public Player GetCurrentPlayer()
        {
            return _players[(int)_currentPlayer];
        }

        public void PassToNextPlayer()
        {
            _currentPlayer = GetNextPlayerID();
        }

        public Players GetNextPlayerID()
        {
            return (Players)(((int)_currentPlayer + 1) % _players.Count);
        }

        public Player GetPlayerByID(Players id)
        {
            if (_players.Count() < (int)id) throw new ArgumentException("Player with this ID don't exist.");
            
            return _players[(int)id];
        }

        public Players GetCurrentPlayerID()
        {
            return _currentPlayer;
        }
    }
}