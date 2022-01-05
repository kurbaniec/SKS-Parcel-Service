import { useState } from 'react';

const initialFormValues = {
  trackingId: '',
  code: ''
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
  const [dirty, setDirty] = useState(false);
  const trackingIdRegex = new RegExp('^[A-Z0-9]{9}$');
  const codeRegex = new RegExp('^[A-Z]{4}\\d{1,4}$');

  const validate = (fieldValues = values) => {
    // this function will check if the form values are valid
    let temp = { ...errors };

    if ('trackingId' in fieldValues) {
      temp.trackingId = fieldValues.trackingId ? '' : 'This field is required';
      if (fieldValues.trackingId) {
        if (!trackingIdRegex.test(fieldValues.trackingId))
          temp.trackingId = 'Tracking ID must match "^[A-Z0-9]{9}$"';
      }
    }
    if ('code' in fieldValues) {
      temp.code = fieldValues.code ? '' : 'This field is required';
      if (fieldValues.code) {
        if (!codeRegex.test(fieldValues.code)) temp.code = 'Code must match "^[A-Z]{4}\\d{1,4}$"';
      }
    }

    setErrors({
      ...temp
    });
  };

  const handleInputValue = (e) => {
    // this function will be triggered by the text field's onBlur and onChange events
    if (!dirty) setDirty(true);
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value
    });
    validate({ [name]: value });
  };

  const handleFormSubmit = async (e, submitFunc) => {
    // this function will be triggered by the submit event
    e.preventDefault();
    if (!dirty) {
      validate(values);
      setDirty(true);
    } else if (formIsValid()) {
      submitFunc();
    }
  };

  const formIsValid = (fieldValues = values) => {
    return (
      Object.values(errors).length === Object.values(initialFormValues).length &&
      Object.values(errors).every((x) => x === '')
    );
  };

  return {
    handleInputValue,
    handleFormSubmit,
    formIsValid,
    errors
  };
};
