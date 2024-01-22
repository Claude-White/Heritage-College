import express, { Express } from 'express';
import path from 'path';
import fs from 'fs';
import { dirname } from 'path';
import { fileURLToPath } from 'url';
const app: Express = express();
const port: number = 8888;

type Movie = {
  Key: number;
  Title: string;
  Genre: string[];
  Actors: string[];
  Year: number;
  Runtime: number;
  Revenue: number;
};

const __dirname: string = dirname(fileURLToPath(import.meta.url));

const movies: string = path.join(__dirname, '../data/movies.json');

app.use(express.static(__dirname + '/../client/public'));
app.use(express.json());

app
  .route('/movies')
  .get((_req, res): void => {
    fs.readFile(movies, (err, data) => {
      if (err) console.log(err);
      else {
        const values = JSON.parse(data.toString());
        values.sort((a: Movie, b: Movie) => a.Title.localeCompare(b.Title));
        const selectedFields = values.map((movie: Movie) => ({
          Key: movie.Key,
          Title: movie.Title,
          Year: movie.Year,
        }));
        res.json(selectedFields);
      }
    });
  })
  .post((req, res) => {
    const { Title, Genre, Actors, Year, Runtime, Revenue } = req.body;
    fs.readFile(movies, (err, data) => {
      if (err) console.log(err);
      else {
        const values = JSON.parse(data.toString());
        const updatedKey =
          Math.max(...values.map((movie: Movie) => movie.Key), 0) + 1;
        const newMovie = {
          Key: updatedKey,
          Title,
          Genre: Genre.filter((genre: string) => genre.length > 0),
          Actors: Actors.filter((actor: string) => actor.length > 0),
          Year,
          Runtime,
          Revenue,
        };
        values.push(newMovie);
        fs.writeFile(movies, JSON.stringify(values, null, 2), (err) => {
          if (err) res.status(500).send('Failed to write new movie to file.');
          else res.status(200).send('Successfully wrote new movie to file.');
        });
      }
    });
  });

app.get('/movies/:id', (req, res) => {
  fs.readFile(movies, (err, data) => {
    if (err) console.log(err);
    else {
      const values = JSON.parse(data.toString());
      const movie = values.find((movie: Movie) => movie.Key == +req.params.id);
      res.json(movie);
    }
  });
});

app.get('/actors/:name', (req, res) => {
  fs.readFile(movies, (err, data) => {
    if (err) console.log(err);
    else {
      const values = JSON.parse(data.toString());
      const selectedMovies = values.filter((movie: Movie) => {
        return movie.Actors.some((actor: string) =>
          actor
            .toLowerCase()
            .trim()
            .includes(req.params.name.toLowerCase().trim())
        );
      });
      let selectedFields = [];
      if (selectedMovies) {
        selectedFields = selectedMovies.map((movie: Movie) => ({
          Key: movie.Key,
          Title: movie.Title,
          Year: movie.Year,
        }));
      }
      res.json(selectedFields);
    }
  });
});

app.get('/years/:year', (req, res) => {
  fs.readFile(movies, (err, data) => {
    if (err) console.log(err);
    else {
      const values = JSON.parse(data.toString());
      const selectedMovies = values.filter(
        (movie: Movie) => +movie.Year === +req.params.year
      );
      let selectedFields = [];
      if (selectedMovies) {
        selectedFields = selectedMovies.map((movie: Movie) => ({
          Key: movie.Key,
          Title: movie.Title,
        }));
      }
      res.json(selectedFields);
    }
  });
});

app.listen(port, () => {
  console.log(`Server listening at http://localhost:${port}`);
});
