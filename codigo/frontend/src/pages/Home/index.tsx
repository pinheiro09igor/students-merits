import { FormControl, FormControlLabel, FormLabel, Radio, RadioGroup } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function Home() {
   const navigate = useNavigate();

   const handleAluno = () => {
      navigate("/aluno/registrar");
   };

   const handleEmpresa = () => {
      navigate("/empresa/registrar");
   };

   return (
      <div>
         <FormControl>
            <FormLabel id="demo-radio-buttons-group-label">
               Tipo de Acesso
            </FormLabel>
            <RadioGroup
               aria-labelledby="demo-radio-buttons-group-label"
               defaultValue="female"
               name="radio-buttons-group"
            />
            <FormControlLabel
               value="female"
               control={<Radio />}
               label="Aluno"
               onClick={handleAluno}
            />
            <FormControlLabel
               value="male"
               control={<Radio />}
               label="Empresa"
               onClick={handleEmpresa}
            />
         </FormControl>
      </div>
   );
}
