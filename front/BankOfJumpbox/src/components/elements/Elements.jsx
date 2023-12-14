import { Typography } from "@mui/material";


export function Copyright(props) {
    return (
      <Typography variant="body2" color="primary.jumpbox" align="center" {...props}>
        {'Copyright © '}
        Bank of Jumpbox{' '}
        {new Date().getFullYear()}
      </Typography>
    );
}