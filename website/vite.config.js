import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': 'http://localhost:8090',
      '/auth': 'http://localhost:8090',
      '/ai': 'http://localhost:5000'
    }
  }
})
