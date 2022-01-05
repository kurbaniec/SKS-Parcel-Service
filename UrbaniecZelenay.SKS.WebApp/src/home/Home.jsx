import { Component } from 'react';
import { NavBar } from '../navbar/NavBar';
import Container from '@mui/material/Container';
import { Typography } from '@mui/material';

export class Home extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <Container>
        <h1>Home</h1>
        <Typography variant={'h5'}>Welcome to the Parcel Service Web-App</Typography>
      </Container>
    );
  }
}
