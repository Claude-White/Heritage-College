package sudoku;

import java.util.HashSet;
import java.util.StringTokenizer;

/*
 * This is a class to handle validation of inputs for a game of sudoku.
 * 
 * Author: Claude White
 */

public class Validation {
	private int row, col;

	public void setRowCol(int ro, int column) {
		row = ro;
		col = column;
	}

	public boolean isPositionEmpty(String position) {
		tokenizeNums(position);
		if (SudokuGame.puzzleBoard[row][col] == 0) {
			return true;
		}
		return false;
	}

	public boolean validateInput(int num) {
		if (!isNumInRow(num) && !isNumInColumn(num) && !isNumInSquare(num)) {
			return true;
		}
		return false;
	}

	public boolean validateNumRange(int num) {
		if (num < 10 && num > 0) {
			return true;
		}
		return false;
	}

	public boolean validateStringNumRange(String position) {
		tokenizeNums(position);
		if ((row < 9 && row >= 0) && (col < 9 && col >= 0)) {
			return true;
		}
		return false;
	}

	private void tokenizeNums(String nums) {
		StringTokenizer tokenizer = new StringTokenizer(nums, " |,");
		row = Integer.parseInt(tokenizer.nextToken()) - 1;
		col = Integer.parseInt(tokenizer.nextToken()) - 1;
	}

	private boolean isNumInRow(int num) {
		for (int i = 0; i < SudokuGame.BOARD_SIZE; i++) {
			if (SudokuGame.puzzleBoard[row][i] == num) {
				return true;
			}
		}
		return false;
	} // isNumInRow(int, int)

	private boolean isNumInColumn(int num) {
		for (int i = 0; i < SudokuGame.BOARD_SIZE; i++) {
			if (SudokuGame.puzzleBoard[i][col] == num) {
				return true;
			}
		}
		return false;
	} // isNumInColumn(int, int)

	private boolean isNumInSquare(int num) {
		int squareRow = row - row % 3;
		int squareCol = col - col % 3;
		for (int boardRow = squareRow; boardRow <= squareRow + 2; boardRow++) {
			for (int boardCol = squareCol; boardCol <= squareCol + 2; boardCol++) {
				if (SudokuGame.puzzleBoard[boardRow][boardCol] == num) {
					return true;
				} // if()
			} // for(col)
		} // for(row)
		return false;
	} // isNumInColumn(int, int, int)

	public boolean isBoardValid() {
		HashSet<Integer> rows = new HashSet<Integer>();
		HashSet<Integer> cols = new HashSet<Integer>();
		HashSet<Integer> boxes = new HashSet<Integer>();
		// I'm using HashSets because when adding an element it return false if there is already an element of the same value.

		// rows
		for (int row = 0; row < SudokuGame.BOARD_SIZE; row++) {
			rows.clear();
			for (int col = 0; col < SudokuGame.BOARD_SIZE; col++) {
				if (SudokuGame.puzzleBoard[row][col] != 0) {
					if (rows.add(SudokuGame.puzzleBoard[row][col]) == false) {
						return false;
					}
				}
			} // for(col)
		} // for(row)

		// cols
		for (int col = 0; col < SudokuGame.BOARD_SIZE; col++) {
			cols.clear();
			for (int row = 0; row < SudokuGame.BOARD_SIZE; row++) {
				if (SudokuGame.puzzleBoard[row][col] != 0) {
					if (cols.add(SudokuGame.puzzleBoard[row][col]) == false) {
						return false;
					}
				}
			} // for(col)
		} // for(row)

		// boxes
		for (int boxRow = 0; boxRow < SudokuGame.BOARD_SIZE; boxRow += 3) {
			boxes.clear();
			for (int boxCol = 0; boxCol < SudokuGame.BOARD_SIZE; boxCol += 3) {
				boxes.clear();
				for (int row = boxRow; row <= boxRow + 2; row++) {
					for (int col = boxCol; col <= boxCol + 2; col++) {
						if (SudokuGame.puzzleBoard[row][col] != 0) {
							if (boxes.add(SudokuGame.puzzleBoard[row][col]) == false) {
								return false;
							}
						}
					} // for(col)
				} // for(row)
			} // for(boxCol)
		} // for(boxRow)

		return true;
	} // isBoardValid()

}
