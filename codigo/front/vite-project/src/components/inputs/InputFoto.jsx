import React, { useState, useEffect } from 'react';

import styles from './InputFoto.module.css'
import {BsBoxArrowUpRight} from 'react-icons/bs'

const InputFoto = ({ initialValue, onChange, required }) => {
  const [value, setValue] = useState("");
  const [fileName, setFileName] = useState("");

  const handleChange = ({ target }) => {
    const file = target.files[0]
    setValue(file)
    setFileName(file.name)
    onChange && onChange(file)
  }

  useEffect(() => {
    if(initialValue){
      setValue(initialValue)
      setFileName(initialValue.name)
    }

    const customFileUpload = document.getElementById('custom-file-upload');
    const fileUpload = document.getElementById('file-upload');

    const handleClick = () => {
      if(fileUpload){
        fileUpload.click();
      }
    }

    customFileUpload.addEventListener('click', handleClick);

    return () => {
      customFileUpload.removeEventListener('click', handleClick);
    };
  }, [initialValue]);

  return (
    <div className={styles.inputComponent}>
      <label htmlFor="foto" id="custom-file-upload">
        {fileName ? (
          <>
            <p className={styles.fileName}>{fileName}</p> <BsBoxArrowUpRight className={styles.uploadIcon} />
          </>
        ) : (
          <>
            Adicionar imagem <BsBoxArrowUpRight className={styles.uploadIcon} />
          </> 
        )}
      </label>
      <input name="foto" id="foto" type="file" className={styles.hiddenFileInput} onChange={handleChange} required={required && required}></input>
    </div>
  );
}

export default InputFoto;
