import { useState, useEffect } from "react";
import type { Movie, MovieCreateDto } from "../types/movie";
import { getMovies, createMovie, updateMovie, deleteMovie } from "../services/movieService";
import { Link } from "react-router-dom";




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
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [selectedGenre, setSelectedGenre] = useState<string>('');
    //######################################################
    // useEffect to fetch movies
    //######################################################
    useEffect(() => {
        async function fetchMovies() {
            try {
                const data = await getMovies(selectedGenre, searchTerm);
                setMovies(data);
            } catch (err: any) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        }

        fetchMovies();
    }, [selectedGenre, searchTerm]);

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

    const handleDelete = async (id: number) => {
        try {
            await deleteMovie(id);
            setMovies((prevMovies) => prevMovies.filter((movie) => movie.id !== id));
        } catch (err: any) {
            alert(`Error deleting movie: ${err.message}`);
        }
    };

    return (
        <div>
            <Link to="/dashboard">Dashboard</Link>
            <hr />
            <div style={{ display: 'flex', gap: '15px', marginBottom: '20px' }}>
                
                <input
                    type="text"
                    placeholder="Search title"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                />

                
                <select
                    value={selectedGenre}
                    onChange={(e) => setSelectedGenre(e.target.value)}
                >
                    <option value="">All</option>
                    <option value="Action">Action</option>
                    <option value="Comedy">Comedy</option>
                    <option value="Drama">Drama</option>
                    <option value="Sci-Fi">Sci-Fi</option>
                    <option value="Horror">Horror</option>
                </select>
            </div>
            <h2>Movie List</h2>
            {movies.length === 0 ? (
                <p>No movies found.</p>
            ) : (
                <ul>
                    {movies.map((movie) => (
                        <li key={movie.id}>
                            <strong>{movie.title}</strong> ({movie.year}) - {movie.genre} [{movie.duration} min]
                            <button onClick={() => handleEdit(movie)} style={{ marginLeft: '10px' }}>Edit</button>
                            <button onClick={() => handleDelete(movie.id)} style={{ marginLeft: '5px' }}>Delete</button>
                            <Link to={`/movies/${movie.id}`} style={{ marginLeft: '10px' }}>
                                <button type="button">More info</button>
                            </Link>

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