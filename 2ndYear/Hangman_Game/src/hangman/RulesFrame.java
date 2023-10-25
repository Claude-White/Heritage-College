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
		setBounds(100, 100, 650, 660);
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
				"Object: One player thinks of a word and the other tries to guess it by\nguessing letters. Each incorrect guess brings you closer to being \"hanged.\"\nThis game helps to sharpen children’s spelling and word-decoding skills.\n\nDirections: Decide who is going first. Start the game by having this person\nchoose a word or phrase in their mind. (Choose a word from the book just\nread, and for quick reference to the word, use a bookmark to keep track of\nthe page it is located on.)\n\nPlace one dash on the bottom of the game board for each letter of the word\nor words chosen. Leave a space between words.\n\nIf the word is dog, draw three spaces, like this: __ __ __.\n\nHave the other player guess one letter at a time - or he or she can use a\nturn to guess the entire word or words.\n\nFill in the letter everywhere it appears on the appropriate dash (or dashes)\neach time the person guesses correctly. Circle the letter on the alphabet if\nis guessed correctly. Add one body part to the drawing each time the letter\nchosen is not in the word. Begin by drawing a head attached to the short\nvertical line (the \"noose\"). Add eyes, ears, nose, hair, body, legs, and arms.\nPut an X through the letter that was guessed and not correct. You may also\nwish to make your drawings very elaborate - one ear at a time, a neck, and\na belly button - so that children will have a lot of guesses before losing.\nIf the drawing of the person is completed before the word or words are\nguessed, the guessing player loses. If the player figures out the word or\nwords first, he or she wins.");
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
