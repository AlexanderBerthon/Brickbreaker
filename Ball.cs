using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brickbreaker {

    //logic for ball and physics interactions

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


        public Ball() {
            currentIndex = 199;
            trajectory = -16;
            random = new Random();
        }

        public void update() {
            currentIndex += trajectory;
        }

        public int getTrajectory() {
            return trajectory;
        }

        //returns the next index the ball will arrive at given the current position and trajectory. Used to trigger collisions
        public int nextMove() {
            return currentIndex + trajectory;
        }

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
        }

        public int getIndex() {
            return currentIndex;
        }

    }
}
