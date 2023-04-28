package sudoku;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import java.awt.BorderLayout;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JOptionPane;
import javax.swing.JMenu;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;

import javax.swing.JComboBox;
import javax.swing.JFileChooser;
import javax.swing.JButton;
import javax.swing.DefaultComboBoxModel;
import java.awt.Color;
import javax.swing.JLabel;
import javax.swing.SwingConstants;

import java.awt.Font;
import javax.swing.border.LineBorder;
import javax.swing.filechooser.FileNameExtensionFilter;
import javax.swing.UIManager;
import javax.swing.UnsupportedLookAndFeelException;
import net.miginfocom.swing.MigLayout;

/*
 * This is a frame for playing sudoku.
 * 
 * Author: Claude White
 */

public class SudokuFrame extends JFrame implements ActionListener {

	private JButton[][] btnSquares = new JButton[SudokuGame.BOARD_SIZE][SudokuGame.BOARD_SIZE];
	private JPanel[][] boxPanels = new JPanel[SudokuGame.BOARD_SIZE
			/ 3][SudokuGame.BOARD_SIZE / 3];
	private int selectedRow, selectedCol;
	private int lastRow, lastCol;
	private JPanel contentPane;
	private JPanel inputPanel;
	private JMenuBar menuBar;
	private JMenu mnFile;
	private JMenuItem mntmSelect;
	private JMenuItem mntmSave;
	private JMenuItem mntmExit;
	private JMenu mnAbout;
	private JPanel boardPanel;
	private JMenuItem mntmHelp;
	private JMenuItem mntmAboutUs;
	private JComboBox<String> cmbxInputNum;
	private JButton btnPlay;
	private JPanel titlePanel;
	private JLabel lblTitle;
	private JButton btnUndo;
	private int row, col;

	SudokuGame game = new SudokuGame();

	public static void main(String[] args) {
		try {
			UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
		}
		catch (ClassNotFoundException | InstantiationException
				| IllegalAccessException | UnsupportedLookAndFeelException e1) {
			e1.printStackTrace();
		}
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					SudokuFrame frame = new SudokuFrame();
					frame.setVisible(true);
				}
				catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	} // main(String[])

	public SudokuFrame() {
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 746, 699);
		setLocationRelativeTo(null);

		menuBar = new JMenuBar();
		setJMenuBar(menuBar);

		mnFile = new JMenu("File");
		menuBar.add(mnFile);

		mntmSelect = new JMenuItem("Select File");
		mntmSelect.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				boolean flag = true;
				clearAllButtons();
				enablePlay(false);
				if (selectSudokuFile()) {
					try {
						if (!game.checkFile()) {
							disableAll();
							JOptionPane.showMessageDialog(new JFrame(),
									"File does not have the correct format.",
									"Invalid File Format", JOptionPane.ERROR_MESSAGE);
							flag = false;
						}
					} // try
					catch (IOException e1) {
						JOptionPane.showMessageDialog(new JFrame(), e1.getMessage(),
								"Error", JOptionPane.ERROR_MESSAGE);
					} // catch
					if (!game.val.isBoardValid() && flag) {
						disableAll();
						JOptionPane.showMessageDialog(new JFrame(), "File is not valid",
								"Invalid File", JOptionPane.ERROR_MESSAGE);
					} // validateBoard
					else {
						if (flag) {
							displayGameBoard();
						} // if(flag)
					} // else
				} // selectFile
			} // actionPerformed()
		});
		mnFile.add(mntmSelect);

		mntmSave = new JMenuItem("Save");
		mntmSave.addActionListener(this);
		mnFile.add(mntmSave);

		mntmExit = new JMenuItem("Exit");
		mntmExit.addActionListener(this);
		mnFile.add(mntmExit);

		mnAbout = new JMenu("About");
		menuBar.add(mnAbout);

		mntmHelp = new JMenuItem("Help");
		mntmHelp.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				JOptionPane.showMessageDialog(new JPanel(), new Help(), "Help",
						JOptionPane.PLAIN_MESSAGE);
			}
		});
		mnAbout.add(mntmHelp);

		mntmAboutUs = new JMenuItem("About Us");
		mntmAboutUs.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				JOptionPane.showMessageDialog(new JPanel(), new AboutUs(), "About Us",
						JOptionPane.PLAIN_MESSAGE);
			}
		});
		mnAbout.add(mntmAboutUs);
		contentPane = new JPanel();
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));

		setContentPane(contentPane);
		contentPane.setLayout(new BorderLayout(0, 0));

		boardPanel = new JPanel();
		boardPanel.setBorder(new LineBorder(new Color(0, 0, 0), 5, false));
		contentPane.add(boardPanel, BorderLayout.CENTER);
		boardPanel.setLayout(new GridLayout(3, 3, 0, 0));

		// boxes
		for (row = 0; row < 9; row++) {
			for (col = 0; col < 9; col++) {
				if (row < 3 && col < 3) {
					boxPanels[row][col] = new JPanel();
					boxPanels[row][col]
							.setBorder(new LineBorder(new Color(0, 0, 0), 3, false));
					boxPanels[row][col].setLayout(new GridLayout(3, 3, 0, 0));
					boardPanel.add(boxPanels[row][col]);
				}
				btnSquares[row][col] = new JButton("");
				boxPanels[row / 3][col / 3].remove(btnSquares[row][col]);
				btnSquares[row][col].addActionListener(this);
				boxPanels[row / 3][col / 3].add(btnSquares[row][col]);
			} // for(col)
		} // for(row)

		inputPanel = new JPanel();
		//inputPanel.setBorder(new Line);
		contentPane.add(inputPanel, BorderLayout.SOUTH);
		inputPanel.setLayout(new MigLayout("", "[736px]", "[29px]"));

		cmbxInputNum = new JComboBox<String>();
		cmbxInputNum.setModel(new DefaultComboBoxModel<String>(
				new String[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" }));
		inputPanel.add(cmbxInputNum, "cell 0 0,grow");

		btnPlay = new JButton("Play");
		btnPlay.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				playMove();
			}
		});
		inputPanel.add(btnPlay, "cell 0 0,alignx left,aligny top");

		btnUndo = new JButton("Undo");
		btnUndo.setEnabled(false);
		btnUndo.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				btnSquares[lastRow][lastCol].setText("");
				btnSquares[lastRow][lastCol].setEnabled(true);
				btnUndo.setEnabled(false);
				game.undoLastMove();
			}
		});
		inputPanel.add(btnUndo, "cell 0 0,alignx left,aligny top");

		titlePanel = new JPanel();
		contentPane.add(titlePanel, BorderLayout.NORTH);

		lblTitle = new JLabel("Heritage Sudoku");
		lblTitle.setFont(new Font("Lucida Grande", Font.PLAIN, 22));
		lblTitle.setHorizontalAlignment(SwingConstants.CENTER);
		titlePanel.add(lblTitle);
		disableAll();
	} // SudokuFrame()

	private boolean selectSudokuFile() {
		boolean flag = true;
		String fileName = "";
		File sudokuFile;
		JFileChooser sudokuFC = new JFileChooser("./");
		FileNameExtensionFilter sudokuFilter = new FileNameExtensionFilter(
				"Text File (.txt)", "txt");
		sudokuFC.setAcceptAllFileFilterUsed(false);
		sudokuFC.setFileFilter(sudokuFilter);
		int i = sudokuFC.showOpenDialog(this);
		if (i == JFileChooser.APPROVE_OPTION) {
			sudokuFile = sudokuFC.getSelectedFile();
			fileName = sudokuFile.getName();
			enableAll();
			game.open(fileName);
			try {
				if (!game.checkFileRows()) {
					disableAll();
					JOptionPane.showMessageDialog(new JFrame(),
							"File does not have the correct format.", "Invalid File Format",
							JOptionPane.ERROR_MESSAGE);
					flag = false;
				}
			}
			catch (FileNotFoundException e1) {
				JOptionPane.showMessageDialog(new JFrame(), e1.getMessage(), "Error",
						JOptionPane.ERROR_MESSAGE);
			}
			if (flag) {
				try {
					game.setGameBoard();
				}
				catch (IOException e) {
					JOptionPane.showMessageDialog(this,
							"ERROR: Game board could not be set: " + e.getMessage() + "\n",
							"Game board not found", JOptionPane.INFORMATION_MESSAGE);
				}
				return true;
			}
			if (i == JFileChooser.CANCEL_OPTION) {
				JOptionPane.showMessageDialog(this, "No file chosen", "Choose file",
						JOptionPane.INFORMATION_MESSAGE);
				disableAll();
				return false;
			}
		}
		return false;
	} // selectSudokuFile()

	private void displayGameBoard() {
		for (row = 0; row < btnSquares.length; row++) {
			for (col = 0; col < btnSquares[row].length; col++) {
				if (SudokuGame.puzzleBoard[row][col] != 0) {
					btnSquares[row][col]
							.setText(String.valueOf(SudokuGame.puzzleBoard[row][col]));
					btnSquares[row][col].setEnabled(false);
				} // if()
			} // for(col)
		} // for(row)
		if (!game.hasNotYetWon()) {
			JOptionPane.showMessageDialog(this, "This save is already full",
					"Finished Save", JOptionPane.INFORMATION_MESSAGE);
			mntmSave.setEnabled(false);
		}
	} // displayGameBoard()

	private void clearAllButtons() {
		for (row = 0; row < btnSquares.length; row++) {
			for (col = 0; col < btnSquares[row].length; col++) {
				btnSquares[row][col].setText("");
			}
		}
	}

	private void playMove() {
		int value = Integer.parseInt((String) cmbxInputNum.getSelectedItem());
		btnSquares[selectedRow][selectedCol].setText(String.valueOf(value));
		btnSquares[selectedRow][selectedCol]
				.setBorder(UIManager.getBorder("Button.border"));
		btnSquares[selectedRow][selectedCol].setEnabled(false);
		int selectedRowPlus1 = selectedRow + 1;
		int selectedColPlus1 = selectedCol + 1;
		game.updateGameBoard(selectedRowPlus1 + "," + selectedColPlus1, value);
		lastRow = selectedRow;
		lastCol = selectedCol;
		btnUndo.setEnabled(true);
		if (!game.hasNotYetWon()) {
			JOptionPane.showMessageDialog(this, "You just won!", "Won",
					JOptionPane.INFORMATION_MESSAGE);
		}
	} // playMove()

	private void disableAll() {
		for (row = 0; row < btnSquares.length; row++) {
			for (col = 0; col < btnSquares[row].length; col++) {
				btnSquares[row][col].setEnabled(false);
			}
		}
		cmbxInputNum.setEnabled(false);
		btnPlay.setEnabled(false);
		mntmSave.setEnabled(false);
	}

	private void enableAll() {
		for (row = 0; row < btnSquares.length; row++) {
			for (col = 0; col < btnSquares[row].length; col++) {
				btnSquares[row][col].setEnabled(true);
			}
		}
		mntmSave.setEnabled(true);
	}

	private void enablePlay(boolean isEnabled) {
		cmbxInputNum.setEnabled(isEnabled);
		btnPlay.setEnabled(isEnabled);
	} // enablePlay()

	private void checkValidMoves() {
		game.val.setRowCol(selectedRow, selectedCol);
		cmbxInputNum.removeAllItems();
		for (int i = 1; i < 10; i++) {
			if (game.val.validateInput(i)) {
				cmbxInputNum.addItem(String.valueOf(i));
				//inputPanel.add(cmbxInputNum, "cell 0 0,grow");
			}
		}
	} // checkValidMoves()

	public void actionPerformed(ActionEvent e) {
		btnSquares[selectedRow][selectedCol]
				.setBorder(UIManager.getBorder("Button.border"));
		for (row = 0; row < btnSquares.length; row++) {
			for (col = 0; col < btnSquares[row].length; col++) {
				if (e.getSource() == btnSquares[row][col]) {
					selectedRow = row;
					selectedCol = col;
					btnSquares[selectedRow][selectedCol]
							.setBorder(new LineBorder(new Color(0, 0, 0), 2, true));
					enablePlay(true);
					checkValidMoves();
				}
			} // for(col)
		} // for(row)
		if (e.getSource() == mntmSave) {
			try {
				game.saveBoard();
				JOptionPane.showMessageDialog(this, "Saved the game", "Saved Game",
						JOptionPane.INFORMATION_MESSAGE);
			}
			catch (IOException e1) {
				JOptionPane.showMessageDialog(this,
						"ERROR: Could not write to file. " + e1.getMessage(),
						"Write unsuccessful", JOptionPane.INFORMATION_MESSAGE);
			}
		}

		if (e.getSource() == mntmExit) {
			JOptionPane.showMessageDialog(this, "Exiting game", "Exit Game",
					JOptionPane.INFORMATION_MESSAGE);
			System.exit(-1);
		}
	} // actionPerformed(ActionEvent)
} // SudokuGame class