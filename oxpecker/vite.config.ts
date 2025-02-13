/// <reference types="vite/client" />
import { defineConfig } from 'vite'
import solidPlugin from 'vite-plugin-solid'
import * as fs from 'fs-extra'
import path from 'path'

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => ({
    clearScreen: false,
    publicDir: '../public',
    base: mode === 'production' ? '/echotone/' : '/',
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
}))