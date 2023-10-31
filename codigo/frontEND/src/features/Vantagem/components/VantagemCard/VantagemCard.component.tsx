interface IProps {
  id: string;
  title: string;
  description: string;
  image: string;
  value: number;
  isAluno: boolean;
  onClick: () => void;
  onClickResgatar: (id: string) => void;
}

export const VantagemCard = ({ id, title, description, image, value, isAluno, onClick, onClickResgatar }: IProps) => {
  return (
    <div className={`card w-96 bg-base-200 shadow-xl ${!isAluno && 'hover:filter hover:brightness-110 active:scale-95 transition duration-300'}`} onClick={onClick}>
      <img
        className='rounded-t-lg'
        style={{
          height: '200px',
          width: '100%',
          overflow: 'hidden',
          objectFit: 'cover',
        }}
        src={image === '' ? '/src/assets/no_image.jpg' : image}
        alt={title}
      />
      <div className='card-body'>
        <div className='flex justify-between'>
          <h2 className='card-title'>{title}</h2>
          <div className='flex text-2xl  items-center gap-1'>
            {value}{' '}
            <svg xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 24 24' strokeWidth={1.5} stroke='currentColor' className='w-6 h-6'>
              <path strokeLinecap='round' strokeLinejoin='round' d='M12 6v12m-3-2.818l.879.659c1.171.879 3.07.879 4.242 0 1.172-.879 1.172-2.303 0-3.182C13.536 12.219 12.768 12 12 12c-.725 0-1.45-.22-2.003-.659-1.106-.879-1.106-2.303 0-3.182s2.9-.879 4.006 0l.415.33M21 12a9 9 0 11-18 0 9 9 0 0118 0z' />
            </svg>
          </div>
        </div>
        <p>{description}</p>
        {isAluno && (
          <div className='card-actions justify-end' onClick={() => onClickResgatar(id)}>
            <button className='btn btn-primary'>Resgatar</button>
          </div>
        )}
      </div>
    </div>
  );
};
