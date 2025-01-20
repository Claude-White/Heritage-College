/** @type {import('tailwindcss').Config} */

module.exports = {
    content: ["./**/*.{razor,cshtml,html}"],
    theme: {
        extend:
        {
            fontFamily: {
                'noto-serif': ['noto-serif', 'serif']
            },
            colors: {
                primary: {
                    0: '#e8a713',
                    100: '#d19611',
                    200: '#ba860f',
                    300: '#a2750d',
                    400: '#8b640b',
                    500: '#74540a',
                    600: '#5d4308',
                    700: '#463206',
                },
            },
            aspectRatio: {
                '16/9': '16 / 9',
            },
        },
    },
    plugins: [
        require("daisyui"),
        require('@tailwindcss/typography')
    ],
    daisyui: {
        themes: [
            {
                corporate: {
                    ...require("daisyui/src/theming/themes")["corporate"],

                    "primary": "#e8a713",
                    "primary-content": "#ffffff",

                    "secondary": "#695e4a",

                    "accent": "#e4e0d5",
                    "neutral": "#1E1A14",
                    "base-100": "#ffffff",
                    "info": "#7dd3fc",
                    "success": "#22c55e",
                    "warning": "#facc15",
                    "error": "#ef4444",

                    "--rounded-btn": "0.5rem",
                },
            }
        ],
    },
}