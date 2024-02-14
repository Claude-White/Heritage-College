function Actor({ actors }: { actors: string[] }) {
  return (
    <div>
      <p>Actors:</p>
      <ul>
        {actors.map((actor, index) => (
          <li key={index} className='pl-2'>
            {actor}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default Actor;
