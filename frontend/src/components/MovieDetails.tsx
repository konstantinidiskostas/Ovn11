import { useParams, Link } from "react-router-dom";
import { useEffect, useState } from "react";
import type { MovieDetails as MovieDetailsType } from "../types/movie";
import { getMovieDetails } from "../services/movieService";

export const MovieDetails = () => {
    const { id } = useParams<{id: string}>();

    const [movie, setMovie] = useState<MovieDetailsType | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!id) return;

        const fetchData = async () => {
            try {
                setLoading(true);
                const data = await getMovieDetails(Number(id));
                setMovie(data);
                setError(null);
            } catch (err: any) {
                setError(err.message || 'Fail to load the movie');
            }
                finally {
                    setLoading(false);
                }
        };

        fetchData();
        }, [id]);
        if (loading) return <p>Loading data</p>;
        if (error) return <p>Error: {error}</p>;
        if (!movie) return <p>Movie </p>;
    

    return (
        <div>
      <Link to="/">← back to home page</Link>

      <h2>{movie.title} ({movie.year})</h2>
      <p><strong>Genre:</strong> {movie.genre}</p>
      <p><strong>Duration:</strong> {movie.duration} λεπτά</p>
      <p><strong>Language:</strong> {movie.language}</p>
      <p><strong>Synopsis:</strong> {movie.synopsis}</p>
    </div>
    );
};