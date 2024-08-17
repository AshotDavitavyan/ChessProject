# ChessBot Game - C# and WPF Project

Welcome to **ChessBot Game**, a chess application developed in C# using Windows Presentation Foundation (WPF). This project features a chess bot with custom movement, logic, and visual representation, allowing players to enjoy a game of chess against the AI or manually control both sides. The game also supports saving and loading game states through a database.

## Features

- **Custom ChessBot AI**: The ChessBot features custom movement and logic, simulating a challenging AI opponent.
- **Game State Management**: Save and load game states via a database, enabling users to navigate back and forth through previous moves.
- **Visual Representation**: Chess pieces and moves are visually represented on the WPF user interface.
- **User-Friendly Interface**: Easy-to-use controls and an intuitive interface for playing chess against the AI.
- **Ability to move back and forth**: Player can easily navigate through the game history, reviewing or replaying previous moves to analyze the game or correct mistakes.

## Technology Stack

- **Programming Language**: C#
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Database**: ADO.NET for database operations to store game states

## Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone git@github.com:AshotDavitavyan/ChessProject.git
   cd ChessProject

2. **Build the Project**

- Open the project in Visual Studio.
- Build the solution by selecting **Build > Build Solution**.

3. **Run the Application**

- Press `F5` to run the application.
- The game window will open, allowing you to start a new chess game or load a saved game.

## Game Controls

- **Start New Game**: Begin a new game of chess by selecting this option.
- **Save Game**: Save the current game state to the database.
- **Load Game**: Load a previously saved game.
- **Navigate Through Moves**: Move backward or forward through the saved game states to review previous moves.

## Project Structure

- **/ChessBotGame/**: The main project directory containing the source code and WPF interface.
- **/Database/**: Contains the scripts for setting up the database used to store game states.
- **/Assets/**: Visual assets like chess pieces and the board.
- **/Documentation/**: Contains additional project documentation and references.

## Future Enhancements

- Implement Castling.
- Implement Special Pawn moves (En Passant, Promotion).
- Improve ChessBot AI to follow more advanced strategies.
- Make a better interface.
- Add multiplayer functionality for users to play against each other.

## Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request.

## Contact

For any questions or feedback, please reach out to ashotd03@gmail.com.

