import { AccountBalance } from "@mui/icons-material";
import { AppBar, Hidden, Toolbar, Typography } from "@mui/material";
import AccountMenu from "./account-menu";

const Header = () => {
  return (
    <AppBar position="relative">
      <Toolbar>
        <AccountBalance sx={{ mr: 2 }} />
        <Hidden smDown>
          <Typography variant="h6" color="inherit" noWrap>
            Main Employee Menu
          </Typography>
        </Hidden>
        <div style={{ flexGrow: 1 }}></div>
        <AccountMenu />
      </Toolbar>
    </AppBar>
  );
};

export default Header;
