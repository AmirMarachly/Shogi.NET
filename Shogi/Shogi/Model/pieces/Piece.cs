﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces
{
    class Piece
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

        private bool isHighlight;
        public bool IsHighlight
        {
            get { return isHighlight; }
            set { isHighlight = value; }
        }

        public Piece(PiecesType _type, Player _owner, bool _isEvolved = false, bool _isHighlight = false)
        {
            pieceType = _type;
            isEvolved = _isEvolved;
            owner = _owner;
            isHighlight = _isHighlight;
            string typeMove = pieceType.ToString() + "Move";

            Type moveClass = Type.GetType("Shogi.Model.pieces.move." + typeMove);

            move = Activator.CreateInstance(moveClass, null) as IMove;
        }

        public Dictionary<string, List<(int, int)>> GetPossibleMove(Board board)
        {

            List<(int, int)> possibleMove;

            if (isEvolved)
            {
                possibleMove = move.Move(pos, owner.IsSente);
            }
            else
            {
                possibleMove = move.EvolvedMove(pos, owner.IsSente);
            }
            
            List<(int, int)> avaibleMove = possibleMove.ConvertAll(move => move);
            List<(int, int)> attackMove = new List<(int, int)>();

            foreach ((int,int) cell in possibleMove)
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
    }
}