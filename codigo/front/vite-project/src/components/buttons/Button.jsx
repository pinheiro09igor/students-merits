import styles from './Button.module.css'

const Button = ({ type, className, id, onClick, children }) => {
    return (
        <button type={ type }
            className={ className && styles[className] }
            id={ id }
            onClick={ onClick && onClick }
            >
                { children }
            </button>
    )
}

export default Button