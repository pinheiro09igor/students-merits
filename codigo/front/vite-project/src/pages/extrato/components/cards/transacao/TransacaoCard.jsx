import styles from './TransacaoCard.module.css'

import { HiArrowDownLeft, HiArrowUpRight } from 'react-icons/hi2'

const TransacaoCard = ({ transaction }) => {
  return (
    <div className={ styles.cardExtrato }>
      <div className={ styles.transactionType }>
        { transaction.tipo == 'transferencia' ?
          <HiArrowUpRight size='1.5rem' color='#FF3E3E'/>
          :
          <HiArrowDownLeft size='1.5rem' color='#1EDA00'/>
        }
      </div>

      <div className={ styles.cardInfos }>
        <div>
          <h3 className={ styles.cardTitle }><span>{ transaction.destino.nome }</span> | { transaction.destino.email }</h3>
          <p className={ styles.cardType }>{ transaction.tipo == 'transferencia' ? 'Transferência' : 'Recebido' }</p>
        </div>
        
        <p className={ styles.cardValue }>{ transaction.tipo == 'transferencia' ? `- ${ transaction.valor }` : `+ ${ transaction.valor }` }</p>
      </div>
    </div>
  )
}

export default TransacaoCard