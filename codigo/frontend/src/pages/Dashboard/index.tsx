import {
   Box,
   Paper,
   Tab,
   Table,
   TableBody,
   TableCell,
   TableContainer,
   TableHead,
   TableRow,
   Tabs,
   Typography,
} from "@mui/material";
import { useEffect, useState } from "react";
import { Aluno, Empresa } from "../../types/User";
import {
   apagarEmpresa,
   apagarAluno,
   obterTodasAsEmpresas,
   obterTodosOsUsuarios,
} from "../../hooks/api";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";

import "./index.css";

interface TabPanelProps {
   children?: React.ReactNode;
   index: number;
   value: number;
}

// const enderecoData: Endereco = {
//    rua: "",
//    numero: -1,
//    bairro: "",
//    cidade: "",
//    cep: "",
// };

// const alunoData: Aluno = {
//    nome: "",
//    email: "",
//    senha: "",
//    rg: "",
//    cpf: "",
//    endereco: enderecoData,
//    instituicaoDeEnsino: "",
// };

// const empresaData: Empresa = {
//    nome: "",
//    email: "",
//    senha: "",
//    cnpj: "",
// };

function CustomTabPanel(props: TabPanelProps) {
   const { children, value, index, ...other } = props;

   return (
      <div
         role="tabpanel"
         hidden={value !== index}
         id={`simple-tabpanel-${index}`}
         aria-labelledby={`simple-tab-${index}`}
         {...other}
      >
         {value === index && (
            <Box sx={{ p: 3 }}>
               <Typography>{children}</Typography>
            </Box>
         )}
      </div>
   );
}

function a11yProps(index: number) {
   return {
      id: `simple-tab-${index}`,
      "aria-controls": `simple-tabpanel-${index}`,
   };
}

const getEmpresas = async () => {
   const empresas: Empresa[] = await obterTodasAsEmpresas();
   return empresas;
};

const empresas = await getEmpresas();

const getAlunos = async () => {
   const alunos: Aluno[] = await obterTodosOsUsuarios();
   return alunos;
};

async function handleDeleteEmpresa(credencial: string) {
   empresas.forEach((element) => {
      if (element.email == credencial) {
         const index = empresas.indexOf(element);
         empresas.splice(index, 1);
      }
   });
   await apagarEmpresa(credencial);
}

async function handleDeleteAluno(credencial: string) {
   alunos.forEach((element) => {
      if (element.email == credencial) {
         const index = alunos.indexOf(element);
         alunos.splice(index, 1);
      }
   });
   await apagarAluno(credencial);
}

// async function handleUpdateAluno(credencial: string) {
//    alunos.forEach((element) => {
//       if (element.email == credencial) {
//          const index = alunos.indexOf(element);
//          alunos[index] = alunoData;
//       }
//    });
//    await atualizarAluno(credencial, alunoData);
// }

// async function handleUpdateEmpresa(credencial: string) {
//    empresas.forEach((element) => {
//       if (element.email == credencial) {
//          const index = empresas.indexOf(element);
//          empresas[index] = empresaData;
//       }
//    });
//    await atualizarEmpresa(credencial, empresaData);
// }

const alunos = await getAlunos();

export default function Dashboard() {
   const [value, setValue] = useState(0);
   const [array, setAlunos] = useState<Aluno[]>([]);
   const [array2, setEmpresas] = useState<Empresa[]>([]);

   useEffect(() => {
      setAlunos(alunos);
      setEmpresas(empresas);
   }, []);

   const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
      setValue(newValue);
   };

   return (
      <>
         <Box sx={{ borderBottom: 1, borderColor: "divider" }}>
            <Tabs
               value={value}
               onChange={handleChange}
               aria-label="basic tabs example"
            >
               <Tab label="Alunos" {...a11yProps(0)} />
               <Tab label="Empresas" {...a11yProps(1)} />
            </Tabs>
         </Box>
         <CustomTabPanel value={value} index={0}>
            <TableContainer component={Paper}>
               <Table sx={{ minWidth: 650 }} aria-label="simple table">
                  <TableHead>
                     <TableRow>
                        <TableCell align="right">Nome</TableCell>
                        <TableCell align="right">Email</TableCell>
                        <TableCell align="right">Senha</TableCell>
                        <TableCell align="right">RG</TableCell>
                        <TableCell align="right">CPF</TableCell>
                        <TableCell align="right">
                           Instituição de Ensino
                        </TableCell>
                        <TableCell align="right">Editar</TableCell>
                        <TableCell align="right">Apagar</TableCell>
                     </TableRow>
                  </TableHead>
                  <TableBody>
                     {array.map((row) => (
                        <TableRow
                           key={row.email}
                           sx={{
                              "&:last-child td, &:last-child th": { border: 0 },
                           }}
                        >
                           <TableCell component="th" scope="row">
                              {row.nome}
                           </TableCell>
                           <TableCell align="right">{row.email}</TableCell>
                           <TableCell align="right">{row.senha}</TableCell>
                           <TableCell align="right">{row.rg}</TableCell>
                           <TableCell align="right">{row.cpf}</TableCell>
                           <TableCell align="right">
                              {row.instituicaoDeEnsino}
                           </TableCell>
                           <TableCell align="right">
                              <button>
                                 <EditIcon />
                              </button>
                           </TableCell>
                           <TableCell align="right">
                              <button
                                 key={row.email}
                                 onClick={() => handleDeleteAluno(row.email)}
                              >
                                 <DeleteIcon />
                              </button>
                           </TableCell>
                        </TableRow>
                     ))}
                  </TableBody>
               </Table>
            </TableContainer>
         </CustomTabPanel>
         <CustomTabPanel value={value} index={1}>
            <TableContainer component={Paper}>
               <Table sx={{ minWidth: 650 }} aria-label="simple table">
                  <TableHead>
                     <TableRow>
                        <TableCell align="right">Nome</TableCell>
                        <TableCell align="right">Email</TableCell>
                        <TableCell align="right">Senha</TableCell>
                        <TableCell align="right">CNPJ</TableCell>
                        <TableCell align="right">Editar</TableCell>
                        <TableCell align="right">Apagar</TableCell>
                     </TableRow>
                  </TableHead>
                  <TableBody>
                     {array2?.map((row) => (
                        <TableRow
                           key={row.email}
                           sx={{
                              "&:last-child td, &:last-child th": { border: 0 },
                           }}
                        >
                           <TableCell align="right" component="th" scope="row">
                              {row.nome}
                           </TableCell>
                           <TableCell align="right">{row.email}</TableCell>
                           <TableCell align="right">{row.senha}</TableCell>
                           <TableCell align="right">{row.cnpj}</TableCell>
                           <TableCell align="right">
                              <button>
                                 <EditIcon />
                              </button>
                           </TableCell>
                           <TableCell align="right">
                              <button
                                 key={row.email}
                                 onClick={() => handleDeleteEmpresa(row.email)}
                              >
                                 <DeleteIcon />
                              </button>
                           </TableCell>
                        </TableRow>
                     ))}
                  </TableBody>
               </Table>
            </TableContainer>
         </CustomTabPanel>
      </>
   );
}
