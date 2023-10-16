package hangman;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import javax.swing.JButton;
import javax.swing.SpringLayout;
import javax.swing.JTextField;
import javax.swing.JLabel;
import javax.swing.JComboBox;

public class LoginFrame extends JFrame {

	private JPanel contentPane;
	private JTextField fldUsername;

	/**
	 * Create the frame.
	 */
	public LoginFrame() {
		setTitle("Login");
		setBounds(100, 100, 900, 600);
		setResizable(false);
		setLocationRelativeTo(null);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		SpringLayout springLayout = new SpringLayout();
		getContentPane().setLayout(springLayout);
		
		JButton btnExit = new JButton("Exit");
		springLayout.putConstraint(SpringLayout.NORTH, btnExit, 10, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, btnExit, 60, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, btnExit, -10, SpringLayout.EAST, getContentPane());
		getContentPane().add(btnExit);
		btnExit.addActionListener(e -> btnExit());
		
		JButton btnScoreboard = new JButton("Scoreboard");
		springLayout.putConstraint(SpringLayout.NORTH, btnScoreboard, 10, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, btnScoreboard, 60, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, btnScoreboard, -10, SpringLayout.WEST, btnExit);
		getContentPane().add(btnScoreboard);
		
		JButton btnStart = new JButton("Start");
		springLayout.putConstraint(SpringLayout.NORTH, btnStart, -60, SpringLayout.SOUTH, getContentPane());
		springLayout.putConstraint(SpringLayout.WEST, btnStart, 375, SpringLayout.WEST, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, btnStart, -10, SpringLayout.SOUTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, btnStart, -375, SpringLayout.EAST, getContentPane());
		getContentPane().add(btnStart);
		btnStart.addActionListener(e -> btnStart());
		
		fldUsername = new JTextField();
		springLayout.putConstraint(SpringLayout.NORTH, fldUsername, 319, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, fldUsername, -516, SpringLayout.EAST, getContentPane());
		getContentPane().add(fldUsername);
		fldUsername.setColumns(10);
		
		JLabel lblUsername = new JLabel("Username:");
		springLayout.putConstraint(SpringLayout.WEST, fldUsername, 6, SpringLayout.EAST, lblUsername);
		springLayout.putConstraint(SpringLayout.EAST, lblUsername, -680, SpringLayout.EAST, getContentPane());
		springLayout.putConstraint(SpringLayout.NORTH, lblUsername, 324, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, lblUsername, -232, SpringLayout.SOUTH, getContentPane());
		getContentPane().add(lblUsername);
		
		JComboBox cmbxSavedPlayers = new JComboBox();
		springLayout.putConstraint(SpringLayout.NORTH, cmbxSavedPlayers, 319, SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.WEST, cmbxSavedPlayers, 588, SpringLayout.WEST, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, cmbxSavedPlayers, 28, SpringLayout.NORTH, fldUsername);
		springLayout.putConstraint(SpringLayout.EAST, cmbxSavedPlayers, -154, SpringLayout.EAST, getContentPane());
		getContentPane().add(cmbxSavedPlayers);
	}
	
	private void btnExit() {
		System.exit(0);
	} // end btnExit

	private void btnScoreboard() {
		// display scoreboard
	} // btnScoreboard()

	private void btnStart() {
		createPlayer();
		HangmanFrame gameFrame = new HangmanFrame();
		gameFrame.setVisible(true);
		gameFrame.setTitle("Hangman");
		gameFrame.setBounds(100, 100, 900, 600);
		gameFrame.setResizable(false);
		gameFrame.setLocationRelativeTo(null);
		gameFrame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setVisible(false);
		this.dispose();
	} // btnStart()

	private void createPlayer() {
		Scoreboard scoreboard = new Scoreboard();
		scoreboard.addPlayer(fldUsername.getText().trim());
		scoreboard.sortScoreboard();
	}	// createPlayer()
	
}
