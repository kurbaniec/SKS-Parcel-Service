import { useState } from 'react';
import { stringOrNumber } from '../utils/stringOrNumber';

const initialFormValues = {
  trackingId: ''
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
  let dirty = false;
  const regex = new RegExp('^[A-Z0-9]{9}$');

  const validate = (fieldValues = values) => {
    // this function will check if the form values are valid
    let temp = { ...errors };

    if ('trackingId' in fieldValues) {
      temp.trackingId = fieldValues.trackingId ? '' : 'This field is required';
      if (fieldValues.trackingId) {
        if (!regex.test(fieldValues.trackingId))
          temp.trackingId = 'Tracking ID must match "^[A-Z0-9]{9}$"';
      }
    }

    setErrors({
      ...temp
    });
  };

  const handleInputValue = (e) => {
    // this function will be triggered by the text field's onBlur and onChange events
    if (!dirty) dirty = true;
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
      dirty = true;
    }
    if (formIsValid()) {
      submitFunc();
      //await postContactForm(values);
      //alert("You've posted your form!")
    }
  };

  const formIsValid = (fieldValues = values) => {
    // this function will check if the form values and return a boolean value
    // const test =
    //   fieldValues.weight &&
    //   fieldValues.recipientName &&
    //   fieldValues.recipientStreet &&
    //   fieldValues.recipientPostalCode &&
    //   fieldValues.recipientCity &&
    //   fieldValues.recipientCountry &&
    //   fieldValues.senderName &&
    //   fieldValues.senderStreet &&
    //   fieldValues.senderPostalCode &&
    //   fieldValues.senderCity &&
    //   fieldValues.senderCountry &&
    //   Object.values(errors).every((x) => x === '');
    // return test;

    return (
      //Object.values(errors).length === Object.values(initialFormValues).length &&
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
