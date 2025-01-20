import forms from "@tailwindcss/forms";
import typography from "@tailwindcss/typography";
import { join } from "path";
import type { Config } from "tailwindcss";
import { skeleton } from "@skeletonlabs/tw-plugin";
import { myCustomTheme } from "./my-custom-theme";

export default {
    darkMode: "class",
    content: [
        "./src/**/*.{html,js,svelte,ts}",
        join(
            require.resolve("@skeletonlabs/skeleton"),
            "../**/*.{html,js,svelte,ts}"
        ),
    ],

    theme: {
        extend: {},
    },
    plugins: [
        typography,
        forms,
        skeleton({
            themes: {
                custom: [myCustomTheme],
            },
        }),
    ],
} satisfies Config;
