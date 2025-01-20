/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.{razor,cshtml,html,js}"],
  
  theme: {
    extend: {
      colors: {
        'text': 'rgb(1, 10, 19)',
        'background': 'rgb(242, 248, 254)',
        'primary': 'rgb(30, 139, 238)',
        'secondary': 'rgb(134, 185, 235)',
        'accent': 'rgb(197, 67, 250)',
      },
      gridTemplateColumns: {
        "responsive": "repeat(auto-fit, minmax(300px, 1fr))"
      }
    },
  },
  plugins: [
    require('@tailwindcss/typography'),
    require('@tailwindcss/forms'),
    require('daisyui'),
  ],
  daisyui: {
    themes: ["light", "dark"],
  },
}

