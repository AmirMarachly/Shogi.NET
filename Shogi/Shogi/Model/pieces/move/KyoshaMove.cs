using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces.move
{
    class KyoshaMove : IMove
    {
        public List<(int, int)> EvolvedMove((int, int) startPos, Board board, bool isSente)
        {
            IMove move = new KinshoMove();

            return move.Move(startPos, board, isSente);
        }

        public List<(int, int)> Move((int, int) startPos, Board board, bool isSente)
        {
            List<(int, int)> result = new List<(int, int)>();


            (int, int) tmpPos = startPos;


            while (tmpPos.Item1 < 9 && tmpPos.Item1 >= 0)
            {
                if (isSente)
                {
                    tmpPos = (tmpPos.Item1 - 1, tmpPos.Item2);
                }
                else
                {
                    tmpPos = (tmpPos.Item1 + 1, tmpPos.Item2);
                }
                result.Add(tmpPos);
                if (board[tmpPos.Item1, tmpPos.Item2] != null)
                {
                    return result;
                }
            }

            return result;
        }
    }
}
