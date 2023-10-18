package hangman;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JTextPane;
import java.awt.BorderLayout;
import javax.swing.JFormattedTextField;
import java.awt.Color;
import java.awt.Dimension;

import javax.swing.JEditorPane;
import javax.swing.JProgressBar;
import javax.swing.SwingConstants;
import javax.swing.JPasswordField;
import javax.swing.JLabel;
import java.awt.Font;
import javax.swing.SpringLayout;
import javax.swing.JMenuBar;
import javax.swing.JMenu;
import javax.swing.JMenuItem;
import javax.swing.JPanel;
import javax.swing.JButton;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class HangmanFrame extends JFrame implements ActionListener {

	private static final long serialVersionUID = 1L;
	private JButton[] btnLetters = new JButton[26];

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
		setupGame(username);
		
		
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
		
		JLabel lblWordToGuess = new JLabel("");
		springLayout.putConstraint(SpringLayout.SOUTH, lblWordToGuess, -154, SpringLayout.NORTH, letterPanel);
		springLayout.putConstraint(SpringLayout.EAST, lblWordToGuess, -274, SpringLayout.EAST, getContentPane());
		getContentPane().add(lblWordToGuess);
		
		JLabel lblWrongGuesses = new JLabel("Wrong Guesses:");
		springLayout.putConstraint(SpringLayout.NORTH, lblWrongGuesses, 84, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, lblWrongGuesses, -241, SpringLayout.EAST, getContentPane());
		getContentPane().add(lblWrongGuesses);
		
		JLabel lblWrongGuessesList = new JLabel("");
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
	}
	
	private void setupGame(String username) {
		Scoreboard scoreboard = new Scoreboard();
		HangmanGame game = new HangmanGame(scoreboard.getPlayer(username));
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		for (int i = 0; i < btnLetters.length; i++) {
			if (e.getSource() == btnLetters[i]) {
				//btnLetters[i].getText()
			}
		}
	}
}
