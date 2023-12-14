import { Container, Typography } from "@mui/material";
import FadingBoxMenus from "../../components/elements/FadingBoxMenus";
import { Copyright } from "../../components/elements/Elements";

const Home = () => {
  return (
    <>
      <Container maxWidth="xl" sx={{textAlign: 'center'}}>
        <Typography
          variant="title"
          sx={{ p: 5, fontSize: "2rem" }}
          textTransform={"uppercase"}
        >
          Welcome To Bank of Jumpbox
        </Typography>
        <FadingBoxMenus />
      </Container>
      <Copyright sx={{ mt: 8, mb: 4 }} />
    </>
  );
};

export default Home;
