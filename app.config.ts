import { defineConfig } from "@solidjs/start/config";

export default defineConfig({
    ssr: false,
    server: {
        preset: "static",
        output: {
            dir: ".",
            publicDir: "www",
            serverDir: "",
        },
    },
});