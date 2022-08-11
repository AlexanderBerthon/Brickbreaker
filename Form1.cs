namespace Brickbreaker {
    public partial class Form1 : Form {
        //Global Variables :(
        //if I put them into separate classes I don't think I need much here
        int score;
        Random random = new Random();
        Button[] btnArray;
        Boolean gameOver;

        Ball ball;
        Paddle paddle;

        //highscore class variable - NYI
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public Form1() {
            InitializeComponent();
            score = 0;
            gameOver = false;
            btnArray = new Button[256];
            Gameboard.Controls.CopyTo(btnArray, 0);

            ball = new Ball();
            paddle = new Paddle();
            btnArray[paddle.getIndex()].BackColor = Color.Red;
            btnArray[paddle.getIndex()].Tag = "Center";
            btnArray[paddle.getIndex() - 1].BackColor = Color.Red;
            btnArray[paddle.getIndex() - 1].Tag = "Left";
            btnArray[paddle.getIndex() + 1].BackColor = Color.Red;
            btnArray[paddle.getIndex() + 1].Tag = "Right";

            for (int i = 17; i < 47; i++) {
                if (i != 31 && i != 32) {
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

            timer.Interval = 100;
            timer.Tick += new EventHandler(TimerEventProcessor);

            //start
            timer.Start();
        }

        private void TimerEventProcessor(Object anObject, EventArgs eventargs) {
            if (!gameOver) {
                //logic

                //ball movement
                //erase ball display
                btnArray[ball.getIndex()].BackgroundImage = null;
                //update position
                if(btnArray[ball.nextMove()].Tag == "Brick") {
                    btnArray[ball.nextMove()].BackColor = Color.Black;
                    btnArray[ball.nextMove()].Tag = "";
                    btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                    btnArray[ball.getIndex() - 16].Tag = "";
                    ball.topBorderCollision();
                }
                else if (btnArray[ball.nextMove()].Tag == "Left Border") {
                    ball.leftBorderCollision();
                    if (btnArray[ball.nextMove()].Tag == "Center"){
                        ball.leftPaddleCollision();
                    }
                }
                else if (btnArray[ball.nextMove()].Tag == "Right Border") {
                    ball.rightBorderCollision();
                    if (btnArray[ball.nextMove()].Tag == "Center") {
                        ball.leftPaddleCollision();
                    }
                }
                else if (btnArray[ball.nextMove()].Tag == "Left") {                    
                    ball.leftPaddleCollision();
                }
                else if (btnArray[ball.nextMove()].Tag == "Right") {
                    ball.rightPaddleCollision();
                }
                else if (btnArray[ball.nextMove()].Tag == "Center") {
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
                paddle.update();

                if (paddle.getNextMove() == -1) {
                    btnArray[paddle.getIndex() + 2].BackColor = Color.Black;
                    btnArray[paddle.getIndex() + 2].Tag = "";
                    btnArray[paddle.getIndex() + 1].Tag = "Right";
                    btnArray[paddle.getIndex()].Tag = "Center";
                    btnArray[paddle.getIndex() - 1].Tag = "Left";
                    btnArray[paddle.getIndex() - 1].BackColor = Color.Red;
                }
                else if(paddle.getNextMove() == 1) {
                    btnArray[paddle.getIndex() - 2].BackColor = Color.Black;
                    btnArray[paddle.getIndex() - 2].Tag = "";
                    btnArray[paddle.getIndex() - 1].Tag = "Left";
                    btnArray[paddle.getIndex()].Tag = "Center";
                    btnArray[paddle.getIndex() + 1].Tag = "Right";
                    btnArray[paddle.getIndex() + 1].BackColor = Color.Red;
                }

                paddle.clear();
               
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


        private void Movement_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 'a' && paddle.getIndex() > 210) {
                paddle.queueMove(-1);
            }
            else if (e.KeyChar == 'd' && paddle.getIndex() < 221) {
                paddle.queueMove(1);
            }
            
        }
    }
}
