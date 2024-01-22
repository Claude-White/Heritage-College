import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react-swc';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/movies': 'http://localhost:8888',
      '/actors': 'http://localhost:8888',
      '/years': 'http://localhost:8888',
    },
  },
});
