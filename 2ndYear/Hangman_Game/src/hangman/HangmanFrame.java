package hangman;

import java.awt.*;
import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class HangmanFrame extends JFrame implements ActionListener {

	private static final long serialVersionUID = 1L;
	private String tempWord;

	private JButton[] btnLetters = new JButton[26];
	private JLabel lblWordToGuess;
	private JLabel lblWrongGuessesList;

	private HangmanGame game;

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
		setTitle("Hangman");
		setBounds(100, 100, 900, 600);
		setResizable(false);
		setLocationRelativeTo(null);
		setVisible(true);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		SpringLayout springLayout = new SpringLayout();
		getContentPane().setLayout(springLayout);
		
		JPanel letterPanel = new JPanel();
		springLayout.putConstraint(SpringLayout.NORTH, letterPanel, -90, SpringLayout.SOUTH, getContentPane());
		springLayout.putConstraint(SpringLayout.WEST, letterPanel, 10, SpringLayout.WEST, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, letterPanel, -10, SpringLayout.SOUTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, letterPanel, -10, SpringLayout.EAST, getContentPane());
		getContentPane().add(letterPanel);
		letterPanel.setLayout(new GridLayout(2, 13, 0, 0));
		
		lblWordToGuess = new JLabel("");
		springLayout.putConstraint(SpringLayout.SOUTH, lblWordToGuess, -154, SpringLayout.NORTH, letterPanel);
		springLayout.putConstraint(SpringLayout.EAST, lblWordToGuess, -274, SpringLayout.EAST, getContentPane());
		getContentPane().add(lblWordToGuess);
		
		JLabel lblWrongGuesses = new JLabel("Wrong Guesses:");
		springLayout.putConstraint(SpringLayout.NORTH, lblWrongGuesses, 84, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, lblWrongGuesses, -241, SpringLayout.EAST, getContentPane());
		getContentPane().add(lblWrongGuesses);
		
		lblWrongGuessesList = new JLabel("");
		springLayout.putConstraint(SpringLayout.NORTH, lblWrongGuessesList, 6, SpringLayout.SOUTH, lblWrongGuesses);
		springLayout.putConstraint(SpringLayout.WEST, lblWrongGuessesList, 0, SpringLayout.WEST, lblWrongGuesses);
		getContentPane().add(lblWrongGuessesList);
		
		char letter = 'A';
		for (int i = 0; i < btnLetters.length; i++) {
			btnLetters[i] = new JButton(Character.toString(letter + i));
			btnLetters[i].addActionListener(this);
			letterPanel.add(btnLetters[i]);
		}
		
		JMenuBar menuBar = new JMenuBar();
		setJMenuBar(menuBar);
		
		JMenu mnFile = new JMenu("File");
		menuBar.add(mnFile);
		
		JMenuItem mntmNewGame = new JMenuItem("New Game");
		mnFile.add(mntmNewGame);
		
		JMenuItem mntmScoreboard = new JMenuItem("Scoreboard");
		mnFile.add(mntmScoreboard);
		mntmScoreboard.addActionListener(e -> new ScoreboardFrame());
		
		JMenuItem mntmQuit = new JMenuItem("Quit");
		mnFile.add(mntmQuit);
		mntmQuit.addActionListener(e -> System.exit(0));
		
		JMenu mnHelp = new JMenu("Help");
		menuBar.add(mnHelp);
		
		JMenuItem mntmRules = new JMenuItem("Rules");
		mnHelp.add(mntmRules);
		mntmRules.addActionListener(e -> new RulesFrame());

		setupGame(username);
	}
	
	private void setupGame(String username) {
		Scoreboard scoreboard = new Scoreboard();
		game = new HangmanGame(scoreboard.getPlayer(username));
		// Not sure if belongs in front end or back end
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

		System.out.println(scoreboard.getPlayer(username).getUsername());
		System.out.println(game.getWordToGuess());
	}

	@Override
	public void actionPerformed(ActionEvent e) {
    for (JButton btnLetter : btnLetters) {
      if (e.getSource() == btnLetter) {
				char letter = btnLetter.getText().charAt(0);
        if (game.checkInput(letter)) {
          System.out.println("Correct");
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
						if (tempWord.charAt(i) == letter) {
							tempLblWordToGuess.setCharAt(i * 2, letter);
							lblWordToGuess.setText(tempLblWordToGuess.toString());
						}
					}
        } else {
          System.out.println("Incorrect");
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
        }
				if(game.hasLost()) {
					System.out.println("You lose");
					JOptionPane.showMessageDialog(new JPanel(), "You lose");
				}
				if(game.hasWon()) {
					System.out.println("You win");
					JOptionPane.showMessageDialog(new JPanel(), "You win");
				}
      }
    }
	}
}
