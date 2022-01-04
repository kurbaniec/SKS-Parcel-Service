import logo from './logo.svg';
import './App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import {
  AppBar,
  Box,
  Button,
  Container,
  IconButton,
  MenuItem,
  Toolbar,
  Tooltip,
  Typography
} from '@mui/material';
import { StyledEngineProvider } from '@mui/material/styles';
import MenuIcon from '@mui/icons-material/Menu';
import { useState } from 'react';
import { Menu } from '@mui/icons-material';
import { Home } from './home/Home';
import { NavBar } from './navbar/NavBar';

const pages = ['Products', 'Pricing', 'Blog'];
const settings = ['Profile', 'Account', 'Dashboard', 'Logout'];

function App() {
  const [anchorElNav, setAnchorElNav] = useState(null);
  const [anchorElUser, setAnchorElUser] = useState(null);

  const handleOpenNavMenu = (event) => {
    setAnchorElNav(event.currentTarget);
  };
  const handleOpenUserMenu = (event) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  // Using Routes
  // See: https://stackoverflow.com/a/69849271/12347616
  // And: https://github.com/howtographql/howtographql/issues/1387
  return (
    // <div className="App">
    //   <header className="App-header">
    //     <img src={logo} className="App-logo" alt="logo" />
    //     <p>
    //       Edit <code>src/App.js</code> and save to reload.
    //     </p>
    //     <a
    //       className="App-link"
    //       href="https://reactjs.org"
    //       target="_blank"
    //       rel="noopener noreferrer">
    //       Learn React
    //     </a>
    //   </header>
    // </div>

    <StyledEngineProvider injectFirst>
      <BrowserRouter>
        <NavBar />
        <Routes>
          <Route exact path={'/'} element={<Home />} />
        </Routes>
      </BrowserRouter>
    </StyledEngineProvider>
  );
}

export default App;
