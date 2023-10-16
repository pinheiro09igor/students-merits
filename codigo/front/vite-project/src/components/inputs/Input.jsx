/* eslint-disable react/prop-types */
import { useEffect, useState } from "react";

import styles from "./Input.module.css";

const Input = ({
  type,
  name,
  id,
  className,
  initialValue,
  onChange,
  label,
  required,
  disabled,
  onKeyDown,
  placeholder,
  onBlur,
}) => {
  const [value, setValue] = useState("");

  const handleChange = ({ target }) => {
    setValue(target.value);
    onChange && onChange(id, target.value);
  };

  useEffect(() => {
    initialValue && setValue(initialValue);
  }, [initialValue]);

  return (
    <div className={styles.input}>
      {label && <label htmlFor={id}>{label}</label>}

      <input
        type={type}
        id={id}
        className={className ? className : ""}
        onChange={handleChange}
        value={value}
        name={name}
        placeholder={placeholder && placeholder}
        required={required && required}
        disabled={disabled && disabled}
        onKeyDown={onKeyDown}
        onBlur={onBlur}
      />
    </div>
  );
};

export default Input;
