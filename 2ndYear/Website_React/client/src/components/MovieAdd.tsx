import { Toaster, toast } from 'sonner';
import { useForm } from 'react-hook-form';
import type { FieldValues } from 'react-hook-form';

function MovieAdd({ setListOfMovies }: { setListOfMovies: Function }) {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm();

  async function addMovie(data: FieldValues) {
    const { Title, Genre, Actors, Year, Runtime, Revenue } = data;
    const newMovie = {
      Title,
      Genre: Genre.split(',').map((item: string) => item.trim()),
      Actors: Actors.split(',').map((item: string) => item.trim()),
      Year: +Year,
      Runtime: +Runtime,
      Revenue: +Revenue,
    };

    const response = await fetch('/movies', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(newMovie),
    });
    if (response.ok) {
      toast.success('Movie Successfully Added');
      reset();
      const response = await fetch('/movies');
      setListOfMovies(await response.json());
    } else {
      toast.error('Error Adding Movie');
    }
  }

  return (
    <div className='grid place-items-center'>
      <Toaster richColors position='top-right' />
      <form
        onSubmit={handleSubmit(addMovie)}
        className='grid grid-cols-1 gap-4 p-4 mt-4 rounded-md sm:grid-cols-2 w-max place-items-center ring-1 ring-gray-400'>
        <input
          {...register('Title', {
            required: 'Title is required',
          })}
          type='text'
          className='inputMovie'
          placeholder='Title'
          name='Title'
          id='Title'
        />
        <input
          {...register('Year', {
            required: 'Year is required',
            min: {
              value: 0,
              message: 'Year must be greater than 0',
            },
            max: {
              value: new Date().getFullYear(),
              message: `Year must be less than ${new Date().getFullYear()}`,
            },
          })}
          type='number'
          className='inputMovie'
          placeholder='Year'
          name='Year'
          id='Year'
        />
        {errors.Title && (
          <span className='col-start-1 text-red-500'>
            {errors.Title.message?.toString()}
          </span>
        )}
        {errors.Year && (
          <span className='col-start-1 text-red-500 sm:col-start-2'>
            {errors.Year.message?.toString()}
          </span>
        )}
        <input
          {...register('Runtime', {
            required: 'Runtime is required',
            min: {
              value: 0,
              message: 'Runtime must be greater than 0',
            },
          })}
          type='number'
          className='inputMovie'
          placeholder='Runtime'
          name='Runtime'
          id='Runtime'
        />
        <input
          {...register('Revenue', {
            required: 'Revenue is required',
            min: {
              value: 0,
              message: 'Revenue must be greater than 0',
            },
          })}
          type='number'
          className='inputMovie'
          placeholder='Revenue'
          name='Revenue'
          id='Revenue'
        />
        {errors.Runtime && (
          <span className='col-start-1 text-red-500'>
            {errors.Runtime.message?.toString()}
          </span>
        )}
        {errors.Revenue && (
          <span className='col-start-1 text-red-500 sm:col-start-2'>
            {errors.Revenue.message?.toString()}
          </span>
        )}
        <input
          {...register('Actors', {
            required: 'Actors are required',
          })}
          type='text'
          className='inputMovieLong'
          placeholder='Actors'
          name='Actors'
          id='Actors'
        />
        {errors.Actors && (
          <span className='col-span-1 text-red-500 sm:col-span-2'>
            {errors.Actors.message?.toString()}
          </span>
        )}
        <input
          {...register('Genre', {
            required: 'Genres are required',
          })}
          type='text'
          className='inputMovieLong'
          placeholder='Genres'
          name='Genre'
          id='Genre'
        />
        {errors.Genre && (
          <span className='col-span-1 text-red-500 sm:col-span-2'>
            {errors.Genre.message?.toString()}
          </span>
        )}
        <button
          type='submit'
          disabled={isSubmitting}
          className='w-full grid-flow-row col-span-1 px-4 py-2 text-white rounded-md sm:col-span-2 h-max bg-sky-500 hover:scale-100 active:scale-95'>
          Add Movie
        </button>
      </form>
    </div>
  );
}

export default MovieAdd;
