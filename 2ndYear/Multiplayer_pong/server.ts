import api from './api.js';
import express from 'express';
import http from 'http';
import { Server } from 'socket.io';
import listen from './sockets.js';

const port: number = 3000;

const httpServer = http.createServer(api);

httpServer.listen(port, (): void => {
  console.log('server running at http://localhost:' + port);
});

const socketServer = new Server(httpServer);

listen(socketServer);
