import express, { Request, Response, Express } from 'express';
import path from 'path';

const api: Express = express();

api.use(express.static('public'));

api.get('/', (req: Request, res: Response): void => {
  res.sendFile(path.join(__dirname, 'index.html'));
});

export default api;
