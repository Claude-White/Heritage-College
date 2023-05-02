class User {
  fName;
  lName;
  username;
  phoneNumber;
  city;
  email;
  bank;

  constructor(fName, lName, username, phoneNumber, city, email, bank) {
    this.fName = fName;
    this.lName = lName;
    this.username = username;
    this.phoneNumber = phoneNumber;
    this.city = city;
    this.email = email;
    this.bank = bank;
  }

  getFullName() {
    return `${this.lName} ${this.fName}`;
  }

  get getBalance() {
    return this.bank;
  }
}

class Dice {
  dice1;
  dice2;
  diceTotal;
  constructor(diceRoll) {
    if (typeof diceRoll === "undefined") {
      this.rollDice();
    } else {
      this.diceTotal = diceRoll;
    }
  }

  get getDice1() {
    return this.dice1;
  }

  get getDice2() {
    return this.dice2;
  }

  rollDice() {
    this.dice1 = Math.floor(Math.random() * 6) + 1;
    this.dice2 = Math.floor(Math.random() * 6) + 1;
    this.diceTotal = this.dice1 + this.dice2;
  }

  get getDiceTotal() {
    return this.diceTotal;
  }
}

class Game {
  user;
  dice;

  betAmount;
  main;
  chance;

  constructor(user, dice) {
    this.user = user;
    this.dice = dice;
  }

  placeBetAmount(bet) {
    if (bet <= this.user.getBalance && bet > 0) {
      this.betAmount = bet;
      return true;
    } else {
      return false;
    }
  }

  betMain(bet) {
    if (bet <= 9 && bet >= 5) {
      this.main = bet;
      return true;
    } else {
      return false;
    }
  }

  increaseBalance() {
    return (this.user.bank += this.betAmount);
  }

  decreaseBalance() {
    return (this.user.bank -= this.betAmount);
  }

  hasWon() {
    if (this.main == this.dice.getDiceTotal) {
      return 1;
    }
    if (this.dice.getDiceTotal == 2 || this.dice.getDiceTotal == 3) {
      return 0;
    }
    if (this.dice.getDiceTotal == 11) {
      if (this.main == 5 || this.main == 6 || this.main == 8 || this.main == 9) {
        return 0;
      }
      if (this.main == 7) {
        return 1;
      }
    }
    if (this.dice.getDiceTotal == 12) {
      if (this.main == 5 || this.main == 7 || this.main == 9) {
        return 0;
      }
      if (this.main == 6 || this.main == 8) {
        return 1;
      }
    }
    return 2;
  }

  set setChance(chance) {
    this.chance = chance;
  }

  playChance() {
    if (this.chance == this.dice.getDiceTotal) {
      return 1;
    }
    if (this.main == this.dice.getDiceTotal) {
      return 0;
    }
    return 2;
  }
}

const logo = document.querySelector("#logo");
logo.src = `images/Logos/logo${Number(localStorage.getItem("profilePicNum"))}.png`;

const form = document.querySelector("#gameForm");
const fName = document.querySelector("#fName");
const lName = document.querySelector("#lName");
const balance = document.querySelector("#balance");
const username = document.querySelector("#username");
const phoneNum = document.querySelector("#phoneNum");
const city = document.querySelector("#city");
const emailAddress = document.querySelector("#emailAddress");

fName.textContent = `${localStorage.getItem("firstName")}`;
lName.textContent = `${localStorage.getItem("lastName")}`;
balance.textContent = `$${localStorage.getItem("bank")}`;
username.textContent = `Username: ${localStorage.getItem("username")}`;
phoneNum.textContent = `Phone: ${localStorage.getItem("phoneNumber")}`;
city.textContent = `City: ${localStorage.getItem("city")}`;
emailAddress.textContent = `Email: ${localStorage.getItem("email")}`;

const bet = document.querySelector("#bet");
const main = document.querySelector("#main");
const btnRoll = document.querySelector("#roll");
const diceLeft = document.querySelector("#diceLeft");
const diceRight = document.querySelector("#diceRight");
const gameStatus = document.querySelector("#status");
const chanceNum = document.querySelector("#chance");

let game = new Game(
  new User(
    localStorage.getItem("firstName"),
    localStorage.getItem("lastName"),
    localStorage.getItem("username"),
    localStorage.getItem("phoneNumber"),
    localStorage.getItem("city"),
    localStorage.getItem("email"),
    Number(localStorage.getItem("bank"))
  ),
  new Dice()
);

bet.addEventListener("change", isBetValid);

function isBetValid() {
  const regBet = /^[0-9]+$/;
  if (!bet.value || !regBet.test(bet.value) || Number(bet.value) > game.user.getBalance) {
    console.log("Error with bet.");
    return false;
  }
  return true;
}

main.addEventListener("change", isMainValid);

function isMainValid() {
  const regMain = /^[5-9]$/;
  if (!main.value || !regMain.test(main.value)) {
    console.log("Error with bet.");
    return false;
  }
  return true;
}

function areInputsValid() {
  if (isBetValid() && isMainValid()) {
    game.placeBetAmount(Number(bet.value));
    game.betMain(Number(main.value));
    return true;
  }
  return false;
}

let rolledChance = false;

btnRoll.addEventListener("click", () => {
  if (areInputsValid()) {
    updateGameState();
  }
});

function updateGameState() {
  if (rolledChance == false) {
    game.dice.rollDice();
    diceLeft.style.visibility = "visible";
    diceRight.style.visibility = "visible";
    diceLeft.src = `images/Dice/Dice${game.dice.getDice1}.png`;
    diceRight.src = `images/Dice/Dice${game.dice.getDice2}.png`;
    console.log(game.dice.getDiceTotal);
    if (game.hasWon() === 1) {
      gameStatus.style.visibility = "visible";
      gameStatus.textContent = "You won, balance increased!";
      game.increaseBalance();
      rolledChance = false;
    } else if (game.hasWon() === 0) {
      gameStatus.style.visibility = "visible";
      gameStatus.textContent = "You lost, balance decreased...";
      game.decreaseBalance();
      rolledChance = false;
    } else {
      gameStatus.style.visibility = "visible";
      gameStatus.textContent = "Went to chance, roll again";
      rolledChance = true;
      game.setChance = game.dice.getDiceTotal;
      chanceNum.textContent = `Chance: ${game.chance}`;
      chanceNum.style.visibility = "visible";
      return;
    }
  } else {
    game.dice.rollDice();
    diceLeft.src = `images/Dice/Dice${game.dice.getDice1}.png`;
    diceRight.src = `images/Dice/Dice${game.dice.getDice2}.png`;
    if (game.playChance() === 1) {
      gameStatus.style.visibility = "visible";
      gameStatus.textContent = "You won, balance increased!";
      game.increaseBalance();
      rolledChance = false;
    } else if (game.playChance() === 0) {
      gameStatus.style.visibility = "visible";
      gameStatus.textContent = "You lost, balance decreased...";
      game.decreaseBalance();
      rolledChance = false;
    } else {
      gameStatus.style.visibility = "visible";
      gameStatus.textContent = "Keep rolling";
      rolledChance = true;
      return;
    }
  }
  chanceNum.style.visibility = "hidden";
  if (game.user.getBalance < 1) {
    localStorage.setItem("bank", game.user.getBalance);
    balance.textContent = `$${localStorage.getItem("bank")}`;
    window.location.href = "lost.html";
  } else {
    localStorage.setItem("bank", game.user.getBalance);
    balance.textContent = `$${localStorage.getItem("bank")}`;
  }
}
