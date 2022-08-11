using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brickbreaker {

    //controls for player paddle

    internal class Paddle {
        private int index;
        private int nextMove;

        public Paddle() {
            index = 215;
        }

        public int update() {
            index += nextMove;            
            return nextMove;
        }

        public int getIndex() {
            return index;
        }

        public void queueMove(int movement) {
            nextMove = movement;
        }

        public void clear() {
            nextMove = 0;
        }


    }
}
