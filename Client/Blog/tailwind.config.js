/** @type {import('tailwindcss').Config} */
// secondary: "#e5e5e5",
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      colors: {
        primary: "#ffffff",
        secondary: "#e2e8f0",
        accent: "#5b9e56",
        textPrimary: "#000000",
        danger: "#dc2626",
        textSecondary: "#5c5754",
        textSecondaryBright: "#acb3bf",
      },
    },
  },
  plugins: [],
};
