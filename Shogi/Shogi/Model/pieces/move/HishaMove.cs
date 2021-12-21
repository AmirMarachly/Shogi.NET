using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces.move
{
    class HishaMove : IMove
    {
        public List<(int, int)> EvolvedMove((int, int) startPos, bool isSente)
        {
            List<(int, int)> result = Move(startPos, isSente);

            result.Add((startPos.Item1 - 1, startPos.Item2 + 1));
            result.Add((startPos.Item1 - 1, startPos.Item2 - 1));
            result.Add((startPos.Item1 + 1, startPos.Item2 + 1));
            result.Add((startPos.Item1 + 1, startPos.Item2 - 1));

            return result;
        }

        public List<(int, int)> Move((int, int) startPos, bool isSente)
        {
            List<(int, int)> result = new List<(int, int)>();

            for (int i = startPos.Item1 - 9; i < 9 - startPos.Item1; i++)
            {
                if (i != 0)
                    result.Add((startPos.Item1 + i, startPos.Item2));
            }

            for (int i = startPos.Item2 - 9; i < 9 - startPos.Item2; i++)
            {
                if (i != 0)
                    result.Add((startPos.Item1, startPos.Item2 + i));
            }

            return result;
        }
    }
}
