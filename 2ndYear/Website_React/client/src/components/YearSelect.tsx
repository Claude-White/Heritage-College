import { useState } from 'react';

function YearSelect({ setMovieList }: { setMovieList: Array<Function> }) {
  const [year, setYear] = useState<number | null>(null);
  const [found, setFound] = useState(true);
  const [setYearMovies, setActorsMovies] = setMovieList;
  async function getYearMovies() {
    setActorsMovies([]);
    const response = await fetch(`/years/${year}`);
    const movies = await response.json();
    setYearMovies(movies);
    movies.length == 0 ? setFound(false) : setFound(true);
  }
  return (
    <div className='inline-grid grid-cols-2 grid-rows-2 gap-2'>
      <input
        className='col-span-2 px-2 py-2 rounded-lg ring-1 ring-gray-300'
        placeholder='Release Year'
        type='number'
        value={year || ''}
        onChange={(e) => setYear(+e.target.value)}
      />
      <button
        className='w-full px-4 py-2 text-white rounded-md bg-sky-500 hover:scale-100 active:scale-95'
        onClick={getYearMovies}>
        Search
      </button>
      <button
        className='w-full px-4 py-2 text-white bg-gray-500 rounded-md hover:scale-100 active:scale-95'
        onClick={() => {
          setYearMovies([]);
          setYear(null);
          setFound(true);
        }}>
        Clear
      </button>
      {!found && (
        <span className='w-full col-span-2 p-3 text-center bg-gray-300 rounded-lg shadow-md'>
          No movies found
        </span>
      )}
    </div>
  );
}

export default YearSelect;
