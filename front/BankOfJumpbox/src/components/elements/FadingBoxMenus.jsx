import * as React from "react";
import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";
import Zoom from "@mui/material/Zoom";
import Typography from "@mui/material/Typography";
import { CssBaseline, ThemeProvider } from "@mui/material";
import theme from "../../theme";

import { Link } from "react-router-dom";

export default function FadingBoxMenus() {
  const [checked, setChecked] = React.useState(false);

  React.useEffect(() => {
    // Automatically set checked to true when the component mounts
    setChecked(true);
  }, []);

  const boxDetails = [
    {
      menu: "PAWN REGISTER",
      detail: "Employee fill detail's pawn items for customer as a service",
      transitionDelay: "0ms",
      link : 'pawn-register'
    },
    {
      menu: "ROLLOVER SUBMIT",
      detail: "Employee fill detail's pawn items for customer as a service",
      transitionDelay: "300ms",
      link : 'pawn-register'
    },
    {
      menu: "ITEM REDEEM",
      detail: "Employee fill detail's pawn items for customer as a service",
      transitionDelay: "600ms",
      link : 'pawn-register'
    },
    {
      menu: "WAREHOUSE",
      detail: "Employee fill detail's pawn items for customer as a service",
      transitionDelay: "900ms",
      link : 'pawn-register'
    },
  ];

  return (
    <>
    <ThemeProvider theme={theme}>
    <Box sx={{textAlign: 'center'}}>
    <CssBaseline />
        {
        boxDetails.map((boxes, index) => (
          <Link key={index} to={`/${boxes.link}`}>
          <Box
            sx={{
              display: "inline-block",
              alignItems: "center",
              marginTop: 1,
              m: 5,
            }}
          >
            <Zoom
              in={checked}
              style={{
                transitionDelay: checked ? boxes.transitionDelay : "0ms",
              }}
            >
              <Paper
                sx={{
                  m: 1,
                  width: 200,
                  height: 200,
                }}
                elevation={4}
              >
                <Box
                  points="0,100 50,00, 100,100"
                  sx={{
                    fill: (theme) => theme.palette.common.white,
                    stroke: (theme) => theme.palette.divider,
                    strokeWidth: 1,
                  }}
                />
                <Typography variant="h6" color="primary">{boxes.menu}</Typography>
                <Typography variant="body1">{boxes.detail}</Typography>
              </Paper>
            </Zoom>
          </Box>
          </Link>
        ))
        }
      </Box>
    </ThemeProvider>

    </>
  );
}
