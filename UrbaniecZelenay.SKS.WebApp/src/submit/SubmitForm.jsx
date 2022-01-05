import { useState } from 'react';

const initialFormValues = {
  weight: 0,
  recipient: {
    name: '',
    street: '',
    postalCode: '',
    city: '',
    country: ''
  },
  sender: {
    name: '',
    street: '',
    postalCode: '',
    city: '',
    country: ''
  }
};

export const useFormControls = () => {
  // We'll update "values" as the form updates
  const [values, setValues] = useState(initialFormValues);
  // "errors" is used to check the form for errors
  const [errors, setErrors] = useState({});
  const validate = (fieldValues = values) => {
    // this function will check if the form values are valid
  };
  const handleInputValue = (fieldValues = values) => {
    // this function will be triggered by the text field's onBlur and onChange events
  };
  const handleFormSubmit = async (e) => {
    // this function will be triggered by the submit event
  };
  const formIsValid = () => {
    // this function will check if the form values and return a boolean value
    return true;
  };
  return {
    handleInputValue,
    handleFormSubmit,
    formIsValid,
    errors
  };
};
