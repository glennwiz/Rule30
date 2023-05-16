using SFML.Graphics;
using SFML.System;
using SFML.Window;

class Program
{
    private const int CellSize = 10;
    private static uint windowWidth = 1024;
    private static uint windowHeight = 800;
    private static int cellCount = (int)(windowWidth / CellSize);  // Calculate the number of cells based on the window's width and the cell size

    static void Main()
    {
        RenderWindow window = InitializeWindow();
        RectangleShape[] cells = InitializeCells();

        ulong state = 1ul << (cellCount / 2);  // Set the initial state to have a single "on" cell in the middle
        window.Clear(Color.Black);

        bool end = true;
        while (window.IsOpen)
        {
            window.DispatchEvents();
            if (!end) { continue; }
            state = DrawAutomaton(window, cells, state, 39);  // Draw 3 lines
            end = false;

            window.Display();
        }
    }

    private static RenderWindow InitializeWindow()
    {
        RenderWindow window = new RenderWindow(new VideoMode(windowWidth, windowHeight), "Rule 30");
        window.Closed += (s, e) => window.Close();
        return window;
    }

    private static RectangleShape[] InitializeCells()
    {
        RectangleShape[] cells = new RectangleShape[cellCount];  // Create an array of cells based on the calculated cell count
        for (int i = 0; i < cells.Length; ++i)
        {
            cells[i] = new RectangleShape(new Vector2f(CellSize, CellSize));
        }
        return cells;
    }

    private static ulong DrawAutomaton(RenderWindow window, RectangleShape[] cells, ulong state, int lines)
    {
        for (int i = 0; i < lines; ++i)
        {
            DrawLine(window, cells, state, i);
            state = CalculateNextState(state);
        }
        return state;
    }

    private static void DrawLine(RenderWindow window, RectangleShape[] cells, ulong state, int line)
    {
        Random rnd = new Random();

        for (int j = 0; j < cellCount; ++j)
        {
            cells[j].Position = new Vector2f(j * CellSize, line * CellSize);

            if ((state >> j & 1) != 0) // If cell is "on"
            {
                int red = rnd.Next(0, 256);   // Generate random red value
                int green = rnd.Next(0, 256); // Generate random green value
                int blue = rnd.Next(0, 256);  // Generate random blue value
                cells[j].FillColor = new Color((byte)red, (byte)green, (byte)blue);
            }
            else // If cell is "off"
            {
                cells[j].FillColor = Color.Black;
            }

            window.Draw(cells[j]);
        }
    }


    private static ulong CalculateNextState(ulong state)
    {
        ulong newState = 0;
        for (int j = 1; j < cellCount - 1; ++j)
        {
            ulong left = (state >> (j - 1)) & 1;
            ulong center = (state >> j) & 1;
            ulong right = (state >> (j + 1)) & 1;
            ulong newCell = (left ^ (center | right));
            newState |= (newCell << j);
        }
        return newState;
    }
}
