function Genre({ genres }: { genres: string[] }) {
  return (
    <div>
      <p>Genres:</p>
      <ul>
        {genres.map((genre, index) => (
          <li key={index} className='pl-2'>
            {genre}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default Genre;
