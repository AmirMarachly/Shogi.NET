using Shogi.Model.pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model
{
    class Board
    {
        private int size;
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        private List<List<Piece>> board;
        public Piece? this[int i, int j]
        {
            get { return board.ElementAt(i).ElementAt(j); }
            set 
            {
                board.ElementAt(i).RemoveAt(j);
                board.ElementAt(i).Insert(j, value);
            }
        }

        public Board(Player p1, Player p2, int _size)
        {
            size = _size;
            board = new List<List<Piece>>();
            Init(p1, p2);
        }

        public void Init(Player p1, Player p2)
        {
            Player p = p2;

            for (int i = 0; i < size; i++)
            {
                board.Append(new List<Piece>());
                for (int j = 0; j < size; j++)
                {
                    board.ElementAt(i).Append(null);

                    if (i > size / 2) p = p1;

                    if (i == 0 || i == size - 1)
                    {
                        switch (j)
                        {
                            case 0:
                            case 8:
                                this[i, j] = new Piece(PiecesType.Lance, p);
                                break;
                            case 1:
                            case 7:
                                this[i, j] = new Piece(PiecesType.Knight, p);
                                break;
                            case 2:
                            case 6:
                                this[i, j] = new Piece(PiecesType.Silver, p);
                                break;
                            case 3:
                            case 5:
                                this[i, j] = new Piece(PiecesType.Gold, p);
                                break;
                            case 4:
                                this[i, j] = new Piece(PiecesType.King, p);
                                break;
                            default:
                                break;
                        }
                    }
                    else if (i == 1)
                    {
                        if (j == 1)
                        {
                            this[i, j] = new Piece(PiecesType.Rook, p);
                        }
                        else if (j == 7)
                        {
                            this[i, j] = new Piece(PiecesType.Bishop, p);
                        }
                    }
                    else if (i == 2)
                    {
                        this[i, j] = new Piece(PiecesType.Pawns, p);
                    }
                    else if(i == 6)
                    {
                        this[i, j] = new Piece(PiecesType.Pawns, p);
                    }
                    else if(i == 7)
                    {
                        if (j == 1)
                        {
                            this[i, j] = new Piece(PiecesType.Bishop, p);
                        }
                        else if (j == 7)
                        {
                            this[i, j] = new Piece(PiecesType.Rook, p);
                        }
                    }

                }
            }

        }

        public bool MoveAPiece(Piece piece, (int,int) nextPos)
        {
            Piece? otherPiece = this[nextPos.Item1, nextPos.Item2];
            Player player = piece.Owner;

            if (otherPiece != null && otherPiece.Owner != player)
            {
                otherPiece.HasBeenEat(player);
                piece.Move(nextPos);
                return true;
            }
            else if (otherPiece == null)
            {
                piece.Move(nextPos);
                return true;
            }

            return false;
            
        }

        public bool MoveAPiece((int,int) currentPos, (int,int) nextPos)
        {
            Piece? piece = this[currentPos.Item1, currentPos.Item2];
            if (piece != null)
            {
               return MoveAPiece(piece, nextPos);
            }

            return false;
        }
    }
}
