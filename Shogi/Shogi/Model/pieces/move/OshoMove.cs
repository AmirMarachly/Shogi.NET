using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces.move
{
    class OshoMove : IMove
    {
        public List<(int, int)> EvolvedMove((int, int) startPos, bool isSente)
        {
            throw new NotImplementedException();
        }

        public List<(int, int)> Move((int, int) startPos, bool isSente)
        {
            List<(int, int)> result = new List<(int, int)>();

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i != 0 && j != 0)
                        result.Add((startPos.Item1 + i, startPos.Item2 + j));
                }

            }

            return result;
        }
    }
}
