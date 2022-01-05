import { Component } from 'react';
import { useFormControls } from './SubmitForm';
import { Container, Grid, TextField } from '@mui/material';
import Button from '@mui/material/Button';

class Submit extends Component {
  constructor(props) {
    super(props);
    // const { handleInputValue, handleFormSubmit, formIsValid, errors } = this.props.validator;
    // this.handleInputValue = handleInputValue;
    // this.handleFormSubmit = handleFormSubmit;
    // this.formIsValid = formIsValid;
    // this.errors = errors;

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

  submitForm() {
    console.log('Hey', this.inputFieldValues);
  }

  renderField(inputFieldValue, handleInputValue, errors) {
    return (
      <Grid item xs={12}>
        <TextField
          type={inputFieldValue.type ?? 'text'}
          onChange={handleInputValue}
          onBlur={handleInputValue}
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
            <h1>Submit</h1>
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
                  <Button type="submit" disabled={!formIsValid()}>
                    Send Message
                  </Button>
                </Grid>
              </Grid>
            </form>
          </Grid>
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
