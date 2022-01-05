import { Component } from 'react';
import { useFormControls } from './SubmitForm';
import { Container, TextField } from '@mui/material';
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
      }
    ];
  }

  submitForm() {
    console.log('Hey', this.inputFieldValues);
  }

  render() {
    const { handleInputValue, handleFormSubmit, formIsValid, errors } = this.props.validator;

    return (
      <Container>
        <h1>Submit</h1>
        <form
          onSubmit={(e) => {
            handleFormSubmit(e, this.submitForm.bind(this));
          }}>
          {this.inputFieldValues.map((inputFieldValue, index) => {
            return (
              <TextField
                key={index}
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
            );
          })}
          <Button type="submit" disabled={!formIsValid()}>
            Send Message
          </Button>
        </form>
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
