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

        //moves the paddle
        public void move() {
            index += nextMove;            
        }

        //return current position / index
        public int getIndex() {
            return index;
        }

        //prepares / stores the next move which will then be executed by the update function on the game tick
        //this logic is broken up into two functions so that only the last input before the tick is counted.
        //This allows the paddle movement to be consistent and predictable.
        public void queueMove(int movement) {
            nextMove = movement;
        }

        //returns current queued move
        public int getNextMove() {
            return nextMove;
        }

        //clears the stored next move
        public void clear() {
            nextMove = 0;
        }

    }
}
