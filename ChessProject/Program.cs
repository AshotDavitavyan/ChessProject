using ChessProject;
using static System.Console;

ChooseGame();

void ChooseGame(){
	string? gameChoice;

	while (true){
		WriteLine("Type in which game do you want to play? (KnightStepCounter/ChessGame), or EXIT to exit");
		gameChoice = ReadLine();
		if (gameChoice == null)
		{
			WriteLine("Invalid input.\n");
			continue;
		}
		switch(gameChoice)
		{
			case "EXIT":
				return;
			case "ChessGame":
				ChessGame.Start();
				break;
			case "KnightStepCounter":
				KnightStepCounter.Start();
				break;
			default:
				WriteLine("Invalid input.\n");
				break;
		}
	}
}
