import './App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { StyledEngineProvider } from '@mui/material/styles';
import { Home } from './home/Home';
import { NavBar } from './navbar/NavBar';
import Submit from './submit/Submit';
import Track from './track/Track';
import Report from './report/Report';

function App() {
  // Using Routes
  // See: https://stackoverflow.com/a/69849271/12347616
  // And: https://github.com/howtographql/howtographql/issues/1387
  // ------------
  // Get Base-URL
  let baseUrl;
  if (process.env.NODE_ENV === 'development') {
    baseUrl = '';
  } else {
    baseUrl = '';
  }
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
          <Route path={'/submit'} element={<Submit baseUrl={baseUrl} />} />
          <Route path={'/track'} element={<Track baseUrl={baseUrl} />} />
          <Route path={'/report'} element={<Report baseUrl={baseUrl} />} />
        </Routes>
      </BrowserRouter>
    </StyledEngineProvider>
  );
}

export default App;
