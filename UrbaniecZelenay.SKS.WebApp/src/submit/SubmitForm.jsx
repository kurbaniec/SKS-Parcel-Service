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

/**
 * Validator
 * Source: https://dev.to/hibaeldursi/creating-a-contact-form-with-validation-with-react-and-material-ui-1am0
 * @returns {{formIsValid: (function(): boolean), handleInputValue: handleInputValue, handleFormSubmit: ((function(*): Promise<*>)|*), errors: {}}}
 */
export const useFormControls = () => {
  // We'll update "values" as the form updates
  const [values, setValues] = useState(initialFormValues);
  // "errors" is used to check the form for errors
  const [errors, setErrors] = useState({});

  const validate = (fieldValues = values) => {
    // this function will check if the form values are valid
    let temp = { ...errors };

    if ('weight' in fieldValues) {
      temp.weight = fieldValues.weight ? '' : 'This field is required';
      if (fieldValues.weight) {
        if (fieldValues.weight <= 0) temp.weight = 'Weight can not be zero or less';
      }
    }

    setErrors({
      ...temp
    });
  };

  const handleInputValue = (e) => {
    // this function will be triggered by the text field's onBlur and onChange events
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value
    });
    validate({ [name]: value });
  };

  const handleFormSubmit = async (e) => {
    // this function will be triggered by the submit event
    e.preventDefault();
    if (formIsValid()) {
      //await postContactForm(values);
      //alert("You've posted your form!")
    }
  };

  const formIsValid = (fieldValues = values) => {
    // this function will check if the form values and return a boolean value
    const isValid = fieldValues.weight && Object.values(errors).every((x) => x === '');
    return isValid;
  };

  return {
    handleInputValue,
    handleFormSubmit,
    formIsValid,
    errors
  };
};
