namespace Brickbreaker {
    /// <summary>
    /// 100 bug fixes later there are still issues with collision, left and right borders need to check for
    /// bricks above, below, and side to side. I thought I fixed this issue already?
    /// 
    /// Also, the game seems super predictable and easy now
    /// every bounce rebounds to the same location. I barely have to move the paddle. it just rebounds back
    /// and fourth. over and over again. every 50 collisions I have to move the paddle slightly left or right
    /// then plant again for another 50 moves?
    /// 
    /// the border fix didn't work.
    /// now it changes trajectory up and skips the one above it..
    /// 
    /// ways to make the game more interesting / less predictable
    /// 
    /// 1) space out the blocks, if you allow gaps for the ball to go through, it can lead to less predictable
    /// behavior and chains
    /// 
    /// 2) indestructible blocks, creates some extra bounces / chaining possibilities
    /// 
    /// 3) guards, moving ai paddles that interfere with normal block destruction
    /// 
    /// 4) moving blocks, if all the blocks moved around that would certainly make it more random. even if
    /// you could make a line of blocks that just went left and looped around the other side infinitely.
    /// 
    /// 5) any combination of above
    /// 
    /// 6) powers, after a certain number of paddle hits, either spawn a power up to catch or give the power
    /// direct to the player and have spacebar activate it or something. something that can get those
    /// annoying little pieces that take forever to get naturally. like the top left or right pieces
    /// 
    /// Question:
    /// rather than literally hard coding every single collision into a logic statement
    /// is there a way to write it where the logic holds in a variety of situations without needing to be singled out? 
    /// ... I don't think so
    /// it just seems so.. amature-ish to write the logic this way.
    /// 
    /// TODO: Fix skip logic. when you skip, you still delete the image of the ball, so the skipped frame looks like an animation glitch bc the ball disapears still but isn't 
    /// redrawn
    /// 
    /// </summary>


    public partial class Form1 : Form {
        //Global Variables :(
        int score;
        Random random = new Random();
        Button[] btnArray;
        Boolean gameOver;
        Boolean skip;

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

            //215, 216, 217
            //change to [index] [ + 1 ] [ + 2 ]
            btnArray[paddle.getIndex()].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex()].Tag = "Paddle 1";
            btnArray[paddle.getIndex() + 1].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 1].Tag = "Paddle 2";
            btnArray[paddle.getIndex() + 2].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 2].Tag = "Paddle 3";

            //print bricks
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

            //speed
            timer.Interval = 500;
            timer.Tick += new EventHandler(TimerEventProcessor);

            timer2.Interval = 150;
            timer2.Tick += new EventHandler(TimerEventProcessor2);

            //start
            timer.Start();
            timer2.Start();
        }

        //ball timer
        private void TimerEventProcessor(Object anObject, EventArgs eventargs) {
            if (!gameOver) {
                btnArray[ball.getIndex()].BackgroundImage = null; //clear previous ball location

                //logic for left border collisions
                if (btnArray[ball.nextMove()].Tag == "Left Border") {
                    if (btnArray[ball.getIndex() - 16].Tag == "Paddle 1" || btnArray[ball.getIndex() - 17].Tag == "Paddle 1") {
                        /*
                        |  /        |  ()
                        |()     ->  | /      
                        |---        |---
                        */
                        ball.leftPaddleCollision();
                    }
                    else if (btnArray[ball.getIndex() - 16].Tag == "Brick") {
                        /*
                        |[][][]      |  [][]    
                        |()      ->  | \ 
                        |  \         |  ()
                        */
                        btnArray[ball.getIndex() - 16].Tag = "";
                        btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                        ball.brickCollision();
                    }
                    else if (btnArray[ball.getIndex() - 15].Tag == "Brick") {
                        /*
                        |  [][]      |    []    
                        |()      ->  | \ 
                        |  \         |  ()
                        */
                        btnArray[ball.getIndex() - 15].Tag = "";
                        btnArray[ball.getIndex() - 15].BackColor = Color.Black;
                        ball.brickCollision();
                    }
                    else{
                        /*
                        |            |  ()
                        |()      ->  | /
                        |  \         |
                        */
                        ball.leftBorderCollision();
                    }
                }
                //logic for right border collisions
                else if (btnArray[ball.nextMove()].Tag == "Right Border") {
                    if(btnArray[ball.getIndex() - 16].Tag == "Paddle 3" || btnArray[ball.getIndex() - 15].Tag == "Paddle 3") {
                        /*
                        \  |      ()  |
                         ()|  ->    \ |     
                        ---|       ---|
                        */
                        ball.leftPaddleCollision();
                    }
                    else if (btnArray[ball.getIndex() - 16].Tag == "Brick") {
                        /*
                        [][][]|       [][]  |    
                            ()|  ->       / |
                           /  |         ()  |
                        */
                        btnArray[ball.getIndex() - 16].Tag = "";
                        btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                        ball.brickCollision();
                    }
                    else if (btnArray[ball.getIndex() - 17].Tag == "Brick") {
                        /*
                        [][]  |       []    |    
                            ()|  ->       / |
                           /  |         ()  |
                        */
                        btnArray[ball.getIndex() - 17].Tag = "";
                        btnArray[ball.getIndex() - 17].BackColor = Color.Black;
                        ball.brickCollision();
                    }
                    else {
                        /*
                            |         ()  |
                          ()|  ->       \ |
                         /  |             |
                        */
                        ball.rightBorderCollision();
                    }
                }
                //up right brick collision
                else if (ball.getTrajectory() == -15 && btnArray[ball.getIndex() + 1].Tag == "Brick") {
                    if (btnArray[ball.getIndex() - 16].Tag == "Brick") {
                        /*
                         [][][]          [][]
                         ()[][]  ->     /  []
                        /             ()
                        */
                        ball.brickCollision();
                        btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                        btnArray[ball.getIndex() - 16].Tag = "";
                    }
                    else {//TODO:Fix diagram if testing goes well
                        /*
                                []            []
                            ()[][]  ->     /  []
                           /             ()
                        */
                        btnArray[ball.getIndex() + 1].BackColor = Color.Black;
                        btnArray[ball.getIndex() + 1].Tag = "";
                        //ball.brickCollision();
                        ball.rightBorderCollision(); //testing
                    }
                }
                //up left brick collision
                else if (ball.getTrajectory() == -17 && btnArray[ball.getIndex() - 1].Tag == "Brick") {
                    if (btnArray[ball.getIndex() - 16].Tag == "Brick") {
                        /*                
                        [][][]        [][]
                        [][]()    ->  []  \
                              \            ()
                        */
                        ball.brickCollision();
                        btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                        btnArray[ball.getIndex() - 16].Tag = "";
                    }
                    else {//TODO:Fix diagram if testing goes well
                        /*
                        []            []
                        [][]()    ->  []   \
                              \             ()
                        */
                        btnArray[ball.getIndex() - 1].BackColor = Color.Black;
                        btnArray[ball.getIndex() - 1].Tag = "";
                        //ball.brickCollision();
                        ball.leftBorderCollision(); //testing
                    }
                }
                //single center up brick collision
                else if (btnArray[ball.getIndex() - 16].Tag == "Brick") {
                    /*
                    [][][]        [][]
                    []  ()    ->  []   \
                          \             ()
                    */
                    btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                    btnArray[ball.getIndex() - 16].Tag = "";
                    ball.topBorderCollision();
                }
                //down left brick collision
                else if (ball.getTrajectory() == + 15 && btnArray[ball.getIndex() - 1].Tag == "Brick") {
                    if (btnArray[ball.getIndex() + 16].Tag == "Brick") {
                        /*
                        []  /         []  ()
                        []()      ->     /
                          []            
                        */
                        ball.brickCollision();
                        btnArray[ball.getIndex() + 16].BackColor = Color.Black;
                        btnArray[ball.getIndex() + 16].Tag = "";
                    }
                    else {//TODO:Fix diagram if testing goes well
                        /*
                        []  /         []  ()
                        []()      ->     /
                                  
                        */
                        btnArray[ball.getIndex() - 1].BackColor = Color.Black;
                        btnArray[ball.getIndex() - 1].Tag = "";
                        //ball.brickCollision();
                        ball.leftBorderCollision(); //testing
                    }
                }
               //down right brick collision
                else if (ball.getTrajectory() == + 17 && btnArray[ball.getIndex() + 1].Tag == "Brick") {
                    if (btnArray[ball.getIndex() + 16].Tag == "Brick") {
                        /*
                           \  []          ()  [] 
                            ()[]      ->    \ 
                            []            
                        */
                        ball.brickCollision();
                        btnArray[ball.getIndex() + 16].BackColor = Color.Black;
                        btnArray[ball.getIndex() + 16].Tag = "";
                    }
                    else {//TODO:Fix diagram if testing goes well
                        /*
                             \  []          ()  [] 
                              ()[]      ->    \ 

                        */
                        //TODO: This should probably bounce like a border collision?
                        btnArray[ball.getIndex() + 1].BackColor = Color.Black;
                        btnArray[ball.getIndex() + 1].Tag = "";
                        //ball.brickCollision();
                        ball.rightBorderCollision(); //testing
                    }
                }
                //single center down brick collision
                else if (btnArray[ball.getIndex() + 16].Tag == "Brick") {
                    /*
                           \          ()  
                            ()  ->      \                
                        [][][]      [][]  
                    */
                    btnArray[ball.getIndex() + 16].BackColor = Color.Black;
                    btnArray[ball.getIndex() + 16].Tag = "";
                    ball.bottomBorderCollision(); //TODO: rename function
                }
                //diagonal collision ~ logic holds for all 4 diagonal collisions
                else if (btnArray[ball.nextMove()].Tag == "Brick") {
                    /*
                       [][]          []  
                       []  ()    ->  []  \
                             \            ()
                    */
                    btnArray[ball.nextMove()].BackColor = Color.Black;
                    btnArray[ball.nextMove()].Tag = "";
                    ball.brickCollision();
                }
                //left edge paddle collision
                else if (ball.getTrajectory() == 17 && btnArray[ball.nextMove()].Tag == "Paddle 1") { 
                    /*
                    
                       \            ()    
                        ()___   ->    \___            
                    */
                    ball.leftPaddleCollision();
                }
                //right edge paddle collision
                else if (ball.getTrajectory() == 15 && btnArray[ball.nextMove()].Tag == "Paddle 3") {
                    /*
                    
                             /          ()
                        ___()   ->  ___/            
                    */
                    ball.rightPaddleCollision();
                }
                //normal paddle collision
                else if (btnArray[ball.nextMove()].Tag == "Paddle 1" ||
                    btnArray[ball.nextMove()].Tag == "Paddle 2" ||
                    btnArray[ball.nextMove()].Tag == "Paddle 3") {
                    /*
                           \                ()
                            ()   ->        / 
                            ---          ___
                    */
                    ball.paddleCollision();
                }
                /*
                --------         --------          ---------
                  ()  []   -->     ()  []    -->       /  []
                 /  []                               ()    

                */
                else if (btnArray[ball.nextMove()].Tag == "Top Border") {
                    /*
                        -------      --------
                         ()      ->       \ 
                        /                  ()
                    */
                    ball.topBorderCollision();
                    if(btnArray[ball.nextMove()].Tag == "Brick") { //**this logic right here, modify
                        /*
                        --------         --------          ---------
                          ()  []   -->     ()  []    -->       /  []
                         /  []                               ()    

                        */
                        if (ball.getTrajectory() == 15) {
                            btnArray[ball.nextMove()].Tag = "";
                            btnArray[ball.nextMove()].BackColor = Color.Black;
                            skip = true;
                            ball.leftBorderCollision();
                        }
                        else if(ball.getTrajectory() == 17) {
                            btnArray[ball.nextMove()].Tag = "";
                            btnArray[ball.nextMove()].BackColor = Color.Black;
                            skip = true; 
                            ball.rightBorderCollision();
                        }
                    }                  
                }
                //bottom border collision
                else if (btnArray[ball.nextMove()].Tag == "Bottom Border") {
                    skip = true;
                    gameOver = true;
                }
                else { }

                //skip tick
                if (!skip) {
                    ball.update();
                    btnArray[ball.getIndex()].BackgroundImage = Properties.Resources.orb; //redraw ball
                }
                else {
                    skip = false;
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
                if (paddle.getNextMove() == -1) {
                    //new slot
                    btnArray[paddle.getIndex() - 1].BackgroundImage = Properties.Resources.Paddle;
                    btnArray[paddle.getIndex() - 1].Tag = "Paddle 1";
                    //recycled slots
                    btnArray[paddle.getIndex()].Tag = "Paddle 2";
                    btnArray[paddle.getIndex() + 1].Tag = "Paddle 3";
                    //removed slot
                    btnArray[paddle.getIndex() + 2].BackgroundImage = null;
                    btnArray[paddle.getIndex() + 2].Tag = "";
                }
                else if (paddle.getNextMove() == 1) {
                    //removed slot
                    btnArray[paddle.getIndex()].BackgroundImage = null;
                    btnArray[paddle.getIndex()].Tag = "";
                    //recycled slots
                    btnArray[paddle.getIndex() + 1].Tag = "Paddle 1";
                    btnArray[paddle.getIndex() + 2].Tag = "Paddle 2";
                    //new slot
                    btnArray[paddle.getIndex() + 3].BackgroundImage = Properties.Resources.Paddle;
                    btnArray[paddle.getIndex() + 3].Tag = "Paddle 3";
                }
                paddle.update();
                paddle.clear();
            }
        }

        private void Movement_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 'a' && paddle.getIndex() > 209) {
                paddle.queueMove(-1);
            }
            else if (e.KeyChar == 'd' && paddle.getIndex() < 220) {
                paddle.queueMove(1);
            }
            
        }
    }
}