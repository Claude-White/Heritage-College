package hangman;

import linked_data_structures.DoublyLinkedList;
import linked_data_structures.SinglyLinkedList;

import java.awt.*;

import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;

public class HangmanFrame extends JFrame implements ActionListener, WindowListener {

	private static final long serialVersionUID = 1L;
	private String tempWord;

	private JButton[] btnLetters = new JButton[26];
	private JLabel lblWordToGuess;
	private JLabel lblWrongGuessesList;
	private JLabel lblUsername;
	private JButton btnHint;
	private JLabel lblLives;
	private JLabel lblHangmanImg;
	private JLabel lblArrowImg;

	private HangmanGame game;
	private Scoreboard scoreboard;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					LoginFrame window = new LoginFrame();
					window.setVisible(true);
				}
				catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	/**
	 * Create the application.
	 */
	public HangmanFrame(Player player, boolean samePlayer) {
		getContentPane().setBackground(Color.WHITE);
		getContentPane().setFont(new Font("Monospaced", Font.PLAIN, 13));
		setTitle("Hangman");
		setBounds(100, 100, 899, 600);
		setResizable(false);
		setLocationRelativeTo(null);
		setVisible(true);
		setDefaultCloseOperation(JFrame.DO_NOTHING_ON_CLOSE);
		SpringLayout springLayout = new SpringLayout();
		getContentPane().setLayout(springLayout);

		addWindowListener(this);

		JPanel letterPanel = new JPanel();
		letterPanel.setBackground(Color.WHITE);
		springLayout.putConstraint(SpringLayout.NORTH, letterPanel, -90, SpringLayout.SOUTH, getContentPane());
		springLayout.putConstraint(SpringLayout.WEST, letterPanel, 10, SpringLayout.WEST, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, letterPanel, -10, SpringLayout.SOUTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, letterPanel, -10, SpringLayout.EAST, getContentPane());
		getContentPane().add(letterPanel);
		letterPanel.setLayout(new GridLayout(2, 13, 0, 0));

		lblWordToGuess = new JLabel("");
		lblWordToGuess.setFont(new Font("Monospaced", Font.PLAIN, 13));
		springLayout.putConstraint(SpringLayout.SOUTH, lblWordToGuess, -154, SpringLayout.NORTH, letterPanel);
		springLayout.putConstraint(SpringLayout.EAST, lblWordToGuess, -274, SpringLayout.EAST, getContentPane());
		getContentPane().add(lblWordToGuess);

		JLabel lblWrongGuesses = new JLabel("Wrong Guesses");
		lblWrongGuesses.setFont(new Font("Monospaced", Font.PLAIN, 13));
		springLayout.putConstraint(SpringLayout.NORTH, lblWrongGuesses, 84, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, lblWrongGuesses, -241, SpringLayout.EAST, getContentPane());
		getContentPane().add(lblWrongGuesses);

		lblWrongGuessesList = new JLabel("");
		springLayout.putConstraint(SpringLayout.NORTH, lblWrongGuessesList, 25, SpringLayout.SOUTH, lblWrongGuesses);
		lblWrongGuessesList.setFont(new Font("Monospaced", Font.PLAIN, 13));
		getContentPane().add(lblWrongGuessesList);

		lblUsername = new JLabel("");
		lblUsername.setFont(new Font("Monospaced", Font.PLAIN, 13));
		springLayout.putConstraint(SpringLayout.NORTH, lblUsername, 10, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.WEST, lblUsername, 0, SpringLayout.WEST, letterPanel);
		getContentPane().add(lblUsername);

		char letter = 'A';
		for (int i = 0; i < btnLetters.length; i++) {
			btnLetters[i] = new JButton(Character.toString(letter + i));
			btnLetters[i].setFont(new Font("Monospaced", Font.PLAIN, 14));
			btnLetters[i].addActionListener(this);
			letterPanel.add(btnLetters[i]);
		}

		btnHint = new JButton("Hint");
		springLayout.putConstraint(SpringLayout.NORTH, btnHint, 10, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.WEST, btnHint, -110, SpringLayout.EAST, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, btnHint, 60, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, btnHint, -10, SpringLayout.EAST, getContentPane());
		btnHint.setFont(new Font("Monospaced", Font.PLAIN, 13));
		btnHint.addActionListener(e -> {
			char hint = game.getHint();
			for (JButton btnLetter : btnLetters) {
				if (btnLetter.getText().charAt(0) == hint) {
					updateHangman();
					btnLetter.doClick();
				}
			}
			if (!game.canGetHint()) {
				btnHint.setEnabled(false);
			}

		});
		getContentPane().add(btnHint);

		lblLives = new JLabel("Lives: ");
		springLayout.putConstraint(SpringLayout.WEST, lblLives, 90, SpringLayout.WEST, getContentPane());
		lblLives.setHorizontalAlignment(SwingConstants.CENTER);
		springLayout.putConstraint(SpringLayout.NORTH, lblLives, 80, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, lblLives, 100, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, lblLives, -600, SpringLayout.EAST, getContentPane());
		lblLives.setFont(new Font("Monospaced", Font.PLAIN, 13));
		getContentPane().add(lblLives);

		lblHangmanImg = new JLabel();
		lblHangmanImg.setSize(250, 400);
		springLayout.putConstraint(SpringLayout.NORTH, lblHangmanImg, 100, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.WEST, lblHangmanImg, 50, SpringLayout.WEST, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, lblHangmanImg, -100, SpringLayout.SOUTH, letterPanel);
		springLayout.putConstraint(SpringLayout.EAST, lblHangmanImg, -600, SpringLayout.EAST, getContentPane());
		getContentPane().add(lblHangmanImg);

		lblArrowImg = new JLabel("");
		springLayout.putConstraint(SpringLayout.WEST, lblWrongGuessesList, 5, SpringLayout.EAST, lblArrowImg);
		springLayout.putConstraint(SpringLayout.SOUTH, lblWrongGuessesList, -4, SpringLayout.SOUTH, lblArrowImg);
		springLayout.putConstraint(SpringLayout.WEST, lblArrowImg, 255, SpringLayout.EAST, lblHangmanImg);
		springLayout.putConstraint(SpringLayout.EAST, lblArrowImg, -64, SpringLayout.EAST, lblWrongGuesses);
		springLayout.putConstraint(SpringLayout.NORTH, lblArrowImg, 6, SpringLayout.SOUTH, lblWrongGuesses);
		springLayout.putConstraint(SpringLayout.SOUTH, lblArrowImg, 46, SpringLayout.SOUTH, lblWrongGuesses);
		lblArrowImg.setSize(40, 40);
		setArrow();
		getContentPane().add(lblArrowImg);

		JMenuBar menuBar = new JMenuBar();
		menuBar.setFont(new Font("Monospaced", Font.PLAIN, 14));
		setJMenuBar(menuBar);

		JMenu mnFile = new JMenu("File");
		mnFile.setFont(new Font("Monospaced", Font.PLAIN, 14));
		menuBar.add(mnFile);

		JMenuItem mntmNewGame = new JMenuItem("New Game");
		mntmNewGame.setFont(new Font("Monospaced", Font.PLAIN, 14));
		mnFile.add(mntmNewGame);
		mntmNewGame.addActionListener(e -> setupNewGame(player, true));

		JMenuItem mntmScoreboard = new JMenuItem("Scoreboard");
		mntmScoreboard.setFont(new Font("Monospaced", Font.PLAIN, 14));
		mnFile.add(mntmScoreboard);
		mntmScoreboard.addActionListener(e -> new ScoreboardFrame());

		JMenuItem mntmQuit = new JMenuItem("Quit");
		mntmQuit.setFont(new Font("Monospaced", Font.PLAIN, 14));
		mnFile.add(mntmQuit);
		mntmQuit.addActionListener(e -> windowClosing(null));

		JMenu mnHelp = new JMenu("Help");
		mnHelp.setFont(new Font("Monospaced", Font.PLAIN, 14));
		menuBar.add(mnHelp);

		JMenuItem mntmRules = new JMenuItem("Rules");
		mntmRules.setFont(new Font("Monospaced", Font.PLAIN, 14));
		mnHelp.add(mntmRules);
		mntmRules.addActionListener(e -> new RulesFrame());

		setupNewGame(player, samePlayer);
	}

	private void updateHangman() {
		BufferedImage originalImage;
		try {
			originalImage = ImageIO.read(new File("./Images/" + (6 - game.getNumErrors()) + "Lives.png"));
			int labelWidth = lblHangmanImg.getWidth();
			int labelHeight = lblHangmanImg.getHeight();
			double scaleFactor = Math.min((double) labelWidth / originalImage.getWidth(), (double) labelHeight / originalImage.getHeight());
			int scaledWidth = (int) (originalImage.getWidth() * scaleFactor);
			int scaledHeight = (int) (originalImage.getHeight() * scaleFactor);
			Image scaledImage = originalImage.getScaledInstance(scaledWidth, scaledHeight, Image.SCALE_SMOOTH);
			ImageIcon hangmanImg = new ImageIcon(scaledImage);
			lblHangmanImg.setIcon(hangmanImg);
		}
		catch (IOException e1) {
			JOptionPane.showMessageDialog(new JPanel(), "Hangman image could not be found.");
			lblHangmanImg.setIcon(null);
		}
	}

	private void setArrow() {
		BufferedImage originalImage;
		try {
			originalImage = ImageIO.read(new File("./Images/Arrow.png"));
			int labelWidth = lblArrowImg.getWidth();
			int labelHeight = lblArrowImg.getHeight();
			double scaleFactor = Math.min((double) labelWidth / originalImage.getWidth(), (double) labelHeight / originalImage.getHeight());
			int scaledWidth = (int) (originalImage.getWidth() * scaleFactor);
			int scaledHeight = (int) (originalImage.getHeight() * scaleFactor);
			Image scaledImage = originalImage.getScaledInstance(scaledWidth, scaledHeight, Image.SCALE_SMOOTH);
			ImageIcon hangmanImg = new ImageIcon(scaledImage);
			lblArrowImg.setIcon(hangmanImg);
		}
		catch (IOException e1) {
			JOptionPane.showMessageDialog(new JPanel(), "Hangman image could not be found.");
			lblArrowImg.setIcon(null);
		}
	}

	private void setupNewGame(Player player, boolean samePlayer) {
		clearAll();
		scoreboard = new Scoreboard();
		if (samePlayer) {
			player.setGame(new HangmanGame(player, player.getGame().getDictionary()));
			game = player.getGame();
			this.setTitle("Hangman - Game " + (game.getTotalWords() - game.getNumWords()) + "/" + game.getTotalWords());
			updateHangman();
			lblUsername.setText("Username: " + player.getUsername());
			lblLives.setText("Lives: " + (6 - game.getNumErrors()));
			tempWord = game.getWordToGuess();
			for (int i = 0; i < tempWord.length(); i++) {
				if (Character.isLetter(tempWord.charAt(i))) {
					lblWordToGuess.setText(lblWordToGuess.getText() + "_");
				}
				else {
					if (!Character.isLetter(tempWord.charAt(i)) && !Character.isWhitespace(tempWord.charAt(i))) {
						lblWordToGuess.setText(lblWordToGuess.getText() + tempWord.charAt(i));
					}
					else {
						lblWordToGuess.setText(lblWordToGuess.getText() + " ");
					}
				}
				lblWordToGuess.setText(lblWordToGuess.getText() + " ");
			}
		}
		else {
			game = player.getGame();
			if (!game.canGetHint()) {
				btnHint.setEnabled(false);
			}
			for (int i = 0; i < game.getWronglyGuessedLettersList().getLength(); i++) {
				incorrectGuess(btnLetters[game.getWronglyGuessedLettersList().getElementAt(i) - 65]); // Because of ascii codes for uppercase letters
			}
			this.setTitle("Hangman - Game " + (game.getTotalWords() - game.getNumWords()) + "/" + game.getTotalWords());
			updateHangman();
			lblUsername.setText("Username: " + player.getUsername());
			lblLives.setText("Lives: " + (6 - game.getNumErrors()));
			tempWord = game.getWordToGuess();

			for (int i = 0; i < tempWord.length(); i++) {
				if (Character.isLetter(tempWord.charAt(i))) {
					lblWordToGuess.setText(lblWordToGuess.getText() + "_");
				}
				else {
					if (!Character.isLetter(tempWord.charAt(i)) && !Character.isWhitespace(tempWord.charAt(i))) {
						lblWordToGuess.setText(lblWordToGuess.getText() + tempWord.charAt(i));
					}
					else {
						lblWordToGuess.setText(lblWordToGuess.getText() + " ");
					}
				}
				lblWordToGuess.setText(lblWordToGuess.getText() + " ");
			}
			SinglyLinkedList<Character> tempLetters = game.getCorrectlyGuessedLettersList();
			game.resetCorrectlyGuessedLetters();
			for (JButton btnLetter : btnLetters) {
				for (int i = 0; i < tempLetters.getLength(); i++) {
					if (btnLetter.getText().charAt(0) == tempLetters.getElementAt(i)) {
						btnLetter.doClick();
					}
				}
			}
		}
	}

	private void clearAll() {
		lblWordToGuess.setText("");
		lblWrongGuessesList.setText("");
		for (JButton btnLetter : btnLetters) {
			btnLetter.setUI(new javax.swing.plaf.basic.BasicButtonUI() {
				@Override
				public void paint(Graphics g, JComponent c) {
					g.setColor(Color.WHITE);
					g.fillRect(0, 0, c.getWidth(), c.getHeight());
					g.setColor(Color.BLACK);
					g.drawRect(0, 0, c.getWidth() - 1, c.getHeight() - 1);
					super.paint(g, c);
				}
			});
			btnLetter.setEnabled(true);
			btnHint.setEnabled(true);
		}
	} // clearAll()

	private void correctGuess(JButton btnLetter) {
		btnLetter.setUI(new javax.swing.plaf.basic.BasicButtonUI() {
			@Override
			public void paint(Graphics g, JComponent c) {
				g.setColor(Color.GREEN);
				g.fillRect(0, 0, c.getWidth(), c.getHeight());
				super.paint(g, c);
			}
		});
		btnLetter.setEnabled(false);
		StringBuilder tempLblWordToGuess;
		for (int i = 0; i < tempWord.length(); i++) {
			tempLblWordToGuess = new StringBuilder(lblWordToGuess.getText());
			if (tempWord.charAt(i) == btnLetter.getText().charAt(0)) {
				tempLblWordToGuess.setCharAt(i * 2, btnLetter.getText().charAt(0));
				lblWordToGuess.setText(tempLblWordToGuess.toString());
			}
		}
	} // correctGuess()

	private void incorrectGuess(JButton btnLetter) {
		updateHangman();
		lblWrongGuessesList.setText(lblWrongGuessesList.getText() + btnLetter.getText().charAt(0) + " ");
		btnLetter.setUI(new javax.swing.plaf.basic.BasicButtonUI() {
			@Override
			public void paint(Graphics g, JComponent c) {
				g.setColor(Color.RED);
				g.fillRect(0, 0, c.getWidth(), c.getHeight());
				super.paint(g, c);
			}
		});
		btnLetter.setEnabled(false);
	} // incorrectGuess()

	@Override
	public void actionPerformed(ActionEvent e) {
		for (JButton btnLetter : btnLetters) {
			if (e.getSource() == btnLetter) {
				char letter = btnLetter.getText().charAt(0);
				if (game.checkInput(letter)) {
					correctGuess(btnLetter);
				}
				else {
					incorrectGuess(btnLetter);
				}
				lblLives.setText("Lives: " + (6 - game.getNumErrors()));
				if (game.hasLost()) {
					JOptionPane.showMessageDialog(new JPanel(), "You lose. Word was: " + tempWord);
					if (game.hasMoreWords()) {
						JOptionPane.showMessageDialog(new JPanel(), "There are no words left.");
						System.exit(0);
					}
					scoreboard.gamePlayed(game.getPlayer().getUsername(), false);
					setupNewGame(game.getPlayer(), true);
				}
				if (game.hasWon()) {
					JOptionPane.showMessageDialog(new JPanel(), "You win");
					if (game.hasMoreWords()) {
						JOptionPane.showMessageDialog(new JPanel(), "There are no words left.");
						System.exit(0);
					}
					scoreboard.gamePlayed(game.getPlayer().getUsername(), true);
					setupNewGame(game.getPlayer(), true);
				}
				if (!game.canGetHint() || game.getNumberOfLetters() - 1 == game.getNumberOfGuessedLetters()) {
					btnHint.setEnabled(false);
				}
			}
		}
	}

	@Override
	public void windowOpened(WindowEvent e) {
	}

	@Override
	public void windowClosing(WindowEvent e) {
		try {
			scoreboard.saveGame();
			System.exit(0);
		}
		catch (IOException ex) {
			ex.printStackTrace();
		}
	}

	@Override
	public void windowClosed(WindowEvent e) {
	}

	@Override
	public void windowIconified(WindowEvent e) {
	}

	@Override
	public void windowDeiconified(WindowEvent e) {
	}

	@Override
	public void windowActivated(WindowEvent e) {
	}

	@Override
	public void windowDeactivated(WindowEvent e) {
	}
}
