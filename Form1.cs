namespace Brickbreaker {
    /// <summary>
    /// 
    /// game can be a little predictable and cheeseable by abusing left/right paddle collision and predictable movement
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
    /// 
    /// should everything be based on the balls next move?
    /// I feel like i am duplicating a lot of code.
    /// 
    /// Still testing skip / pauses where the ball collides and changes directions multiple times in the same tick. Should I skip a tick to convey multiple direction changes?
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
            //[index] [ += 1 ] [ += 2 ]
            btnArray[paddle.getIndex()].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex()].Tag = "Paddle 1";
            btnArray[paddle.getIndex() + 1].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 1].Tag = "Paddle 2";
            btnArray[paddle.getIndex() + 2].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 2].Tag = "Paddle 3";

            //print bricks test
            int[] brickIni = {19, 20, 23, 24, 27, 28, 35, 36, 39, 40, 43, 44, 51, 52, 55, 56, 59, 60,
                  67, 68, 71, 72, 75, 76, 83, 84, 87, 88, 91, 92};


            brickPatternIni();
           
            /*
            //print bricks
            for (int i = 17; i < 79; i++) {
                if (i != 31 && i != 32 && i != 47 && i != 48 && i != 63 && i != 64) {
                    int j = random.Next(0, 6);
                    if (j >3) {
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
            }
            */

            //speed
            timer.Interval = 200;
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
                    if (btnArray[ball.getIndex() + 16].Tag == "Paddle 1" || btnArray[ball.getIndex() + 17].Tag == "Paddle 1") {
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
                        score++;
                    }
                    else if (btnArray[ball.getIndex() + 16].Tag == "Brick") {
                        /*
                           |  /        | ()
                           |()     ->  |/
                           |[]         |
                        */
                        btnArray[ball.getIndex() + 16].Tag = "";
                        btnArray[ball.getIndex() + 16].BackColor = Color.Black;
                        ball.brickCollision();
                        score++;
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
                        score++;
                    }
                    else if (btnArray[ball.getIndex() + 17].Tag == "Brick") {
                        /*
                           |  /         | ()   
                           |()      ->  |/
                           |  []        |
                        */
                        btnArray[ball.getIndex() + 17].Tag = "";
                        btnArray[ball.getIndex() + 17].BackColor = Color.Black;
                        ball.brickCollision();
                        score++;
                    }
                    else {
                        /*
                        |            |  ()            |  /         |
                        |()    ->    | /      ||      |()    ->    | \
                        |  \         |                |            |  ()
                        */
                        ball.leftBorderCollision();
                    }
                }
                //logic for right border collisions
                else if (btnArray[ball.nextMove()].Tag == "Right Border") {
                    if(btnArray[ball.getIndex() + 16].Tag == "Paddle 3" || btnArray[ball.getIndex() + 15].Tag == "Paddle 3") {
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
                        score++;
                    }
                    else if (btnArray[ball.getIndex() + 16].Tag == "Brick") {
                        /*
                           \  |         ()  |    
                            ()|  ->       \ |
                        [][][]|       [][]  |
                        */
                        btnArray[ball.getIndex() + 16].Tag = "";
                        btnArray[ball.getIndex() + 16].BackColor = Color.Black;
                        ball.brickCollision();
                        score++;
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
                        score++;
                    }
                    else if (btnArray[ball.getIndex() + 15].Tag == "Brick") {
                        /*
                           \  |         ()  |    
                            ()|  ->       \ |
                        [][]  |       []    |
                        */
                        btnArray[ball.getIndex() + 15].Tag = "";
                        btnArray[ball.getIndex() + 15].BackColor = Color.Black;
                        ball.brickCollision();
                        score++;
                    }
                    else {
                        /*
                            |          ()  |            \  |              |
                          ()|   ->       \ |     ||      ()|     ->     / |  
                         /  |              |               |          ()  |
                        */
                        ball.rightBorderCollision();
                    }
                }
                //up right brick collision
                else if (ball.getTrajectory() == -15 && btnArray[ball.getIndex() + 1].Tag == "Brick") {
                    btnArray[ball.getIndex() + 1].BackColor = Color.Black;
                    btnArray[ball.getIndex() + 1].Tag = "";
                    if (btnArray[ball.getIndex() - 16].Tag == "Brick") {
                        /*
                         [][][]          [][]
                         ()[][]  ->     /  []
                        /             ()
                        */
                        ball.brickCollision();
                        btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                        btnArray[ball.getIndex() - 16].Tag = "";
                        score++;
                    }
                    else if (btnArray[ball.getIndex() - 17].Tag == "Brick") {
                        /*
                          []    []            []
                            ()[][]  ->     /  []
                           /             ()
                        */
                        btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                        btnArray[ball.getIndex() - 16].Tag = "";
                        ball.brickCollision();
                        score++;
                    }
                    else {//TODO:Fix diagram if testing goes well
                        /*
                                []            []
                            ()[][]  ->     /  []
                           /             ()
                        */
                        if(btnArray[ball.nextMove()].Tag == "Top Border") {
                            ball.brickCollision();
                        }
                        else {
                            ball.rightBorderCollision();
                        }
                    }
                }
                //up left brick collision
                else if (ball.getTrajectory() == -17 && btnArray[ball.getIndex() - 1].Tag == "Brick") {
                    btnArray[ball.getIndex() - 1].BackColor = Color.Black;
                    btnArray[ball.getIndex() - 1].Tag = "";
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
                    else if (btnArray[ball.getIndex() - 15].Tag == "Brick") {
                        /*
                          []    []        []    
                          [][]()     ->   []  \
                                \              ()
                        */
                        ball.brickCollision();
                        btnArray[ball.getIndex() - 15].BackColor = Color.Black;
                        btnArray[ball.getIndex() - 15].Tag = "";
                    }
                    else {//TODO:Fix diagram if testing goes well
                        /*
                        []            []
                        [][]()    ->  []   \
                              \             ()
                        */
                        if(btnArray[ball.nextMove()].Tag == "Top Border") {
                            ball.brickCollision();
                        }
                        else {
                            ball.leftBorderCollision();
                        }
                    }
                }
                //single center brick collision (only one current use case, but keep for future needs)
                //also note there is no logic for straight down collision either, must implement if that functionallity is ever needed
                else if (btnArray[ball.getIndex() - 16].Tag == "Brick") {
                    /*
                        [][][]         []  []           []  []
                          ()    -/>      /       -/>       \
                          ||           ()                   ()
                    */
                    btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                    btnArray[ball.getIndex() - 16].Tag = "";
                    ball.topBorderCollision();
                    score++;
                }

                //down left brick collision
                else if (ball.getTrajectory() == + 15 && btnArray[ball.getIndex() - 1].Tag == "Brick") {
                    btnArray[ball.getIndex() - 1].BackColor = Color.Black;
                    btnArray[ball.getIndex() - 1].Tag = "";
                    if (btnArray[ball.getIndex() + 16].Tag == "Brick") {
                        /*
                        []  /         []  ()
                        []()      ->     /
                          []            
                        */
                        ball.brickCollision();
                        btnArray[ball.getIndex() + 16].BackColor = Color.Black;
                        btnArray[ball.getIndex() + 16].Tag = "";
                        score++;
                    }
                    else if (btnArray[ball.getIndex() + 17].Tag == "Brick") {
                        /*
                        []  /         []  ()
                        []()      ->     /
                            []            
                        */
                        ball.brickCollision();
                        btnArray[ball.getIndex() + 17].BackColor = Color.Black;
                        btnArray[ball.getIndex() + 17].Tag = "";
                        score++;
                    }
                    else {
                        /*
                        []  /         []  
                        []()      ->     \
                                          ()
                        */
                        btnArray[ball.getIndex() - 1].BackColor = Color.Black;
                        btnArray[ball.getIndex() - 1].Tag = "";
                        ball.leftBorderCollision();
                        score++;
                    }
                }
               //down right brick collision
                else if (ball.getTrajectory() == + 17 && btnArray[ball.getIndex() + 1].Tag == "Brick") {
                    btnArray[ball.getIndex() + 1].BackColor = Color.Black;
                    btnArray[ball.getIndex() + 1].Tag = "";
                    if (btnArray[ball.getIndex() + 16].Tag == "Brick") {
                        /*
                           \  []          ()  [] 
                            ()[]      ->    \ 
                            []            
                        */
                        ball.brickCollision();
                        btnArray[ball.getIndex() + 16].BackColor = Color.Black;
                        btnArray[ball.getIndex() + 16].Tag = "";
                        score++;
                    }
                    else if (btnArray[ball.getIndex() + 15].Tag == "Brick") {
                        /*
                           \  []          ()  [] 
                            ()[]      ->    \ 
                          []            
                        */
                        ball.brickCollision();
                        btnArray[ball.getIndex() + 15].BackColor = Color.Black;
                        btnArray[ball.getIndex() + 15].Tag = "";
                        score++;
                    }
                    else {
                        /*
                             \  []              [] 
                              ()[]      ->     / 
                                             ()
                        */
                        btnArray[ball.getIndex() + 1].BackColor = Color.Black;
                        btnArray[ball.getIndex() + 1].Tag = "";
                        ball.rightBorderCollision();
                        score++;
                    }
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
                    score++;
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
                           \            ()
                            ()   ->    / 
                            ---      ___
                    */
                    ball.paddleCollision();
                }
                else if (btnArray[ball.nextMove()].Tag == "Top Border") {
                    /*
                        -------      --------
                         ()      ->       \ 
                        /                  ()
                    */
                    ball.topBorderCollision();
                    if(btnArray[ball.nextMove()].Tag == "Brick") {
                        /*
                        --------         --------          ---------
                          ()  []   -->     ()  []    -->       /  []
                         /  []                               ()    
                        */
                        if (ball.getTrajectory() == 15) {
                            btnArray[ball.nextMove()].Tag = "";
                            btnArray[ball.nextMove()].BackColor = Color.Black;
                            score++;
                            skip = true;
                            ball.leftBorderCollision();
                        }
                        else if(ball.getTrajectory() == 17) {
                            btnArray[ball.nextMove()].Tag = "";
                            btnArray[ball.nextMove()].BackColor = Color.Black;
                            score++;
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
                }
                else {
                    skip = false;
                }
                btnArray[ball.getIndex()].BackgroundImage = Properties.Resources.orb; //redraw ball
                ScoreLabel.Text = ""+score;
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

        private void brickPatternIni() {
            int choice = random.Next(0, 8);
            int brickPattern;
            
            switch (choice) {
                case 0:
                    brickPattern[] = {19, 20, 23, 24, 27, 28, 35, 36, 39, 40, 43, 44, 51, 52, 55, 56, 59, 60, 67, 68, 71, 72, 75, 76, 83, 84, 87, 88, 91, 92};
                    break;
                case 1:
                    int[] brickPattern = { 19, 20, 23, 24, 27, 28, 35, 36, 39, 40, 43, 44, 51, 52, 55, 56, 59, 60, 67, 68, 71, 72, 75, 76, 83, 84, 87, 88, 91, 92 };
                    break;
                case 2:
                    int[] brickPattern = { 19, 20, 23, 24, 27, 28, 35, 36, 39, 40, 43, 44, 51, 52, 55, 56, 59, 60, 67, 68, 71, 72, 75, 76, 83, 84, 87, 88, 91, 92 };
                    break;
                case 3:
                    int[] brickPattern = { 19, 20, 23, 24, 27, 28, 35, 36, 39, 40, 43, 44, 51, 52, 55, 56, 59, 60, 67, 68, 71, 72, 75, 76, 83, 84, 87, 88, 91, 92 };
                    break;
                case 4:
                    int[] brickPattern = { 19, 20, 23, 24, 27, 28, 35, 36, 39, 40, 43, 44, 51, 52, 55, 56, 59, 60, 67, 68, 71, 72, 75, 76, 83, 84, 87, 88, 91, 92 };
                    break;
                case 5:
                    int[] brickPattern = { 19, 20, 23, 24, 27, 28, 35, 36, 39, 40, 43, 44, 51, 52, 55, 56, 59, 60, 67, 68, 71, 72, 75, 76, 83, 84, 87, 88, 91, 92 };
                    break;
                case 6:
                    int[] brickPattern = { 19, 20, 23, 24, 27, 28, 35, 36, 39, 40, 43, 44, 51, 52, 55, 56, 59, 60, 67, 68, 71, 72, 75, 76, 83, 84, 87, 88, 91, 92 };
                    break;
                case 7:
                    int[] brickPattern = { 19, 20, 23, 24, 27, 28, 35, 36, 39, 40, 43, 44, 51, 52, 55, 56, 59, 60, 67, 68, 71, 72, 75, 76, 83, 84, 87, 88, 91, 92 };
                    break;
            }


            for (int i = 0; i < brickPattern.Count(); i++) {
                btnArray[brickPattern[i]].Tag = "Brick";
                btnArray[brickPattern[i]].BackColor = Color.OldLace;
            }
        }

    }
}


/*
BRICK PATTERNS

Base grid
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31
32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47
48 49 50 51 52 53 54 55 56 57 58 59 60 61 62 63
64 65 66 67 68 69 70 71 72 73 74 75 76 77 78 79
80 81 82 83 84 85 86 87 88 89 90 91 92 93 94 95

Version One
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16       19 20       23 24       27 28       31
32       35 36       39 40       43 44       47
48       51 52       55 56       59 60       63
64       67 68       71 72       75 76       79
80       83 84       87 88       91 92       95

int[] brickIni = {19, 20, 23, 24, 27, 28, 35, 36, 39, 40, 43, 44, 51, 52, 55, 56, 59, 60
                  67, 68, 71, 72, 75, 76, 83, 84, 87, 88, 91, 92}

Version Two
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16    18 19       22 23 24 25       28 29    31
32    34 35       38 39 40 41       44 45    47
48                                           63
64    66 67       70 71 72 73       76 77    79
80    82 83       86 87 88 89       92 93    95

{18, 19, 22, 23, 24, 25, 28, 29, 66, 67, 70, 71, 72, 73, 76, 77, 82, 83, 86, 87, 88, 89, 92, 93}

Version Three
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16          20                   27          31
32       35 36 37             42 43 44       47
48    50 51 52 53 54       57 58 59 60 61    63
64       67 68 69             74 75 76       79
80          84                   91          95

{20, 27, 35, 36, 37, 42, 43, 44, 50, 51, 52, 53, 54, 57, 58, 59, 60, 61, 67, 68, 69, 74, 75, 76, 84, 91}

Version Four
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31
32                                           47
48                                           63
64                                           79
80 81 82 83 84 85 86 87 88 89 90 91 92 93 94 95

{17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94}

Version Five
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16    18    20    22    24    26    28       31
32       35       38       41       44       47
48       51       54       57       60       63
64       67       70       73       76       79
80       83    85    87    89    91    93    95

{18, 20, 22, 24, 26, 28, 35, 38, 41, 44, 51, 54, 57, 60, 67, 70, 76, 76, 83, 85, 87, 89, 91, 93}

Version Six
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16       19 20       23 24       27 28       31
32 33 34       37 38       41 42       45 46 47
48       51 52       55 56       59 60       63
64 65 66       69 70       73 74       77 78 79
80       83 84       87 88      91 92        95

{19, 20, 23, 34, 27, 28, 33, 34, 37, 38, 41, 42, 45, 46, 51, 52, 55, 56, 59, 60, 65, 66, 69, 70, 73, 74, 77, 78, 83, 84, 87, 88, 91, 92}

Version Seven
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31
32                                           47
48    50    52    54       57    59    61    63
64                                           79
80 81 82 83 84 85 86 87 88 89 90 91 92 93 94 95

{ 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 50, 52, 54, 57, 59, 61, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94}




*/


