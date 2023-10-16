import { Link } from 'react-router-dom'
import Button from '../buttons/Button'

const LinkedButton = ({ type, className, id, onClick, to, children }) => {
    return (
        <Link to={ to }>
            <Button type={ type }
                className={ className && className }
                id={ id }
                onClick={ onClick && onClick }
                >
                    { children }
                </Button>
        </Link>
    )
}

export default LinkedButton