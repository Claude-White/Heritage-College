package hangman;

import linked_data_structures.DoublyLinkedList;

import java.awt.EventQueue;
import javax.swing.*;
import javax.imageio.ImageIO;
import java.awt.Font;
import java.awt.Image;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.awt.Color;
import java.awt.Component;
import java.awt.event.KeyListener;
import java.awt.event.KeyEvent;

public class LoginFrame extends JFrame {

	private JPanel contentPane;
	private JTextField fldUsername;
	private BufferedImage originalImage;
	private JLabel lblHangmanImg;
	private JLabel lblTitle;
	private JComboBox cmbxSavedPlayers;
	private DoublyLinkedList<Player> players;
	private boolean easterEgg = true;

	/**
	 * Create the frame.
	 */
	public LoginFrame() {
		getContentPane().setBackground(Color.WHITE);
		setTitle("Login");
		setBounds(100, 100, 900, 600);
		setResizable(false);
		setLocationRelativeTo(null);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		SpringLayout springLayout = new SpringLayout();
		getContentPane().setLayout(springLayout);
		this.addKeyListener(new KeyListener() {
			@Override
			public void keyPressed(KeyEvent e) {
				int keyCode = e.getKeyCode();
				if (KeyEvent.getKeyText(keyCode).equalsIgnoreCase("i") && easterEgg) {
					updateTitle("Full");
					easterEgg = false;
				}
				if (!KeyEvent.getKeyText(keyCode).equalsIgnoreCase("i") && easterEgg) {
					updateHangman("0");
					easterEgg = false;
				}
			}

			@Override
			public void keyTyped(KeyEvent e) {
			}

			@Override
			public void keyReleased(KeyEvent e) {
			}
		});
		setFocusable(true);
		requestFocusInWindow();

		JButton btnExit = new JButton("Exit");
		btnExit.setFont(new Font("Monospaced", Font.PLAIN, 13));
		springLayout.putConstraint(SpringLayout.NORTH, btnExit, 10,
				SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, btnExit, 60,
				SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, btnExit, -10,
				SpringLayout.EAST, getContentPane());
		getContentPane().add(btnExit);
		btnExit.addActionListener(e -> btnExit());

		JButton btnScoreboard = new JButton("Scoreboard");
		btnScoreboard.setFont(new Font("Monospaced", Font.PLAIN, 13));
		springLayout.putConstraint(SpringLayout.NORTH, btnScoreboard, 10,
				SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, btnScoreboard, 60,
				SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, btnScoreboard, -10,
				SpringLayout.WEST, btnExit);
		getContentPane().add(btnScoreboard);
		btnScoreboard.addActionListener(e -> new ScoreboardFrame());

		JButton btnStart = new JButton("Start");
		btnStart.setFont(new Font("Monospaced", Font.PLAIN, 13));
		springLayout.putConstraint(SpringLayout.NORTH, btnStart, -60,
				SpringLayout.SOUTH, getContentPane());
		springLayout.putConstraint(SpringLayout.WEST, btnStart, 375,
				SpringLayout.WEST, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, btnStart, -10,
				SpringLayout.SOUTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, btnStart, -375,
				SpringLayout.EAST, getContentPane());
		getContentPane().add(btnStart);
		btnStart.addActionListener(e -> btnStart());

		cmbxSavedPlayers = new JComboBox();
		cmbxSavedPlayers.setRenderer(new DefaultListCellRenderer() {
			@Override
			public Component getListCellRendererComponent(JList<?> list, Object value,
					int index, boolean isSelected, boolean cellHasFocus) {
				Component c = super.getListCellRendererComponent(list, value, index,
						isSelected, cellHasFocus);
				if (isSelected) {
					c.setForeground(Color.RED);
				}
				else {
					c.setForeground(Color.BLACK);
				}
				return c;
			}
		});
		springLayout.putConstraint(SpringLayout.EAST, cmbxSavedPlayers, -109,
				SpringLayout.EAST, getContentPane());
		cmbxSavedPlayers.setModel(
				new DefaultComboBoxModel(new String[] { "Players" }));
		cmbxSavedPlayers.setForeground(new Color(238, 238, 238));
		cmbxSavedPlayers.setFont(new Font("Monospaced", Font.PLAIN, 13));
		getContentPane().add(cmbxSavedPlayers);

		lblHangmanImg = new JLabel("");
		lblHangmanImg.setSize(200, 200);
		springLayout.putConstraint(SpringLayout.NORTH, lblHangmanImg, 50,
				SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.WEST, lblHangmanImg, 50,
				SpringLayout.WEST, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, lblHangmanImg, 250,
				SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, lblHangmanImg, -650,
				SpringLayout.EAST, getContentPane());
		lblHangmanImg.setHorizontalAlignment(SwingConstants.CENTER);
		updateHangman("1");
		getContentPane().add(lblHangmanImg);

		JLabel lblNewPlayer = new JLabel("New Player");
		springLayout.putConstraint(SpringLayout.WEST, lblNewPlayer, 100,
				SpringLayout.WEST, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, lblNewPlayer, -536,
				SpringLayout.EAST, getContentPane());
		lblNewPlayer.setHorizontalAlignment(SwingConstants.CENTER);
		getContentPane().add(lblNewPlayer);

		JPanel newPlayerPanel = new JPanel();
		springLayout.putConstraint(SpringLayout.NORTH, cmbxSavedPlayers, 0,
				SpringLayout.NORTH, newPlayerPanel);
		springLayout.putConstraint(SpringLayout.WEST, cmbxSavedPlayers, 180,
				SpringLayout.EAST, newPlayerPanel);
		springLayout.putConstraint(SpringLayout.SOUTH, cmbxSavedPlayers, 0,
				SpringLayout.SOUTH, newPlayerPanel);
		springLayout.putConstraint(SpringLayout.NORTH, newPlayerPanel, 319,
				SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, lblNewPlayer, -6,
				SpringLayout.NORTH, newPlayerPanel);
		springLayout.putConstraint(SpringLayout.WEST, newPlayerPanel, 0,
				SpringLayout.WEST, lblNewPlayer);
		springLayout.putConstraint(SpringLayout.EAST, newPlayerPanel, 0,
				SpringLayout.EAST, lblNewPlayer);
		newPlayerPanel.setBorder(null);
		newPlayerPanel.setBackground(Color.WHITE);
		getContentPane().add(newPlayerPanel);

		JLabel lblUsername = new JLabel("Username:");
		newPlayerPanel.add(lblUsername);
		lblUsername.setFont(new Font("Monospaced", Font.PLAIN, 13));
		springLayout.putConstraint(SpringLayout.EAST, lblUsername, -680,
				SpringLayout.EAST, getContentPane());
		springLayout.putConstraint(SpringLayout.NORTH, lblUsername, 324,
				SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, lblUsername, -232,
				SpringLayout.SOUTH, getContentPane());
		springLayout.putConstraint(SpringLayout.WEST, lblUsername, 6,
				SpringLayout.EAST, getContentPane());

		fldUsername = new JTextField();
		newPlayerPanel.add(fldUsername);
		fldUsername.setBackground(new Color(238, 238, 238));
		fldUsername.setFont(new Font("Monospaced", Font.PLAIN, 13));
		springLayout.putConstraint(SpringLayout.NORTH, fldUsername, 319,
				SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, fldUsername, -516,
				SpringLayout.EAST, getContentPane());
		fldUsername.setColumns(20);

		JLabel lblSavedPlayers = new JLabel("Saved Players");
		springLayout.putConstraint(SpringLayout.WEST, lblSavedPlayers, 0,
				SpringLayout.WEST, cmbxSavedPlayers);
		springLayout.putConstraint(SpringLayout.SOUTH, lblSavedPlayers, -6,
				SpringLayout.NORTH, cmbxSavedPlayers);
		springLayout.putConstraint(SpringLayout.EAST, lblSavedPlayers, 0,
				SpringLayout.EAST, cmbxSavedPlayers);
		lblSavedPlayers.setHorizontalAlignment(SwingConstants.CENTER);
		getContentPane().add(lblSavedPlayers);

		lblTitle = new JLabel("");
		lblTitle.setSize(300, 60);
		lblTitle.setHorizontalAlignment(SwingConstants.CENTER);
		springLayout.putConstraint(SpringLayout.NORTH, lblTitle, 60,
				SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.WEST, lblTitle, 300,
				SpringLayout.WEST, getContentPane());
		springLayout.putConstraint(SpringLayout.SOUTH, lblTitle, 120,
				SpringLayout.NORTH, getContentPane());
		springLayout.putConstraint(SpringLayout.EAST, lblTitle, -300,
				SpringLayout.EAST, getContentPane());
		updateTitle("");
		getContentPane().add(lblTitle);
		addSavedPlayers();
	}

	private void addSavedPlayers() {
		File file = new File("./save_game.ser");
		if (!file.exists() || file.length() == 0) {
			return;
		}
		Scoreboard sc = new Scoreboard();
		try {
			players = sc.loadGames();
			String[] savedPlayers = new String[players.getLength() + 1];
			savedPlayers[0] = "Players";
			for (int i = 0; i < players.getLength(); i++) {
				savedPlayers[i + 1] = players.getElementAt(i).getUsername();
			}
			cmbxSavedPlayers.setModel(new DefaultComboBoxModel(savedPlayers));
		}
		catch (IOException e) {
			e.printStackTrace();
		}
		catch (ClassNotFoundException e) {
			e.printStackTrace();
		}
	} // addSavedPlayers()

	private void updateTitle(String title) {
		BufferedImage originalImage;
		try {
			originalImage = ImageIO.read(new File("./Images/" + title + "Title.png"));
			int labelWidth = lblTitle.getWidth();
			int labelHeight = lblTitle.getHeight();
			double scaleFactor = Math.min(
					(double) labelWidth / originalImage.getWidth(),
					(double) labelHeight / originalImage.getHeight());
			int scaledWidth = (int) (originalImage.getWidth() * scaleFactor);
			int scaledHeight = (int) (originalImage.getHeight() * scaleFactor);
			Image scaledImage = originalImage.getScaledInstance(scaledWidth,
					scaledHeight, Image.SCALE_SMOOTH);
			ImageIcon hangmanImg = new ImageIcon(scaledImage);
			lblTitle.setIcon(hangmanImg);
		}
		catch (IOException e1) {
			JOptionPane.showMessageDialog(new JPanel(),
					"Hangman image could not be found.");
			lblTitle.setIcon(null);
		}
	}

	private void updateHangman(String num) {
		BufferedImage originalImage;
		try {
			originalImage = ImageIO.read(new File("./Images/" + num + "Lives.png"));
			int labelWidth = lblHangmanImg.getWidth();
			int labelHeight = lblHangmanImg.getHeight();
			double scaleFactor = Math.min(
					(double) labelWidth / originalImage.getWidth(),
					(double) labelHeight / originalImage.getHeight());
			int scaledWidth = (int) (originalImage.getWidth() * scaleFactor);
			int scaledHeight = (int) (originalImage.getHeight() * scaleFactor);
			Image scaledImage = originalImage.getScaledInstance(scaledWidth,
					scaledHeight, Image.SCALE_SMOOTH);
			ImageIcon hangmanImg = new ImageIcon(scaledImage);
			lblHangmanImg.setIcon(hangmanImg);
		}
		catch (IOException e1) {
			JOptionPane.showMessageDialog(new JPanel(),
					"Hangman image could not be found.");
			lblHangmanImg.setIcon(null);
		}
	}

	private void btnExit() {
		System.exit(0);
	} // end btnExit

	private void btnStart() {
		if (cmbxSavedPlayers.getSelectedIndex() != 0) {
			Player player = players.getElementAt(
					cmbxSavedPlayers.getSelectedIndex() - 1);
			new HangmanFrame(player, false);
			this.setVisible(false);
			this.dispose();
		}
		else {
			Scoreboard scoreboard = new Scoreboard();
			String username = fldUsername.getText().trim();
			if (scoreboard.addPlayer(username)) {
				new HangmanFrame(scoreboard.getPlayer(username), false);
				this.setVisible(false);
				this.dispose();
			}
			else {
				JOptionPane.showMessageDialog(new JPanel(), "Please enter a username.");
			}
		}
	} // btnStart()
}
