import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuIcon from '@mui/icons-material/Menu';
import Container from '@mui/material/Container';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import Tooltip from '@mui/material/Tooltip';
import MenuItem from '@mui/material/MenuItem';
import { useNavigate } from 'react-router-dom';
import { Link } from '@mui/material';
import { LocalShippingOutlined } from '@mui/icons-material';

const pages = ['Home', 'Submit', 'Track', 'Report'];
const urls = new Map([
  ['Home', '/'],
  ['Submit', 'submit'],
  ['Track', 'track'],
  ['Report', 'report']
]);

/**
 * Simple NavBar
 * Source: https://mui.com/components/app-bar/
 * @returns {JSX.Element}
 * @constructor
 */
export const NavBar = () => {
  // See: https://stackoverflow.com/a/42121109/12347616
  // And: https://stackoverflow.com/a/66971821/12347616
  const navigate = useNavigate();
  const [anchorElNav, setAnchorElNav] = React.useState(null);

  const handleOnClick = (event, url) => {
    if (url !== undefined) navigate(url);
    handleCloseNavMenu(event);
  };

  const handleOpenNavMenu = (event) => {
    setAnchorElNav(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  return (
    <AppBar position="static">
      <Container maxWidth="xl">
        <Toolbar disableGutters>
          <Typography
            variant="h6"
            noWrap
            component="div"
            onClick={(event) => {
              handleOnClick(event, urls.get('Home'));
            }}
            sx={{ mr: 2, display: { xs: 'none', md: 'flex' } }}>
            <LocalShippingOutlined />
          </Typography>

          <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleOpenNavMenu}
              color="inherit">
              <MenuIcon />
            </IconButton>
            <Menu
              id="menu-appbar"
              anchorEl={anchorElNav}
              anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'left'
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'left'
              }}
              open={Boolean(anchorElNav)}
              onClose={handleCloseNavMenu}
              sx={{
                display: { xs: 'block', md: 'none' }
              }}>
              {pages.map((page) => (
                <MenuItem
                  key={page}
                  onClick={(event) => {
                    handleOnClick(event, urls.get(page));
                  }}>
                  <Typography textAlign="center">{page}</Typography>
                </MenuItem>
              ))}
            </Menu>
          </Box>
          <Typography
            variant="h6"
            noWrap
            component="div"
            onClick={(event) => {
              handleOnClick(event, urls.get('Home'));
            }}
            sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
            <LocalShippingOutlined />
          </Typography>
          <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
            {pages.map((page) => (
              <Button
                key={page}
                onClick={(event) => {
                  handleOnClick(event, urls.get(page));
                }}
                sx={{ my: 2, color: 'white', display: 'block' }}>
                {page}
              </Button>
            ))}
          </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
};
