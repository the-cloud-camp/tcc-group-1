import * as React from 'react';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Zoom from '@mui/material/Zoom';
import Typography from '@mui/material/Typography';


export default function AutoFadingBox() {
  const [checked, setChecked] = React.useState(false);

  React.useEffect(() => {
    // Automatically set checked to true when the component mounts
    setChecked(true);
  }, []);

  return (
    <>
      <Box sx={{color: "blue" }}>
        <Box sx={{ display: 'inline-block', alignItems: 'center', m : 5  }}>
          <Zoom in={checked}>
            <Paper sx={{ m: 1, width: 250, height: 250 }} elevation={4}>
              <Box
                points="0,100 50,00, 100,100"
                sx={{
                  fill: (theme) => theme.palette.common.white,
                  stroke: (theme) => theme.palette.divider,
                  strokeWidth: 1,
                }}
              />
              <Typography variant="h6">Box 1</Typography>
              <Typography variant="body1">Description for Box 1</Typography>
            </Paper>
          </Zoom>
        </Box>
        <Box sx={{ display: 'inline-block', alignItems: 'center', marginTop: 2, m:5 }}>
          <Zoom in={checked} style={{ transitionDelay: checked ? '500ms' : '0ms' }}>
            <Paper sx={{ m: 1, width: 250, height: 250 }} elevation={4}>
              <Box
                points="0,100 50,00, 100,100"
                sx={{
                  fill: (theme) => theme.palette.common.white,
                  stroke: (theme) => theme.palette.divider,
                  strokeWidth: 1,
                }}
              />
              <Typography variant="h6">Box 2</Typography>
              <Typography variant="body1">Description for Box 2</Typography>
            </Paper>
          </Zoom>
        </Box>
        <Box sx={{ display: 'inline-block', alignItems: 'center', marginTop: 2, m: 5 }}>
          <Zoom in={checked} style={{ transitionDelay: checked ? '1000ms' : '0ms' }}>
            <Paper sx={{ m: 1, width: 250, height: 250 }} elevation={4}>
              <Box
                points="0,100 50,00, 100,100"
                sx={{
                  fill: (theme) => theme.palette.common.white,
                  stroke: (theme) => theme.palette.divider,
                  strokeWidth: 1,
                }}
              />
              <Typography variant="h6">Box 3</Typography>
              <Typography variant="body1">Description for Box 3</Typography>
            </Paper>
          </Zoom>
        </Box>
      </Box>
    </>
  );
}
