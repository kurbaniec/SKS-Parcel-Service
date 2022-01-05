import { Component } from 'react';
import { useFormControls } from './TrackForm';
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Alert,
  AlertTitle,
  Container,
  Grid,
  TextField,
  Typography
} from '@mui/material';
import Button from '@mui/material/Button';
import axios from 'axios';
import { stringOrNumber } from '../utils/stringOrNumber';
import { ExpandMore } from '@mui/icons-material';

class Track extends Component {
  constructor(props) {
    super(props);

    this.state = {
      response: false,
      ok: false,
      data: undefined
    };

    this.inputFieldValues = [
      {
        name: 'trackingId',
        label: 'Tracking ID',
        id: 'tracking-id',
        props: {
          inputProps: {
            maxLength: 9
          }
        }
      }
    ];
  }

  onChange(event, inputValue, formCallback) {
    formCallback(event);
    const { value } = event.target;
    inputValue.value = value;
  }

  async submitForm() {
    this.setState({
      response: false
    });
    try {
      let response = await axios.get(
        `${this.props.baseUrl}/parcel/${this.inputFieldValues[0].value}`
      );
      this.setState({
        response: true,
        ok: true,
        data: response.data
      });
    } catch (error) {
      this.setState({
        response: true,
        ok: false,
        data: error.response.data
      });
    }
  }

  renderField(inputFieldValue, handleInputValue, errors) {
    return (
      <Grid item xs={12}>
        <TextField
          type={inputFieldValue.type ?? 'text'}
          onChange={(e) => this.onChange(e, inputFieldValue, handleInputValue)}
          onBlur={(e) => this.onChange(e, inputFieldValue, handleInputValue)}
          name={inputFieldValue.name}
          label={inputFieldValue.label}
          multiline={inputFieldValue.multiline ?? false}
          fullWidth
          rows={inputFieldValue.rows ?? 1}
          autoComplete="none"
          autoCorrect={'true'}
          InputProps={inputFieldValue.props ?? {}}
          {...(errors[inputFieldValue.name] && {
            error: true,
            helperText: errors[inputFieldValue.name]
          })}
        />
      </Grid>
    );
  }

  render() {
    const { handleInputValue, handleFormSubmit, formIsValid, errors } = this.props.validator;

    let response;
    if (this.state.response) {
      if (this.state.ok) {
        response = (
          <Grid container item xs={12} spacing={2}>
            <Grid item xs={12}>
              <Alert severity="info">
                <AlertTitle>Parcel Status</AlertTitle>
                <Grid container item xs={12}>
                  <Grid item xs={12}>
                    <Typography>
                      State: <strong>{this.state.data.state}</strong>
                    </Typography>
                  </Grid>
                  {this.state.data.visitedHops.length > 0 && (
                    <div>
                      <Grid item xs={12}>
                        <Typography>Visited Hops:</Typography>
                      </Grid>
                      {this.state.data.visitedHops.map((hop, index) => {
                        return (
                          <Grid item xs={12} key={index} sx={{ m: 0.5 }}>
                            <Accordion>
                              <AccordionSummary expandIcon={<ExpandMore />}>
                                <Typography>{hop.code}</Typography>
                              </AccordionSummary>
                              <AccordionDetails>
                                <Typography>{hop.description}</Typography> <br />
                                <Typography>{hop.dateTime}</Typography>
                              </AccordionDetails>
                            </Accordion>
                          </Grid>
                        );
                      })}
                    </div>
                  )}
                  {this.state.data.futureHops.length > 0 && (
                    <div>
                      <Grid item xs={12}>
                        <Typography>Future Hops:</Typography>
                      </Grid>
                      {this.state.data.futureHops.map((hop, index) => {
                        return (
                          <Grid item xs={12} key={index} sx={{ m: 0.5 }}>
                            <Accordion>
                              <AccordionSummary expandIcon={<ExpandMore />}>
                                <Typography>{hop.code}</Typography>
                              </AccordionSummary>
                              <AccordionDetails>
                                <Typography>{hop.description}</Typography> <br />
                                <Typography>{hop.dateTime}</Typography>
                              </AccordionDetails>
                            </Accordion>
                          </Grid>
                        );
                      })}
                    </div>
                  )}
                </Grid>
              </Alert>
            </Grid>
          </Grid>
        );
      } else {
        response = (
          <Grid container item xs={12} spacing={2}>
            <Grid item xs={12}>
              <Alert severity="error">
                <AlertTitle>Encountered Error</AlertTitle>
                {this.state.data.errorMessage}
              </Alert>
            </Grid>
          </Grid>
        );
      }
    } else {
      response = <div />;
    }

    return (
      <Container>
        <Grid container spacing={2}>
          <Grid item xs={12}>
            <h1>Track Parcel</h1>
          </Grid>
          <Grid item xs={12}>
            <form
              onSubmit={(e) => {
                handleFormSubmit(e, this.submitForm.bind(this));
              }}>
              <Grid container item xs={12} spacing={2}>
                {this.renderField(this.inputFieldValues[0], handleInputValue, errors)}
                <Grid item xs={12}>
                  <Button type="submit" variant={'contained'} disabled={!formIsValid()}>
                    SUBMIT
                  </Button>
                </Grid>
              </Grid>
            </form>
          </Grid>
          {response}
        </Grid>
      </Container>
    );
  }
}

// Use Hooks with old school class components
// See: https://stackoverflow.com/a/70375132/12347616
const withValidatorHook = (Component) => (props) => {
  const validator = useFormControls();
  return <Component {...props} validator={validator} />;
};

export default withValidatorHook(Track);
