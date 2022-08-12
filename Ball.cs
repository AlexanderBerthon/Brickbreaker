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

        //returns the next index the ball will arrive at given the current position and trajectory. Used to trigger collisions
        public int nextMove() {
            return currentIndex + trajectory;
        }

        public void leftBorderCollision() {
            if (trajectory > 0) {
                trajectory = 17;
            }
            else {
                trajectory = -15;
            }
        }

        public void rightBorderCollision() {
            if (trajectory > 0) {
                trajectory = 15;
            }
            else {
                trajectory = -17;
            }
        }

        public void leftPaddleCollision() {
            trajectory = -17;
        }
        public void rightPaddleCollision() {
            trajectory = -15;
        }
        public void centerPaddleCollision() {
            trajectory = -16;
        }
        //test all this individually first, will make it easier
        public void topBorderCollision() {
            //invert trajectory and scramble direction?
            //this works unless it hits the top at the top left corner or top right corner, then it will have weird behavior. might need a check for that
            if(trajectory == -15) {
                trajectory = 17;
            }
            else if(trajectory == -17){
                trajectory = 15;
            }
            else {
                if(random.Next(0, 2)==0){
                    trajectory = 15;
                }
                else {
                    trajectory = 17;
                }
            }
        }

        public int getIndex() {
            return currentIndex;
        }

    }
}
