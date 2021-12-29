using Shogi.Model.pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model
{
    public class Player
    {

        private Piece king;
        public Piece King
        {
            get { return king; }
            set { king = value; }
        }
        private String name;
        public String Name
        {
            get { return name; }
        }

        private List<Piece> piecesOnBoard;
        public List<Piece> PiecesOnBoard
        {
            get { return piecesOnBoard; }
        }

        private List<Piece> piecesInHand;
        public List<Piece> PiecesInHand
        {
            get { return piecesInHand; }
        }

        private bool isSente;
        public bool IsSente
        {
            get { return isSente; }
        }

        private bool isPlaying;
        public bool IsPlaying
        {
            get { return isPlaying; }
        }

        public Player(String _name, bool _isSente, bool _isPlaying)
        {
            name = _name;
            isSente = _isSente;
            isPlaying = _isPlaying;

            piecesOnBoard = new List<Piece>();
            piecesInHand = new List<Piece>();
        }


        public void HasPlayed()
        {
            isPlaying = !isPlaying;
        }

        public bool HasLost()
        {
            return !piecesOnBoard.Contains(king);
        }
    }
}
