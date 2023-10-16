import styles from './SaldoCard.module.css'

import { GiTwoCoins } from 'react-icons/gi'

const SaldoCard = ({ saldo, user }) => {
  return (
    <div className={ `${ styles.saldoCard } box-shadow` }>
      <h3 className={ styles.nomeUser }>{ user.nome }</h3>

      <div className={ styles.saldoUser }>
        <span>SALDO</span>
        
        <div>
          <span>{ saldo }</span>
          <GiTwoCoins size='2rem' color='#FFC700'/>
        </div>
      </div>
    </div>
  )
}

export default SaldoCard