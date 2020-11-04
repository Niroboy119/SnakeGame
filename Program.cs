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
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("\t\t                    Snake Game");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("\n1. Play Game!");
            Console.WriteLine("2. Exit.");

            int foodCountLimit = 50;
            int obstacleLimit = 6;
            // delay to slow down the character movement so you can see it
            int delayInMillisecs = 50;
            string[] arr = { "1", "2" };
            string menuChoice = validEntry(arr);

            if (menuChoice == "1")
            {
                Console.Clear();
                Console.WriteLine("Choose a Difficulty: ");
                Console.WriteLine("\n1. Easy");
                Console.WriteLine("2. Intermediate");
                Console.WriteLine("3. Hard");
                string[] arr1 = { "1", "2", "3" };
                string diff = validEntry(arr1);

                if (diff == "2")

                {
                    foodCountLimit = 40;
                    obstacleLimit = 10;
                    delayInMillisecs = 35;
                }
                else if (diff == "3")
                {
                    foodCountLimit = 35;
                    obstacleLimit = 15;
                    delayInMillisecs = 30;
                }

            }
            else if (menuChoice == "2")
            {
                Environment.Exit(0);
            }





            // display this char on the console during the game

            List<string> ch = new List<string>();

            // stores the obstacles created
            List<Obstacle> obstacleList = new List<Obstacle>();
            ch.Add("*");
            ch.Add("*");
            ch.Add("*");
            bool gameLive = true;
            ConsoleKeyInfo consoleKey; // holds whatever key is pressed
            int score = 0;
            var rand = new Random();
            // set player lives left
            int playerLife = 2;

            // location info & display
            int x = 3, y = 4; // y is 2 to allow the top row for directions & space
            int dx = 1, dy = 0;
            int consoleWidthLimit = 116;
            int consoleHeightLimit = 30;
            bool isUP = false;
            List<int> st = new List<int>();
            st.Add(x);
            st.Add(y);
            st.Add(x + 1);
            st.Add(y);
            st.Add(x + 2);
            st.Add(y);
            var food = new Food(rand.Next(1, consoleWidthLimit - 2), rand.Next(1, consoleHeightLimit - 2));
            var obstacle = new Obstacle(rand.Next(1, consoleWidthLimit - 2), rand.Next(1, consoleHeightLimit - 2));
            bool[] exist = new bool[obstacleLimit];
            var powerUpFood = new Food(rand.Next(1, consoleWidthLimit - 2), rand.Next(1, consoleHeightLimit - 2));


            // clear to color
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Clear();

            // whether to keep trails
            bool trail = false;

            // keeps track of time passed
            int tickTime = 0;

            // keeps tracks of how many times powerup food is consumed by snake   
            int powerFoodCount = 0;


            // This loop draws new Obstacles at a random locations on the screen
            for (int i = 0; i < obstacleLimit; i++)
            {
                if ((obstacle.XPosObs > 3 || obstacle.XPosObs < 3))
                {
                    obstacle = DrawObstacle(obstacle, rand, consoleWidthLimit, consoleHeightLimit);
                    // add the obstacles created in to the list
                    obstacleList.Add(obstacle);
                }

            }

            for (int i = 0; i < obstacleLimit; i++)
            {
                exist[i] = true;
            }

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

                if (tickTime >= foodCountLimit)
                {
                    // the powerupfood can only be consumed 3 times 
                    if ((rand.Next(1, 100) <= 40) && powerFoodCount < 3)
                    {
                        // lock on to current position of food
                        SetCursorPosition(powerUpFood.XPos, powerUpFood.YPos);

                        // erase the current position of food (to not form a trail)
                        if (trail == false)
                        {
                            Console.Write(' ');
                        }

                        // draw a new food at a random location on the screen while avoiding the obstacle
                        powerUpFood = DrawPowerUpFood(powerUpFood, rand, consoleWidthLimit, consoleHeightLimit);
                    }
                    else
                    {
                        // lock on to current position of food
                        SetCursorPosition(food.XPos, food.YPos);

                        // erase the current position of food (to not form a trail)
                        if (trail == false)
                        {
                            Console.Write(' ');
                        }

                        // draw a new food at a random location on the screen while avoiding the obstacle
                        food = DrawFood(food, rand, consoleWidthLimit, consoleHeightLimit);
                    }
                    // set timer back to zero
                    tickTime = 0;
                }
                // check every obstacle location and sees wheather
                // the snake head position overlaps with an obstacles position
                for (int i = 0; i < obstacleList.Count; i++)
                {
                    if (exist[i] == true && (((st[0] >= obstacleList[i].XPosObs && st[0] <= obstacleList[i].XPosObs + 1) && st[1] == obstacleList[i].YPosObs) || ((st[st.Count - 2] >= obstacleList[i].XPosObs && (st[st.Count - 2] <= obstacleList[i].XPosObs + 1) && st[st.Count - 1] == obstacleList[i].YPosObs))))
                    {
                        // reduced player life by 1
                        playerLife -= 1;
                        SetCursorPosition(obstacleList[i].XPosObs, obstacleList[i].YPosObs);
                        Console.Write("  ");
                        exist[i] = false;


                    }
                }

                if ((st[0] == food.XPos && st[1] == food.YPos) || (st[st.Count - 2] == food.XPos && st[st.Count - 1] == food.YPos))
                {
                    score += 1;
                    ch.Add("*");
                    st.Add(x + ch.Count);
                    st.Add(y);
                    food = DrawFood(food, rand, consoleWidthLimit, consoleHeightLimit);
                }


                // checks if snake hits a powerUp food object
                if ((st[0] == powerUpFood.XPos && st[1] == powerUpFood.YPos) || (st[st.Count - 2] == powerUpFood.XPos && st[st.Count - 1] == powerUpFood.YPos))
                {
                    // increase player life 
                    playerLife = playerLife + 1;
                    powerFoodCount++;
                    powerUpFood.XPos = 0;
                    powerUpFood.YPos = 0;
                    //powerUpFood = DrawFood(powerUpFood, rand, consoleWidthLimit, consoleHeightLimit);
                }


                if (score >= 2)
                {

                    gameLive = false;

                }

                // checks player has anymore lives left
                if (playerLife == 0)
                {

                    gameLive = false;
                }

                // display current score and lives left
                SetCursorPosition(110, 0);
                Console.WriteLine("Score: " + score);
                SetCursorPosition(80, 0);
                Console.WriteLine("Player Life: " + playerLife);

                // pause to allow eyeballs to keep up
                System.Threading.Thread.Sleep(delayInMillisecs);

            } while (gameLive);

            // clears the sreen and displays to the user that game is over and
            // display the player score
            Console.Clear();
            ConsoleColor endScreenBackground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
            if (score < 2)
            {
                Console.WriteLine("Game over your score is " + score);
            }
            else
            {
                Console.WriteLine("Congradulations!!! you won, your score is " + score);
            }
            Console.WriteLine("Press 'Enter' to quit");
            //Console.ForegroundColor = endScreenBackground;
            ConsoleKeyInfo keycheck;
            keycheck = Console.ReadKey();

            while (keycheck.Key != ConsoleKey.Enter)
            {
                Console.WriteLine("Press 'Enter' to quit please");

                keycheck = Console.ReadKey();
            }

        }


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

        struct Obstacle
        {
            public Obstacle(int xPosObs, int yPosObs)
            {
                XPosObs = xPosObs;
                YPosObs = yPosObs;
            }
            public int XPosObs { get; set; }
            public int YPosObs { get; set; }
        }

        // method to insert obstacle onto the screen
        static Obstacle DrawObstacle(Obstacle obstacle, Random random, int consWidth, int consHeight)
        {
            obstacle.XPosObs = random.Next(1, consWidth - 2);
            obstacle.YPosObs = random.Next(1, consHeight - 2);
            SetCursorPosition(obstacle.XPosObs, obstacle.YPosObs);
            Console.Write("||");
            SetCursorPosition(0, 0);
            return obstacle;
        }

        static string validEntry(string[] arr)
        {
            bool entry = false;
            string choice;
            do
            {
                Console.WriteLine("\nChoose an Option to Proceed: ");
                choice = Console.ReadLine();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (choice == arr[i])
                    {
                        entry = true;
                    }
                }
            } while (entry == false);
            return choice;
        }

        // method to draw the powerUp food on the screen
        static Food DrawPowerUpFood(Food food, Random random, int consWidth, int consHeight)
        {
            food.XPos = random.Next(1, consWidth - 2);
            food.YPos = random.Next(1, consHeight - 2);
            SetCursorPosition(food.XPos, food.YPos);
            Console.Write("P");
            SetCursorPosition(0, 0);
            return food;
        }


    }

}
