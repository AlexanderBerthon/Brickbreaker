using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brickbreaker {

    internal class Ball {
        private Random random;
        private int currentIndex;
        private int trajectory;

        //trajectory key
        // + 16 = straight down
        // + 15 = down-left
        // + 17 = down-right
        // - 16 = straight up
        // - 15 = up-right
        // - 17 = up-left


        public Ball(int index) {
            random = new Random();
            currentIndex = index;
            if(random.Next(0,2) == 0) {
                trajectory = -15;
            }
            else {
                trajectory = -17;
            }
        }

        //moves the ball
        public void move() {
            currentIndex += trajectory;
        }

        //return current trajectory
        public int getTrajectory() {
            return trajectory;
        }

        //returns the next index the ball WILL arrive at given the current position and trajectory. Used to check for collisions
        public int nextMove() {
            return currentIndex + trajectory;
        }

        //reverse current trajectory
        public void reverse() {
            if (trajectory == 15) {
                trajectory = -15;
            }
            else if (trajectory == 17) {
                trajectory = -17;
            }
            else if (trajectory == -15) {
                trajectory = 15;
            }
            else if (trajectory == -17) {
                trajectory = 17;
            }
        }

        //bounce/redirect current trajectory. Mainly used for brick and left/right border collisions
        public void deflectHorizontal() {
            if (trajectory == 15) {
                trajectory = 17;
            }
            else if (trajectory == 17) {
                trajectory = 15;
            }
            else if (trajectory == -15) {
                trajectory = -17;
            }
            else if (trajectory == -17) {
                trajectory = -15;
            }
        }

        //bounce/redirect current trajectory. Mainly used for paddle and top border collisions
        public void deflectVertical() {
            if (trajectory == 17) {
                trajectory = -15;
            }
            else if (trajectory  == 15) {
                trajectory = -17;
            }
            else if (trajectory == -17) {
                trajectory = 15;
            }
            else if (trajectory == -15) {
                trajectory = 17;
            }
            else if (trajectory == -16) {
                if(random.Next(0,2) == 0) {
                    trajectory = 15;
                }
                else {
                    trajectory = 17;
                }
            }
        }

        //return current position/index
        public int getIndex() {
            return currentIndex;
        }

    }
}
