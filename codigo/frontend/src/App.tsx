import { Route, Routes } from 'react-router-dom';
import './App.css'
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';

function App() {
  return (
    <>
      <nav>
        
      </nav>
      <Routes>
        <Route index element={<Home/>}/>
        <Route path='logar' element={<Login/>}/>
        <Route path='registrar' element={<Register/>}/>
      </Routes>
    </>
  )
}

export default App;
