using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class Program
{
    static void Main()
    {
        // Create the main window
        RenderWindow app = new RenderWindow(new VideoMode(1024, 800), "Rule 30 Cellular Automaton");
        app.Closed += new EventHandler(OnClosed);
        CellularAutomaton automaton = new CellularAutomaton(1024);
        app.MouseWheelScrolled += new EventHandler<MouseWheelScrollEventArgs>(OnMouseWheelScrolled);

        static void OnMouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            // e.Delta will be positive if the wheel was scrolled up, negative if down
            if (e.Delta > 0)
            {
                // Scroll up
            }
            else
            {
                // Scroll down
            }
        }

        // Create a clock to control the update rate
        Clock clock = new Clock();
        int generation = 0;

        // Start the game loop
        while (app.IsOpen)
        {
            // Process events
            app.DispatchEvents();

            // Clear screen
            app.Clear(Color.Red);

            // Draw the automaton
            DrawAutomaton(app, automaton.cells, generation);

            // Update the automaton if enough time has passed
            if (clock.ElapsedTime.AsSeconds() >= 0.1f)  // 0.1 seconds per update
            {
                automaton.cells = UpdateAutomaton(automaton.cells);
                clock.Restart();
                generation++;
            }

            // Update the window
            app.Display();
        }
    }

    public class CellularAutomaton
    {
        public bool[] cells;

        public CellularAutomaton(int size)
        {
            cells = new bool[size];
            cells[size / 2] = true; // Set the middle cell to "on"
        }
    }

    static void DrawAutomaton(RenderWindow window, bool[] automaton, int generation)
    {
        int cellSize = 10;  // You can adjust this as needed

        // Go through the automaton
        for (int i = 0; i < automaton.Length; i++)
        {
            // Create a square for the cell
            RectangleShape cell = new RectangleShape(new Vector2f(cellSize, cellSize));

            // Set the position of the cell.
            cell.Position = new Vector2f(i * cellSize, generation * cellSize);

            // Set the color of the cell based on its state
            if (automaton[i])
            {
                // "On" cells are white
                cell.FillColor = Color.White;
            }
            else
            {
                // "Off" cells are black
                cell.FillColor = Color.Black;
            }

            // Draw the cell
            window.Draw(cell);
        }
    }

    public static bool[] UpdateAutomaton(bool[] currentState)
    {
        int size = currentState.Length;
        bool[] nextState = new bool[size];

        for (int i = 0; i < size; i++)
        {
            // Get the state of the current cell and its neighbors.
            // We'll use the modulus operator to handle the edges of the array,
            // effectively creating a "wrap-around" effect.
            bool left = currentState[(i - 1 + size) % size];
            bool center = currentState[i];
            bool right = currentState[(i + 1) % size];

            // Apply Rule 30 to compute the next state of the current cell.
            nextState[i] = (left && !center && !right) ||
                           (!left && center && !right) ||
                           (!left && !center && right) ||
                           (!left && !center && !right);
        }

        return nextState;
    }


    static void OnClosed(object sender, EventArgs e)
    {
        // Close the window when OnClosed event is received
        RenderWindow window = (RenderWindow)sender;
        window.Close();
    }
}
