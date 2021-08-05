using System;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using System.Collections.Generic;

namespace snake
{

    class Game: Form
    {
        public static int width = 1200;
        public static int height = 800;
        private Font fnt = new Font("Arial", 10);
        PictureBox p = new PictureBox();


        private void Tick(object sender, EventArgs e)
        {
            this.Invalidate();
            
        }

        public Game()
        {
            // Timer
            System.Timers.Timer t = new System.Timers.Timer(10);
            t.Enabled = true;
            t.AutoReset = true;
            t.Elapsed += new ElapsedEventHandler(Tick);

            // Set the size and color the window
            this.Size = new Size(width, height);
            this.BackColor = Color.Black;

            // Add the picture box
            
            p.BackColor = Color.Black;
            p.Dock = DockStyle.Fill;

            Snake s = new Snake(width, height);
            this.Paint += new PaintEventHandler(s.Draw);

            Food f = new Food();
            this.Paint += new PaintEventHandler(f.Draw);

            

            // Add the key press event handler
            this.KeyDown += new KeyEventHandler(s.Move);

        }


        public static void Main()
        {
            Application.Run(new Game());
        }
    }

    class Snake
    {
        public int box_x, box_y, head_x, head_y, length, size;
        
        public direction dir;
        public int padding = 3;
        public enum direction  {
            UP = 0,
            DOWN = 1,
            LEFT = 2,
            RIGHT = 3
        };
        public List<direction> history = new List<direction>();


    public Snake(int GameWidth, int GameHeight)
        {
            // Timer
            System.Timers.Timer advanceTimer = new System.Timers.Timer(500);
            advanceTimer.Enabled = true;
            advanceTimer.AutoReset = true;
            advanceTimer.Elapsed += new ElapsedEventHandler(Advance);

            this.size = 40;
            this.head_x = GameWidth / 2;
            this.head_y = GameHeight / 2;
            this.dir = direction.UP;
            this.length = 8;
            this.history.Add(direction.UP);
            this.history.Add(direction.UP);
            this.history.Add(direction.UP);
            this.history.Add(direction.UP); 
            this.history.Add(direction.UP);
            this.history.Add(direction.UP);
            this.history.Add(direction.UP);
            this.history.Add(direction.UP);




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
                g.FillRectangle(b, start_x, start_y, this.size, this.size);

                switch (d)
                {
                    case direction.UP:
                        start_y += (size + padding);
                        break;

                    case direction.DOWN:
                        start_y -= (size + padding);
                        break;

                    case direction.LEFT:
                        start_x += (size + padding);
                        break;

                    case direction.RIGHT:
                        start_x -= (size + padding);
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
            this.head_x += (40 + padding);
            this.dir = direction.RIGHT;
            this.history.Insert(0, direction.RIGHT);
            this.history.RemoveAt(this.history.Count - 1);
        }

        private void MoveLeft()
        {
            this.head_x -= (40 + padding);
            this.dir = direction.LEFT;
            this.history.Insert(0, direction.LEFT);
            this.history.RemoveAt(this.history.Count - 1);
        }

        private void MoveUp()
        {
            this.head_y -= (40 + padding);
            this.dir = direction.UP;
            this.history.Insert(0, direction.UP);
            this.history.RemoveAt(this.history.Count - 1);
        }

        private void MoveDown()
        {
            this.head_y += (40 + padding);
            this.dir = direction.DOWN;
            this.history.Insert(0, direction.DOWN);
            this.history.RemoveAt(this.history.Count - 1);
        }

        
    }
}

class Food
{
    int x, y, size;
    public Food()
    {
        this.x = 50;
        this.y = 500;
        this.size = 40;
    }

    public void Draw(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        Brush b = new SolidBrush(Color.Red);
        g.FillRectangle(b, this.x, this.y, this.size, this.size);
    }


}
