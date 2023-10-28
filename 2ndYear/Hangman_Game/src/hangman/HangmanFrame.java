package hangman;

import java.awt.*;

import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

public class HangmanFrame extends JFrame implements ActionListener {

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
	public HangmanFrame(String username) {
		getContentPane().setBackground(Color.WHITE);
		getContentPane().setFont(new Font("Monospaced", Font.PLAIN, 13));
		setTitle("Hangman");
		setBounds(100, 100, 899, 600);
		setResizable(false);
		setLocationRelativeTo(null);
		setVisible(true);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		SpringLayout springLayout = new SpringLayout();
		getContentPane().setLayout(springLayout);
		
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
		mntmNewGame.addActionListener(e -> setupNewGame(false));
		
		JMenuItem mntmScoreboard = new JMenuItem("Scoreboard");
		mntmScoreboard.setFont(new Font("Monospaced", Font.PLAIN, 14));
		mnFile.add(mntmScoreboard);
		mntmScoreboard.addActionListener(e -> new ScoreboardFrame());
		
		JMenuItem mntmQuit = new JMenuItem("Quit");
		mntmQuit.setFont(new Font("Monospaced", Font.PLAIN, 14));
		mnFile.add(mntmQuit);
		mntmQuit.addActionListener(e -> System.exit(0));
		
		JMenu mnHelp = new JMenu("Help");
		mnHelp.setFont(new Font("Monospaced", Font.PLAIN, 14));
		menuBar.add(mnHelp);
		
		JMenuItem mntmRules = new JMenuItem("Rules");
		mntmRules.setFont(new Font("Monospaced", Font.PLAIN, 14));
		mnHelp.add(mntmRules);
		mntmRules.addActionListener(e -> new RulesFrame());

		setupNewGame(username);
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
	
	private void setupNewGame(String username) {
		clearAll();
		scoreboard = new Scoreboard();
		game = new HangmanGame(scoreboard.getPlayer(username));
		this.setTitle("Hangman - Game " + (game.getTotalWords() - game.getNumWords()) + "/" + game.getTotalWords());
		updateHangman();
		lblUsername.setText("Username: " + username);
		lblLives.setText("Lives: " + (6 - game.getNumErrors()));
		tempWord = game.getWordToGuess();
		for (int i = 0; i < tempWord.length(); i++) {
			if (Character.isLetter(tempWord.charAt(i))) {
				lblWordToGuess.setText(lblWordToGuess.getText() + "_");
			} else if (!Character.isLetter(tempWord.charAt(i))&& !Character.isWhitespace(tempWord.charAt(i))) {
				lblWordToGuess.setText(lblWordToGuess.getText() + tempWord.charAt(i));
			} else {
				lblWordToGuess.setText(lblWordToGuess.getText() + " ");
			}
			lblWordToGuess.setText(lblWordToGuess.getText() + " ");
		}
	}

	private void setupNewGame(boolean hasWon) {
		scoreboard.gamePlayed(game.getPlayer().getUsername(), hasWon);
		clearAll();
		scoreboard = new Scoreboard();
		game = new HangmanGame();
		this.setTitle("Hangman - Game " + (game.getTotalWords() - game.getNumWords()) + "/" + game.getTotalWords());
		updateHangman();
		lblLives.setText("Lives: " + (6 - game.getNumErrors()));
		tempWord = game.getWordToGuess();
		for (int i = 0; i < tempWord.length(); i++) {
			if (Character.isLetter(tempWord.charAt(i))) {
				lblWordToGuess.setText(lblWordToGuess.getText() + "_");
			} else if (!Character.isLetter(tempWord.charAt(i))&& !Character.isWhitespace(tempWord.charAt(i))) {
				lblWordToGuess.setText(lblWordToGuess.getText() + tempWord.charAt(i));
			} else {
				lblWordToGuess.setText(lblWordToGuess.getText() + " ");
			}
			lblWordToGuess.setText(lblWordToGuess.getText() + " ");
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

	@Override
	public void actionPerformed(ActionEvent e) {
    for (JButton btnLetter : btnLetters) {
      if (e.getSource() == btnLetter) {
				char letter = btnLetter.getText().charAt(0);
        if (game.checkInput(letter)) {
					btnLetter.setUI(new javax.swing.plaf.basic.BasicButtonUI() {
						@Override
						public void paint(Graphics g, JComponent c) {
							g.setColor(Color.GREEN);
							g.fillRect(0, 0, c.getWidth(), c.getHeight());
							super.paint(g, c);
						}
					});
					btnLetter.setEnabled(false);
					lblLives.setText("Lives: " + (6 - game.getNumErrors()));
					StringBuilder tempLblWordToGuess;
					for (int i = 0; i < tempWord.length(); i++) {
						tempLblWordToGuess = new StringBuilder(lblWordToGuess.getText());
						if (tempWord.charAt(i) == letter) {
							tempLblWordToGuess.setCharAt(i * 2, letter);
							lblWordToGuess.setText(tempLblWordToGuess.toString());
						}
					}
        } else {
        	updateHangman();
					lblWrongGuessesList.setText(lblWrongGuessesList.getText() + letter + " ");
					btnLetter.setUI(new javax.swing.plaf.basic.BasicButtonUI() {
						@Override
						public void paint(Graphics g, JComponent c) {
							g.setColor(Color.RED);
							g.fillRect(0, 0, c.getWidth(), c.getHeight());
							super.paint(g, c);
						}
					});
					btnLetter.setEnabled(false);
					lblLives.setText("Lives: " + (6 - game.getNumErrors()));
        }
				if(game.hasLost()) {
					JOptionPane.showMessageDialog(new JPanel(), "You lose. Word was: " + tempWord);
					if (game.hasMoreWords()) {
						JOptionPane.showMessageDialog(new JPanel(), "There are no words left.");
						System.exit(0);
					}
					setupNewGame(false);
				}
				if(game.hasWon()) {
					JOptionPane.showMessageDialog(new JPanel(), "You win");
					if (game.hasMoreWords()) {
						JOptionPane.showMessageDialog(new JPanel(), "There are no words left.");
						System.exit(0);
					}
					setupNewGame(false);
				}
				if (!game.canGetHint() || game.getNumberOfLetters() - 1 == game.getNumberOfGuessedLetters()) {
					btnHint.setEnabled(false);
				}
      }
    }
	}
}
