using Shogi.Model.pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model
{
    public class Board
    {

        private Player p1;
        private Player p2;

        private int size;
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        private Piece[,] board;
        public Piece this[int i, int j]
        {
            get 
            {
                try
                {
                    return board[i, j];
                }
                catch (IndexOutOfRangeException)
                {
                    return null;
                }
            }
            set 
            {
                board[i,j] = value;
            }
        }

        public Board(Player p1, Player p2, int _size = 9)
        {
            size = _size;
            board = new Piece[size, size];
            Init(p1, p2);
        }

        public void Init(Player p1, Player p2)
        {
            Player p = p2;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i,j] = null;

                    if (i > size / 2) p = p1;

                    if (i == 0 || i == size - 1)
                    {
                        switch (j)
                        {
                            case 0:
                            case 8:
                                board[i, j] = new Piece(PiecesType.Kyosha, p, (i, j));
                                break;
                            case 1:
                            case 7:
                                board[i, j] = new Piece(PiecesType.Keima, p, (i, j));
                                break;
                            case 2:
                            case 6:
                                board[i, j] = new Piece(PiecesType.Ginsho, p, (i, j));
                                break;
                            case 3:
                            case 5:
                                board[i, j] = new Piece(PiecesType.Kinsho, p, (i, j));
                                break;
                            case 4:
                                if(p.IsSente)
                                {
                                    p.King = new Piece(PiecesType.Osho, p, (i, j));
                                    board[i, j] = p.King;
                                }
                                else
                                {
                                    p.King = new Piece(PiecesType.Gyokusho, p, (i, j));
                                    board[i, j] = p.King;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else if (i == 1)
                    {
                        if (j == 1)
                        {
                            board[i, j] = new Piece(PiecesType.Hisha, p, (i, j));
                        }
                        else if (j == 7)
                        {
                            board[i, j] = new Piece(PiecesType.Kakugyo, p, (i, j));
                        }
                    }
                    else if (i == 2)
                    {
                        board[i, j] = new Piece(PiecesType.Fuhyo, p, (i, j));
                    }
                    else if(i == 6)
                    {
                        board[i, j] = new Piece(PiecesType.Fuhyo, p, (i, j));
                    }
                    else if(i == 7)
                    {
                        if (j == 1)
                        {
                            board[i, j] = new Piece(PiecesType.Kakugyo, p, (i, j));
                        }
                        else if (j == 7)
                        {
                            board[i, j] = new Piece(PiecesType.Hisha, p, (i, j));
                        }
                    }

                }
            }

        }

        public bool MoveAPiece(Piece piece, (int,int) nextPos)
        {
            Piece otherPiece = board[nextPos.Item1, nextPos.Item2];
            Player player = piece.Owner;

            if (otherPiece != null && otherPiece.Owner != player)
            {
                otherPiece.HasBeenEat(player);
                board[piece.Pos.Item1, piece.Pos.Item2] = null;
                piece.Move(nextPos);
                board[nextPos.Item1, nextPos.Item2] = piece;
                return true;
            }
            else if (otherPiece == null)
            {
                board[piece.Pos.Item1, piece.Pos.Item2] = null;
                piece.Move(nextPos);
                board[nextPos.Item1, nextPos.Item2] = piece;
                return true;
            }

            return false;
            
        }

        public List<(int, int)> GetEmptyCell()
        {
            List<(int, int)> emptyCells = new List<(int, int)>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i,j] == null)
                    {
                        emptyCells.Add((i, j));
                    }
                }
            }

            return emptyCells;
        }

        public bool ParachuteAPiece(Piece piece, (int, int) parachutePos)
        {
            if (GetEmptyCell().Contains(parachutePos) || !piece.Owner.PiecesInHand.Contains(piece))
            {
                return false;
            }

            board[parachutePos.Item1, parachutePos.Item2] = piece;
            piece.Owner.PiecesInHand.Remove(piece);
            piece.Owner.PiecesOnBoard.Add(piece);

            return true;
        }

        public bool MoveAPiece((int,int) currentPos, (int,int) nextPos)
        {
            Piece piece = this[currentPos.Item1, currentPos.Item2];
            if (piece != null)
            {
               return MoveAPiece(piece, nextPos);
            }

            return false;
        }


        public Player CheckCheckmate()
        {
            Piece osho = p1.PiecesOnBoard.Where(p => p.PieceType == PiecesType.Osho)
                                         .FirstOrDefault();
            Piece gyokusho = p2.PiecesOnBoard.Where(p => p.PieceType == PiecesType.Gyokusho)
                                         .FirstOrDefault();

            bool oshoIsCheckmate = true;
            bool gyokushoIsCheckmate = true;

            Dictionary<string, List<(int, int)>> tmp;

            tmp = osho.GetPossibleMove(this);

            List<(int, int)> oshoPossibleMove = tmp["availableMove"];
            oshoPossibleMove.AddRange(tmp["attackMove"]);
            oshoPossibleMove.Add(osho.Pos);

            tmp = gyokusho.GetPossibleMove(this);

            List<(int, int)> gyokushoPossibleMove = tmp["availableMove"];
            gyokushoPossibleMove.AddRange(tmp["attackMove"]);
            gyokushoPossibleMove.Add(gyokusho.Pos);

            List<(int, int)> allMovePossible = new List<(int, int)>();
            foreach (Piece piece in osho.Owner.PiecesOnBoard)
            {
                tmp = piece.GetPossibleMove(this);
                allMovePossible.AddRange(tmp["availableMove"]);
            }

            foreach ((int,int) coor in gyokushoPossibleMove)
            { 
                if (!allMovePossible.Contains(coor))
                {
                    gyokushoIsCheckmate = false;
                    break;
                }
            }

            allMovePossible.Clear();
            foreach (Piece piece in osho.Owner.PiecesOnBoard)
            {
                tmp = piece.GetPossibleMove(this);
                allMovePossible.AddRange(tmp["availableMove"]);
            }

            foreach ((int, int) coor in oshoPossibleMove)
            {
                if (!allMovePossible.Contains(coor))
                {
                    oshoIsCheckmate = false;
                    break;
                }
            }

            if(oshoIsCheckmate)
            {
                return osho.Owner;
            }
            else if (gyokushoIsCheckmate)
            {
                return gyokusho.Owner;
            }

            return null;
        }
    }
}
