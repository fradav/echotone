/// <reference types="vite/client" />
import { defineConfig } from 'vite'
import solidPlugin from 'vite-plugin-solid';

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => ({
    clearScreen: false,
    publicDir: '../public',
    // prefix all relative links with "echotone" if in production mode
    base: mode === 'production' ? '/echotone/' : '/',
    build: {
        outDir: '../public',
        emptyOutDir: false,
        // make the build output without the hash in the filename
        manifest: false,
        rollupOptions: {
            output: {
                entryFileNames: '[name].js',
                chunkFileNames: '[name].js',
                assetFileNames: '[name].[ext]'
            }
        }

    },
    server: {
        watch: {
            ignored: [
                "**/*.md" , // Don't watch markdown files
                "**/*.fs" , // Don't watch F# files
                "**/*.fsx"  // Don't watch F# script files
            ]
        }
    },
    plugins: [
        solidPlugin()
    ],
}))