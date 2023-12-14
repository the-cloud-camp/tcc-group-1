import { createTheme } from "@mui/material/styles";
import { red } from "@mui/material/colors";

// A custom theme for this app
const theme = createTheme({
  palette: {
    primary: {
      main: "#556cd6",
      jumpbox: "#9900FF"
    },
    secondary: {
      main: "#19857b",
    },
    error: {
      main: red.A700,
      secondary: red.A100
    },
  },
});

export default theme;