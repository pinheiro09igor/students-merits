import { useState } from 'react';
import { Alunos } from '../../Aluno';
import { Empresas } from '../../Empresa';

export const Admin = () => {
  const [currentTab, setCurrentTab] = useState(0);

  return (
    <div
      className='flex flex-col grow h-full'
      style={{
        backgroundImage: 'radial-gradient(rgba(255, 255, 255, 0.1) 1px, transparent 1px)',
        backgroundPosition: '50% 50%',
        backgroundSize: '1.1rem 1.1rem',
      }}
    >
      <div className='tabs mx-12 my-6'>
        <a className={`tab tab-bordered tab-lg ${currentTab === 0 && 'tab-active'}`} onClick={() => setCurrentTab(0)}>
          Alunos
        </a>
        <a className={`tab tab-bordered tab-lg ${currentTab === 1 && 'tab-active'}`} onClick={() => setCurrentTab(1)}>
          Empresas
        </a>
      </div>
      {currentTab === 0 && <Alunos />}
      {currentTab === 1 && <Empresas />}
    </div>
  );
};
