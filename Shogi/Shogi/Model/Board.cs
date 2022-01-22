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
                board[i, j] = value;
            }
        }

        public Board(Player p1, Player p2, int _size = 9)
        {
            size = _size;
            board = new Piece[size, size];
            Init(p1, p2);
        }

        /// <summary>
        /// Init the board with placing every piece it
        /// </summary>
        /// <param name="p1">The Sente player</param>
        /// <param name="p2">The Goto player</param>
        public void Init(Player p1, Player p2)
        {
            Player p = p2;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = null;

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
                                if (p.IsSente)
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
                    else if (i == 6)
                    {
                        board[i, j] = new Piece(PiecesType.Fuhyo, p, (i, j));
                    }
                    else if (i == 7)
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

        /// <summary>
        /// Move a piece of the board
        /// </summary>
        /// <param name="piece">The piece that will be moved</param>
        /// <param name="nextPos">The next position of the piece</param>
        /// <returns>True if every things works good, false otherwise</returns>
        public bool MoveAPiece(Piece piece, (int, int) nextPos)
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

        /// <summary>
        /// Get every cell that don't contains a piece
        /// </summary>
        /// <returns>The list of all empty cell</returns>
        public List<(int, int)> GetEmptyCell()
        {
            List<(int, int)> emptyCells = new List<(int, int)>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] == null)
                    {
                        emptyCells.Add((i, j));
                    }
                }
            }

            return emptyCells;
        }

        /// <summary>
        /// Parachute a piece on the board
        /// </summary>
        /// <param name="piece">The piece that will be parachuted</param>
        /// <param name="parachutePos">The parachuted position</param>
        /// <returns>True if every things works good, false otherwise</returns>
        public bool ParachuteAPiece(Piece piece, (int, int) parachutePos)
        {
            if (!GetEmptyCell().Contains(parachutePos) || !piece.Owner.PiecesInHand.Contains(piece))
            {
                return false;
            }

            board[parachutePos.Item1, parachutePos.Item2] = piece;
            piece.Owner.PiecesInHand.Remove(piece);
            piece.Owner.PiecesOnBoard.Add(piece);
            piece.Move(parachutePos);

            return true;
        }
    }
}
