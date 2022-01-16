import { Component } from 'react';
import { useFormControls as useFormControlsDelivery } from './ReportDeliveryForm';
import { useFormControls as useFormControlsHop } from './ReportHopForm';
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

class Report extends Component {
  constructor(props) {
    super(props);

    this.state = {
      responseHop: false,
      okHop: false,
      dataHop: undefined,
      responseDelivery: false,
      okDelivery: false,
      dataDelivery: undefined
    };

    this.inputFieldValues = [
      {
        name: 'trackingId',
        label: 'Tracking ID',
        id: 'tracking-id-hop',
        props: {
          inputProps: {
            maxLength: 9
          }
        }
      },
      {
        name: 'code',
        label: 'Code',
        id: 'code-hop',
        props: {
          inputProps: {
            maxLength: 8
          }
        }
      },
      {
        name: 'trackingId',
        label: 'Tracking ID',
        id: 'tracking-id-delivery',
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

  async submitFormHop() {
    this.setState({
      responseHop: false
    });
    const trackingId = this.inputFieldValues[0].value;
    const code = this.inputFieldValues[1].value;
    try {
      let response = await axios.post(
        `${this.props.baseUrl}/parcel/${trackingId}/reportHop/${code}`
      );
      this.setState({
        responseHop: true,
        okHop: true,
        dataHop: response.data
      });
    } catch (error) {
      this.setState({
        responseHop: true,
        okHop: false,
        dataHop: error.response.data
      });
    }
  }

  async submitFormDelivery() {
    this.setState({
      responseDelivery: false
    });
    const trackingId = this.inputFieldValues[2].value;
    try {
      let response = await axios.post(`${this.props.baseUrl}/parcel/${trackingId}/reportDelivery`);
      this.setState({
        responseDelivery: true,
        okDelivery: true,
        dataDelivery: response.data
      });
    } catch (error) {
      this.setState({
        responseDelivery: true,
        okDelivery: false,
        dataDelivery: error.response.data
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
    const handleInputValueHop = this.props.validatorHop.handleInputValue;
    const handleFormSubmitHop = this.props.validatorHop.handleFormSubmit;
    const formIsValidHop = this.props.validatorHop.formIsValid;
    const errorsHop = this.props.validatorHop.errors;

    const handleInputValueDelivery = this.props.validatorDelivery.handleInputValue;
    const handleFormSubmitDelivery = this.props.validatorDelivery.handleFormSubmit;
    const formIsValidDelivery = this.props.validatorDelivery.formIsValid;
    const errorsDelivery = this.props.validatorDelivery.errors;

    let responseHop;
    if (this.state.responseHop) {
      if (this.state.okHop) {
        responseHop = (
          <Grid container item xs={12} spacing={2}>
            <Grid item xs={12}>
              <Alert severity="info">
                <AlertTitle>Report Hop was successful</AlertTitle>
              </Alert>
            </Grid>
          </Grid>
        );
      } else {
        responseHop = (
          <Grid container item xs={12} spacing={2}>
            <Grid item xs={12}>
              <Alert severity="error">
                <AlertTitle>Encountered Error</AlertTitle>
                {this.state.dataHop.errorMessage}
              </Alert>
            </Grid>
          </Grid>
        );
      }
    } else {
      responseHop = <div />;
    }
    let responseDelivery;
    if (this.state.responseDelivery) {
      if (this.state.okDelivery) {
        responseDelivery = (
          <Grid container item xs={12} spacing={2}>
            <Grid item xs={12}>
              <Alert severity="info">
                <AlertTitle>Report Delivery was successful</AlertTitle>
              </Alert>
            </Grid>
          </Grid>
        );
      } else {
        responseDelivery = (
          <Grid container item xs={12} spacing={2}>
            <Grid item xs={12}>
              <Alert severity="error">
                <AlertTitle>Encountered Error</AlertTitle>
                {this.state.dataDelivery.errorMessage}
              </Alert>
            </Grid>
          </Grid>
        );
      }
    } else {
      responseDelivery = <div />;
    }

    return (
      <Container>
        <Grid container spacing={2}>
          <Grid item xs={12}>
            <h1>Report Hop</h1>
          </Grid>
          <Grid item xs={12}>
            <form
              onSubmit={(e) => {
                handleFormSubmitHop(e, this.submitFormHop.bind(this));
              }}>
              <Grid container item xs={12} spacing={2}>
                {this.renderField(this.inputFieldValues[0], handleInputValueHop, errorsHop)}
                {this.renderField(this.inputFieldValues[1], handleInputValueHop, errorsHop)}
                <Grid item xs={12}>
                  <Button type="submit" variant={'contained'} disabled={!formIsValidHop()}>
                    SUBMIT
                  </Button>
                </Grid>
              </Grid>
            </form>
          </Grid>
          {responseHop}
          <Grid item xs={12}>
            <h1>Report Delivery</h1>
          </Grid>
          <Grid item xs={12}>
            <form
              onSubmit={(e) => {
                handleFormSubmitDelivery(e, this.submitFormDelivery.bind(this));
              }}>
              <Grid container item xs={12} spacing={2}>
                {this.renderField(
                  this.inputFieldValues[2],
                  handleInputValueDelivery,
                  errorsDelivery
                )}
                <Grid item xs={12}>
                  <Button type="submit" variant={'contained'} disabled={!formIsValidDelivery()}>
                    SUBMIT
                  </Button>
                </Grid>
              </Grid>
            </form>
          </Grid>
          {responseDelivery}
        </Grid>
      </Container>
    );
  }
}

// Use Hooks with old school class components
// See: https://stackoverflow.com/a/70375132/12347616
const withValidatorHook = (Component) => (props) => {
  const validatorHop = useFormControlsHop();
  const validatorDelivery = useFormControlsDelivery();
  return <Component {...props} validatorHop={validatorHop} validatorDelivery={validatorDelivery} />;
};

export default withValidatorHook(Report);
