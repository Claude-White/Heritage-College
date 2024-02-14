import api from './api.js';
import http from 'http';
import { Server } from 'socket.io';
import listen from './sockets.js';
const port = 3000;
const httpServer = http.createServer(api);
const socketServer = new Server(httpServer);
httpServer.listen(port, () => {
    console.log('server running at http://localhost:' + port);
});
listen(socketServer);
//# sourceMappingURL=server.js.map