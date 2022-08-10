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
            btnArray[paddle.getIndex()-1].BackColor = Color.Red;
            btnArray[paddle.getIndex()+1].BackColor = Color.Red;


            timer.Interval = 300;
            timer.Tick += new EventHandler(TimerEventProcessor);

            //start
            timer.Start();
        }

        private void TimerEventProcessor(Object anObject, EventArgs eventargs) {
            //logic here
            if (!gameOver) {
                //logic


                //ball movement
                //erase ball display
                btnArray[ball.getIndex()].BackColor = Color.Black;
                //update position
                if(btnArray[ball.nextMove()].Tag == "Bottom Border") {
                    ball.bottomBorderCollision();
                    //gameOver = true;
                }
                else if(btnArray[ball.nextMove()].Tag == "Left Border") {
                    ball.leftBorderCollision();
                } 
                else if(btnArray[ball.nextMove()].Tag == "Right Border") {
                    ball.rightBorderCollision();
                }
                else if (btnArray[ball.nextMove()].Tag == "Top Border") {
                    ball.topBorderCollision();
                }
                else if(btnArray[ball.nextMove()].BackColor == Color.Red){ // TODO: replace with real paddle color
                    //this might look a little janky, but it makes sense logically
                }
                else { }
                
                ball.update();

                if(paddle.update() > 0){
                    //remove old
                    btnArray[paddle.getIndex() - 2].BackColor = Color.Black;

                    //add new
                    btnArray[paddle.getIndex() + 1].BackColor = Color.Red;
                }
                else {
                    //remove old
                    btnArray[paddle.getIndex() + 2].BackColor = Color.Black;

                    //add new
                    btnArray[paddle.getIndex() - 1].BackColor = Color.Red;
                }
                paddle.clear();

            if (!gameOver) {
                    //redraw ball
                    btnArray[ball.getIndex()].BackColor = Color.Red; // TODO: change to icon
                }

           

            }
            else {
                Application.Exit();
                //gameOver() //display gameover menu / highscore / etc. 
            }

        }

        private void Movement_KeyPress(object sender, KeyPressEventArgs e) {
            btnArray[0].BackColor = Color.Purple;
            if (e.KeyChar == 'a') {
                paddle.queueMove(-1);
            }
            else if (e.KeyChar == 'd') {
                paddle.queueMove(1);
            }
        }

        //Variables
        /*
        Int score
        
        //make this into a ball class? most likely
        //can make methods to change more easily
        Int ballPosition
        Int trajectoryX
        Int trajectoryY

        */

        //pick a random index below half way point (120+ closest row) to spawn the ball

        //randomly generate colored squares across the top 3-5 rows?

        //paddle should spawn in the middle every time, 3 or 5 length 
        //ball hits middle part, goes straight up
        //ball hits left part, goes diagonal up-left
        //ball hits right part, goes diagonal up-right

        //ball hits a wall, reverse x-trajectory but maintain y

        //ball collides with brick or top border, reverse y-trajectory rng x 

        //ball collides with bottom border, end game

        //chain colored bricks together? or pixel by pixel?
        //I think chain could work, but doesn't make sense vertically
        //like hitting a brick on the third row shouldn't ever break a tile on the second
        //row but maybe break 2 bricks on the 3rd row if they are the same color and next to eachother
        //can assign a different color group to each row if necessary and limit the rng
        //red green blue
        //yellow orange purple
        //red green blue
        //etc. 

        //circle icon for the ball
        //bricks and paddle can stay backround colors
    }
}
