import { useEffect, useState } from 'react';
import List from './List';
import ActorSelect from './ActorSelect';
import Movie from './Movie';
import YearSelect from './YearSelect';
import MovieAdd from './MovieAdd';

type MovieType = {
  Key: number;
  Title: string;
  Year: number;
};

function App() {
  const [listOfMovies, setListOfMovies] = useState<MovieType[]>([]);
  const [movie, setMovie] = useState<MovieType | null>(null);
  const [actorsMovies, setActorsMovies] = useState([]);
  const [yearMovies, setYearMovies] = useState([]);
  const [search, setSearch] = useState('');

  useEffect(() => {
    if (search === 'null') {
      setYearMovies([]);
      setActorsMovies([]);
    }
  }, [search]);

  return (
    <>
      <header className='py-4 text-center shadow-md'>
        <h1 className='text-6xl font-bold text-transparent bg-[linear-gradient(120deg,#bd34fe,30%,#41d1ff)] bg-clip-text'>
          Movie Library
        </h1>
      </header>
      <main className='mx-4'>
        <div className='grid gap-2 mt-4 place-items-center'>
          <select
            onChange={(e) => setSearch(e.target.value)}
            className='px-4 py-2.5 bg-gray-300 rounded-md ring-2 ring-gray-400 w-36 broder focus:ring-blue-500 focus:border-blue-500'
            name='search'
            id='search'>
            <option value='null'>Search By</option>
            <option value='Actor'>Actor</option>
            <option value='Year'>Year</option>
          </select>
          {search === 'Actor' && (
            <ActorSelect setMovieList={[setActorsMovies, setYearMovies]} />
          )}
          {search === 'Year' && (
            <YearSelect setMovieList={[setYearMovies, setActorsMovies]} />
          )}
        </div>
        <MovieAdd setListOfMovies={setListOfMovies} />
        <div className=''>{movie && <Movie movie={movie} />}</div>
        <div className='grid grid-cols-1 gap-4 mt-4 md:grid-cols-2 lg:grid-cols-3'>
          <List
            setMovie={setMovie}
            movieLists={[listOfMovies, actorsMovies, yearMovies]}
          />
        </div>
      </main>
    </>
  );
}

export default App;
