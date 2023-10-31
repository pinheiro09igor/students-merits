import { useFormik } from 'formik';
import * as Yup from 'yup';
import { ChangeEvent, useRef } from 'react';
import { IVantagem } from '../../services/interfaces';
import { VantagemService } from '../../services';
import { useQuery } from '@tanstack/react-query';

interface IProps {
  id: string | null;
  modalId: string;
  refetchVantagens: () => void;
  onClose?: () => void;
}

export const VantagemForm = ({ id, modalId, refetchVantagens, onClose }: IProps) => {
  const checkboxRef = useRef<HTMLInputElement>(null);
  const fotoRef = useRef<HTMLInputElement>(null);
  const idLogged = localStorage.getItem('id');

  const formik = useFormik<IVantagem>({
    initialValues: {
      nome: '',
      descricao: '',
      valor: 0,
      fotoName: '',
      foto: '',
      idEmpresa: idLogged!,
    },
    validationSchema: Yup.object({
      nome: Yup.string().required('Campo obrigatório'),
      descricao: Yup.string().required('Campo obrigatório'),
      valor: Yup.number().required('Campo obrigatório').min(1, 'Valor mínimo de 1'),
      foto: Yup.string().required('Campo obrigatório'),
    }),
    onSubmit: async (values) => {
      try {
        if (id) {
          await VantagemService.updateVantagem(values);
        } else {
          await VantagemService.createVantagem(values);
        }
      } catch (error) {
        console.error(error);
      } finally {
        formik.resetForm();
        fotoRef.current!.value = '';

        if (checkboxRef.current) checkboxRef.current.checked = false;
        onClose && onClose();
        refetchVantagens();
      }
    },
  });

  const excluirVantagem = async () => {
    try {
      await VantagemService.deleteVantagem(id!);
      formik.resetForm();
      fotoRef.current!.value = '';
      if (checkboxRef.current) checkboxRef.current.checked = false;
      onClose && onClose();
      refetchVantagens();
    } catch (error) {
      console.log(error);
    }
  };

  const handleImageUpload = (event: ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files ? event.target.files[0] : '';
    const reader = new FileReader();

    reader.onloadend = () => {
      const base64String = reader.result;
      formik.setFieldValue('fotoName', event.target.files?.[0]?.name ?? '');
      formik.setFieldValue('foto', base64String);
    };

    if (file) {
      reader.readAsDataURL(file);
    }
  };

  const getVantagemSelecionada = async () => {
    try {
      if (id) {
        const vantagem = await VantagemService.getVantagemById(id);

        formik.setValues(vantagem);

        return vantagem;
      } else {
        return {
          nome: '',
          descricao: '',
          valor: 0,
          foto: '',
          fotoName: '',
          idEmresa: '',
        };
      }
    } catch (error) {
      return {
        nome: '',
        descricao: '',
        valor: 0,
        foto: '',
        fotoName: '',
        idEmresa: '',
      };
    }
  };

  const { isLoading, isFetching } = useQuery({
    queryKey: ['vantagem'],
    queryFn: getVantagemSelecionada,
    enabled: !!id,
  });

  return (
    <>
      <input ref={checkboxRef} type='checkbox' id={modalId} className='modal-toggle' />
      <div className='modal'>
        <div className='modal-box max-h-[700px]'>
          <div className='flex justify-between items-center mb-6'>
            <h2 className='text-xl'>{id ? 'Editar' : 'Criar'} Vantagem</h2>
            <label
              htmlFor={modalId}
              className='modal-close btn btn-ghost'
              onClick={() => {
                formik.resetForm();
                onClose && onClose();
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
                  <input type='number' placeholder='Valor' id='valor' name='valor' className={`input input-bordered w-full ${formik.errors.valor && formik.touched.valor && 'input-error'}`} value={formik.values.valor} onChange={formik.handleChange} />
                  {formik.errors.valor && formik.touched.valor && (
                    <label className='label'>
                      <span className='label-text-alt text-error'>{formik.errors.valor}</span>
                    </label>
                  )}
                </div>
                <div className='form-control w-full'>
                  <textarea placeholder='Descrição' id='descricao' name='descricao' className={`textarea textarea-bordered w-full ${formik.errors.descricao && formik.touched.descricao && 'textarea-error'}`} value={formik.values.descricao} onChange={formik.handleChange} />
                  {formik.errors.descricao && formik.touched.descricao && (
                    <label className='label'>
                      <span className='label-text-alt text-error'>{formik.errors.descricao}</span>
                    </label>
                  )}
                </div>
                {id && <p>{formik.values.fotoName}</p>}
                <div className='form-control w-full'>
                  <input ref={fotoRef} type='file' id='foto' name='foto' className={`file-input w-full max-w-xs ${formik.errors.foto && formik.touched.foto && 'input-error'}`} onChange={handleImageUpload} />
                  {formik.errors.foto && formik.touched.foto && (
                    <label className='label'>
                      <span className='label-text-alt text-error'>{formik.errors.foto}</span>
                    </label>
                  )}
                </div>
              </div>
              <div className='modal-action'>
                {id && (
                  <button type='button' className='btn btn-error' onClick={() => excluirVantagem()}>
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
