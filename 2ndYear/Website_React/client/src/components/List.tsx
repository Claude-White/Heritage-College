import { useEffect, useState } from 'react';

type Movie = {
  Key: number;
  Title: string;
  Year: number;
};

function List({
  setMovie,
  movieLists,
}: {
  setMovie: Function;
  movieLists: Array<Movie[]>;
}) {
  const [listOfMovies, actorsMovies, yearMovies] = movieLists;
  const [movieList, setMovieList] = useState([]);
  useEffect(() => {
    const loadMovies = async () => {
      const response = await fetch('/movies');
      setMovieList(await response.json());
    };
    loadMovies();
  }, [listOfMovies]);

  if (actorsMovies.length > 0) {
    return (
      <>
        {actorsMovies.map((movie: Movie) => (
          <div
            key={movie.Key}
            className='p-4 bg-gray-300 rounded-lg shadow-md hover:bg-gray-400 hover:scale-100 active:scale-[99%] hover:cursor-pointer'
            onClick={() => setMovie(movie)}>
            <h3 className='text-2xl'>{movie.Title}</h3>
            <p>Year: {movie.Year}</p>
          </div>
        ))}
      </>
    );
  }
  if (yearMovies.length > 0) {
    return (
      <>
        {yearMovies.map((movie: Movie) => (
          <div
            key={movie.Key}
            className='p-4 bg-gray-300 rounded-lg shadow-md hover:bg-gray-400 hover:scale-100 active:scale-[99%] hover:cursor-pointer'
            onClick={() => setMovie(movie)}>
            <h3 className='text-2xl'>{movie.Title}</h3>
          </div>
        ))}
      </>
    );
  } else {
    return (
      <>
        {movieList.map((movie: Movie) => (
          <div
            key={movie.Key}
            className='p-4 bg-gray-300 rounded-lg shadow-md hover:bg-gray-400 hover:scale-100 active:scale-[99%] hover:cursor-pointer'
            onClick={() => setMovie(movie)}>
            <h3 className='text-2xl'>{movie.Title}</h3>
            <p>Year: {movie.Year}</p>
          </div>
        ))}
      </>
    );
  }
}

export default List;
