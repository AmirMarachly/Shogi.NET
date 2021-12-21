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

        private Piece[,] board;
        public Piece? this[int i, int j]
        {
            get { return board[i,j]; }
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
                                board[i, j] = new Piece(PiecesType.Kyosha, p);
                                break;
                            case 1:
                            case 7:
                                board[i, j] = new Piece(PiecesType.Keima, p);
                                break;
                            case 2:
                            case 6:
                                board[i, j] = new Piece(PiecesType.Ginsho, p);
                                break;
                            case 3:
                            case 5:
                                board[i, j] = new Piece(PiecesType.Kinsho, p);
                                break;
                            case 4:
                                board[i, j] = new Piece(PiecesType.Osho, p);
                                break;
                            default:
                                break;
                        }
                    }
                    else if (i == 1)
                    {
                        if (j == 1)
                        {
                            board[i, j] = new Piece(PiecesType.Hisha, p);
                        }
                        else if (j == 7)
                        {
                            board[i, j] = new Piece(PiecesType.Kakugyo, p);
                        }
                    }
                    else if (i == 2)
                    {
                        board[i, j] = new Piece(PiecesType.Fuhyo, p);
                    }
                    else if(i == 6)
                    {
                        board[i, j] = new Piece(PiecesType.Fuhyo, p);
                    }
                    else if(i == 7)
                    {
                        if (j == 1)
                        {
                            board[i, j] = new Piece(PiecesType.Kakugyo, p);
                        }
                        else if (j == 7)
                        {
                            board[i, j] = new Piece(PiecesType.Hisha, p);
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
                piece.Move(nextPos);
                board[piece.Pos.Item1, piece.Pos.Item2] = null;
                board[nextPos.Item1, nextPos.Item2] = piece;
                return true;
            }
            else if (otherPiece == null)
            {
                piece.Move(nextPos);
                board[piece.Pos.Item1, piece.Pos.Item2] = null;
                board[nextPos.Item1, nextPos.Item2] = piece;
                return true;
            }

            return false;
            
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

        public void resetHighlight()
        {
            foreach (Piece p in board)
            {
                if (p != null)
                {
                    p.IsHighlight = false;
                }
            }
        }
    }
}
