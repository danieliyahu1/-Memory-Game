## C# Memory Game - Console Application

This project implements a classic Memory Game for the console in C#. It showcases object-oriented programming principles, data structures, and interaction with an external assembly (.dll).

**Objectives:**

- Utilize classes for object-oriented design.
- Employ constructors, enums, properties, and access modifiers for data management.
- Leverage arrays or collections to represent the game board.
- Manipulate strings for UI elements.
- Reference an external assembly (Ex02.ConsoleUtils.dll) for console utilities.

**The Game:**

This console-based Memory Game allows two human players or a human player against the computer. Players try to match pairs of hidden letters on a board by revealing cells one at a time.

**Program Flow:**

1. **Player Setup:**
    - Enter player name (max 20 characters, no spaces).
    - Choose opponent: human or computer. If human, enter opponent's name.
    - Select board size (4x4 minimum, 6x6 maximum, even number of cells only).
2. **Game Loop:**
    - Display an empty board.
    - Player's turn:
        - Enter a cell choice (letter and number combination).
        - Validate input (within board boundaries, not previously revealed).
        - Reveal the chosen cell.
        - If a matching pair is revealed, keep it exposed and award a point.
        - If not a match, show it for 2 seconds and then hide both cells.
    - Switch turns.
3. **Game Over:**
    - When all pairs are matched, declare the winner with the highest score.
4. **Play Again:**
    - Ask the user if they want to play another round.

**Architecture:**

- Object-oriented design principles for code organization and reusability.
- Separated user interface (UI) logic from game logic for potential future UI modifications.
- Focus on creating reusable components as much as possible.
