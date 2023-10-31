import { useFormik } from 'formik';
import * as Yup from 'yup';
import { useRef, useState } from 'react';
import { IAluno } from '../services/interfaces';
import { AlunoService } from '../services';
import { useQuery } from '@tanstack/react-query';

interface IProps {
  id: string | null;
  modalId: string;
  refetchAlunos: () => void;
  onClose: () => void;
}

export const AlunoForm = ({ id, modalId, refetchAlunos, onClose }: IProps) => {
  const checkboxRef = useRef<HTMLInputElement>(null);

  const formik = useFormik<IAluno>({
    initialValues: {
      nome: '',
      email: '',
      senha: '',
      moedas: 0,
      cpf: '',
      rg: '',
      endereco: '',
    },
    validationSchema: Yup.object({
      nome: Yup.string().required('Campo obrigatório'),
      email: Yup.string().email('Email inválido').required('Campo obrigatório'),
      senha: id ? Yup.string() : Yup.string().required('Campo obrigatório'),
      moedas: id ? Yup.number().required('Campo obrigatório') : Yup.number(),
      cpf: Yup.string().required('Campo obrigatório'),
      rg: Yup.string().required('Campo obrigatório'),
      endereco: Yup.string().required('Campo obrigatório'),
    }),
    onSubmit: async (values) => {
      try {
        if (id) {
          await AlunoService.updateAluno(values);
        } else {
          await AlunoService.createAluno(values);
        }
      } catch (error) {
        console.error(error);
      } finally {
        formik.resetForm();
        if (checkboxRef.current) checkboxRef.current.checked = false;
        onClose();
        refetchAlunos();
      }
    },
  });

  const excluirAluno = async () => {
    try {
      await AlunoService.deleteAluno(id!);
      formik.resetForm();
      if (checkboxRef.current) checkboxRef.current.checked = false;
      onClose();
      refetchAlunos();
    } catch (error) {
      console.log(error);
    }
  };

  const getAlunoSelecionado = async () => {
    try {
      if (id) {
        const aluno = await AlunoService.getAlunoById(id);

        formik.setValues(aluno);

        return aluno;
      } else {
        return {};
      }
    } catch (error) {
      onClose();

      return {
        nome: '',
        email: '',
        senha: '',
        moedas: 0,
        cpf: '',
        rg: '',
        endereco: '',
      };
    }
  };

  const { isLoading, isFetching } = useQuery({
    queryKey: ['aluno', id],
    queryFn: getAlunoSelecionado,
    enabled: !!id,
  });

  return (
    <>
      <input ref={checkboxRef} type='checkbox' id={modalId} className='modal-toggle' />
      <div className='modal'>
        <div className='modal-box max-h-[700px]'>
          <div className='flex justify-between items-center mb-6'>
            <h2 className='text-xl'>{id ? 'Editar' : 'Criar'} Aluno</h2>
            <label
              htmlFor={modalId}
              className='modal-close btn btn-ghost'
              onClick={() => {
                formik.resetForm();
                onClose();
              }}
            >
              <span>X</span>
            </label>
          </div>
          {id && (isLoading || isFetching) ? (
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
                  <input type='text' placeholder='Nome' id='nome' name='nome' className={`input input-bordered w-full ${formik.errors.nome && formik.touched.nome && 'input-error'}`} value={formik.values.nome} onChange={formik.handleChange} />
                  {formik.errors.nome && formik.touched.nome && (
                    <label className='label'>
                      <span className='label-text-alt text-error'>{formik.errors.nome}</span>
                    </label>
                  )}
                </div>
                <div className='form-control w-full'>
                  <input type='email' placeholder='Email' id='email' name='email' className={`input input-bordered w-full ${formik.errors.email && formik.touched.email && 'input-error'}`} value={formik.values.email} onChange={formik.handleChange} />
                  {formik.errors.email && formik.touched.email && (
                    <label className='label'>
                      <span className='label-text-alt text-error'>{formik.errors.email}</span>
                    </label>
                  )}
                </div>
                <div className='form-control w-full'>
                  <input type='password' placeholder='Senha' id='senha' name='senha' className={`input input-bordered w-full ${formik.errors.senha && formik.touched.senha && 'input-error'}`} value={formik.values.senha} onChange={formik.handleChange} />
                  {formik.errors.senha && formik.touched.senha && (
                    <label className='label'>
                      <span className='label-text-alt text-error'>{formik.errors.senha}</span>
                    </label>
                  )}
                </div>
                {id && (
                  <div className='form-control w-full'>
                    <input type='number' placeholder='Moeda' id='moedas' name='moedas' className={`input input-bordered w-full ${formik.errors.moedas && formik.touched.moedas && 'input-error'}`} value={formik.values.moedas} onChange={formik.handleChange} />
                    {formik.errors.moedas && formik.touched.moedas && (
                      <label className='label'>
                        <span className='label-text-alt text-error'>{formik.errors.moedas}</span>
                      </label>
                    )}
                  </div>
                )}
                <div className='form-control w-full'>
                  <input type='text' placeholder='CPF' id='cpf' name='cpf' className={`input input-bordered w-full ${formik.errors.cpf && formik.touched.cpf && 'input-error'}`} value={formik.values.cpf} onChange={formik.handleChange} />
                  {formik.errors.cpf && formik.touched.cpf && (
                    <label className='label'>
                      <span className='label-text-alt text-error'>{formik.errors.cpf}</span>
                    </label>
                  )}
                </div>
                <div className='form-control w-full'>
                  <input type='text' placeholder='RG' id='rg' name='rg' className={`input input-bordered w-full ${formik.errors.rg && formik.touched.rg && 'input-error'}`} value={formik.values.rg} onChange={formik.handleChange} />
                  {formik.errors.rg && formik.touched.rg && (
                    <label className='label'>
                      <span className='label-text-alt text-error'>{formik.errors.rg}</span>
                    </label>
                  )}
                </div>
                <div className='form-control w-full'>
                  <input type='text' placeholder='Endereço' id='endereco' name='endereco' className={`input input-bordered w-full ${formik.errors.endereco && formik.touched.endereco && 'input-error'}`} value={formik.values.endereco} onChange={formik.handleChange} />
                  {formik.errors.endereco && formik.touched.endereco && (
                    <label className='label'>
                      <span className='label-text-alt text-error'>{formik.errors.endereco}</span>
                    </label>
                  )}
                </div>
              </div>
              <div className='modal-action'>
                {id && (
                  <button className='btn btn-error' type='button' onClick={() => excluirAluno()}>
                    Excluir
                  </button>
                )}
                <label htmlFor={modalId}>
                  <button type='submit' className='btn'>
                    Salvar
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
