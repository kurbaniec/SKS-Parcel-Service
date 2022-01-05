import { Component } from 'react';
import { useFormControls } from './SubmitForm';
import { TextField } from '@mui/material';
import Button from '@mui/material/Button';

class Submit extends Component {
  constructor(props) {
    super(props);
    const { handleInputValue, handleFormSubmit, formIsValid, errors } = this.props.validator;
    this.handleInputValue = handleInputValue;
    this.handleFormSubmit = handleFormSubmit;
    this.formIsValid = formIsValid;
    this.errors = errors;
  }

  render() {
    const baum = this.formIsValid();
    return (
      <div>
        <h1>Submit</h1>
        <form onSubmit={this.handleFormSubmit}>
          <TextField></TextField>
          <Button type="submit" disabled={!this.formIsValid()}>
            Send Message
          </Button>
        </form>
      </div>
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
