using System;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using System.Collections.Generic;

public enum direction
{
    UP = 0,
    DOWN = 1,
    LEFT = 2,
    RIGHT = 3
};




namespace snake
{
    class Game : Form
    {
        public const int SCREEN_WIDTH = 20;
        public const int SCREEN_HEIGHT = 15;
        public const int SIZE = 50;
        public const int SCOREBAR_HEIGHT = 100;
        public const int SPEED = 75;

        Food f = new Food();
        Snake s = new Snake();
        PictureBox p = new PictureBox();
        PictureBox scoreField = new PictureBox();
        Label scoreTitle = new Label();
        Label scoreValue = new Label();
        Label gameOver = new Label();

        bool gameActive = true;
        System.Timers.Timer t = new System.Timers.Timer(SPEED);

        public int score;


        public void GameTimer(object sender, EventArgs e)
        {

            // Determine if the snake's head is over food.
            if (s.head_x == f.x && s.head_y == f.y)
            {
                Eat();
            }

            // Get the snakes current direction, increment its location, save the
            // tail history and remove the last node in the tail.
            switch (s.dir)
            {
                case direction.UP:
                    s.MoveUp();
                    break;

                case direction.DOWN:
                    s.MoveDown();
                    break;

                case direction.RIGHT:
                    s.MoveRight();
                    break;

                case direction.LEFT:
                    s.MoveLeft();
                    break;
            }

            s.history.Insert(0, (s.head_x, s.head_y));
            s.history.RemoveAt(s.history.Count - 1);

            // Determine if the snake ran into itself
            for (int i = 1; i < s.history.Count; i++)
            {
                if (s.head_x == s.history[i].Item1 && s.head_y == s.history[i].Item2)
                {
                    this.gameActive = false;
                }
            }


            // Refresh the play area
            p.Invalidate();
            scoreField.Invalidate();


            // If the game is lost, stop all movement on the board.
            if (!this.gameActive)
            {
                t.Stop();
            }
            
        }

        public Game()
        {
            // Timer
            t.Enabled = true;
            t.AutoReset = true;
            t.Elapsed += new ElapsedEventHandler(GameTimer);

            // Configure the PictureBox (Game Field)
            p.BackColor = Color.Black;  
            p.Location = new Point(0, SCOREBAR_HEIGHT);
            p.Size = new Size((SCREEN_WIDTH + 1) * SIZE, (SCREEN_HEIGHT + 1) * SIZE);

            // Configure the Score Area
            scoreField.BackColor = Color.FromArgb(0, 25, 35);
            scoreField.Location = new Point(0, 0);
            scoreField.Size = new Size((SCREEN_WIDTH + 1) * SIZE, SCOREBAR_HEIGHT);

            // Set the Score and display the Score label
            this.score = 0;
            scoreTitle.Text = "SCORE";
            scoreTitle.Location = new Point(10, 10);
            scoreTitle.Size = new Size(200, 45);
            scoreTitle.ForeColor = Color.White;
            scoreTitle.Parent = scoreField;
            scoreTitle.Font = new Font("Consolas", 34);

            // Set the score value field below the label
            scoreValue.Text = this.score.ToString();
            scoreValue.Location = new Point(15, 60);
            scoreValue.Size = new Size(200, 30);
            scoreValue.ForeColor = Color.White; 
            scoreValue.Parent = scoreField;
            scoreValue.Font = new Font("Consolas", 24);

            // Configure the Game Over screen
            Size szGameOver = new Size(300, 300);
            gameOver.Text = "GAME\nOVER";
            gameOver.TextAlign = ContentAlignment.MiddleCenter;
            gameOver.Size = szGameOver;
            gameOver.Location = new Point(((SCREEN_WIDTH * SIZE) / 2) - 150, ((SCREEN_HEIGHT * SIZE) / 2) - 150);
            gameOver.ForeColor = Color.White;
            gameOver.BackColor = Color.Transparent;
            gameOver.Parent = p;
            gameOver.Font = new Font("Consolas", 58, FontStyle.Bold);
            gameOver.Visible = false;



            // Set the size and color the window
            this.Size = new Size((SCREEN_WIDTH + 1) * SIZE, (SCREEN_HEIGHT + 1) * SIZE + SCOREBAR_HEIGHT);
            this.BackColor = Color.White;

            // Draw the board
            Controls.Add(p);
            Controls.Add(scoreField);

            // Set the title
            this.Text = "Snake - By: George Kochera";
            

            // Draw the food
            p.Paint += new PaintEventHandler(f.Draw);

            // Draw the snake
            p.Paint += new PaintEventHandler(s.Draw);

            // Prepare the Game Over Text
            p.Paint += new PaintEventHandler(Die);

            // Draw the score
            scoreField.Paint += new PaintEventHandler(this.UpdateScore);

            // Add the key press event handler
            this.KeyDown += new KeyEventHandler(Input);
            this.KeyDown += new KeyEventHandler(Pause);
        }

        public void Eat()
        {
            this.score++;
            s.length++;
            s.history.Add((s.head_x, s.head_y));
            f.NextLocation();
            
        }

        public void Pause(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P)
            {
                t.Enabled = !t.Enabled;
            }
        }

        public void UpdateScore(object sender, PaintEventArgs e)
        {
            scoreValue.Text = this.score.ToString();
        }

        public void Die(object sender, PaintEventArgs e)
        {
            if (!gameActive)
            {
                gameOver.Visible = true;
            }
        }

        public void Input(object sender, KeyEventArgs e)
        {
            // Up Arrow
            if (e.KeyCode == Keys.Up && s.dir != direction.DOWN)
            {
                s.dir = direction.UP;
            }

            // Down Arrow
            else if (e.KeyCode == Keys.Down && s.dir != direction.UP)
            {
                s.dir = direction.DOWN;
            }

            // Left Arrow
            else if (e.KeyCode == Keys.Left && s.dir != direction.RIGHT)
            {
                s.dir = direction.LEFT;
            }

            // Right Arrow
            else if (e.KeyCode == Keys.Right && s.dir != direction.LEFT)
            {
                s.dir = direction.RIGHT;
            }

            // Q key - Quits Game
            else if (e.KeyCode == Keys.Q)
            {
                Application.Exit();
            }

            // N key - Starts New Game
            else if (e.KeyCode == Keys.N)
            {
                s.Reset();
                t.Start();
                gameOver.Visible = false;
                this.gameActive = true;
                this.score = 0;
                f.NextLocation();
            }

        }


        public static void Main()
        {
            Application.Run(new Game());
            
        }
    }

    class Snake
    {
        public int head_x {get; set;}
        public int head_y { get; set; }
        public int length { get; set; }
        public List<(int, int)> history { get; set; }
        public direction dir;


        public Snake()
        {
            this.head_x = ((Game.SCREEN_WIDTH / 2) * Game.SIZE);
            this.head_y = ((Game.SCREEN_HEIGHT / 2) * Game.SIZE);
            this.dir = direction.UP;
            this.length = 1;
            this.history = new List<(int, int)>(1) { (this.head_x, this.head_y) };
        }

        public void Reset()
        {
            this.head_x = ((Game.SCREEN_WIDTH / 2) * Game.SIZE);
            this.head_y = ((Game.SCREEN_HEIGHT / 2) * Game.SIZE);
            this.dir = direction.UP;
            this.length = 1;
            this.history = new List<(int, int)>(1) { (this.head_x, this.head_y) };
        }

        public void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int green = 255;
            Color c = Color.FromArgb(0, green, 0);
            Brush b = new SolidBrush(c);

            foreach ((int, int) d in history)
            {
                g.FillRectangle(b, d.Item1, d.Item2, Game.SIZE, Game.SIZE);
                green -= 10;
                if (green < 20)
                {
                    green = 20;
                }
                c = Color.FromArgb(0, green, 0);
                b = new SolidBrush(c);
            }
        }



        public void MoveRight()
        {
            this.head_x += Game.SIZE;
            if (this.head_x >= Game.SCREEN_WIDTH * Game.SIZE)
            {
                this.head_x = 0;
            }
            this.dir = direction.RIGHT;

        }

        public void MoveLeft()
        {
            this.head_x -= Game.SIZE;
            if (this.head_x < 0)
            {
                this.head_x = Game.SCREEN_WIDTH * Game.SIZE;
            }
            this.dir = direction.LEFT;
        }

        public void MoveUp()
        {
            this.head_y -= Game.SIZE;
            if (this.head_y < 0)
            {
                this.head_y = Game.SCREEN_HEIGHT * Game.SIZE;
            }
            this.dir = direction.UP;
        }

        public void MoveDown()
        {
            this.head_y += Game.SIZE;
            if (this.head_y >= Game.SCREEN_HEIGHT * Game.SIZE)
            {
                this.head_y = 0;
            }

            this.dir = direction.DOWN;
        }

        
    }

    class Food
    {
        public int x { get; set; }
        public int y { get; set; }

        public Random r = new Random();

        public Food()
        {
            this.x = r.Next(0, Game.SCREEN_WIDTH) * Game.SIZE;
            this.y = r.Next(0, Game.SCREEN_HEIGHT) * Game.SIZE;
        }

        public void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush b = new SolidBrush(Color.Red);
            Rectangle rect = new Rectangle(this.x, this.y, Game.SIZE, Game.SIZE);
            g.FillEllipse(b, rect);
        }

        public void NextLocation()
        {
            this.x = r.Next(0, Game.SCREEN_WIDTH) * Game.SIZE;
            this.y = r.Next(0, Game.SCREEN_HEIGHT) * Game.SIZE;
        }


    }
}


