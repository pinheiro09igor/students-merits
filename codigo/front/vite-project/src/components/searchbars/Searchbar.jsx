import styles from './Searchbar.module.css'

import { FiSearch } from 'react-icons/fi'

const Searchbar = ({ onChange, placeholder }) => {

    const handlChange = ({ target }) => {
        onChange(target.value)
    }
    
    return (
        <div className={ styles.searchbar + ' flex align-center' }>
            <input type='search' placeholder={ placeholder } onChange={ handlChange }/>
            <FiSearch size='1.5rem' color='#A1A1A1'/>
        </div>
    )
}

export default Searchbar