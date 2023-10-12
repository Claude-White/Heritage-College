package hangman;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import javax.swing.JButton;
import javax.swing.SpringLayout;
import javax.swing.JTextField;
import javax.swing.JLabel;

public class LoginFrame extends JFrame {

	private JPanel contentPane;
	private JTextField fldUsername;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					LoginFrame frame = new LoginFrame();
					frame.setVisible(true);
				}
				catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

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
		
		fldUsername = new JTextField();
		springLayout.putConstraint(SpringLayout.WEST, fldUsername, 188, SpringLayout.WEST, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, fldUsername, -227, SpringLayout.SOUTH, getContentPane());
		getContentPane().add(fldUsername);
		fldUsername.setColumns(10);
		
		JLabel lblUsername = new JLabel("Username:");
		springLayout.putConstraint(SpringLayout.NORTH, lblUsername, 5, SpringLayout.NORTH, fldUsername);
		springLayout.putConstraint(SpringLayout.SOUTH, lblUsername, -5, SpringLayout.SOUTH, fldUsername);
		springLayout.putConstraint(SpringLayout.EAST, lblUsername, -6, SpringLayout.WEST, fldUsername);
		getContentPane().add(lblUsername);
	}
}
