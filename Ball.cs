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


        Ball() {
            random = new Random();
            currentIndex = random.Next(65, 113);
            trajectory = 16;
        }

        public void update() {
            currentIndex += trajectory;
        }

        public void leftBorderCollision() {

        }

        public void rightBorderCollision() {

        }

        public void bottomBorderCollision() {

        }

        public void topBorderCollision() {
            //invert trajectory and scramble direction?
            //this works unless it hits the top at the top left corner or top right corner, then it will have weird behavior. might need a check for that
            if(trajectory == -16) {
                if(random.Next(0, 1) == 0) {
                    trajectory = 15;
                }
                else {
                    trajectory = 17;
                }
            }
            else if(trajectory == -15) {
                trajectory = 17;
            }
            else {
                trajectory = 15;
            }

        }

        public int getIndex() {
            return currentIndex;
        }

    }
}
