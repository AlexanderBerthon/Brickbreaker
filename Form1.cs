using Microsoft.VisualBasic.ApplicationServices;
using System.Text.RegularExpressions;

/*
Known Issues / Bugs

TODO: 
    - Add more patterns/levels (adds more variety and less chance of getting the same levels back to back)
    - icon looks like a dead battery, really need to redesign it
*/
namespace Brickbreaker {    
    public partial class Form1 : Form {
        int score;
        Random random = new Random();
        Button[] btnArray; 
        Boolean gameOver;              
        Boolean checkCollision;        
        int brickCount;              

        Ball ball;
        Paddle paddle;

        List<int> projectiles;

        Highscore[] highScores;

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
            projectiles = new List<int>();

            checkCollision = false;

            ballTimer = new System.Windows.Forms.Timer();
            paddleTimer = new System.Windows.Forms.Timer();

            //Initialize highscore variables
            string[] inputData;
            highScores = new Highscore[5];

            //create the highscore file if it doesn't exist
            if (!File.Exists("C:\\Users\\" + Environment.UserName + "\\Desktop\\Brickbreaker_Highscores.txt")) {
                string[] temp = { "Jeff 0", "Kenny 0", "Taylor 0", "Alex 0", "Martin 0" };
                File.WriteAllLines("C:\\Users\\" + Environment.UserName + "\\Desktop\\Brickbreaker_Highscores.txt", temp); //creates files and populates with dummy data
            }

            inputData = System.IO.File.ReadAllLines("C:\\Users\\" + Environment.UserName + "\\Desktop\\Brickbreaker_Highscores.txt");

            if (inputData.Length > 0) {
                for (int i = 0; i < inputData.Length; i++) {
                    string[] split = new string[2];
                    split = inputData[i].Split(" ");
                    highScores[i] = new Highscore(split[0], int.Parse(split[1]));
                }
            }

            //create paddle / initial paddle position
            //[index][ += 1 ][ += 2 ]
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
                            paddle.powerUp();
                            powerUpProgress.PerformStep();
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
                            paddle.powerUp();
                            powerUpProgress.PerformStep();
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
                        brickCount--;
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
                        brickCount--;
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
                        brickCount--;
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
                        brickCount--;
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
                        brickCount--;
                        score++;
                    }
                    //left edge paddle collision
                    else if (ball.getTrajectory() == 17 && btnArray[ball.nextMove()].Tag == "Paddle 1") {
                        /*

                           \            ()    
                            ()___   ->    \___            
                        */
                        ball.reverse();
                        paddle.powerUp();
                        powerUpProgress.PerformStep();

                        ballTimer.Interval -= 10; //ball speed increase

                    }
                    //right edge paddle collision
                    else if (ball.getTrajectory() == 15 && btnArray[ball.nextMove()].Tag == "Paddle 3") {
                        /*

                                 /          ()
                            ___()   ->  ___/            
                        */
                        ball.reverse();
                        paddle.powerUp();
                        powerUpProgress.PerformStep();
                        ballTimer.Interval -= 10;//ball speed increase
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
                        paddle.powerUp();
                        powerUpProgress.PerformStep();

                        ballTimer.Interval -= 10; //ball speed increase

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
                displayGameOver(); //display gameover menu / highscore / etc. 
            }
        }

        //paddle movement clock
        private void TimerEventProcessor2(Object anObject, EventArgs eventargs) {
            if (!gameOver) {

                List<int> temp = new List<int>();
                                                  
                //projectile code update here
                for(int i = 0; i<projectiles.Count; i++) {
                    if (btnArray[projectiles[i] - 16].Tag == "Top Border") {
                        //destroy projectile
                        btnArray[projectiles[i]].BackgroundImage = null;
                        btnArray[projectiles[i]].Tag = "";
                    }
                    else if (btnArray[projectiles[i] - 16].Tag == "Brick") {
                        //destroy brick
                        btnArray[projectiles[i] - 16].BackColor = Color.Black;
                        btnArray[projectiles[i] - 16].Tag = "";
                        //destroy projectile
                        btnArray[projectiles[i]].BackgroundImage = null;
                        btnArray[projectiles[i]].Tag = "";
                        //update variables
                        brickCount--;
                        score++;
                    }
                    else if (btnArray[projectiles[i] - 16].Tag == "ball") {
                        //destroy projectile
                        btnArray[projectiles[i]].BackgroundImage = null;
                        btnArray[projectiles[i]].Tag = "";
                    }
                    else {
                        //clear previous projectile position
                        btnArray[projectiles[i]].BackgroundImage = null;
                        btnArray[projectiles[i]].Tag = "";
                        //update projectile position
                        projectiles[i] -= 16;
                        //redraw projectile
                        btnArray[projectiles[i]].BackgroundImage = Properties.Resources.projectile;
                        btnArray[projectiles[i]].Tag = "projectile";

                        //add to temp array or array list
                        temp.Add(projectiles[i]);
                    }
                }
                projectiles = temp;

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

        /// function for player/paddle movement
        private void Movement_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 'a' && paddle.getIndex() > 209) {
                paddle.queueMove(-1);
            }
            else if (e.KeyChar == 'd' && paddle.getIndex() < 220) {
                paddle.queueMove(1);
            }
            else if (e.KeyChar == ' ') {
                if (paddle.isPoweredUp()) {
                    projectiles.Add(paddle.getIndex() - 16); //left
                    projectiles.Add(paddle.getIndex() - 15); //middle
                    projectiles.Add(paddle.getIndex() - 14); //right
                    btnArray[paddle.getIndex() - 16].Tag = "projectile";
                    btnArray[paddle.getIndex() - 16].BackgroundImage = Properties.Resources.projectile;
                    btnArray[paddle.getIndex() - 15].Tag = "projectile";
                    btnArray[paddle.getIndex() - 15].BackgroundImage = Properties.Resources.projectile;
                    btnArray[paddle.getIndex() - 14].Tag = "projectile";
                    btnArray[paddle.getIndex() - 14].BackgroundImage = Properties.Resources.projectile;
                    powerUpProgress.Value = 0;
                    paddle.powerDown();
                }
            }
        }

        /// Function to randomly select a level design / brick pattern from a set of designs
        private void brickPatternIni() {
            brickCount = 0;
            int choice = random.Next(0, 11);
            List<int> brickPattern = new List<int>();

            int[] pattern0 = { 19, 20, 23, 24, 27, 28, 35, 36, 39, 40, 43, 44, 51, 52, 55, 56, 59, 60, 67, 68, 71, 72, 75, 76, 83, 84, 87, 88, 91, 92 };
            int[] pattern1 = { 18, 19, 22, 23, 24, 25, 28, 29, 66, 67, 70, 71, 72, 73, 76, 77, 82, 83, 86, 87, 88, 89, 92, 93 };
            int[] pattern2 = { 20, 27, 35, 36, 37, 42, 43, 44, 50, 51, 52, 53, 54, 57, 58, 59, 60, 61, 67, 68, 69, 74, 75, 76, 84, 91 };
            int[] pattern3 = { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94};
            int[] pattern4 = { 18, 20, 21, 23, 24, 26, 27, 29, 33, 35, 38, 41, 44, 46, 49, 51, 54, 57, 60, 62, 65, 67, 70, 73, 76, 78, 82, 84, 85, 87, 88, 90, 91, 93 };
            int[] pattern5 = { 19, 20, 27, 28, 37, 38, 41, 42, 55, 56, 69, 70, 73, 74, 83, 84, 91, 92 };
            int[] pattern6 = { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 50, 52, 54, 57, 59, 61, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94 };
            int[] pattern7 = { 17, 18, 19, 20, 21, 26, 27, 28, 29, 30, 33, 34, 35, 36, 43, 44, 45, 46, 49, 50, 51, 60, 61, 62, 65, 66, 77, 78, 81, 94 };
            int[] pattern8 = { 22, 23, 24, 25, 37, 38, 39, 40, 41, 42, 52, 53, 54, 55, 56, 57, 58, 59, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93 };
            int[] pattern9 = { 38, 39, 40, 41, 54, 55, 56, 57, 70, 71, 72, 73, 86, 87, 88, 89 };
            int[] pattern10 = { 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94 };


            switch (choice) {
                case 0:
                    brickPattern.AddRange(pattern0);
                    break;
                case 1:
                    brickPattern.AddRange(pattern1);
                    break;
                case 2:
                    brickPattern.AddRange(pattern2);
                    break;
                case 3:
                    brickPattern.AddRange(pattern3);
                    break;
                case 4:
                    brickPattern.AddRange(pattern4);
                    break;
                case 5:
                    brickPattern.AddRange(pattern5);
                    break;
                case 6:
                    brickPattern.AddRange(pattern6);
                    break;
                case 7:
                    brickPattern.AddRange(pattern7);
                    break;
                case 8:
                    brickPattern.AddRange(pattern8);
                    break;
                case 9:
                    brickPattern.AddRange(pattern9);
                    break;
                case 10:
                    brickPattern.AddRange(pattern10);
                    break;
            }

            Color randomColor = new Color();
            switch (random.Next(0, 10)) {
                case 0:
                    randomColor = Color.SaddleBrown;
                    break;
                case 1:
                    randomColor = Color.Gold;
                    break;
                case 2:
                    randomColor = Color.Green;
                    break;
                case 3:
                    randomColor = Color.Aqua;
                    break;
                case 4:
                    randomColor = Color.Plum;
                    break;
                case 5:
                    randomColor = Color.DeepPink;
                    break;
                case 6:
                    randomColor = Color.Navy;
                    break;
                case 7:
                    randomColor = Color.White;
                    break;
                case 8:
                    randomColor = Color.Olive;
                    break;
                case 9:
                    randomColor = Color.DarkViolet;
                    break;
            }

            for (int i = 0; i < brickPattern.Count(); i++) {
                btnArray[brickPattern[i]].Tag = "Brick";
                btnArray[brickPattern[i]].BackColor = randomColor;
                brickCount++;
            }
        }

        /// Loads the next level upon successful completion of the previous level
        private void nextLevel() {
            ballTimer.Stop();
            paddleTimer.Stop();

            foreach (Button btn in btnArray) {
                if (btn.Tag != "Left Border" && btn.Tag != "Right Border" && btn.Tag != "Top Border" && btn.Tag != "Bottom Border") {
                    btn.Tag = "";
                    btn.BackgroundImage = null;
                    btn.BackColor = Color.Black;
                }
            }

            ballTimer.Interval = 300 - score;
            random = new Random();
            int initialPos = random.Next(210, 219);
            paddle = new Paddle(initialPos);
            ball = new Ball(initialPos - 15);
            projectiles = new List<int>();
            powerUpProgress.Value = 0;
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

        /// Displays the game over UI
        /// If a new highscore is achieved display new highscore UI menu first
        /// Disply highscores and buttons to try again or exit
        private void displayGameOver() {
            ballTimer.Stop();
            paddleTimer.Stop();

            Boolean newHighScore = false;

            //check for new highscore
            for (int i = 0; i < 5; i++) {
                if (score >= highScores[i].getScore()) {
                    newHighScore = true;
                }
            }

            if (newHighScore) {
                //display new highscore UI
                newHighScorePanel.Visible = true;
            }
            else {
                //populate highscore board
                highscoreName1.Text = highScores[0].getName();
                highscoreName2.Text = highScores[1].getName();
                highscoreName3.Text = highScores[2].getName();
                highscoreName4.Text = highScores[3].getName();
                highscoreName5.Text = highScores[4].getName();
                highscore1.Text = highScores[0].getScore().ToString();
                highscore2.Text = highScores[1].getScore().ToString();
                highscore3.Text = highScores[2].getScore().ToString();
                highscore4.Text = highScores[3].getScore().ToString();
                highscore5.Text = highScores[4].getScore().ToString();

                //display gameover UI
                highscorePanel.Visible = true;
                playAgainLabel.Visible = true;
                continueButton.Visible = true;
                exitButton.Visible = true;

                //hide poweup UI
                powerUpProgress.Visible = false;
                powerUpProgress.Value = 0;
            }
        }

        /// Restarts the game / new game
        private void restart() {
            ballTimer.Stop();
            paddleTimer.Stop();

            //turn off UI elements
            highscorePanel.Visible = false;
            playAgainLabel.Visible = false;
            continueButton.Visible = false;
            exitButton.Visible = false;

            //turn on UI elements
            powerUpProgress.Visible = true;

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
            paddle = new Paddle(initialPos);
            ball = new Ball(initialPos - 15);
            projectiles = new List<int>();
            powerUpProgress.Value = 0;
            btnArray[paddle.getIndex()].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex()].Tag = "Paddle 1";
            btnArray[paddle.getIndex() + 1].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 1].Tag = "Paddle 2";
            btnArray[paddle.getIndex() + 2].BackgroundImage = Properties.Resources.Paddle;
            btnArray[paddle.getIndex() + 2].Tag = "Paddle 3";
            brickPatternIni();
            gameOver = false;
            ballTimer.Interval = 300;
            ballTimer.Start();
            paddleTimer.Start();
        }

        private void exitButton_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void continueButton_Click(object sender, EventArgs e) {
            restart();
        }

        /// Helper function to update highscore sheet
        /// error checking on user input to ensure proper format
        private void confirmUserInputButton_Click(object sender, EventArgs e) {
            String userInput = "";
            Regex regex = new Regex("[0-9]");
            if (newHighScoreTextbox.Text != null) {
                userInput = newHighScoreTextbox.Text;

                if (regex.IsMatch(userInput)) {
                    userInputErrorLabel.Text = "Error: no numbers allowed";
                    userInputErrorLabel.Visible = true;
                }
                else if (userInput.Contains(" ")) {
                    userInputErrorLabel.Text = "Error: no spaces allowed";
                    userInputErrorLabel.Visible = true;
                }
                else if (userInput.Length < 1) {
                    userInputErrorLabel.Text = "Error: please enter a name";
                    userInputErrorLabel.Visible = true;
                }
                else {
                    //add new highscore to list
                    highScores[4] = new Highscore(newHighScoreTextbox.Text, score);

                    Array.Sort(highScores, Highscore.SortScoreAcending());

                    //close new highscore menu
                    newHighScorePanel.Visible = false;

                    //populate highscore board
                    highscoreName1.Text = highScores[0].getName();
                    highscoreName2.Text = highScores[1].getName();
                    highscoreName3.Text = highScores[2].getName();
                    highscoreName4.Text = highScores[3].getName();
                    highscoreName5.Text = highScores[4].getName();
                    highscore1.Text = highScores[0].getScore().ToString();
                    highscore2.Text = highScores[1].getScore().ToString();
                    highscore3.Text = highScores[2].getScore().ToString();
                    highscore4.Text = highScores[3].getScore().ToString();
                    highscore5.Text = highScores[4].getScore().ToString();

                    //display highscore board
                    highscorePanel.Visible = true;
                    continueButton.Visible = true;
                    exitButton.Visible = true;
                    playAgainLabel.Visible = true;

                    //hide powerUp UI
                    powerUpProgress.Visible = false;
                    powerUpProgress.Value = 0;

                    String[] temp = new string[5];

                    //write to file
                    for (int i = 0; i < 5; i++) {
                        temp[i] = highScores[i].getName() + " " + highScores[i].getScore().ToString();
                    }

                    File.WriteAllLines("C:\\Users\\" + Environment.UserName + "\\Desktop\\Brickbreaker_Highscores.txt", temp);
                }
            }
        }

        /// Helper function that clears error message upon user interaction on text box
        /// Prevents a permanent error message showing , makes it more clear that format is incorrect on multiple user attempts at adding a new highscore
        private void NewHighScoreTextBox_TextChanged(object sender, EventArgs e) {
            userInputErrorLabel.Visible = false;
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

{17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 50, 52, 54, 57, 59, 61, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94}
 
Version Eight
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16 17 18 19 20 21             26 27 28 29 30 31
32 33 34 35 36                   43 44 45 46 47
48 49 50 51                         60 61 62 63
64 65 66                               77 78 79
80 81                                     94 95

{17, 18, 19, 20, 21, 26, 27, 28, 29, 30, 33, 34, 35, 36, 43, 44, 45, 46, 49, 50, 51, 60, 61, 62, 65, 66, 77, 78, 81, 94}

Version Nine
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16                22 23 24 25                31
32             37 38 39 40 41 42             47
48          52 53 54 55 56 57 58 59          63
64       67 68 69 70 71 72 73 74 75 76       79
80    82 83 84 85 86 87 88 89 90 91 92 93    95

{22, 23, 24, 25, 37, 38, 39, 40, 41, 42, 52, 53, 54, 55, 56, 57, 58, 59, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93}

Verson Ten
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16                                           31
32                38 39 40 41                47
48                54 55 56 57                63
64                70 71 72 73                79
80                86 87 88 89                95
 
{38, 39, 40, 41, 54, 55, 56, 57, 70, 71, 72, 73, 86, 87, 88, 89}

Version Eleven
00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15
16                                           31
32                                           47
48 49 50 51 52 53 54 55 56 57 58 59 60 61 62 63
64 65 66 67 68 69 70 71 72 73 74 75 76 77 78 79
80 81 82 83 84 85 86 87 88 89 90 91 92 93 94 95

{49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94}
*/

