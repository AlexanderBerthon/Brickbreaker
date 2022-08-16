namespace Brickbreaker {

    public partial class Form1 : Form {
        //Global Variables :(
        int score;
        Random random = new Random();
        Button[] btnArray;
        Boolean gameOver;

        Ball ball;
        Paddle paddle;

        //highscore class variable - NYI
        System.Windows.Forms.Timer timer;

        System.Windows.Forms.Timer timer2;

        public Form1() {
            InitializeComponent();
            score = 0;
            gameOver = false;
            btnArray = new Button[256];
            Gameboard.Controls.CopyTo(btnArray, 0);

            ball = new Ball();
            paddle = new Paddle();

            timer = new System.Windows.Forms.Timer();
            timer2 = new System.Windows.Forms.Timer();

            //209 - 222
            //below is for testing, remove after
            //btnArray[paddle.getIndex()].BackColor = Color.Red;
            btnArray[paddle.getIndex()].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex()].Tag = "Paddle 1";
            //btnArray[paddle.getIndex()+13].BackColor = Color.Red;
            
            for (int i = 1; i< 13; i++) {
                //btnArray[paddle.getIndex()+i].BackColor = Color.Red;
                btnArray[paddle.getIndex() + i].BackgroundImage = Properties.Resources.Paddle;
                btnArray[paddle.getIndex() + i].Tag = "Paddle 2";

            }
            btnArray[paddle.getIndex() + 14].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 14].Tag = "Paddle 4";




            //change to [index] [ + 1 ] [ + 2 ] [ + 3 ]
            //have to change all the logic to go with this..
            /* re-enable after testing
            btnArray[paddle.getIndex()].BackColor = Color.Red;
            btnArray[paddle.getIndex()].Tag = "Paddle 1";
            btnArray[paddle.getIndex() + 1].BackColor = Color.Red;
            btnArray[paddle.getIndex() + 1].Tag = "Paddle 2";
            btnArray[paddle.getIndex() + 2].BackColor = Color.Red;
            btnArray[paddle.getIndex() + 2].Tag = "Paddle 3";
            btnArray[paddle.getIndex() + 3].BackColor = Color.Red;
            btnArray[paddle.getIndex() + 3].Tag = "Paddle 4";
            */


            for (int i = 17; i < 79; i++) {
                if (i != 31 && i != 32 && i != 47 && i != 48 && i != 63 && i != 64) {
                    int j = random.Next(0, 6);
                    btnArray[i].Tag = "Brick";
                    switch (j) {
                        case 0:
                            btnArray[i].BackColor = Color.Red;
                            break;
                        case 1:
                            btnArray[i].BackColor = Color.Blue;
                            break;
                        case 2:
                            btnArray[i].BackColor = Color.Green;
                            break;
                        case 3:
                            btnArray[i].BackColor = Color.Purple;
                            break;
                        case 4:
                            btnArray[i].BackColor = Color.Yellow;
                            break;
                        case 5:
                            btnArray[i].BackColor = Color.Orange;
                            break;
                    }
                }
            }

            timer.Interval = 500;
            timer.Tick += new EventHandler(TimerEventProcessor);

            timer2.Interval = 150;
            timer2.Tick += new EventHandler(TimerEventProcessor2);

            //start
            timer.Start();
            timer2.Start();
        }

        private void TimerEventProcessor(Object anObject, EventArgs eventargs) {
            if (!gameOver) {
                //logic

                /*
TODO: brick collison

if trajectory is up right and the block to the right of the balls current pos is a brick
then destroy that brick and rebound down left

else if trajectory is up left and the block to the left of the balls current pos is a brick
then destroy that brick and rebound down right

else if there is a brick directly above the balls current pos
then destroy that brick and rebound opposite of current trajectory

else if none of the above but trajectory next move will hit a brick (current logic)
then destroy that brick and reboud opposite of current trajectory

else if ... all the rest of the crap / border collisions

try this logic, implement it, and test

*/

                


                    //ball movement
                    //erase ball display
                    btnArray[ball.getIndex()].BackgroundImage = null;
                //update position
                //this will mess up on the corner since it is run before the border code
                //|[][][]|
                //|()    |
                //|  \   |
                //overflow sees brick to the "left" (-1 of curent index) even though it is hitting
                //the border
                if (ball.getTrajectory() == -15 && btnArray[ball.getIndex() + 1].Tag == "Brick") {
                    btnArray[ball.getIndex() + 1].BackColor = Color.Black;
                    btnArray[ball.getIndex() + 1].Tag = "";
                    ball.brickCollision(); 
                    if(btnArray[ball.getIndex() - 16].Tag == "Brick") {
                        btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                        btnArray[ball.getIndex() - 16].Tag = "";
                    }
                }
                else if (ball.getTrajectory() == -17 && btnArray[ball.getIndex() - 1].Tag == "Brick") {
                    btnArray[ball.getIndex() - 1].BackColor = Color.Black;
                    btnArray[ball.getIndex() - 1].Tag = "";
                    ball.brickCollision();
                    if (btnArray[ball.getIndex() - 16].Tag == "Brick") {
                        btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                        btnArray[ball.getIndex() - 16].Tag = "";
                    }
                }
                else if (btnArray[ball.getIndex() - 16].Tag == "Brick") {
                    btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                    btnArray[ball.getIndex() - 16].Tag = "";
                    ball.topBorderCollision();
                }
                else if (btnArray[ball.nextMove()].Tag == "Brick") {
                    btnArray[ball.nextMove()].BackColor = Color.Black;
                    btnArray[ball.nextMove()].Tag = "";
                    ball.brickCollision();
                }
                else if (btnArray[ball.nextMove()].Tag == "Left Border") {
                    ball.leftBorderCollision();
                    if (btnArray[ball.nextMove()].Tag == "Paddle 2"){
                        ball.leftPaddleCollision();
                    }
                }
                else if (btnArray[ball.nextMove()].Tag == "Right Border") {
                    ball.rightBorderCollision();
                    if (btnArray[ball.nextMove()].Tag == "Paddle 3") {
                        ball.leftPaddleCollision();
                    }
                }
                else if (btnArray[ball.nextMove()].Tag == "Paddle 1") {                    
                    ball.leftPaddleCollision();
                }
                else if (btnArray[ball.nextMove()].Tag == "Paddle 4") {
                    ball.rightPaddleCollision();
                }
                else if (btnArray[ball.nextMove()].Tag == "Paddle 2" || btnArray[ball.nextMove()].Tag == "Paddle 3") {
                    ball.centerPaddleCollision();
                }
                else if (btnArray[ball.nextMove()].Tag == "Top Border") {
                    ball.topBorderCollision();
                }
                else if (btnArray[ball.nextMove()].Tag == "Bottom Border") {
                    Application.Exit();
                    gameOver = true;
                }
                else { }
                
            ball.update();
               
            if (!gameOver) {
                    //redraw ball
                    btnArray[ball.getIndex()].BackgroundImage = Properties.Resources.orb;
                }
            }
            else {
                Application.Exit();
                //gameOver() //display gameover menu / highscore / etc. 
            }
        }

        //paddle movement clock
        private void TimerEventProcessor2(Object anObject, EventArgs eventargs) {
            if (!gameOver) {
                paddle.update();
                if (paddle.getNextMove() == -1) {
                    //btnArray[paddle.getIndex()].BackColor = Color.Red;
                    btnArray[paddle.getIndex()].BackgroundImage = Properties.Resources.Paddle;

                    btnArray[paddle.getIndex()].Tag = "Paddle 1";

                    btnArray[paddle.getIndex() + 1].Tag = "Paddle 2";

                    btnArray[paddle.getIndex() + 2].Tag = "Paddle 3";

                    btnArray[paddle.getIndex() + 3].Tag = "Paddle 4";

                    //btnArray[paddle.getIndex() + 4].BackColor = Color.Black;
                    btnArray[paddle.getIndex() + 4].BackgroundImage = Properties.Resources.Paddle;
                    btnArray[paddle.getIndex() + 4].Tag = "";
                }
                else if (paddle.getNextMove() == 1) {
                    //btnArray[paddle.getIndex() - 1].BackColor = Color.Black;
                    btnArray[paddle.getIndex() - 1].BackgroundImage = Properties.Resources.Paddle;
                    btnArray[paddle.getIndex() - 1].Tag = "";

                    btnArray[paddle.getIndex()].Tag = "Paddle 1";

                    btnArray[paddle.getIndex() + 1].Tag = "Paddle 2";

                    btnArray[paddle.getIndex() + 2].Tag = "Paddle 3";

                    //btnArray[paddle.getIndex() + 3].BackColor = Color.Red;
                    btnArray[paddle.getIndex() + 3].BackgroundImage = Properties.Resources.Paddle;
                    btnArray[paddle.getIndex() + 3].Tag = "Paddle 4";
                }

                paddle.clear();
            }
        }



        private void Movement_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 'a' && paddle.getIndex() > 209) {
                paddle.queueMove(-1);
            }
            else if (e.KeyChar == 'd' && paddle.getIndex() < 219) {
                paddle.queueMove(1);
            }
            
        }
    }
}