import { defineConfig } from 'vite'
import { svelte } from '@sveltejs/vite-plugin-svelte'
import tailwindcss from '@tailwindcss/vite'
// https://vite.dev/config/
export default defineConfig({
  plugins: [
    tailwindcss(),
    svelte()
  ],

  server: {
    host: true,
    port: 3000,  // Forzamos el puerto 3000 interno
    strictPort: true,
    watch: {
      usePolling: true
    }
  }
})
