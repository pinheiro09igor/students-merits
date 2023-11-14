import { useNavigate } from 'react-router-dom';
import { Extrato } from './Extrato.component';
import { Vantagens } from '../../Vantagem';
import { useState } from 'react';

export const Home = () => {
  const [currentTab, setCurrentTab] = useState(0);

  const navigate = useNavigate();

  const tipo = localStorage.getItem('tipo');

  return (
    <div
      className='w-full h-full flex flex-col'
      style={{
        backgroundImage: 'radial-gradient(rgba(255, 255, 255, 0.1) 1px, transparent 1px)',
        backgroundPosition: '50% 50%',
        backgroundSize: '1.1rem 1.1rem',
      }}
    >
      <div className='navbar bg-base-200'>
        <div className='flex-1'>
          <a className='btn btn-ghost normal-case text-xl'>Sistema de Mérito Escolar</a>
        </div>
        <div className='flex-none'>
          <div className='dropdown dropdown-end'>
            <label tabIndex={0} className='btn btn-ghost btn-circle avatar'>
              <svg xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 24 24' strokeWidth={1.5} stroke='currentColor' className='w-6 h-6'>
                <path strokeLinecap='round' strokeLinejoin='round' d='M17.982 18.725A7.488 7.488 0 0012 15.75a7.488 7.488 0 00-5.982 2.975m11.963 0a9 9 0 10-11.963 0m11.963 0A8.966 8.966 0 0112 21a8.966 8.966 0 01-5.982-2.275M15 9.75a3 3 0 11-6 0 3 3 0 016 0z' />
              </svg>
            </label>
            <ul tabIndex={0} className='menu menu-compact dropdown-content mt-3 p-2 shadow bg-base-200 rounded-box w-52'>
              <li>
                <a
                  onClick={() => {
                    localStorage.removeItem('id');
                    localStorage.removeItem('tipo');

                    navigate('/login');
                  }}
                >
                  Logout
                </a>
              </li>
            </ul>
          </div>
        </div>
      </div>
      {tipo === 'professor' && <Extrato />}
      {tipo === 'aluno' && (
        <div className='flex flex-col grow h-full'>
          <div className='tabs mx-12 my-6'>
            <a className={`tab tab-bordered tab-lg ${currentTab === 0 && 'tab-active'}`} onClick={() => setCurrentTab(0)}>
              Extrato
            </a>
            <a className={`tab tab-bordered tab-lg ${currentTab === 1 && 'tab-active'}`} onClick={() => setCurrentTab(1)}>
              Vantagens
            </a>
          </div>
          {currentTab === 0 && <Extrato />}
          {currentTab === 1 && <Vantagens />}
        </div>
      )}
      {tipo === 'empresa' && <Vantagens />}
    </div>
  );
};
