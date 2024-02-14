package hangman;

import linked_data_structures.DoublyLinkedList;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import javax.swing.SpringLayout;
import javax.swing.JButton;
import javax.swing.JTextArea;
import java.awt.Font;

public class ScoreboardFrame extends JFrame {

	private JPanel contentPane;
	private JTextArea areaDisplay;

	/**
	 * Create the frame.
	 */
	public ScoreboardFrame() {
		setTitle("Scoreboard");
		setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
		setBounds(100, 100, 450, 300);
		setResizable(false);
		setLocationRelativeTo(null);
		setVisible(true);
		contentPane = new JPanel();
		setContentPane(contentPane);
		SpringLayout sl_contentPane = new SpringLayout();
		contentPane.setLayout(sl_contentPane);
		
		JButton btnExit = new JButton("Exit");
		btnExit.setFont(new Font("Monospaced", Font.PLAIN, 13));
		sl_contentPane.putConstraint(SpringLayout.NORTH, btnExit, 10, SpringLayout.NORTH, contentPane);
		sl_contentPane.putConstraint(SpringLayout.EAST, btnExit, -10, SpringLayout.EAST, contentPane);
		contentPane.add(btnExit);
		
		areaDisplay = new JTextArea();
		areaDisplay.setFont(new Font("Monospaced", Font.PLAIN, 13));
		areaDisplay.setEditable(false);
		sl_contentPane.putConstraint(SpringLayout.NORTH, areaDisplay, 10, SpringLayout.SOUTH, btnExit);
		sl_contentPane.putConstraint(SpringLayout.WEST, areaDisplay, 10, SpringLayout.WEST, contentPane);
		sl_contentPane.putConstraint(SpringLayout.SOUTH, areaDisplay, -10, SpringLayout.SOUTH, contentPane);
		sl_contentPane.putConstraint(SpringLayout.EAST, areaDisplay, -10, SpringLayout.EAST, contentPane);
		contentPane.add(areaDisplay);
		btnExit.addActionListener(e -> btnExit());
		displayScoreboard();
	}
	
	private void btnExit() {
		this.dispose();
	}
	
	private void displayScoreboard() {
		areaDisplay.setText(String.format("%-23s%10s%20s\n", "Username", "Wins", "Games Played"));
		areaDisplay.append(String.format("%-23s%10s%20s\n", "------------", "--------", "----------------"));
		Scoreboard scoreboard = new Scoreboard();
		DoublyLinkedList<Player> players = scoreboard.getPlayers();
		for (int i = 0; i < players.getLength(); i++) {
			areaDisplay.append(String.format("%-23s%10s%20s\n", players.getElementAt(i).getUsername(), players.getElementAt(i).getNumGamesWon(), players.getElementAt(i).getNumGamesPlayed()));
		}
	}
}
