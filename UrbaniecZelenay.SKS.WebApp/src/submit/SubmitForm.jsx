import { useState } from 'react';
import { stringOrNumber } from '../utils/stringOrNumber';

const initialFormValues = {
  weight: 0,
  recipientName: '',
  recipientStreet: '',
  recipientPostalCode: '',
  recipientCity: '',
  recipientCountry: '',
  senderName: '',
  senderStreet: '',
  senderPostalCode: '',
  senderCity: '',
  senderCountry: ''
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

  const validate = (fieldValues = values) => {
    // this function will check if the form values are valid
    let temp = { ...errors };

    if ('weight' in fieldValues) {
      temp.weight = fieldValues.weight ? '' : 'This field is required';
      if (fieldValues.weight) {
        if (fieldValues.weight <= 0) temp.weight = 'Weight can not be zero or less';
      }
    }
    if ('recipientName' in fieldValues)
      temp.recipientName = fieldValues.recipientName ? '' : 'This field is required';
    if ('recipientStreet' in fieldValues)
      temp.recipientStreet = fieldValues.recipientStreet ? '' : 'This field is required';
    if ('recipientPostalCode' in fieldValues) {
      temp.recipientPostalCode = fieldValues.recipientPostalCode ? '' : 'This field is required';
      if (fieldValues.recipientPostalCode) {
        if (fieldValues.recipientPostalCode <= 0)
          temp.recipientPostalCode = 'Postal Code can not be zero or less';
      }
    }
    if ('recipientCity' in fieldValues)
      temp.recipientCity = fieldValues.recipientCity ? '' : 'This field is required';
    if ('recipientCountry' in fieldValues)
      temp.recipientCountry = fieldValues.recipientCountry ? '' : 'This field is required';
    if ('senderName' in fieldValues)
      temp.senderName = fieldValues.senderName ? '' : 'This field is required';
    if ('senderStreet' in fieldValues)
      temp.senderStreet = fieldValues.senderStreet ? '' : 'This field is required';
    if ('senderPostalCode' in fieldValues) {
      temp.senderPostalCode = fieldValues.senderPostalCode ? '' : 'This field is required';
      if (fieldValues.senderPostalCode) {
        if (fieldValues.senderPostalCode <= 0)
          temp.senderPostalCode = 'Postal Code can not be zero or less';
      }
    }
    if ('senderCity' in fieldValues)
      temp.senderCity = fieldValues.senderCity ? '' : 'This field is required';
    if ('senderCountry' in fieldValues)
      temp.senderCountry = fieldValues.senderCountry ? '' : 'This field is required';

    setErrors({
      ...temp
    });
  };

  const handleInputValue = (e) => {
    // this function will be triggered by the text field's onBlur and onChange events
    if (!dirty) dirty = true;
    let { name, value } = e.target;
    value = stringOrNumber(value);
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
