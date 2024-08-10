import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function getErrorMessage(control: AbstractControl): string {
  if (control.hasError('required')) {
    return `This field is required`;
  }
  if (control.hasError('email')) {
    return `Enter a valid email address`;
  }
  if (control.hasError('minlength')) {
    const requiredLength = control.getError('minlength').requiredLength;
    return `Minimum length is ${requiredLength} characters`;
  }
  if (control.hasError('maxlength')) {
    const currentLength = control.value.length;
    const requiredLength = control.getError('maxlength').requiredLength;
    return `Content too long ${currentLength}/${requiredLength}`;
  }
  if (control.hasError('noSpecialCharacters')) {
    return 'No special characters allowed';
  }
  if (control.hasError('validImageUrl')) {
    return 'Url is not valid, make sure the url ends in either: .png, .jpg or .jpeg';
  }
  return '';
}

export function noSpecialCharacters(): ValidatorFn {
  return (contorl: AbstractControl): { [key: string]: any } | null => {
    const pattern = /^[a-zA-Z0-9]*$/;
    const valid = pattern.test(contorl.value);
    return valid ? null : { noSpecialCharacters: { value: contorl.value } };
  };
}
export function validImageUrl(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const url = control.value;
    if (!url) {
      return null;
    }

    const isValidUrl = (str: string) => {
      try {
        new URL(str);
        return true;
      } catch (_) {
        return false;
      }
    };

    // Regular expression to check if the URL ends with .png, .jpg, or .jpeg
    const imageExtensionPattern = /\.(png|jpg|jpeg)$/i;

    const valid = isValidUrl(url) && imageExtensionPattern.test(url);

    return valid ? null : { validImageUrl: { value: url } };
  };
}
