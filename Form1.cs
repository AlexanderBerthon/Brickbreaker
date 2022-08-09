namespace Brickbreaker {
    public partial class Form1 : Form {
        //Global Variables :(
        //if I put them into separate classes I don't think I need much here
        int score;
        Random random = new Random();
        Button[] btnArray;
        Boolean gameOver;
        //highscore class variable - NYI
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();


        public Form1() {
            InitializeComponent();
            score = 0;
            gameOver = false;
            btnArray = new Button[256];
            Gameboard.Controls.CopyTo(btnArray, 0);

            timer.Interval = 300;
            timer.Tick += new EventHandler(TimerEventProcessor);

            //start
            timer.Start();
        }

        private void TimerEventProcessor(Object anObject, EventArgs eventargs) {
            //logic here
            if (!gameOver) {
                //logic
            }
            else {
                //gameOver() //display gameover menu / highscore / etc. 
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
