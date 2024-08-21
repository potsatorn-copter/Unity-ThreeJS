import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000, // เปลี่ยนเป็นพอร์ตที่คุณต้องการ เช่น 3000
  },
});