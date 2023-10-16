import styles from './DetalhesCard.module.css'

import { FaUniversity } from 'react-icons/fa'
import { HiUser } from 'react-icons/hi2'
import { IoCalendarOutline } from 'react-icons/io5'
import { TbCoins, TbFileDescription } from 'react-icons/tb'

import moment from 'moment'
import 'moment/dist/locale/pt-br'

const DetalhesCard = ({ transaction }) => {
    const formatDate = date => {
        moment.locale('pt-br')
        return moment(date).format('LLL')
    }

    return (
        <div className={ styles.cardDetalhes }>
            <h1 className={ styles.detalhesTitle }>DETALHES<span>.</span></h1>

            <div className={ styles.typeTransaction }>
                <div className={ styles.typeIcon }>
                    { transaction.origem.tipo === 'empresa' ? <FaUniversity size='2rem' /> : <HiUser size='2rem' /> }
                </div>

                <div className={ styles.infoType }>
                    <p className={ styles.type }>{ transaction.tipo === 'transferencia' ? 'Transaferência' : 'Recebiento' }</p>
                    <p className={ styles.value }>{ transaction.tipo === 'transferencia' ? '-' : '+'} { transaction.valor }</p>
                </div>
            </div>


            <div className={ styles.detalhesInfo }>
                <div className={ styles.date }>
                    <p className={ styles.label }><IoCalendarOutline /> Data</p>
                    <p className={ styles.data }>{ formatDate(transaction.data) }</p>
                </div>

                <div className={ styles.from }>
                    <p className={ styles.label }>{ transaction.origem.tipo === 'professor' ? <HiUser />  : <FaUniversity /> } Recebido de</p>
                    <p className={ styles.data }>{ transaction.origem.nome }</p>
                </div>

                <div className={ styles.value }>
                    <p className={ styles.label }><TbCoins /> Valor</p>
                    <p className={ styles.data }>{ transaction.valor } moedas</p>
                </div>
                
                <div className={ styles.description }>
                    <p className={ styles.label }><TbFileDescription /> Descrição</p>
                    <p className={ styles.data }>{ transaction.descricao }</p>
                </div>
            </div>
        </div>
    )
}

export default DetalhesCard