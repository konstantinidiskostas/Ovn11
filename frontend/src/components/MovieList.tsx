import { useState, useEffect } from "react";
import type { Movie } from "../types/movie";
import { getMovies } from "../services/movieService";




export const MovieList = () => {

//######################################################
// State variables
//######################################################
    const [movies, setMovies] = useState<Movie[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

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
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

