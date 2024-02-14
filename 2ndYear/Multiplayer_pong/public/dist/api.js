import express from 'express';
import path from 'path';
const api = express();
api.use(express.static('public'));
api.get('/', (req, res) => {
    res.sendFile(path.join(__dirname, 'index.html'));
});
export default api;
//# sourceMappingURL=api.js.map