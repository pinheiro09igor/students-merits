import { useFormik } from 'formik';
import * as Yup from 'yup';
import { useRef, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { INewTransacao } from '../../services/interfaces';
import { HomeService } from '../../services';
import { AlunoService } from '../../../Aluno/services';

interface IProps {
  modalId: string;
  refetchTransacoes: () => void;
}

export const TransacaoForm = ({ modalId, refetchTransacoes }: IProps) => {
  const checkboxRef = useRef<HTMLInputElement>(null);

  const formik = useFormik<INewTransacao>({
    initialValues: {
      professorId: localStorage.getItem('id')!,
      alunoId: '',
      moedas: 0,
      descricao: '',
    },
    validationSchema: Yup.object({
      alunoId: Yup.string().required('Campo obrigatório'),
      moedas: Yup.number().required('Campo obrigatório').min(1, 'Valor mínimo é 1'),
      descricao: Yup.string().required('Campo obrigatório'),
    }),
    onSubmit: async (values) => {
      try {
        await HomeService.postTransacao(values.professorId, values.alunoId, values.moedas, values.descricao);
      } catch (error) {
        console.error(error);
      } finally {
        formik.resetForm();
        if (checkboxRef.current) checkboxRef.current.checked = false;
        refetchTransacoes();
      }
    },
  });

  const getAlunos = async () => {
    try {
      const alunos = await AlunoService.getAllAlunos();

      return alunos;
    } catch (error) {
      return [];
    }
  };

  const { data, isLoading, isFetching } = useQuery({
    queryKey: ['alunos'],
    queryFn: getAlunos,
  });

  return (
    <>
      <input ref={checkboxRef} type='checkbox' id={modalId} className='modal-toggle' />
      <div className='modal'>
        <div className='modal-box max-h-[700px]'>
          <div className='flex justify-between items-center mb-6'>
            <h2 className='text-xl'>Nova Transação</h2>
            <label
              htmlFor={modalId}
              className='modal-close btn btn-ghost'
              onClick={() => {
                formik.resetForm();
              }}
            >
              <span>X</span>
            </label>
          </div>
          {isLoading || isFetching ? (
            <div className='flex justify-center items-center h-96'>
              <div className='loader ease-linear rounded-full border-8 border-t-8 border-gray-600 h-16 w-16'></div>
            </div>
          ) : (
            <form
              onSubmit={(e) => {
                formik.handleSubmit(e);
              }}
            >
              <div className='flex flex-col gap-4 overflow-y-auto max-h-[460px] p-2'>
                <div className='form-control w-full'>
                  <select id='alunoId' name='alunoId' className={`select select-bordered w-full ${formik.errors.alunoId && formik.touched.alunoId && 'input-error'}`} value={formik.values.alunoId} onChange={formik.handleChange}>
                    <option value=''>Selecione um aluno</option>
                    {data?.map((aluno) => (
                      <option key={aluno._id} value={aluno._id}>
                        {aluno.nome}
                      </option>
                    ))}
                  </select>
                  {formik.errors.alunoId && formik.touched.alunoId && (
                    <label className='label'>
                      <span className='label-text-alt text-error'>{formik.errors.alunoId}</span>
                    </label>
                  )}
                </div>
                <div className='form-control w-full'>
                  <input type='number' placeholder='Moedas' id='moedas' name='moedas' className={`input input-bordered w-full ${formik.errors.moedas && formik.touched.moedas && 'input-error'}`} value={formik.values.moedas} onChange={formik.handleChange} />
                  {formik.errors.moedas && formik.touched.moedas && (
                    <label className='label'>
                      <span className='label-text-alt text-error'>{formik.errors.moedas}</span>
                    </label>
                  )}
                </div>
                <div className='form-control w-full'>
                  <textarea placeholder='Descriçao' id='descricao' name='descricao' className={`textarea textarea-bordered w-full ${formik.errors.descricao && formik.touched.descricao && 'input-error'}`} value={formik.values.descricao} onChange={formik.handleChange} />
                  {formik.errors.descricao && formik.touched.descricao && (
                    <label className='label'>
                      <span className='label-text-alt text-error'>{formik.errors.descricao}</span>
                    </label>
                  )}
                </div>
              </div>
              <div className='modal-action'>
                <label htmlFor={modalId}>
                  <button type='submit' className='btn'>
                    Transferir
                  </button>
                </label>
              </div>
            </form>
          )}
        </div>
      </div>
    </>
  );
};
