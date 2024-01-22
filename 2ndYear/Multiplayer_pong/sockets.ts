let readyPlayerCount: number = 0;

export default function listen(io: any): void {
  const pongNameSpace = io.of('/pong');
  pongNameSpace.on('connection', (socket: any): void => {
    let room: string;
    console.log(`A user connected with ${socket.id}`);

    socket.on('ready', (): void => {
      room = 'room' + Math.floor(readyPlayerCount / 2);
      socket.join(room);
      console.log(`Player with id ${socket.id} ready ${room}`);
      console.log(`Player with id ${socket.id} ready`);
      readyPlayerCount++;
      if (readyPlayerCount % 2 === 0) {
        pongNameSpace.in(room).emit('startGame', socket.id);
      }
    });

    socket.on('paddleMove', (paddleData: { yPosition: number }): void => {
      socket.to(room).emit('paddleMove', paddleData);
    });

    socket.on(
      'ballMove',
      (ballData: { ballX: number; ballY: number; score: number[] }): void => {
        socket.to(room).emit('ballMove', ballData);
      }
    );

    socket.on('disconnect', (reason: any): void => {
      console.log(`Client ${socket.id} disconnected: ${reason}`);
      socket.leave(room);
    });
  });
}
