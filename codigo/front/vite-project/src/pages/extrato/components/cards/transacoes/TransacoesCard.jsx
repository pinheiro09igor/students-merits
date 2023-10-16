import styles from "./TransacoesCard.module.css"

import TransacaoCard from "../transacao/TransacaoCard"
import { useEffect, useState } from "react"

import moment from 'moment'
import 'moment/dist/locale/pt-br'

const TransacoesCard = ({ transactions, onClick }) => {
  const [formattedTransactions, setFormattedTransactions] = useState([])

  useEffect(() => {
    setFormattedTransactions(formatTransactionsData(transactions))
    moment.locale('pt-br')
  }, [transactions])

  const formatTransactionsData = data => {
    const allDates = data.map(transaction => { return moment(transaction.data).format('ll') })
      .filter((dataDate, index, self) => self.indexOf(dataDate) === index)
      
      return allDates.map(date => {
        const transactionsFiltered = data.filter(transaction => moment(transaction.data).format('ll') == date)

        if(date === moment().format('ll'))
          date = `Hoje | ${ date }`

        return {
          date,
          data: transactionsFiltered
        }
      })
  }

  const calcTotalBalanceDay = transactions => {
    const totalBalanceDay = transactions.reduce((accomulator, transaction) => {
      return transaction.tipo === 'transferencia' ?
        accomulator - transaction.valor
      :
        accomulator + transaction.valor
    }, 0)

    return totalBalanceDay
  }

  const handleOnClick = target => {
    onClick(target)
  }

  return (
    <div className={ styles.cardTransacoes }>
      <h1 className={ styles.transacoesTitle }>TRANSAÇÕES<span>.</span></h1>

      <ul className={ styles.transactionBody }>
        { formattedTransactions.map(formatterTransaction => (
          <li key={ Math.random() * 1000 } className={ styles.transactionCard }>
            <div className={ styles.transactionHeader }>
              <h5 className={ styles.transactionDate }>
                { formatterTransaction.date } ({ formatterTransaction.data.length })
              </h5>

              <p className={ styles.totalBalance }>
                { calcTotalBalanceDay(formatterTransaction.data) }
              </p>
            </div>

            <ul className={ styles.transactionData }>
              { formatterTransaction.data.map(transaction => (
                <li key={ Math.random() * 100 } onClick={ () => handleOnClick(transaction) }>
                  <TransacaoCard transaction={ transaction } />
                </li>
              ))}
            </ul>
          </li>
        ))}
      </ul>
    </div>
  )
}

export default TransacoesCard
