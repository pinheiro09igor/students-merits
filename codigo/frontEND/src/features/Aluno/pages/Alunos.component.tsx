import { useQuery } from '@tanstack/react-query';
import { useState } from 'react';
import { AlunoService } from '../services';
import { IAluno } from '../services/interfaces';
import { AlunoForm } from '../components';

const modalId: string = 'alunoForm';

export const Alunos = () => {
  const [selectedAluno, setSelectedAluno] = useState<string | null>(null);

  const getListaAlunos = async () => {
    try {
      return await AlunoService.getAllAlunos();
    } catch (error) {
      return [];
    }
  };

  const { data, refetch } = useQuery<IAluno[]>({
    queryKey: ['alunos'],
    queryFn: getListaAlunos,
    refetchOnWindowFocus: false,
  });

  return (
    <>
      <div className='flex justify-end items-center mx-12'>
        <label htmlFor={modalId} className='btn'>
          Criar
        </label>
      </div>
      <div className='flex flex-col h-full bg-base-200 rounded-md shadow-lg mx-12 my-6'>
        <table className='table w-full'>
          <thead>
            <tr>
              <th>Nome</th>
              <th>Email</th>
              <th>Moedas</th>
              <th></th>
            </tr>
          </thead>
          <tbody className='overflow-x-auto'>
            {data &&
              data.length > 0 &&
              data.map(
                (aluno: IAluno) => (
                  <tr key={aluno._id}>
                    <td>{aluno.nome}</td>
                    <td>{aluno.email}</td>
                    <td>{aluno.moedas}</td>
                    <td>
                      <div className='flex justify-end'>
                        <label
                          htmlFor={modalId}
                          className='btn btn-sm'
                          onClick={() => {
                            setSelectedAluno(aluno._id!);
                          }}
                        >
                          Editar
                        </label>
                      </div>
                    </td>
                  </tr>
                ),
                []
              )}
          </tbody>
        </table>
        {(data?.length === 0 || data === undefined) && (
          <div className='flex justify-center items-center p-2'>
            <p className='text-gray-500'>Nenhum aluno cadastrado</p>
          </div>
        )}
      </div>
      <AlunoForm id={selectedAluno} modalId={modalId} refetchAlunos={refetch} onClose={() => setSelectedAluno(null)} />
    </>
  );
};
