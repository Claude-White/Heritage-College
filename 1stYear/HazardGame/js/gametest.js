console.log("Testing inputs:\n");

let gameInput = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice());
console.table(gameInput.user);
console.log(`Full name: ${gameInput.user.getFullName()}`);
console.log(`Balance: $${gameInput.user.getBalance()}`);

console.log(`-------------------------------------------------------------------------------------`);
console.log("Check if random dice rolls are valid:");

let gameDice = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice());
console.table(gameDice.dice);

console.log("Check if dice roll method actually changes dice values:");
gameDice.dice.rollDice();
console.table(gameDice.dice);

console.log(`-------------------------------------------------------------------------------------`);
console.log("Games that don't go to chance:");

for (let i = 5; i <= 9; i++) {
  let game1 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(i));
  game1.placeBetAmount(10);
  game1.betMain(i);
  console.log(`Test 1(${i}): Main = ${i}, Roll = ${i}, Bet: 10. Expect: Won | Got: ${game1.hasWon() ? `Won, balance: ${game1.increaseBalance()}` : `Lost, balance: ${game1.decreaseBalance()}`}`);
}

let game2 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(2));
game2.placeBetAmount(10);
game2.betMain(5);
console.log(`Test 2: Main = 5, Roll = 2, Bet: 10. Expect: Lost | Got: ${game2.hasWon() ? `Won, balance: ${game2.increaseBalance()}` : `Lost, balance: ${game2.decreaseBalance()}`}`);

let game3 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(3));
game3.placeBetAmount(10);
game3.betMain(5);
console.log(`Test 3: Main = 5, Roll = 3, Bet: 10. Expect: Lost | Got: ${game3.hasWon() ? `Won, balance: ${game3.increaseBalance()}` : `Lost, balance: ${game3.decreaseBalance()}`}`);

let game4 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(11));
game4.placeBetAmount(10);
game4.betMain(5);
console.log(`Test 4: Main = 5, Roll = 11, Bet: 10. Expect: Lost | Got: ${game4.hasWon() ? `Won, balance: ${game4.increaseBalance()}` : `Lost, balance: ${game4.decreaseBalance()}`}`);

let game5 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(12));
game5.placeBetAmount(10);
game5.betMain(5);
console.log(`Test 5: Main = 5, Roll = 12, Bet: 10. Expect: Lost | Got: ${game5.hasWon() ? `Won, balance: ${game5.increaseBalance()}` : `Lost, balance: ${game5.decreaseBalance()}`}`);

let game6 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(11));
game6.placeBetAmount(10);
game6.betMain(9);
console.log(`Test 6: Main = 9, Roll = 11, Bet: 10. Expect: Lost | Got: ${game6.hasWon() ? `Won, balance: ${game6.increaseBalance()}` : `Lost, balance: ${game6.decreaseBalance()}`}`);

let game7 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(12));
game7.placeBetAmount(10);
game7.betMain(9);
console.log(`Test 7: Main = 9, Roll = 12, Bet: 10. Expect: Lost | Got: ${game7.hasWon() ? `Won, balance: ${game7.increaseBalance()}` : `Lost, balance: ${game7.decreaseBalance()}`}`);

let game8 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(11));
game8.placeBetAmount(10);
game8.betMain(6);
console.log(`Test 8: Main = 6, Roll = 11, Bet: 10. Expect: Lost | Got: ${game8.hasWon() ? `Won, balance: ${game8.increaseBalance()}` : `Lost, balance: ${game8.decreaseBalance()}`}`);

let game9 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(12));
game9.placeBetAmount(10);
game9.betMain(6);
console.log(`Test 9: Main = 6, Roll = 12, Bet: 10. Expect: Won | Got: ${game9.hasWon() ? `Won, balance: ${game9.increaseBalance()}` : `Lost, balance: ${game9.decreaseBalance()}`}`);

let game10 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(11));
game10.placeBetAmount(10);
game10.betMain(8);
console.log(`Test 10: Main = 8, Roll = 11, Bet: 10. Expect: Lost | Got: ${game10.hasWon() ? `Won, balance: ${game10.increaseBalance()}` : `Lost, balance: ${game10.decreaseBalance()}`}`);

let game11 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(12));
game11.placeBetAmount(10);
game11.betMain(8);
console.log(`Test 11: Main = 8, Roll = 12, Bet: 10. Expect: Won | Got: ${game11.hasWon() ? `Won, balance: ${game11.increaseBalance()}` : `Lost, balance: ${game11.decreaseBalance()}`}`);

let game12 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(11));
game12.placeBetAmount(10);
game12.betMain(7);
console.log(`Test 12: Main = 7, Roll = 11, Bet: 10. Expect: Won | Got: ${game12.hasWon() ? `Won, balance: ${game12.increaseBalance()}` : `Lost, balance: ${game12.decreaseBalance()}`}`);

let game13 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(12));
game13.placeBetAmount(10);
game13.betMain(7);
console.log(`Test 13: Main = 7, Roll = 12, Bet: 10. Expect: Lost | Got: ${game13.hasWon() ? `Won, balance: ${game13.increaseBalance()}` : `Lost, balance: ${game13.decreaseBalance()}`}`);

game13.placeBetAmount(10);
game13.betMain(7);
console.log(`Test 14: Main = 7, Roll = 12, Bet: 10. Expect: Lost | Got: ${game13.hasWon() ? `Won, balance: ${game13.increaseBalance()}` : `Lost, balance: ${game13.decreaseBalance()}`}`);

console.log(`-------------------------------------------------------------------------------------`);
console.log("Games that go to chance:");

for (let x = 5; x <= 9; x++) {
  for (let i = 4; i <= 10; i++) {
    if (i != x) {
      let game15 = new Game(new User("White", "Claude", "CWhite", "819-123-1234", "Gatineau", "example@company.com"), new Dice(i));
      game15.placeBetAmount(10);
      game15.betMain(x);
      let win = game15.hasWon();
      console.log(
        `Test 1${x}(${i}): Main = ${x}, Roll = ${i}, Bet: 10, Won = ${game15.won}. Expect: ${game15.won ? "Won" : "Lost"} | Got: ${
          win ? `Won, balance: ${game15.increaseBalance()}` : `Lost, balance: ${game15.decreaseBalance()}`
        }`
      );
    }
  }
}
