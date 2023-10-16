import { useState, forwardRef, useContext } from "react";

import Input from "../../components/inputs/Input";
import InputFoto from "../../components/inputs/InputFoto";
import Button from "../../components/buttons/Button";

import styles from "./CadastroDeVantagens.module.css";

import Snackbar from "@mui/material/Snackbar";
import MuiAlert from "@mui/material/Alert";

import { useNavigate } from "react-router-dom";
import { LoginContext } from "../../context/LoginContext";

const Alert = forwardRef(function Alert(props, ref) {
  return <MuiAlert elevation={6} ref={ref} variant="filled" {...props} />;
});

function CadastroDeVantagens() {
  const [openMessage, setOpenMessage] = useState(false);
  const [openErrorMessage, setOpenErrorMessage] = useState(false);

  const navigate = useNavigate();

  const { user } = useContext(LoginContext);

  const cadastrarVantagem = async (e) => {
    e.preventDefault();

    var nome = document.getElementById("inputVantagem").value;
    var descricao = document.getElementById("inputDesc").value;
    var preco = document.getElementById("inputValor").value;
    var foto = document.getElementById("foto").files[0];

    if (!nome || !descricao || !preco || !foto) {
      setOpenErrorMessage(true);
      return;
    }

    const formData = new FormData();
    formData.append("nome", nome);
    formData.append("descricao", descricao);
    formData.append("preco", preco);
    formData.append("foto", foto);
    formData.append("empresa", user._id);

    try {
      await fetch("http://localhost:7077/api/vantagem", {
        method: "POST",
        body: formData,
      })
        .then(setOpenMessage(true))
        .then(setTimeout(() => navigate("/"), 1000))
        .catch((err) => console.log(err));
    } catch (error) {
      console.error("Erro de rede:", error);
    }
  };

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
    }
  }

  return (
    <div className={styles.parent}>
      <div className={styles.leftScreen}>
        <div>
          <h1 className={styles.title}>Cadastre Vantagens</h1>
          <span className={styles.ponto}>.</span>
        </div>
        <div>
          <p className={styles.subtitle}>
            Cadastrar vantagens possibilita que os alunos de escolas e
            universidades comprem
          </p>
        </div>
        <form onSubmit={cadastrarVantagem} encType="multipart/form-data">
          <div className={styles.inputParent}>
            <div className={styles.input}>
              <Input
                type="text"
                name="inputVantagem"
                label="Nome da vantagem"
                id="inputVantagem"
                required
              />
            </div>
          </div>
          <div className={styles.inputParent}>
            <div className={styles.input}>
              <Input
                type="number"
                name="inputValor"
                label="Valor em moedas"
                id="inputValor"
                onKeyDown={handleKeyDown}
                required
              />
            </div>
          </div>
          <div className={styles.inputParent}>
            <div className={styles.input}>
              <Input
                type="text"
                name="inputDesc"
                label="Descrição"
                id="inputDesc"
                required
              />
            </div>
          </div>
          <div className={styles.inputParent}>
            <div className={styles.input}>
              <InputFoto id="foto" />
            </div>
          </div>
          <div className={styles.divButton}>
            <Button type="submit" className="submit" id="btnEnviar">
              Enviar
            </Button>
          </div>
        </form>
      </div>
      <div className={styles.rightScreen}>
        <img src="/cart.png" alt="" className={styles.cart} />
      </div>
      <Snackbar
        open={openMessage}
        autoHideDuration={2000}
        onClose={handleClose}
      >
        <Alert onClose={handleClose} severity="success" sx={{ width: "100%" }}>
          Vantagem cadastrada com sucesso!
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
}

export default CadastroDeVantagens;
