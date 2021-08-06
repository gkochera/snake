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

    class Game: Form
    {
        public const int SCREEN_WIDTH = 20;
        public const int SCREEN_HEIGHT = 20;
        public const int SIZE = 50;
        public const int OFFSET = SIZE / 2;



        Food f = new Food();
        Snake s = new Snake(SCREEN_WIDTH, SCREEN_HEIGHT);
        Random r = new Random();
        public int score = 0;


        public void GameTimer(object sender, EventArgs e)
        {
            if (s.head_x == f.x && s.head_y == f.y)
            {
                Eat();
            }

            this.Invalidate();
            
        }

        public Game()
        {
            // Timer
            System.Timers.Timer t = new System.Timers.Timer(10);
            t.Enabled = true;
            t.AutoReset = true;
            t.Elapsed += new ElapsedEventHandler(GameTimer);

            // Set the size and color the window
            this.Size = new Size((SCREEN_WIDTH + 1) * SIZE, (SCREEN_HEIGHT + 1) * SIZE);
            this.BackColor = Color.Black;

            // Draw the snake
            this.Paint += new PaintEventHandler(s.Draw);

            // Draw the food
            this.Paint += new PaintEventHandler(f.Draw);

            // Add the key press event handler
            this.KeyDown += new KeyEventHandler(s.Move);
        }

        public void Eat()
        {
            score++;
            s.length++;
            s.history.Add(s.dir);
            f.x = r.Next(0, SCREEN_WIDTH) * SIZE;
            f.y = r.Next(0, SCREEN_HEIGHT) * SIZE;
            
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
        public const int SIZE = 50;
        public List<direction> history { get; set; }
        public direction dir;


    public Snake(int GameWidth, int GameHeight)
        {

            // Timer
            System.Timers.Timer advanceTimer = new System.Timers.Timer(100);
            advanceTimer.Enabled = true;
            advanceTimer.AutoReset = true;
            advanceTimer.Elapsed += new ElapsedEventHandler(Advance);

            this.head_x = 100;
            this.head_y = 400;
            this.dir = direction.UP;
            this.length = 1;
            this.history = new List<direction>(1) { direction.UP };





    }

        public void Advance(object sender, ElapsedEventArgs e)
        {
            switch (this.dir)
            {
                case direction.UP:
                    MoveUp();
                    break;

                case direction.DOWN:
                    MoveDown();
                    break;

                case direction.RIGHT:
                    MoveRight();
                    break;

                case direction.LEFT:
                    MoveLeft();
                    break;
            }
        }

        public void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush b = new SolidBrush(Color.Lime);
            int start_x = this.head_x;
            int start_y = this.head_y;

            
            foreach (direction d in history)
            {
                g.FillRectangle(b, start_x, start_y, SIZE, SIZE);

                switch (d)
                {
                    case direction.UP:
                        start_y += (SIZE);
                        break;

                    case direction.DOWN:
                        start_y -= (SIZE);
                        break;

                    case direction.LEFT:
                        start_x += (SIZE);
                        break;

                    case direction.RIGHT:
                        start_x -= (SIZE);
                        break;
                }
            }
                
        }

        public void Move(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                this.dir = direction.UP;
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.dir = direction.DOWN;
            }
            else if (e.KeyCode == Keys.Left)
            {
                this.dir = direction.LEFT;
            }
            else if (e.KeyCode == Keys.Right)
            {
                this.dir = direction.RIGHT;
            }
            else if (e.KeyCode == Keys.Q)
            {
                Application.Exit();
            }
        }

        private void MoveRight()
        {
            this.head_x += (SIZE);
            this.dir = direction.RIGHT;
            this.history.Insert(0, direction.RIGHT);
            this.history.RemoveAt(this.history.Count - 1);
        }

        private void MoveLeft()
        {
            this.head_x -= (SIZE);
            this.dir = direction.LEFT;
            this.history.Insert(0, direction.LEFT);
            this.history.RemoveAt(this.history.Count - 1);
        }

        private void MoveUp()
        {
            this.head_y -= (SIZE);
            this.dir = direction.UP;
            this.history.Insert(0, direction.UP);
            this.history.RemoveAt(this.history.Count - 1);
        }

        private void MoveDown()
        {
            this.head_y += (SIZE);
            this.dir = direction.DOWN;
            this.history.Insert(0, direction.DOWN);
            this.history.RemoveAt(this.history.Count - 1);
        }

        
    }
}

class Food
{
    public int x { get; set; }
    public int y { get; set; }
    public const int SIZE = 50;

    public Food()
    {
        this.x = 200;
        this.y = 400;
    }

    public void Draw(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        Brush b = new SolidBrush(Color.Red);
        g.FillRectangle(b, this.x, this.y, SIZE, SIZE);
    }


}
