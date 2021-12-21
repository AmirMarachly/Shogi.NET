using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces.move
{
    class GinshoMove : IMove
    {
        public List<(int, int)> EvolvedMove((int, int) startPos, bool isSente)
        {
            List<(int, int)> result = new List<(int, int)>();

            result.Add((startPos.Item1, startPos.Item2 + 1));
            result.Add((startPos.Item1, startPos.Item2 - 1));

            result.Add((startPos.Item1 + 1, startPos.Item2));
            result.Add((startPos.Item1 - 1, startPos.Item2));

            if (isSente)
            {
                result.Add((startPos.Item1 + 1, startPos.Item2 - 1));
                result.Add((startPos.Item1 - 1, startPos.Item2 - 1));
            }
            else
            {
                result.Add((startPos.Item1 + 1, startPos.Item2 + 1));
                result.Add((startPos.Item1 - 1, startPos.Item2 + 1));
            }


            return result;
        }

        public List<(int, int)> Move((int, int) startPos, bool isSente)
        {
            List<(int, int)> result = new List<(int, int)>();

            result.Add((startPos.Item1 - 1, startPos.Item2 + 1));
            result.Add((startPos.Item1 - 1, startPos.Item2 - 1));
            result.Add((startPos.Item1 + 1, startPos.Item2 + 1));
            result.Add((startPos.Item1 + 1, startPos.Item2 - 1));

            if (isSente)
            {
                result.Add((startPos.Item1, startPos.Item2 - 1));
            }
            else
            {
                result.Add((startPos.Item1, startPos.Item2 + 1));
            }


            return result;
        }
    }
}
