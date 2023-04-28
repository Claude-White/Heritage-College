package sudoku;

import java.io.IOException;
import java.util.Scanner;

/*
 * This is a CLI sudoku game.
 * 
 * Author: Claude White
 */

public class SudokuInterface {

	private static String position;

	public static void main(String[] args) {
		SudokuInterface sudokuI = new SudokuInterface();

		SudokuGame game = new SudokuGame();
		Scanner input = new Scanner(System.in);

		System.out.println("Welcome to Heritage Sudoku!");
		System.out.print("Please enter the filename for your puzzle: ");
		String filename = input.nextLine();
		if (filename.equals("")) {
			game.open();
		} // if(filename)
		else {
			game.open(filename);
		} // else
		try {
			if (!game.checkFile()) {
				System.out.println("File does not have the correct file format.");
				main(null);
			}
		}
		catch (IOException e) {
			System.out.println("ERROR: Could not read file. " + e.getMessage());
		}
		try {
			game.setGameBoard();
		} // try
		catch (IOException e) {
			System.out.println(
					"ERROR: Game board could not be set: " + e.getMessage() + "\n");
			main(null);
		} // catch()

		System.out.println(
				"Type Q at any time to exit the game, S to save the game or U to undo last move.");
		sudokuI.gameLoop(input, game);

		input.close();
	} //main(String[])

	private void gameLoop(Scanner input, SudokuGame game) {
		if (!game.hasNotYetWon()) {
			System.out.println("Your board is already complete.");
			System.out.print("Do you want to play again? (Y or N): ");
			if (input.nextLine().equalsIgnoreCase("Y")) {
				main(null);
			} // if(input)
			else {
				System.exit(-1);
			} // else
		} // if(hasNotYetWon())
		printBoard();
		// Ask for position
		System.out.print("\nEnter a square number (row, column): ");
		position = input.nextLine();
		checkSpecialInputs(position, input, game);
		// Check position is between 1 and 9
		while (!game.val.validateStringNumRange(position)) {
			System.out.print(
					"Row and column must be between 1 and 9. Please enter again: ");
			position = input.nextLine();
			checkSpecialInputs(position, input, game);
		}
		// Check if position is empty
		while (!game.val.isPositionEmpty(position)) {
			System.out.println("Already a value in position: " + position
					+ ". Please enter again: ");
			position = input.nextLine();
		}
		// Ask for value
		System.out.print("Enter value: ");
		String value = input.nextLine();
		checkSpecialInputs(value, input, game);
		while (!game.val.validateNumRange(Integer.parseInt(value))) {
			System.out.print("Value must be between 1 and 9. Please enter again: ");
			value = input.nextLine();
			checkSpecialInputs(value, input, game);
		}
		// Check if position and value are valid
		if (!game.val.validateInput(Integer.parseInt(value))) {
			System.out.println("Invalid move. There is already a " + value
					+ " in that row/column/square. Please try again.");
			gameLoop(input, game);
		}
		// Update board with new values
		game.updateGameBoard(position, Integer.parseInt(value));
		// Check if game has been won
		if (!game.hasNotYetWon()) {
			System.out.println("Congratulations you won the game!");
			System.out.print("Do you want to play again? (Y or N): ");
			if (input.nextLine().equalsIgnoreCase("Y")) {
				main(null);
			}
			else {
				System.exit(-1);
			}
		}
		gameLoop(input, game);
	} // gameLoop(Scanner, SudokuGame)

	private void checkSpecialInputs(String userInput, Scanner input,
			SudokuGame game) {
		// Check if pressed "q"
		if (userInput.equalsIgnoreCase("q")) {
			System.out.println("Thanks for playing Heritage Sudoku!");
			System.exit(-1);
		}
		// Check if pressed "s"
		if (userInput.equalsIgnoreCase("s")) {
			try {
				game.openWriter();
			} // try
			catch (IOException e) {
				System.out.println("ERROR: Could not open writer. " + e.getMessage());
			} // catch()
			try {
				game.saveBoard();
			} // try
			catch (IOException e) {
				System.out.println("ERROR: Could not write to file. " + e.getMessage());
			} // catch()
			try {
				game.close();
			} // try
			catch (IOException e) {
				System.out.println("ERROR: Could not close Writer" + e.getMessage());
			} // catch()
			System.out.println("Game has been saved successfully.");
			gameLoop(input, game);
		}
		// Check if pressed "u"
		if (userInput.equalsIgnoreCase("u") && game.getCanUndo()) {
			game.undoLastMove();
			System.out.println("Your last move was undone.");
			gameLoop(input, game);
		}
		if (userInput.equalsIgnoreCase("u") && !game.getCanUndo()) {
			System.out.println(
					"You cannot undo more than once or at the start of a game. Sorry keep playing.");
			gameLoop(input, game);
		}
	}

	private void printBoard() {
		System.out.println();
		for (int row = 0; row < SudokuGame.BOARD_SIZE; row++) {
			if (row % 3 == 0 && row != 0) {
				System.out.println("------+-------+------");
			} // if()
			for (int column = 0; column < SudokuGame.BOARD_SIZE; column++) {
				if (column % 3 == 0 && column != 0) {
					System.out.print("| ");
				} // if()
				if (SudokuGame.puzzleBoard[row][column] == 0) {
					System.out.print("* ");
				} // if()
				else {
					System.out.print(SudokuGame.puzzleBoard[row][column] + " ");
				} // else
			} // for(column)
			System.out.println();
		} // for(row)
	} // printBoard()
} // SudokuInterface class
