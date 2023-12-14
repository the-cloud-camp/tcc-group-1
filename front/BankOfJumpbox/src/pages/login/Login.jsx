import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import { Link } from 'react-router-dom';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import AccountCircleTwoToneIcon from '@mui/icons-material/AccountCircleTwoTone';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { Copyright } from '../../components/elements/Elements'



export default function Login() {
  const handleSubmit = (event) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    console.log({
      email: data.get('email'),
      password: data.get('password'),
    });
  };

  return (
    <>
      <Container component="main" maxWidth="xs">
        <Box
          sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            border : '0px solid',
            boxShadow: '0px 0px 25px 0px rgba(0,0,0,0.5)',
            borderRadius: '15px',
            padding : 4
          }}
        >
          <Avatar sx={{m: 2, backgroundColor: "#4a148c"}} >
            <AccountCircleTwoToneIcon sx={{fontSize: 40}} />
          </Avatar>
          <Typography component="h2" variant="h4" color={"primary.jumpbox"} noWrap>
            Bank of Jumpbox
          </Typography>
          <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 3 }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="username"
              label="Username"
              name="username"
              autoFocus
            />
            <TextField
              margin="normal"
              required
              fullWidth
              name="password"
              label="Password"
              type="password"
              id="password"
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              SIGN IN
            </Button>
            <Grid container>
              <Grid>
                <Link to="/" variant="body2">
                  Service for Customer CLICK
                </Link>
              </Grid>
            </Grid>
          </Box>
        </Box>
      </Container>
      <Copyright sx={{ mt: 8, mb: 4 }} />
    </>
  );
}