/// <reference types="vite/client" />
import { createLogger, defineConfig } from 'vite'
import solidPlugin from 'vite-plugin-solid'
import fs from 'fs-extra'
import path from 'path'
import autoprefixer from 'autoprefixer'

const logger = createLogger()
const loggerWarn = logger.warn

logger.warn = (msg, options) => {
  // Ignore empty CSS files warning  
  if (msg.includes('unsupported-@namespace') || msg.includes('not separate folders')) return
  loggerWarn(msg, options)
}

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => ({
  clearScreen: false,
  customLogger: logger,
  base: mode === 'production' ? '/echotone/' : '/',
  publicDir: '../public',
  build: {
    outDir: '../public',
    // Do not empty the outDir automatically.
    emptyOutDir: false,
  },
  plugins: [
    {
      name: 'empty-assets-dir',
      buildStart() {
        // Determine the assets directory inside outDir
        const outDir = path.resolve(__dirname, '../public')
        const assetsDir = path.join(outDir, 'assets')
        // Empty the assets directory if it exists
        fs.emptyDirSync(assetsDir)
      }
    },
    solidPlugin()
  ],
  css: {
    preprocessorOptions: {
      scss: {
        api: 'modern-compiler' // or "modern"
      }
    },
    postcss: {
      plugins: [
        autoprefixer()
      ]
    }
  },
}))