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

        static bool[,] grid;
        static RenderWindow window;

        static void Main(string[] args)
        {
            grid = new bool[gridSize, generations];
            InitializeGrid();

            window = new RenderWindow(new VideoMode(gridSize * cellSize, generations * cellSize), "Rule 30 Simulation");
            window.Closed += (sender, e) => window.Close();

            while (window.IsOpen)
            {
                window.DispatchEvents();

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
            return (left && !center && !right) || (!left && center && right) || (!left && center && !right) || (!left && !center && right);
        }

        static void DrawGrid()
        {
            window.Clear(Color.Black);

            for (uint y = 0; y < generations; y++)
            {
                for (uint x = 0; x < gridSize; x++)
                {
                    if (grid[x, y])
                    {
                        RectangleShape cell = new RectangleShape(new Vector2f(cellSize, cellSize))
                        {
                            Position = new Vector2f(x * cellSize, y * cellSize),
                            FillColor = Color.White
                        };
                        window.Draw(cell);
                    }
                }
            }
        }
    }
}
