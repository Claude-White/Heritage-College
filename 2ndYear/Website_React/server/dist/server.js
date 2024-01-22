import express from 'express';
import path from 'path';
import fs from 'fs';
import { dirname } from 'path';
import { fileURLToPath } from 'url';
const app = express();
const port = 8888;
const __dirname = dirname(fileURLToPath(import.meta.url));
const movies = path.join(__dirname, '../data/movies.json');
app.use(express.static(__dirname + '/../client/public'));
app.use(express.json());
app
  .route('/movies')
  .get((_req, res) => {
    fs.readFile(movies, (err, data) => {
      if (err) console.log(err);
      else {
        const values = JSON.parse(data.toString());
        values.sort((a, b) => a.Title.localeCompare(b.Title));
        const selectedFields = values.map((movie) => ({
          Key: movie.Key,
          Title: movie.Title,k
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
        const updatedKey = Math.max(...values.map((movie) => movie.Key), 0) + 1;
        const newMovie = {
          Key: updatedKey,
          Title,
          Genre: Genre.filter((genre) => genre.length > 0),
          Actors: Actors.filter((actor) => actor.length > 0),
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
      const movie = values.find((movie) => movie.Key == +req.params.id);
      res.json(movie);
    }
  });
});
app.get('/actors/:name', (req, res) => {
  fs.readFile(movies, (err, data) => {
    if (err) console.log(err);
    else {
      const values = JSON.parse(data.toString());
      const selectedMovies = values.filter((movie) => {
        return movie.Actors.some((actor) =>
          actor
            .toLowerCase()
            .trim()
            .includes(req.params.name.toLowerCase().trim())
        );
      });
      let selectedFields = [];
      if (selectedMovies) {
        selectedFields = selectedMovies.map((movie) => ({
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
        (movie) => +movie.Year === +req.params.year
      );
      let selectedFields = [];
      if (selectedMovies) {
        selectedFields = selectedMovies.map((movie) => ({
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
//# sourceMappingURL=server.js.map
