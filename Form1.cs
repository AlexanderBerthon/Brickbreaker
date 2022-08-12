namespace Brickbreaker {

    /*
    need better collision logic
    left and right need redirect, so they don't skip past row 1 //multi directional in same tick
    brick collision needs context. if collision happens and there is a brick to the left
    deflect up right, insead of always back the way it came
    similar to if there was a brick on the right of it, deflect up left

    [1]  [2]
    [3]()
         \
          \
           \

    with current logic, this situation would take out box 1, then deflect backwards
    along the same path the ball came from

    better logic would be to take out cube 3, redirect up to 2, then redirect down left
          /
    [1]  /
       ()

    it is just hard because the ball travels diagonally, so even if visually it makes sense
    to destroy the adjacent tiles, logically it messes up the pathing, and will cause weirdness
    almost like you need to remove the tag from [1] but leave the visual
    and remove the visual from [2] and [3] but keep the tag?
    what happens to the logic in this situation?

    [1]  [2]
    [3]()



    */


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

            //change to [index] [ + 1 ] [ + 2 ] [ + 3 ]
            //have to change all the logic to go with this..
            btnArray[paddle.getIndex()].BackColor = Color.Red;
            btnArray[paddle.getIndex()].Tag = "Paddle 1";
            btnArray[paddle.getIndex() + 1].BackColor = Color.Red;
            btnArray[paddle.getIndex() + 1].Tag = "Paddle 2";
            btnArray[paddle.getIndex() + 2].BackColor = Color.Red;
            btnArray[paddle.getIndex() + 2].Tag = "Paddle 3";
            btnArray[paddle.getIndex() + 3].BackColor = Color.Red;
            btnArray[paddle.getIndex() + 3].Tag = "Paddle 4";

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

            timer.Interval = 150;
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

                //ball movement
                //erase ball display
                btnArray[ball.getIndex()].BackgroundImage = null;
                //update position
                if(btnArray[ball.nextMove()].Tag == "Brick") {
                    //btnArray[ball.nextMove()].BackColor = Color.Black;
                    //btnArray[ball.nextMove()].Tag = "";
                    if (btnArray[ball.getIndex() - 16].BackColor == Color.Black) {
                        btnArray[ball.nextMove()].BackColor = Color.Black;
                        btnArray[ball.nextMove()].Tag = "";
                    }
                    btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                    btnArray[ball.getIndex() - 16].Tag = "";
               
                    ball.topBorderCollision();
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

        private void TimerEventProcessor2(Object anObject, EventArgs eventargs) {
            if (!gameOver) {
                paddle.update();
                if (paddle.getNextMove() == -1) {
                    btnArray[paddle.getIndex()].BackColor = Color.Red;
                    btnArray[paddle.getIndex()].Tag = "Paddle 1";

                    btnArray[paddle.getIndex() + 1].Tag = "Paddle 2";

                    btnArray[paddle.getIndex() + 2].Tag = "Paddle 3";

                    btnArray[paddle.getIndex() + 3].Tag = "Paddle 4";

                    btnArray[paddle.getIndex() + 4].BackColor = Color.Black;
                    btnArray[paddle.getIndex() + 4].Tag = "";
                }
                else if (paddle.getNextMove() == 1) {
                    btnArray[paddle.getIndex() - 1].BackColor = Color.Black;
                    btnArray[paddle.getIndex() - 1].Tag = "";

                    btnArray[paddle.getIndex()].Tag = "Paddle 1";

                    btnArray[paddle.getIndex() + 1].Tag = "Paddle 2";

                    btnArray[paddle.getIndex() + 2].Tag = "Paddle 3";

                    btnArray[paddle.getIndex() + 3].BackColor = Color.Red;
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