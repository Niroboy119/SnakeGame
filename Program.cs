using System;
using static System.Console;
using System.Collections.Generic;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // start game
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            // display this char on the console during the game
            List<string> ch = new List<string>();
            ch.Add("*");
            ch.Add("*");
            ch.Add("*");
            bool gameLive = true;
            ConsoleKeyInfo consoleKey; // holds whatever key is pressed

            var rand = new Random();

            // location info & display
            int x = 3, y = 4; // y is 2 to allow the top row for directions & space
            int dx = 1, dy = 0;
            int consoleWidthLimit = 79;
            int consoleHeightLimit = 24;
            bool isUP = false;
            int score = 0;
            var food = new Food(rand.Next(1, consoleWidthLimit - 2), rand.Next(1, consoleHeightLimit - 2));
            List<int> st = new List<int>();
            st.Add(x);
            st.Add(y);
            st.Add(x + 1);
            st.Add(y);
            st.Add(x + 2);
            st.Add(y);

            // clear to color
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Clear();

            // delay to slow down the character movement so you can see it
            int delayInMillisecs = 50;

            // whether to keep trails
            bool trail = false;

            // keeps track of time passed
            int tickTime = 0;
            

            do // until escape
            {
                // print directions at top, then restore position
                // save then restore current color
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Arrows move up/down/right/left. Press 'esc' quit.");
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = cc;

                // see if a key has been pressed
                if (Console.KeyAvailable)
                {
                    // get key and use it to set options
                    consoleKey = Console.ReadKey(true);
                    switch (consoleKey.Key)
                    {
                      
                        case ConsoleKey.UpArrow: //UP
                            dx = 0;
                            dy = -1;
                            Console.ForegroundColor = ConsoleColor.Red;
                            isUP = true;
                            break;
                        case ConsoleKey.DownArrow: // DOWN
                            dx = 0;
                            dy = 1;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            isUP = true;
                            break;
                        case ConsoleKey.LeftArrow: //LEFT
                            dx = -1;
                            dy = 0;
                            Console.ForegroundColor = ConsoleColor.Green;
                            isUP = false;
                            break;
                        case ConsoleKey.RightArrow: //RIGHT
                            dx = 1;
                            dy = 0;
                            Console.ForegroundColor = ConsoleColor.Black;
                            isUP = false;
                            break;
                        case ConsoleKey.Escape: //END
                            gameLive = false;
                            break;
                    }
                }

                // find the current position in the console grid & erase the character there if don't want to see the trail
                Console.SetCursorPosition(x, y);
                if (trail == false)
                {

                    for (int i = 0; i < st.Count / 2; i++)
                    {
                        Console.SetCursorPosition(st[i * 2], st[(2 * i) + 1]);
                        Console.Write(" ");
                    }

                }

                // calculate the new position
                // note x set to 0 because we use the whole width, but y set to 1 because we use top row for instructions
                x += dx;
                if (x > consoleWidthLimit)
                    x = 0;
                if (x < 0)
                    x = consoleWidthLimit;

                y += dy;
                if (y > consoleHeightLimit)
                    y = 2; // 2 due to top spaces used for directions
                if (y < 2)
                    y = consoleHeightLimit;

                // write the character in the new position
                Console.SetCursorPosition(x, y);
                st[0] = x;
                st[1] = y;

                Console.Write(ch[0]);
                for (int i = 1; i < ch.Count; i++)
                {
                    if (isUP == true)
                    {
                        Console.SetCursorPosition(x, y + i);
                        st[2 * i] = x;
                        st[(2 * i) + 1] = y + i;
                        Console.Write(ch[i]);
                    }
                    else if (isUP == false)
                    {
                        Console.SetCursorPosition(x + i, y);
                        st[2 * i] = x + i;
                        st[(2 * i) + 1] = y;
                        Console.Write(ch[i]);
                    }

                }

                // increments the time passed
                tickTime += 1;

                if (tickTime >= 50)
                {
                    // lock on to current position of food
                    SetCursorPosition(food.XPos, food.YPos);
                    // erase the current position of food (to not form a trail)
                    if (trail == false)
                    {
                        Console.Write(' ');
                    }
                    // draw a new food at a random location on the screen
                    food = DrawFood(food, rand, consoleWidthLimit, consoleHeightLimit);

                    // set timer back to zero
                    tickTime = 0;
                }else if((st[0]==food.XPos&&st[1]==food.YPos)|| (st[st.Count-2] == food.XPos && st[st.Count-1] == food.YPos))
                {
                    score += 1;
                    ch.Add("*");
                    st.Add(x + ch.Count);
                    st.Add(y);
                    food = DrawFood(food, rand, consoleWidthLimit, consoleHeightLimit);
                }

                SetCursorPosition(110, 0);
                Console.WriteLine("SCORE: " + score);
                // pause to allow eyeballs to keep up
                System.Threading.Thread.Sleep(delayInMillisecs);

            } while (gameLive);
        }

        // contains all information of the food
        struct Food
        {
            public Food(int xPos, int yPos)
            {
                XPos = xPos;
                YPos = yPos;
            }
            public int XPos { get; set; }
            public int YPos { get; set; }
        }

        // method to insert food onto the screen
        static Food DrawFood(Food food, Random random, int consWidth, int consHeight)
        {
            food.XPos = random.Next(1, consWidth - 2);
            food.YPos = random.Next(1, consHeight - 2);
            SetCursorPosition(food.XPos, food.YPos);
            Console.Write("O");
            SetCursorPosition(0, 0);
            return food;
        }

    }
    
}
