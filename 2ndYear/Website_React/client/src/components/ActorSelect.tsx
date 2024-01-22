import { useState } from 'react';

function ActorSelect({ setMovieList }: { setMovieList: Array<Function> }) {
  const [actorName, setActorName] = useState('');
  const [found, setFound] = useState(true);
  const [setActorsMovies, setYearMovies] = setMovieList;
  async function getActorsMovies() {
    setYearMovies([]);
    const response = await fetch(`/actors/${actorName}`);
    const movies = await response.json();
    setActorsMovies(movies);
    movies.length == 0 ? setFound(false) : setFound(true);
  }
  return (
    <div className='inline-grid grid-cols-2 grid-rows-2 gap-2'>
      <input
        className='col-span-2 px-2 py-2 rounded-lg ring-1 ring-gray-300'
        placeholder='Actor Name'
        type='text'
        value={actorName}
        onChange={(e) => setActorName(e.target.value)}
      />
      <button
        className='w-full px-4 py-2 text-white rounded-md bg-sky-500 hover:scale-100 active:scale-95'
        onClick={getActorsMovies}>
        Search
      </button>
      <button
        className='w-full px-4 py-2 text-white bg-gray-500 rounded-md hover:scale-100 active:scale-95'
        onClick={() => {
          setActorsMovies([]);
          setActorName('');
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

export default ActorSelect;
