import { useState, useEffect } from "react";
import type { Movie, MovieCreateDto } from "../types/movie";
import { getMovies, createMovie, updateMovie } from "../services/movieService";




export const MovieList = () => {

//######################################################
// State variables
//######################################################
    const [movies, setMovies] = useState<Movie[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [formData, setFormData] = useState<MovieCreateDto>({
        title: '',
        year: 0,
        genre: '',
        duration: 0,
    });
    const [editingMovieId, setEditingMovieId] = useState<number | null>(null);

//######################################################
// useEffect to fetch movies
//######################################################
useEffect(() => {
  async function fetchMovies() {
    try {
      const data = await getMovies();
      setMovies(data);
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  fetchMovies();
}, []);

  if (loading) return <p>Movie loading...</p>;
  if (error) return <p>Error: {error}</p>;

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'number' ? Number(value) : value,
    });
  };



  const handleEdit = (movie: Movie) => {
    setEditingMovieId(movie.id);
    setFormData({
      title: movie.title,
      year: movie.year,
      genre: movie.genre,
      duration: movie.duration,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingMovieId !== null) {
        const updatedMovie = await updateMovie(editingMovieId, formData);
        setMovies((prevMovies) =>
          prevMovies.map((movie) =>
            movie.id === editingMovieId ? updatedMovie : movie
          )
        );
        setEditingMovieId(null);
      } else {
        const newMovie = await createMovie(formData);
        setMovies((prevMovies) => [...prevMovies, newMovie]);
      }
      setFormData({ title: '', year: 0, genre: '', duration: 0 });
    } catch (err: any) {
      alert(`Error saving movie: ${err.message}`);
    }
  };

  return (
    <div>
      <h2>Movie List</h2>
      {movies.length === 0 ? (
        <p>No movies found.</p>
      ) : (
        <ul>
          {movies.map((movie) => (
            <li key={movie.id}>
              <strong>{movie.title}</strong> ({movie.year}) - {movie.genre} [{movie.duration} min]
              <button onClick={() => handleEdit(movie)} style={{ marginLeft: '10px' }}>Edit</button>
            </li>
          ))}
        </ul>
      )}
      <form onSubmit={handleSubmit} style={{ marginBottom: '20px' }}>
  <h3>Add a new movie</h3>
  
  <div>
    <label>Title: </label>
    <input
      type="text"
      name="title"
      value={formData.title}
      onChange={handleChange}
      required
    />
  </div>

  <div>
    <label>Year: </label>
    <input
      type="number"
      name="year"
      value={formData.year}
      onChange={handleChange}
      required
    />
  </div>

  <div>
    <label>Genre: </label>
    <input
      type="text"
      name="genre"
      value={formData.genre}
      onChange={handleChange}
      required
    />
  </div>

  <div>
    <label>Duration (minutes): </label>
    <input
      type="number"
      name="duration"
      value={formData.duration}
      onChange={handleChange}
      required
    />
  </div>

  <button type="submit" style={{ marginTop: '10px' }}>Save</button>
</form>
    </div>
    
  );
};