import { Component } from 'react';
import { useFormControls } from './SubmitForm';
import { Alert, AlertTitle, Container, Grid, TextField } from '@mui/material';
import Button from '@mui/material/Button';
import axios from 'axios';
import { stringOrNumber } from '../utils/stringOrNumber';

class Submit extends Component {
  constructor(props) {
    super(props);

    this.state = {
      response: false,
      ok: false,
      data: undefined
    };

    this.inputFieldValues = [
      {
        name: 'weight',
        label: 'Weight (kg)',
        id: 'weight',
        type: 'number',
        props: {
          inputProps: {
            min: 0
          }
        }
      },
      {
        name: 'recipientName',
        label: 'Recipient Name',
        id: 'recipient-name'
      },
      {
        name: 'recipientStreet',
        label: 'Recipient Street',
        id: 'recipient-street'
      },
      {
        name: 'recipientPostalCode',
        label: 'Recipient Postal Code',
        id: 'recipient-postal-code',
        type: 'number',
        props: {
          inputProps: {
            min: 0
          }
        }
      },
      {
        name: 'recipientCity',
        label: 'Recipient City',
        id: 'recipient-city'
      },
      {
        name: 'recipientCountry',
        label: 'Recipient Country',
        id: 'recipient-country'
      },
      {
        name: 'senderName',
        label: 'Sender Name',
        id: 'sender-name'
      },
      {
        name: 'senderStreet',
        label: 'Sender Street',
        id: 'sender-street'
      },
      {
        name: 'senderPostalCode',
        label: 'Sender Postal Code',
        id: 'sender-postal-code',
        type: 'number',
        props: {
          inputProps: {
            min: 0
          }
        }
      },
      {
        name: 'senderCity',
        label: 'Sender City',
        id: 'sender-city'
      },
      {
        name: 'senderCountry',
        label: 'Sender Country',
        id: 'recipient-country'
      }
    ];
  }

  onChange(event, inputValue, formCallback) {
    formCallback(event);
    const { value } = event.target;
    inputValue.value = stringOrNumber(value);
  }

  async submitForm() {
    this.setState({
      response: false
    });
    const data = {
      weight: this.inputFieldValues[0].value,
      recipient: {
        name: this.inputFieldValues[1].value,
        street: this.inputFieldValues[2].value,
        postalCode: this.inputFieldValues[3].value,
        city: this.inputFieldValues[4].value,
        country: this.inputFieldValues[5].value
      },
      sender: {
        name: this.inputFieldValues[6].value,
        street: this.inputFieldValues[7].value,
        postalCode: this.inputFieldValues[8].value,
        city: this.inputFieldValues[9].value,
        country: this.inputFieldValues[10].value
      }
    };
    try {
      let response = await axios.post(`${this.props.baseUrl}/parcel`, data);
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
      // console.error(error);
      // console.error(error.response.data);
    }

    //console.log('Hey', this.inputFieldValues);
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

    const weight = (
      <Grid container item xs={12}>
        {this.renderField(this.inputFieldValues[0], handleInputValue, errors)}
      </Grid>
    );

    const recipient = (
      <Grid container item xs={12} md={6} spacing={2}>
        {this.renderField(this.inputFieldValues[1], handleInputValue, errors)}
        {this.renderField(this.inputFieldValues[2], handleInputValue, errors)}
        {this.renderField(this.inputFieldValues[3], handleInputValue, errors)}
        {this.renderField(this.inputFieldValues[4], handleInputValue, errors)}
        {this.renderField(this.inputFieldValues[5], handleInputValue, errors)}
      </Grid>
    );
    const sender = (
      <Grid container item xs={12} md={6} spacing={2}>
        {this.renderField(this.inputFieldValues[6], handleInputValue, errors)}
        {this.renderField(this.inputFieldValues[7], handleInputValue, errors)}
        {this.renderField(this.inputFieldValues[8], handleInputValue, errors)}
        {this.renderField(this.inputFieldValues[9], handleInputValue, errors)}
        {this.renderField(this.inputFieldValues[10], handleInputValue, errors)}
      </Grid>
    );

    let response;
    if (this.state.response) {
      if (this.state.ok) {
        response = (
          <Grid container item xs={12} spacing={2}>
            <Grid item xs={12}>
              <Alert severity="info">
                <AlertTitle>Submit was successful</AlertTitle>
                Tracking ID: <strong>{this.state.data.trackingId}</strong>
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

    /// {this.inputFieldValues.map((inputFieldValue, index) => {
    //                   return (
    //                     <Grid item xs={12} md={inputFieldValue.name === 'weight' ? 12 : 6} key={index}>
    //                       <TextField
    //                         type={inputFieldValue.type ?? 'text'}
    //                         onChange={handleInputValue}
    //                         onBlur={handleInputValue}
    //                         name={inputFieldValue.name}
    //                         label={inputFieldValue.label}
    //                         multiline={inputFieldValue.multiline ?? false}
    //                         fullWidth
    //                         rows={inputFieldValue.rows ?? 1}
    //                         autoComplete="none"
    //                         autoCorrect={'true'}
    //                         InputProps={inputFieldValue.props ?? {}}
    //                         {...(errors[inputFieldValue.name] && {
    //                           error: true,
    //                           helperText: errors[inputFieldValue.name]
    //                         })}
    //                       />
    //                     </Grid>
    //                   );
    //                 })}

    return (
      <Container>
        <Grid container spacing={2}>
          <Grid item xs={12}>
            <h1>Submit Parcel</h1>
          </Grid>
          <Grid item xs={12}>
            <form
              onSubmit={(e) => {
                handleFormSubmit(e, this.submitForm.bind(this));
              }}>
              <Grid container item xs={12} spacing={2}>
                {weight}
                {recipient}
                {sender}
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

export default withValidatorHook(Submit);
