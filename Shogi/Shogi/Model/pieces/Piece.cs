using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces
{
    public class Piece
    {

        private IMove move;
        private PiecesType pieceType;
        public PiecesType PieceType
        {
            get { return pieceType; }
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

        public Piece(PiecesType _type, Player _owner, (int, int) _pos, bool _isEvolved = false)
        {
            pieceType = _type;
            isEvolved = _isEvolved;
            owner = _owner;
            pos = _pos;
            string typeMove = pieceType.ToString() + "Move";

            Type moveClass = Type.GetType("Shogi.Model.pieces.move." + typeMove);

            move = Activator.CreateInstance(moveClass, null) as IMove;

            owner.PiecesOnBoard.Add(this);
        }

        public Dictionary<string, List<(int, int)>> GetPossibleMove(Board board)
        {

            List<(int, int)> possibleMove;

            if (!isEvolved)
            {
                possibleMove = move.Move(pos, board, owner.IsSente);
            }
            else
            {
                possibleMove = move.EvolvedMove(pos, board, owner.IsSente);
            }
            
            List<(int, int)> avaibleMove = possibleMove.ConvertAll(move => move);
            List<(int, int)> attackMove = new List<(int, int)>();

            foreach ((int,int) cell in possibleMove)
            {
                if (cell.Item1 < 9 && cell.Item1 >= 0 && cell.Item2 < 9 && cell.Item2 >= 0)
                {
                    Piece piece = board[cell.Item1, cell.Item2];
                    if (piece != null)
                    {
                         avaibleMove.Remove(cell);
                        if (piece.Owner != owner)
                        {
                            attackMove.Add(cell);
                        }
                    }
                }
                else
                {
                    avaibleMove.Remove(cell);
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
                pieceType = (PiecesType)((int)pieceType - 8);
                isEvolved = !isEvolved;
            }
            return this;
        }

        public Piece Evolve()
        {
            if (!isEvolved)
            {
                pieceType = (PiecesType)((int)pieceType + 8);
                isEvolved = !isEvolved;

            }
            return this;
        }

        public List<(int,int)> GetPossibleParachute(Board board, Player opponnent)
        {

            List<(int, int)> result = board.GetEmptyCell();

            if (pieceType == PiecesType.Fuhyo || pieceType == PiecesType.Keima || pieceType == PiecesType.Kyosha)
            {
                foreach ((int, int) cell in result.ConvertAll(x => x))
                {
                    if ((cell.Item1 == 0 && owner.IsSente) || (cell.Item1 == 8 && !owner.IsSente))
                    {
                        result.Remove(cell);
                    }
                }
            }

            if (pieceType == PiecesType.Keima)
            {
                foreach ((int, int) cell in result.ConvertAll(x => x))
                {
                    if ((cell.Item1 == 1 && owner.IsSente) || (cell.Item1 == 7 && !owner.IsSente))
                    {
                        result.Remove(cell);
                    }
                }
            }

            if (pieceType == PiecesType.Fuhyo)
            {
                if (opponnent.IsSente && result.Contains((opponnent.King.Pos.Item1 + 1, opponnent.King.Pos.Item2)))
                {
                    result.Remove((opponnent.King.Pos.Item1 + 1, opponnent.King.Pos.Item2));
                }
                else if(result.Contains((opponnent.King.Pos.Item1 - 1, opponnent.King.Pos.Item2)))
                {
                    result.Remove((opponnent.King.Pos.Item1 - 1, opponnent.King.Pos.Item2));
                }

                foreach (Piece piece in owner.PiecesOnBoard)
                {
                    if(piece.PieceType == PiecesType.Fuhyo)
                    {
                        foreach ((int, int) cell in result.ConvertAll(x=>x))
                        {
                            if(cell.Item2 == piece.Pos.Item2)
                            {
                                result.Remove(cell);
                            }
                        }
                    }
                }
            }


            return result;
        }

        public bool CanPromote((int, int) nextPos)
        {
            if (owner.IsSente && (nextPos.Item1 <= 2 || pos.Item1 <= 2))
            {
                return true;
            }
            else if (!owner.IsSente && (nextPos.Item1 >= 6 || pos.Item1 >= 6))
            {
                return true;
            }

            return false;
        }

        public void NeedPromote()
        {
            if (owner.IsSente)
            {
                if (pos.Item1 == 0)
                {
                    if (pieceType == PiecesType.Fuhyo || pieceType == PiecesType.Kyosha ||
                        pieceType == PiecesType.Keima)
                    {
                        Evolve();
                    }
                }

                if (pos.Item1 == 1 && pieceType == PiecesType.Keima)
                {
                    Evolve();
                }
            }
            else
            {
                if (pos.Item1 == 8)
                {
                    if (pieceType == PiecesType.Fuhyo || pieceType == PiecesType.Kyosha ||
                        pieceType == PiecesType.Keima)
                    {
                        Evolve();
                    }
                }

                if (pos.Item1 == 7 && pieceType == PiecesType.Keima)
                {
                    Evolve();
                }
            }
            
        }


    }
}
