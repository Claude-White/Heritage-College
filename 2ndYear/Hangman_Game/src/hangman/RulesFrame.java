package hangman;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import javax.swing.JTextArea;
import javax.swing.SpringLayout;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.SwingConstants;
import java.awt.Font;
import javax.swing.JScrollBar;

public class RulesFrame extends JFrame {

	private JPanel contentPane;

	/**
	 * Create the frame.
	 */
	public RulesFrame() {
		setTitle("Rules");
		setBounds(100, 100, 600, 400);
		setResizable(false);
		setLocationRelativeTo(null);
		setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
		setVisible(true);
		contentPane = new JPanel();
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));

		setContentPane(contentPane);
		SpringLayout sl_contentPane = new SpringLayout();
		contentPane.setLayout(sl_contentPane);

		JTextArea areaDisplay = new JTextArea();
		areaDisplay.setFont(new Font("Monospaced", Font.PLAIN, 16));
		areaDisplay.setWrapStyleWord(true);
		areaDisplay.setText(
				"The game randomly selects a word and the\nplayer tries to guess it by guessing letters. Each\nincorrect guess brings you closer to being \"hanged\".\n\nIf the word is dog, the spaces are drawn\nlike this: _ _ _.\n\nThe player has to guess one letter at a time. The letter\nis filled in everywhere it appears on the appropriate\ndash (or dashes) each time the person guesses\ncorrectly. One body part is added to the drawing each\ntime the letter chosen is not in the word. If the\ndrawing of the person is completed before the word\nis guessed, the player loses. If the player figures out\nthe word first, they win.");
		areaDisplay.setEditable(false);
		sl_contentPane.putConstraint(SpringLayout.WEST, areaDisplay, 10,
				SpringLayout.WEST, contentPane);
		sl_contentPane.putConstraint(SpringLayout.SOUTH, areaDisplay, -10,
				SpringLayout.SOUTH, contentPane);
		sl_contentPane.putConstraint(SpringLayout.EAST, areaDisplay, -10,
				SpringLayout.EAST, contentPane);
		contentPane.add(areaDisplay);

		JButton btnExit = new JButton("Exit");
		btnExit.setFont(new Font("Monospaced", Font.PLAIN, 13));
		sl_contentPane.putConstraint(SpringLayout.NORTH, areaDisplay, 10,
				SpringLayout.SOUTH, btnExit);
		sl_contentPane.putConstraint(SpringLayout.NORTH, btnExit, 10,
				SpringLayout.NORTH, contentPane);
		sl_contentPane.putConstraint(SpringLayout.EAST, btnExit, -10,
				SpringLayout.EAST, contentPane);
		contentPane.add(btnExit);
		btnExit.addActionListener(e -> btnExit());

		JLabel lblTitle = new JLabel("List of Players");
		lblTitle.setFont(new Font("Monospaced", Font.PLAIN, 13));
		sl_contentPane.putConstraint(SpringLayout.EAST, lblTitle, -10,
				SpringLayout.EAST, contentPane);
		lblTitle.setHorizontalAlignment(SwingConstants.CENTER);
		sl_contentPane.putConstraint(SpringLayout.NORTH, lblTitle, 0,
				SpringLayout.NORTH, btnExit);
		sl_contentPane.putConstraint(SpringLayout.WEST, lblTitle, 10,
				SpringLayout.WEST, contentPane);
		sl_contentPane.putConstraint(SpringLayout.SOUTH, lblTitle, -10,
				SpringLayout.NORTH, areaDisplay);
		contentPane.add(lblTitle);
	}

	private void btnExit() {
		this.dispose();
	}
}
