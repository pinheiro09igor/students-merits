import { HomeService } from '../services';
import { ISaldo } from '../services/interfaces';
import { useQuery } from '@tanstack/react-query';
import { TransacaoForm } from '../components';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

const modalId: string = 'transacaoForm';

export const Extrato = () => {
  const id = localStorage.getItem('id');
  const tipo = localStorage.getItem('tipo');

  const getListaEmpresas = async (): Promise<ISaldo> => {
    try {
      return await HomeService.getSaldo();
    } catch (error) {
      return { moedas: 0, transacoes: [] };
    }
  };

  const { data, refetch } = useQuery<ISaldo>({
    queryKey: ['saldo'],
    queryFn: getListaEmpresas,
    refetchOnWindowFocus: false,
  });

  const onClickGenerateReport = (): void => {
    const doc = new jsPDF();

    const lineY = 20;
    const date = new Date().toLocaleDateString();

    doc.setLineWidth(0.5);
    doc.setFontSize(14);
    doc.text(`Saldo: ${data?.moedas}`, 15, lineY);
    doc.setFontSize(10);
    doc.text(`Gerado em: ${date}`, doc.internal.pageSize.getWidth() - 15 - doc.getStringUnitWidth(date) * doc.getFontSize(), lineY);

    autoTable(doc, {
      startY: 28,
      headStyles: { fillColor: '#661ae6' },
      head: [['Tipo', 'Remetente', 'Destinatário', 'Valor', 'Descrição']],
      body: data?.transacoes.map((transacao) => [transacao.remetenteId !== id ? 'Recebimento' : 'Envio', id == transacao.remetenteId ? '-' : transacao.remetenteNome, id == transacao.destinatarioId ? '-' : transacao.destinatarioNome, transacao.valor, transacao.descricao]),
    });

    doc.save('extrato.pdf');
  };

  return (
    <>
      <div className='flex flex-col grow h-full'>
        <div className={`flex mx-12 ${tipo === 'professor' && 'mt-6'} justify-between`}>
          <div className='flex text-2xl  items-center gap-1'>
            <b>Saldo:</b> {data?.moedas}{' '}
            <svg xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 24 24' strokeWidth={1.5} stroke='currentColor' className='w-6 h-6'>
              <path strokeLinecap='round' strokeLinejoin='round' d='M12 6v12m-3-2.818l.879.659c1.171.879 3.07.879 4.242 0 1.172-.879 1.172-2.303 0-3.182C13.536 12.219 12.768 12 12 12c-.725 0-1.45-.22-2.003-.659-1.106-.879-1.106-2.303 0-3.182s2.9-.879 4.006 0l.415.33M21 12a9 9 0 11-18 0 9 9 0 0118 0z' />
            </svg>
          </div>
          <div className='flex gap-2'>
            <button className='btn btn-primary shadow' onClick={() => onClickGenerateReport()}>
              Gerar Relatório
            </button>
            {tipo === 'professor' && (
              <label htmlFor={modalId} className='btn btn-primary shadow'>
                Nova Transação
              </label>
            )}
          </div>
        </div>
        <div className='flex flex-col mx-12 my-6 h-full bg-base-200 rounded-md shadow-lg'>
          <table className='table w-full'>
            <thead>
              <tr>
                <th>Tipo</th>
                <th>Remetente</th>
                <th>Destinatário</th>
                <th>Valor</th>
                <th>Descrição</th>
              </tr>
            </thead>
            <tbody className='overflow-x-auto'>
              {data &&
                data.transacoes.length > 0 &&
                data.transacoes.map(
                  (transacao) => (
                    <tr key={transacao._id}>
                      <td>{transacao.remetenteId !== id ? 'Recebimento' : 'Envio'}</td>
                      <td>{id == transacao.remetenteId ? '-' : transacao.remetenteNome}</td>
                      <td>{id == transacao.destinatarioId ? '-' : transacao.destinatarioNome}</td>
                      <td>{transacao.valor}</td>
                      <td className='text-ellipsis'>{transacao.descricao}</td>
                    </tr>
                  ),
                  []
                )}
            </tbody>
          </table>
          {(data?.transacoes.length === 0 || data === undefined) && (
            <div className='flex justify-center items-center p-2'>
              <p className='text-gray-500'>Nenhuma transação encontrada</p>
            </div>
          )}
        </div>
      </div>
      <TransacaoForm modalId={modalId} refetchTransacoes={refetch} />
    </>
  );
};
