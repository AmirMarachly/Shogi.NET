using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces
{
    class Piece
    {

        private IMove move;
        private PiecesType type;
        public PiecesType Type
        {
            get { return type; }
        }

        private bool isEvolved;
        public bool IsEvolved
        {
            get { return isEvolved; }
            set { isEvolved = value; }
        }

        private Player owner;
        public Player Owner
        {
            get { return owner; }
        }

        private (int, int) pos;
        public (int, int) Pos
        {
            get { return pos; }
        }

        public Piece(PiecesType _type, Player _owner, bool _isEvolved = false)
        {
            type = _type;
            isEvolved = _isEvolved;
            owner = _owner;

            switch (type)
            {
                case PiecesType.King:

                    break;
                case PiecesType.Rook:
                    break;
                case PiecesType.Bishop:
                    break;
                case PiecesType.Gold:
                    break;
                case PiecesType.Silver:
                    break;
                case PiecesType.Knight:
                    break;
                case PiecesType.Lance:
                    break;
                case PiecesType.Pawns:
                    break;
                default:
                    break;
            }
        }

        public Dictionary<string, List<(int, int)>> GetPossibleMove(Board board)
        {
            List<(int, int)> possibleMove = move.Move(pos);
            List<(int, int)> avaibleMove = possibleMove.ConvertAll(move => move);
            List<(int, int)> attackMove = new List<(int, int)>();

            foreach ((int,int) cell in possibleMove)
            {
                Piece? piece = board[cell.Item1, cell.Item2];
                if (piece != null)
                {
                    avaibleMove.Remove(cell);
                    if (piece.Owner != owner)
                    {
                        attackMove.Add(cell);
                    }
                }
            }

            Dictionary<string, List<(int, int)>> returnDict = new Dictionary<string, List<(int, int)>>();
            returnDict.Add("avaibleMove", avaibleMove);
            returnDict.Add("attackMove", attackMove);

            return returnDict;
        }

        public void Move((int, int) newPos) => pos = newPos;

        public void HasBeenEat(Player p)
        {
            owner.PiecesOnBoard.Remove(this);
            owner = p;
            owner.PiecesInHand.Add(this.Unevolved());
        }

        public Piece Unevolved()
        {
            if (isEvolved)
            {
                isEvolved = !isEvolved;
            }
            return this;
        }

        public Piece Evolve()
        {
            if (!isEvolved)
            {
                isEvolved = !isEvolved;
            }
            return this;
        }
    }
}
