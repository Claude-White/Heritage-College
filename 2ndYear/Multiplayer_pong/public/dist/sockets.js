let readyPlayerCount = 0;
export default function listen(io) {
    const pongNameSpace = io.of('/pong');
    pongNameSpace.on('connection', (socket) => {
        let room;
        console.log(`A user connected with ${socket.id}`);
        socket.on('ready', () => {
            room = 'room' + Math.floor(readyPlayerCount / 2);
            socket.join(room);
            console.log(`Player with id ${socket.id} ready ${room}`);
            console.log(`Player with id ${socket.id} ready`);
            readyPlayerCount++;
            if (readyPlayerCount % 2 === 0) {
                pongNameSpace.in(room).emit('startGame', socket.id);
            }
        });
        socket.on('paddleMove', (paddleData) => {
            socket.to(room).emit('paddleMove', paddleData);
        });
        socket.on('ballMove', (ballData) => {
            socket.to(room).emit('ballMove', ballData);
        });
        socket.on('disconnect', (reason) => {
            console.log(`Client ${socket.id} disconnected: ${reason}`);
            socket.leave(room);
        });
    });
}
//# sourceMappingURL=sockets.js.map