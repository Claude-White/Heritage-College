package hangman;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import javax.swing.SpringLayout;
import javax.swing.JButton;
import javax.swing.JTextArea;

public class ScoreboardFrame extends JFrame {

	private JPanel contentPane;

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
		sl_contentPane.putConstraint(SpringLayout.NORTH, btnExit, 10, SpringLayout.NORTH, contentPane);
		sl_contentPane.putConstraint(SpringLayout.EAST, btnExit, -10, SpringLayout.EAST, contentPane);
		contentPane.add(btnExit);
		
		JTextArea areaDisplay = new JTextArea();
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
		
	}
}
