using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Rule30Simulation
{
    class Program
    {
        static uint gridSize = 100;
        static uint cellSize = 5;
        static uint generations = 100;
        static uint windowSizeX = gridSize * cellSize;
        static uint windowSizeY = generations * cellSize;

        static bool[,] grid;
        static RenderWindow window;

        static bool isDownKeyPressed = false;
        static bool isUpKeyPressed = false;

        static void Main(string[] args)
        {
            grid = new bool[gridSize, generations];
            InitializeGrid();

            window = new RenderWindow(new VideoMode(windowSizeX, windowSizeY), "Rule 30 Simulation");
            window.Closed += (sender, e) => window.Close();
            window.KeyPressed += OnKeyPressed;
            window.KeyReleased += OnKeyReleased;

            Clock clock = new Clock();

            while (window.IsOpen)
            {
                window.DispatchEvents();

                if (isDownKeyPressed)
                {
                    if (clock.ElapsedTime.AsMilliseconds() > 200)
                    {
                        ScrollGridUp();
                        clock.Restart();
                    }
                }

                if (isUpKeyPressed)
                {
                    if (clock.ElapsedTime.AsMilliseconds() > 200)
                    {
                        ScrollGridDown();
                        clock.Restart();
                    }
                }

                UpdateGrid();
                DrawGrid();

                window.Display();
            }
        }

        static void InitializeGrid()
        {
            // Set the initial configuration of the grid
            grid[gridSize / 2, 0] = true;
        }

        static void UpdateGrid()
        {
            for (uint y = 1; y < generations; y++)
            {
                for (uint x = 0; x < gridSize; x++)
                {
                    // Get the indices of the left, center, and right neighbors with wrapping behavior
                    uint leftIndex = (x == 0) ? (gridSize - 1) : (x - 1);
                    uint rightIndex = (x == gridSize - 1) ? 0 : (x + 1);

                    // Apply Rule 30 to determine the new state of the cell
                    bool left = grid[leftIndex, y - 1];
                    bool center = grid[x, y - 1];
                    bool right = grid[rightIndex, y - 1];

                    grid[x, y] = ApplyRule30(left, center, right);
                }
            }
        }

        static bool ApplyRule30(bool left, bool center, bool right)
        {
            if (left && !center && !right) return true;  // \\ 100 in haed like bom not not
            if (!left && center && right) return true;   // \\ 011 in head like not bom bom
            if (!left && center && !right) return true;  // \\ 010 in head like not bom not
            if (!left && !center && right) return true;  // \\ 001 in head like not not bom

            return false;
        }

        static void ScrollGridUp()
        {
            for (int y = (int)(generations - 1); y >= 1; y--)
            {
                for (uint x = 0; x < gridSize; x++)
                {
                    grid[x, y] = grid[x, y - 1];
                }
            }

            // Clear the top row
            for (uint x = 0; x < gridSize; x++)
            {
                grid[x, 0] = false;
            }
        }

        static void ScrollGridDown()
        {
            for (uint y = generations - 1; y > 0; y--)
            {
                for (uint x = 0; x < gridSize; x++)
                {
                    grid[x, y] = grid[x, y - 1];
                }
            }

            // Clear the bottom row
            for (uint x = 0; x < gridSize; x++)
            {
                grid[x, 0] = false;
            }
        }

        // Linearly interpolate between two colors based on a factor between 0 and 1
        static Color LerpColor(Color a, Color b, float factor)
        {
            return new Color(
                (byte)(a.R + factor * (b.R - a.R)),
                (byte)(a.G + factor * (b.G - a.G)),
                (byte)(a.B + factor * (b.B - a.B))
            );
        }

        static void DrawGrid()
        {
            window.Clear(Color.Black);
            uint segment = generations / 4;

            for (uint y = 0; y < generations; y++)
            {
                for (uint x = 0; x < gridSize; x++)
                {
                    if (grid[x, y])
                    {
                        Color color;
                        if (y < segment) // White to Red
                        {
                            color = LerpColor(Color.White, Color.Red, (float)y / segment);
                        }
                        else if (y < 2 * segment) // Red to Blue
                        {
                            color = LerpColor(Color.Red, Color.Blue, (float)(y - segment) / segment);
                        }
                        else if (y < 3 * segment) // Blue to Green
                        {
                            color = LerpColor(Color.Blue, Color.Green, (float)(y - 2 * segment) / segment);
                        }
                        else // Green to White
                        {
                            color = LerpColor(Color.Green, Color.White, (float)(y - 3 * segment) / segment);
                        }

                        RectangleShape cell = new RectangleShape(new Vector2f(cellSize, cellSize))
                        {
                            Position = new Vector2f(x * cellSize, y * cellSize),
                            FillColor = color
                        };
                        window.Draw(cell);
                    }
                }
            }
        }


        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Down)
            {
                isDownKeyPressed = true;
            }

            if (e.Code == Keyboard.Key.Up)
            {
                isUpKeyPressed = true;
            }
        }

        static void OnKeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Down)
            {
                isDownKeyPressed = false;
            }

            if (e.Code == Keyboard.Key.Up)
            {
                isUpKeyPressed = false;
            }
        }
    }
}
