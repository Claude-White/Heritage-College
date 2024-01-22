import { useEffect, useState } from 'react';
import Actor from './Actor';
import Genre from './Genre';

type Movie = {
  Key: number;
  Title: string;
  Year: number;
};

type SpecificMovie = {
  Title: string;
  Year: number;
  Runtime: number;
  Revenue: number;
  Genre: string[];
  Actors: string[];
};

function Movie({ movie }: { movie: Movie }) {
  const [oneMovie, setOneMovie] = useState<SpecificMovie | null>(null);

  useEffect(() => {
    const loadMovie = async () => {
      const response = await fetch(`/movies/${movie.Key}`);
      setOneMovie(await response.json());
    };
    loadMovie();
  }, [movie]);
  return (
    <>
      {oneMovie && (
        <div className='fixed z-10 flex flex-row translate-x-1/2 -translate-y-1/2 bg-gray-300 rounded-lg shadow-md top-1/2 right-1/2 w-max'>
          <button className='z-20' onClick={() => setOneMovie(null)}>
            <img
              className='absolute top-0 right-0 w-8 h-8 bg-red-600 rounded-lg'
              src='./src/assets/close.svg'
              alt='Close'
            />
          </button>
          <div className='relative flex flex-row gap-4 p-4 pr-12'>
            <div>
              <h3 className='text-2xl'>{oneMovie?.Title}</h3>
              <p>Year: {oneMovie?.Year}</p>
              <p>Runtime: {oneMovie?.Runtime} minutes</p>
              <p>Revenue: ${oneMovie?.Revenue}M</p>
            </div>
            {oneMovie?.Actors && <Actor actors={oneMovie.Actors} />}
            {oneMovie?.Genre && <Genre genres={oneMovie.Genre} />}
          </div>
        </div>
      )}
    </>
  );
}

export default Movie;
