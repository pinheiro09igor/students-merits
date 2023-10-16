import React, { useContext, useState, useEffect } from "react";
import styles from "./EnviarMoedas.module.css";
import Input from "../../components/inputs/Input";
import Button from "../../components/buttons/Button";

import Snackbar from "@mui/material/Snackbar";
import MuiAlert from "@mui/material/Alert";

import { BiErrorCircle } from "react-icons/bi";

import { LoginContext } from "../../context/LoginContext";

const Alert = React.forwardRef(function Alert(props, ref) {
  return <MuiAlert elevation={6} ref={ref} variant="filled" {...props} />;
});

const EnviarMoedas = () => {
  const [aluno, setAluno] = useState({});
  const [valor, setValor] = useState(0);
  const [moedas, setMoedas] = useState(0);
  const [saldoTotal, setSaldoTotal] = useState(0);
  const [disabled, setDisabled] = useState(false);
  const [openMessage, setOpenMessage] = useState(false);
  const [openErrorMessage, setOpenErrorMessage] = useState(false);
  const [openCoinErrorMessage, setCoinErrorMessage] = useState(false);
  const [validEmail, setValidEmail] = useState(false);
  const { user } = useContext(LoginContext);

  useEffect(() => {
    if (user && user.pessoa && user.pessoa.carteira) {
      setSaldoTotal(user.pessoa.carteira.saldo);
    }
  }, [user]);

  function getAluno(e) {
    const email = e.target.value;

    fetch(`http://localhost:7077/api/aluno/email/${email}`, {
      //Não implementado
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((resp) => resp.json())
      .then((data) => {
        if (data && data.pessoa) {
          setAluno(data);
          setValidEmail(true);
        } else {
          setAluno({});
          setValidEmail(false);
        }
      });
  }

  function coinValidation(e) {
    const saldoTotal =
      user && user.pessoa && user.pessoa.carteira
        ? user.pessoa.carteira.saldo
        : 0;
    const valorDigitado = parseInt(e.target.value);

    if (valorDigitado > saldoTotal) {
      setDisabled(true);
      setCoinErrorMessage(true);
    } else {
      setDisabled(false);
      setMoedas(valorDigitado.toString());
      setCoinErrorMessage(false);
    }
  }

  const handleClose = (event, reason) => {
    if (reason === "clickaway") {
      return;
    }

    setOpenMessage(false);
    setOpenErrorMessage(false);
  };

  function handleKeyDown(event) {
    if (event.keyCode === 189 || event.keyCode === 109) {
      event.preventDefault();
      setValor(0);
    }
  }

  function enviarMoedas(e) {
    e.preventDefault();
    var email = document.getElementById("inputMatricula").value;
    var moedas = document.getElementById("inputMoedas").value;
    var mensagem = document.getElementById("inputMensagem").value;

    if (!email || !moedas || !mensagem || !validEmail) {
      setOpenErrorMessage(true);
      return;
    }

    if (!aluno || !aluno.pessoa || !aluno.pessoa._id) {
      setOpenErrorMessage(true);
      return;
    }

    fetch(`http://localhost:7077/api/carteira/transacao`, {
      //Não implementado
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        descricao: mensagem,
        destino: aluno.pessoa._id,
        origem: user.pessoa._id,
        valor: parseInt(moedas),
      }),
    })
      .then((resp) => resp.json())
      .then(setOpenMessage(true))
      .then(setValidEmail(false))
      .catch((err) => console.error(err));
  }

  return (
    <div className={styles.parent}>
      <div className={styles.leftScreen}>
        <div>
          <h1 className={styles.title}>Enviar moedas</h1>
          <span className={styles.ponto}>.</span>
        </div>
        <div>
          <p className={styles.subtitle}>
            Envie moedas para seus alunos como forma de reconhecimento por bom
            comportamento, participação nas aulas, etc.
          </p>
        </div>
        <div className={styles.inputParent}>
          <div className={styles.input}>
            <Input
              type="text"
              name="Email"
              label="Email"
              id="inputMatricula"
              onBlur={getAluno}
            />
          </div>
          <div className={styles.inputInformations}>
            <p>Nome: {aluno && aluno.pessoa ? aluno.pessoa.nome : ""}</p>
            <p>Curso: {aluno && aluno.curso ? aluno.curso : ""}</p>
          </div>
        </div>
        {openCoinErrorMessage && (
          <div className={styles.pCoinMessage}>
            <p>
              <BiErrorCircle className={styles.iconError} /> Digite um valor
              menor que o seu saldo atual
            </p>
          </div>
        )}
        <div className={styles.inputParent}>
          <div className={styles.input}>
            <Input
              type="number"
              name="Moedas"
              label="Moedas"
              id="inputMoedas"
              onKeyDown={handleKeyDown}
              min="1"
              onBlur={coinValidation}
            />
          </div>
          <div className={styles.inputInformations}>
            <p>
              Saldo atual:{" "}
              {user && user.pessoa && user.pessoa.carteira
                ? user.pessoa.carteira.saldo
                : ""}
            </p>
            <p>
              Saldo final:{" "}
              {user && user.pessoa && user.pessoa.carteira && moedas
                ? isNaN(user.pessoa.carteira.saldo - moedas)
                  ? "0"
                  : user.pessoa.carteira.saldo - moedas
                : "0"}
            </p>
          </div>
        </div>
        <div>
          <Input
            type="text"
            className={styles.inputMensagem}
            name="Mensagem"
            label="Mensagem"
            id="inputMensagem"
            disabled={disabled && disabled}
          />
        </div>
        <div className={styles.divButton}>
          <Button
            type="submit"
            className="submit"
            id="btnEnviar"
            children="Enviar"
            onClick={enviarMoedas}
          />
        </div>
      </div>
      <div className={styles.rightScreen}>
        <img
          src="../../../public/dinheiro.png"
          alt=""
          className={styles.coin}
        />
      </div>
      <Snackbar
        open={openMessage}
        autoHideDuration={2000}
        onClose={handleClose}
      >
        <Alert onClose={handleClose} severity="success" sx={{ width: "100%" }}>
          Moedas enviadas com sucesso!
        </Alert>
      </Snackbar>
      <Snackbar
        open={openErrorMessage}
        autoHideDuration={2000}
        onClose={handleClose}
      >
        <Alert onClose={handleClose} severity="error" sx={{ width: "100%" }}>
          Preencha todos os campos!
        </Alert>
      </Snackbar>
    </div>
  );
};

export default EnviarMoedas;
