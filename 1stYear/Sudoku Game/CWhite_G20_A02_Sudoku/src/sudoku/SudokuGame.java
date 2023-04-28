package sudoku;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Scanner;
import java.util.StringTokenizer;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/*
 * This is a the server class that handles the logic of the sudoku game.
 * 
 * Author: Claude White
 */

public class SudokuGame {
	public static final int BOARD_SIZE = 9;
	public static int[][] puzzleBoard;
	private String gameFile;
	private FileWriter gameWriter;
	private File gameBoard;
	public Validation val = new Validation();
	private int lastRow, lastCol;
	private static boolean canUndo = false;
	
	public void setCanUndo(boolean can) {
		canUndo = can;
	} // setCanUndo(boolean)
	
	public boolean getCanUndo() {
		return canUndo;
	} // getCanUndo()

	public void open() {
		gameBoard = new File("sudoku.txt");
	} // openFile()

	public void open(String filename) {
		gameFile = filename;
		gameBoard = new File(gameFile);
	} // openFile(String)

	public void openWriter() throws IOException {
		gameWriter = new FileWriter(gameBoard);
	} // catch()

	public boolean checkFileRows() throws FileNotFoundException {
		int lines = 0;
		Scanner readFile = new Scanner(gameBoard);
		while (readFile.hasNextLine()) {
			if (!readFile.nextLine().isEmpty()) {
				lines++;
			}
		}
		readFile.close();

		if (lines != 9) {
			return false;
		}
		return true;
	} // checkFileRows()

	public boolean setGameBoard() throws IOException {
		String value;
		puzzleBoard = new int[BOARD_SIZE][BOARD_SIZE];
		Scanner readBoard = new Scanner(gameBoard);
		readBoard.useDelimiter("~|\n");
		for (int row = 0; row < BOARD_SIZE; row++) {
			for (int col = 0; col < BOARD_SIZE; col++) {
				value = readBoard.next();
				if (!value.equals("*")) {
					puzzleBoard[row][col] = Integer.parseInt(value);
				} // if()
				if (col == 8) {
					value = readBoard.nextLine();
				}
			} // for(column)
		} // for(row)
		readBoard.close();
		return true;
	} // setGameBoard()

	public boolean saveBoard() throws IOException {
		openWriter();
		for (int row = 0; row < BOARD_SIZE; row++) {
			for (int col = 0; col < BOARD_SIZE; col++) {
				if (puzzleBoard[row][col] == 0) {
					gameWriter.write("*~");
				}
				else {
					gameWriter.write(puzzleBoard[row][col] + "~");
				}
			} // for(column)
			if (row != 8) {
				gameWriter.write("\n");
			}
		} // for(row)
		close();
		return true;
	} // saveBoard()

	public void close() throws IOException {
		gameWriter.close();
	} // close()

	public void undoLastMove() {
		puzzleBoard[lastRow][lastCol] = 0;
		canUndo = false;
	} // undoLastMove()

	public void updateGameBoard(String position, int value) {
		int row, col;
		StringTokenizer tokenizer = new StringTokenizer(position, " |,");
		row = Integer.parseInt(tokenizer.nextToken()) - 1;
		col = Integer.parseInt(tokenizer.nextToken()) - 1;
		puzzleBoard[row][col] = value;
		//set lastRow and lastCol
		lastRow = row;
		lastCol = col;
		// Make undo available
		canUndo = true;
	} // updateGameBoard(String, int)

	public boolean hasNotYetWon() {
		for (int row = 0; row < BOARD_SIZE; row++) {
			for (int col = 0; col < BOARD_SIZE; col++) {
				if (puzzleBoard[row][col] == 0) {
					return true;
				} // if(puzzleBoard[row][col] != 0)
			} // for (col)
		} // for(row)
		return false;
	} // hasWon()

	public boolean checkFile() throws IOException {
		String line;
		boolean flag = false;
		Scanner readFile = new Scanner(gameBoard);
		readFile.useDelimiter("\n");
		while (readFile.hasNextLine()) {
			line = readFile.nextLine();
			if (Pattern.matches(
					"[1-9*]~[1-9*]~[1-9*]~[1-9*]~[1-9*]~[1-9*]~[1-9*]~[1-9*]~[1-9*]~",
					line)) {
				flag = true;
			} // if(Pattern)
			else {
				return false;
			} // else
		} // while(hasNextLine())
		readFile.close();
		return flag;
	} // checkFile()

} // SudokuGame class
