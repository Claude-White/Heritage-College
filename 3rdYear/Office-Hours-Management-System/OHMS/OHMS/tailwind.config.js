/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Pages/**/*.{razor, html}",
        "./Layout/**/*.{razor, html}",
        "./**/*.{razor,html,js}",
        "./wwwroot/js/**/*.{razor, html, js}"
    ],
    theme: {
        colors: {
            'white': '#FFFFFF',
            'secondary': '#1F7A8C', // light teal
            'primary': '#022B3A', // dark teal
            'blue-grey': '#8EAAC6',
            'background': '#E1E5F2', // light blue/off-white
            'salmon': '#FF7E70',
            'red-error': '#791E15',
            'yellow-warning': '#E2D43D',
            'grey': '#AEAEAE',
            'darkslate': '#333333',
            'dark-grey': '#38383B',
            'black': '#000000',
            'lightyellow': '#FFF9C4',
        },
        fontFamily: {
            // Not Final
            sans: ['Century Gothic', 'sans-serif']
        },
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms'),
    ],
}
