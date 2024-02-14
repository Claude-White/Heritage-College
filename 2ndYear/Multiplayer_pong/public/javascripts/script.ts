declare const io: (arg0: string) => any;
// Canvas Related
const canvas: HTMLCanvasElement = document.createElement(
  'canvas'
) as HTMLCanvasElement;
const context: CanvasRenderingContext2D = canvas.getContext(
  '2d'
) as CanvasRenderingContext2D;
const socket = io('/pong');
let paddleIndex: number = 0;
const maxSpeed: number = 8;

let isReferee: boolean = false;

let width: number = 700;
let height: number = 500;

// Paddle
let paddleHeight: number = 50;
let paddleWidth: number = 10;
let paddleDiff: number = 25;
let paddleY: number[] = [225, 225];
let trajectoryY: number[] = [0, 0];
let playerMoved: boolean = false;

// Ball
let ballY: number = 250;
let ballX: number = 350;
let ballRadius: number = 5;
let ballDirection: number = 1;

// Speed
let speedX: number = 2;
let speedY: number = 0;

// Score for Both Players
let score: number[] = [0, 0];

// Create Canvas Element
function createCanvas(): void {
  canvas.id = 'canvas';
  canvas.width = width;
  canvas.height = height;
  document.body.appendChild(canvas);
  renderCanvas();
}

// Wait for Opponents
function renderIntro(): void {
  // Canvas Background
  context!.fillStyle = 'black';
  context!.fillRect(0, 0, width, height);

  // Intro Text
  context!.fillStyle = 'white';
  context!.font = '32px Courier New';
  context!.fillText(
    'Waiting for opponent...',
    canvas.width / 2 - 220,
    canvas.height / 2
  );
}

// Render Everything on Canvas
function renderCanvas(): void {
  // Canvas Background
  context!.fillStyle = 'black';
  context!.fillRect(0, 0, width, height);

  // Paddle Color
  context!.fillStyle = 'white';

  // Bottom Paddle
  context!.fillRect(width - 20, paddleY[0], paddleWidth, paddleHeight);

  // Top Paddle
  context!.fillRect(10, paddleY[1], paddleWidth, paddleHeight);

  // Dashed Center Line
  context!.beginPath();
  context!.setLineDash([4]);
  context!.moveTo(350, 0);
  context!.lineTo(350, 500);
  context!.strokeStyle = 'grey';
  context!.stroke();

  // Ball
  context!.beginPath();
  context!.arc(ballX, ballY, ballRadius, 2 * Math.PI, 0);
  context!.fillStyle = 'white';
  context!.fill();

  // Score
  context!.font = '32px Courier New';
  context!.fillText(score[0].toString(), canvas.width / 2 + 30, 40);
  context!.fillText(score[1].toString(), canvas.width / 2 - 50, 40);
}

// Reset Ball to Center
function ballReset(): void {
  ballX = width / 2;
  ballY = height / 2;
  speedY = 3;
  socket.emit('ballMove', {
    ballX,
    ballY,
    score,
  });
}

// Adjust Ball Movement
function ballMove(): void {
  // Vertical Speed
  ballX += speedX * ballDirection;
  // Horizontal Speed
  if (playerMoved) {
    ballY += speedY;
  }
  socket.emit('ballMove', {
    ballX,
    ballY,
    score,
  });
}

// Determine What Ball Bounces Off, Score Points, Reset Ball
function ballBoundaries(): void {
  // Bounce off Bottom Wall
  if (ballY < 0 && speedY < 0) {
    speedY = -speedY;
  }
  // Bounce off Top Wall
  if (ballY > height && speedY > 0) {
    speedY = -speedY;
  }
  // Bounce off player paddle (right)
  if (ballX > width - paddleDiff) {
    if (ballY >= paddleY[0] && ballY <= paddleY[0] + paddleHeight) {
      // Add Speed on Hit
      if (playerMoved) {
        speedX += 1;
        // Max Speed
        if (speedX > maxSpeed) {
          speedX = maxSpeed;
        }
      }
      ballDirection = -ballDirection;
      trajectoryY[0] = ballY - (paddleY[0] + paddleDiff);
      speedY = trajectoryY[0] * 0.3;
    }
  }
  if (ballX > width) {
    ballReset();
    score[1]++;
  }
  // Bounce off opponent paddle (left)
  if (ballX < paddleDiff) {
    if (ballY >= paddleY[1] && ballY <= paddleY[1] + paddleHeight) {
      // Add Speed on Hit
      if (playerMoved) {
        speedX += 1;
        // Max Speed
        if (speedX > maxSpeed) {
          speedX = maxSpeed;
        }
      }
      ballDirection = -ballDirection;
      trajectoryY[1] = ballY - (paddleY[1] + paddleDiff);
      speedY = trajectoryY[1] * 0.3;
    }
  }
  if (ballX < 0) {
    ballReset();
    score[0]++;
  }
}

// Called Every Frame
function animate(): void {
  if (isReferee) {
    ballMove();
    ballBoundaries();
  }
  renderCanvas();
  window.requestAnimationFrame(animate);
}

function loadGame(): void {
  createCanvas();
  renderIntro();
  socket.emit('ready');
}

// Start Game, Reset Everything
function startGame(): void {
  paddleIndex = isReferee ? 0 : 1;
  window.requestAnimationFrame(animate);
  document.addEventListener('mousemove', (e: MouseEvent): void => {
    const canvasRect: DOMRect = canvas.getBoundingClientRect();
    const yPos: number = e.clientY - canvasRect.top - paddleHeight / 2;
    playerMoved = true;
    paddleY[paddleIndex] = yPos;

    if (paddleY[paddleIndex] < -paddleHeight / 8) {
      paddleY[paddleIndex] = -paddleHeight / 8;
    }
    if (paddleY[paddleIndex] > height - paddleHeight + paddleHeight / 8) {
      paddleY[paddleIndex] = height - paddleHeight + paddleHeight / 8;
    }

    socket.emit('paddleMove', {
      yPosition: paddleY[paddleIndex],
    });
    // Hide Cursor
    canvas.style.cursor = 'none';
  });
}

// On Load
loadGame();
socket.on('connect', (): void => {
  console.log(`Connected as ${socket.id}`);
});
socket.on('startGame', (refereeId: number): void => {
  console.log(`Referee is ${refereeId}`);
  socket.id == refereeId ? (isReferee = true) : (isReferee = false);
  startGame();
});
socket.on('paddleMove', (paddleData: { yPosition: number }): void => {
  // Toggle 1 to 0, and 0 to 1
  const opponentPaddleIndex: number = 1 - paddleIndex;
  paddleY[opponentPaddleIndex] = paddleData.yPosition;
});
socket.on(
  'ballMove',
  (ballData: { ballX: number; ballY: number; score: number[] }): void => {
    ({ ballX, ballY, score } = ballData);
  }
);
