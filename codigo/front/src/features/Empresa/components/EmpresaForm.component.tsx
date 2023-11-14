import { useFormik } from 'formik';
import * as Yup from 'yup';
import { useRef, useState } from 'react';
import { IEmpresa } from '../services/interfaces';
import { EmpresaService } from '../services';
import { useQuery } from '@tanstack/react-query';

interface IProps {
  id: string | null;
  modalId: string;
  refetchEmpresas: () => void;
  onClose: () => void;
}

export const EmpresaForm = ({ id, modalId, refetchEmpresas, onClose }: IProps) => {
  const checkboxRef = useRef<HTMLInputElement>(null);

  const formik = useFormik<IEmpresa>({
    initialValues: {
      nome: '',
      email: '',
      senha: '',
    },
    validationSchema: Yup.object({
      nome: Yup.string().required('Campo obrigatório'),
      email: Yup.string().email('Email inválido').required('Campo obrigatório'),
      senha: id ? Yup.string() : Yup.string().required('Campo obrigatório'),
    }),
    onSubmit: async (values) => {
      try {
        if (id) {
          await EmpresaService.updateEmpresa(values);
        } else {
          await EmpresaService.createEmpresa(values);
        }
      } catch (error) {
        console.error(error);
      } finally {
        formik.resetForm();
        if (checkboxRef.current) checkboxRef.current.checked = false;
        onClose();
        refetchEmpresas();
      }
    },
  });

  const excluirEmpresa = async () => {
    try {
      await EmpresaService.deleteEmpresa(id!);
      formik.resetForm();
      if (checkboxRef.current) checkboxRef.current.checked = false;
      onClose();
      refetchEmpresas();
    } catch (error) {
      console.log(error);
    }
  };

  const getEmpresaSelecionada = async () => {
    try {
      if (id) {
        const empresa = await EmpresaService.getEmpresaById(id);

        formik.setValues(empresa);

        return empresa;
      } else {
        return {};
      }
    } catch (error) {
      return {
        nome: '',
        email: '',
        senha: '',
      };
    }
  };

  const { isLoading, isFetching } = useQuery({
    queryKey: ['empresa', id],
    queryFn: getEmpresaSelecionada,
  });

  return (
    <>
      <input ref={checkboxRef} type='checkbox' id={modalId} className='modal-toggle' />
      <div className='modal'>
        <div className='modal-box max-h-[700px]'>
          <div className='flex justify-between items-center mb-6'>
            <h2 className='text-xl'>{id ? 'Editar' : 'Criar'} Empresa</h2>
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
            <form onSubmit={formik.handleSubmit}>
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
              </div>
              <div className='modal-action'>
                {id && (
                  <button type='button' className='btn btn-error' onClick={() => excluirEmpresa()}>
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
