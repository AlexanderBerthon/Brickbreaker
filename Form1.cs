namespace Brickbreaker {
    /// <summary>
    /// TODO:
    ///     random brick colors? 
    ///     game over menu
    ///     scoreboard
    ///     highscores
    ///     powerup 
    ///         I want each section of the paddle to glow when it is hit
    ///         user presses spacebar to activate all glowing paddle sections to launch a projectile directly
    ///         above it. destroys first brick on contact
    ///         this will make the game more skill based and make the end of each level less annoying
    ///         instead of waiting for the ball to randomly go hit that last piece
    ///         last prio
    ///         not MVP
    /// </summary>
    
    public partial class Form1 : Form {
        //Global Variables :(
        int score;
        Random random = new Random();
        Button[] btnArray; 
        Boolean gameOver;              //used to end the game / load game over menu and final score menu
        Boolean checkCollision;        //used for collision processing
        int brickCount;                //used to track the number of bricks that are active on the board. win condition. when brickCount = 0, go to next stage.

        Ball ball;
        Paddle paddle;

        //highscore class variable - NYI

        System.Windows.Forms.Timer ballTimer;
        System.Windows.Forms.Timer paddleTimer;

        public Form1() {
            InitializeComponent();
            score = 0;
            gameOver = false;
            btnArray = new Button[256];
            Gameboard.Controls.CopyTo(btnArray, 0);
            int initialPos = random.Next(210, 219);
            paddle = new Paddle(initialPos);
            ball = new Ball(initialPos - 15);

            checkCollision = false;

            ballTimer = new System.Windows.Forms.Timer();
            paddleTimer = new System.Windows.Forms.Timer();

            //create paddle / initial paddle position
            //215, 216, 217
            //[index] [ += 1 ] [ += 2 ]
            btnArray[paddle.getIndex()].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex()].Tag = "Paddle 1";
            btnArray[paddle.getIndex() + 1].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 1].Tag = "Paddle 2";
            btnArray[paddle.getIndex() + 2].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 2].Tag = "Paddle 3";

            //print bricks
            brickPatternIni();

            //speed
            ballTimer.Interval = 300;
            ballTimer.Tick += new EventHandler(TimerEventProcessor); //can these be renamed? BallTimerEventProcessor

            paddleTimer.Interval = 200;
            paddleTimer.Tick += new EventHandler(TimerEventProcessor2); //can these be renamed? PaddleTimerEventProcessor

            //start
            ballTimer.Start();
            paddleTimer.Start();
        }

        //ball timer
        private void TimerEventProcessor(Object anObject, EventArgs eventargs) {
            if (!gameOver) {
                do {
                    btnArray[ball.getIndex()].BackgroundImage = null; //clear previous ball location
                    checkCollision = false;
                    //logic for left border collisions
                    if (btnArray[ball.nextMove()].Tag == "Left Border") {
                        if (btnArray[ball.getIndex() + 16].Tag == "Paddle 1" || btnArray[ball.getIndex() + 17].Tag == "Paddle 1") {
                            /*
                            |  /        |  ()
                            |()     ->  | /      
                            |---        |---
                            */
                            ball.reverse();
                        }
                        else {
                            /*
                            |            |  ()            |  /         |
                            |()    ->    | /      or      |()    ->    | \
                            |  \         |                |            |  ()
                            */
                            ball.deflectHorizontal();
                            checkCollision = true; //loop to check for other collisions
                        }
                    }
                    //logic for right border collisions
                    else if (btnArray[ball.nextMove()].Tag == "Right Border") {
                        if (btnArray[ball.getIndex() + 16].Tag == "Paddle 3" || btnArray[ball.getIndex() + 15].Tag == "Paddle 3") {
                            /*
                            \  |      ()  |
                             ()|  ->    \ |     
                            ---|       ---|
                            */
                            ball.reverse();
                        }
                        else {
                            /*
                                   |          ()  |            \  |              |
                                 ()|   ->       \ |     or      ()|     ->     / |  
                                /  |              |               |          ()  |
                            */
                            ball.deflectHorizontal();
                            checkCollision = true;
                        }
                    }
                    //logic for left brick collisions
                    else if (btnArray[ball.getIndex() - 1].Tag == "Brick") {
                        /*
                                             ()                /          
                            []()    ->      /      or      []()    ->      \
                                \                                           ()
                        */
                        btnArray[ball.getIndex() - 1].BackColor = Color.Black;
                        btnArray[ball.getIndex() - 1].Tag = "";
                        ball.deflectHorizontal();
                        checkCollision = true;
                        score++;
                    }
                    //logic for right brick collisions
                    else if (btnArray[ball.getIndex() + 1].Tag == "Brick") {
                        /*
                                               ()               \                   
                                 ()[]   ->       \       or      ()[]     ->     /    
                                /                                              ()   
                        */
                        btnArray[ball.getIndex() + 1].BackColor = Color.Black;
                        btnArray[ball.getIndex() + 1].Tag = "";
                        ball.deflectHorizontal();
                        checkCollision = true;
                        score++;
                    }
                    //logic for top brick collisions
                    else if (btnArray[ball.getIndex() - 16].Tag == "Brick") {
                        /*
                            [][][]         []  []           []  []
                              ()     ->      /       or        \
                                           ()                   ()
                        */
                        btnArray[ball.getIndex() - 16].BackColor = Color.Black;
                        btnArray[ball.getIndex() - 16].Tag = "";
                        ball.deflectVertical();
                        checkCollision = true;
                        score++;
                    }
                    //logic for bottom brick collisions
                    else if (btnArray[ball.getIndex() + 16].Tag == "Brick") {
                        /*
                                              ()           ()  
                              ()     ->      /       or      \
                            [][][]        []  []           []  []
                        */
                        btnArray[ball.getIndex() + 16].BackColor = Color.Black;
                        btnArray[ball.getIndex() + 16].Tag = "";
                        ball.deflectVertical();
                        checkCollision = true;
                        score++;
                    }
                    //diagonal collision ~ logic holds for all 4 directions
                    else if (btnArray[ball.nextMove()].Tag == "Brick") {
                        /*
                           [][]          []  
                           []  ()    ->  []  \
                                 \            ()
                        */
                        btnArray[ball.nextMove()].BackColor = Color.Black;
                        btnArray[ball.nextMove()].Tag = "";
                        ball.reverse();
                        checkCollision = true;
                        score++;
                    }
                    //not sure if I can simplify these..
                    //left edge paddle collision
                    else if (ball.getTrajectory() == 17 && btnArray[ball.nextMove()].Tag == "Paddle 1") {
                        /*

                           \            ()    
                            ()___   ->    \___            
                        */
                        ball.reverse();
                    }
                    //right edge paddle collision
                    else if (ball.getTrajectory() == 15 && btnArray[ball.nextMove()].Tag == "Paddle 3") {
                        /*

                                 /          ()
                            ___()   ->  ___/            
                        */
                        ball.reverse();
                    }
                    //normal paddle collision
                    else if (btnArray[ball.nextMove()].Tag == "Paddle 1" ||
                        btnArray[ball.nextMove()].Tag == "Paddle 2" ||
                        btnArray[ball.nextMove()].Tag == "Paddle 3") {
                        /*
                               \            ()
                                ()   ->    / 
                                ---      ---
                        */
                        ball.deflectVertical();
                    }
                    //logic for top border collisions
                    else if (btnArray[ball.nextMove()].Tag == "Top Border") {
                        if (btnArray[ball.getIndex() - 1].Tag == "Left Border" || btnArray[ball.getIndex() + 1].Tag == "Right Border") {
                            /*
                             ____         ____
                            |()      ->  |\   
                            |  \         | ()

                            */
                            ball.reverse();
                        }
                        else {
                            /*
                            -------      --------
                             ()      ->       \ 
                            /                  ()
                            */
                            ball.deflectVertical();
                        }
                        checkCollision = true;
                    }
                    //bottom border collision / game over
                    else if (btnArray[ball.nextMove()].Tag == "Bottom Border") {
                        checkCollision = false;
                        gameOver = true;
                    }
                    else { }

                    if (brickCount == 0) {
                        nextLevel();
                    }
                } while (checkCollision); //loop to check for multi-collisions
                ball.move();
                btnArray[ball.getIndex()].BackgroundImage = Properties.Resources.orb; //redraw ball
                ScoreLabel.Text = "" + score;
            }
            else {
                displayGameOver() //display gameover menu / highscore / etc. 
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
                paddle.move();
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

        //can this code be improved?
        //it is only run 1 time, and all of these variables are declared in a small scope, so.. it should be ok?
        private void brickPatternIni() {
            int choice = random.Next(0, 7);
            List<int> brickPattern = new List<int>();

            int[] pattern1 = { 19, 20, 23, 24, 27, 28, 35, 36, 39, 40, 43, 44, 51, 52, 55, 56, 59, 60, 67, 68, 71, 72, 75, 76, 83, 84, 87, 88, 91, 92 };
            int[] pattern2 = { 18, 19, 22, 23, 24, 25, 28, 29, 66, 67, 70, 71, 72, 73, 76, 77, 82, 83, 86, 87, 88, 89, 92, 93 };
            int[] pattern3 = { 20, 27, 35, 36, 37, 42, 43, 44, 50, 51, 52, 53, 54, 57, 58, 59, 60, 61, 67, 68, 69, 74, 75, 76, 84, 91 };
            int[] pattern4 = { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94};
            int[] pattern5 = { 18, 20, 21, 23, 24, 26, 27, 29, 33, 35, 38, 41, 44, 46, 49, 51, 54, 57, 60, 62, 65, 67, 70, 73, 76, 78, 82, 84, 85, 87, 88, 90, 91, 93 };
            int[] pattern6 = { 19, 20, 27, 28, 37, 38, 41, 42, 55, 56, 69, 70, 73, 74, 83, 84, 91, 92 };
            int[] pattern7 = { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 50, 52, 54, 57, 59, 61, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94 };

            switch (choice) {
                case 0:
                    brickPattern.AddRange(pattern1);
                    break;
                case 1:
                    brickPattern.AddRange(pattern2);
                    break;
                case 2:
                    brickPattern.AddRange(pattern3);
                    break;
                case 3:
                    brickPattern.AddRange(pattern4);
                    break;
                case 4:
                    brickPattern.AddRange(pattern5);
                    break;
                case 5:
                    brickPattern.AddRange(pattern6);
                    break;
                case 6:
                    brickPattern.AddRange(pattern7);
                    break;
            }

            for (int i = 0; i < brickPattern.Count(); i++) {
                btnArray[brickPattern[i]].Tag = "Brick";
                btnArray[brickPattern[i]].BackColor = Color.DodgerBlue;
                brickCount++;
            }
        }

        private void nextLevel() {
            ballTimer.Stop(); //will this throw an error if the timer is already stopped?
            paddleTimer.Stop(); //will this throw an error if the timer is already stopped?

            foreach (Button btn in btnArray) {
                if (btn.Tag != "Left Border" && btn.Tag != "Right Border" && btn.Tag != "Top Border" && btn.Tag != "Bottom Border") {
                    btn.Tag = "";
                    btn.BackgroundImage = null;
                    btn.BackColor = Color.Black;
                }
            }

            ballTimer.Interval -= 50; //speeds up tick rate for ball movement every time a level is cleared, makes the game progressively harder
            random = new Random();
            int initialPos = random.Next(210, 219);
            paddle = new Paddle(initialPos); //what happens to the original
            ball = new Ball(initialPos - 15); //what happens to the original? 
            btnArray[paddle.getIndex()].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex()].Tag = "Paddle 1";
            btnArray[paddle.getIndex() + 1].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 1].Tag = "Paddle 2";
            btnArray[paddle.getIndex() + 2].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 2].Tag = "Paddle 3";
            brickPatternIni();
            ballTimer.Start();
            paddleTimer.Start();
        }

        private void displayGameOver() {
            ballTimer.Stop();
            paddleTimer.Stop();

            //if highscore, load highscore menu
            //filter user input
            //if good, add to highscore sheet

            //close highscore menu

            //load game over menu
            highscorePanel.Visible = true;
            playAgainLabel.Visible = true;
            continueButton.Visible = true;
            exitButton.Visible = true;

            //pull data / display highscores


            //else

            //load game over menu
            //display highscores
            //button to exit
            //button to restart
            //restart()

        }

        private void restart() {
            ballTimer.Stop(); //will this throw an error if the timer is already stopped?
            paddleTimer.Stop(); //will this throw an error if the timer is already stopped?

            //turn off UI elements
            highscorePanel.Visible = false;
            playAgainLabel.Visible = false;
            continueButton.Visible = false;
            exitButton.Visible = false;

            score = 0;

            foreach (Button btn in btnArray) {
                if (btn.Tag != "Left Border" && btn.Tag != "Right Border" && btn.Tag != "Top Border" && btn.Tag != "Bottom Border") {
                    btn.Tag = "";
                    btn.BackgroundImage = null;
                    btn.BackColor = Color.Black;
                }
            }

            random = new Random();
            int initialPos = random.Next(210, 219);
            paddle = new Paddle(initialPos); //what happens to the original
            ball = new Ball(initialPos - 15); //what happens to the original? 
            btnArray[paddle.getIndex()].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex()].Tag = "Paddle 1";
            btnArray[paddle.getIndex() + 1].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 1].Tag = "Paddle 2";
            btnArray[paddle.getIndex() + 2].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 2].Tag = "Paddle 3";
            brickPatternIni();
            gameOver = false;
            ballTimer.Start();
            paddleTimer.Start();
        }

        private void exitButton_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void continueButton_Click(object sender, EventArgs e) {
            restart();
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
16    18    20 21    23 24    26 27    29    31
32 33    35       38       41       44    46 47
48 49    51       54       57       60    62 63
64 65    67       70       73       76    78 79
80    82    84 85    87 88    90 91    93    95

{18, 20, 21, 23, 24, 26, 27, 29, 33, 35, 38, 41, 44, 46, 49, 51, 54, 57, 60, 62, 65, 67, 70, 73, 76, 78, 82, 84, 85, 87, 88, 90, 91, 93}


Version Six
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16       19 20                   27 28       31
32             37 38       41 42             47
48                   55 56                   63
64             69 70       73 74             79
80       83 84                  91 92        95

{19, 20, 27, 28, 37, 38, 41, 42, 55, 56, 69, 70, 73, 74, 83, 84, 91, 92}

Version Seven
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31
32                                           47
48    50    52    54       57    59    61    63
64                                           79
80 81 82 83 84 85 86 87 88 89 90 91 92 93 94 95

{ 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 50, 52, 54, 57, 59, 61, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94}
 */


