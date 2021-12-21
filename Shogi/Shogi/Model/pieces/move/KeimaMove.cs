using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces.move
{
    class KeimaMove : IMove
    {
        public List<(int, int)> EvolvedMove((int, int) startPos, bool isSente)
        {
            //TODO
            throw new NotImplementedException();
        }

        public List<(int, int)> Move((int, int) startPos, bool isSente)
        {
            List<(int, int)> result = new List<(int, int)>();

            if(isSente)
            {
                result.Add((startPos.Item1 - 1, startPos.Item2 - 2));
            }
            else
            {
                result.Add((startPos.Item1 + 1, startPos.Item2 - 2));
            }


            return result;
        }
    }
}
