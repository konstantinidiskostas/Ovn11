import type { Movie, MovieCreateDto } from "../types/movie";

const API_URL = 'http://localhost:5106/api/v1/Movie'

//######################################################
// Get all movies from the API
//######################################################
export const getMovies = async(): Promise<Movie[]> => {
    const response = await fetch(API_URL);
    if (!response.ok) {
        throw new Error('Error fetching movies');
    }
    return response.json();
};

//######################################################
// Post a new movie to the API
//######################################################
export const createMovie = async(movieData: MovieCreateDto): Promise<Movie> => {
    return fetch(API_URL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(movieData),
    }).then(response => {
        if (!response.ok) {
            throw new Error('Error creating movie');
        }
    return response.json();
});
};

//######################################################
// Put (update) an existing movie in the API
//######################################################
export const updateMovie = async(id: number, movieData: MovieCreateDto): Promise<Movie> => {
    const response = await fetch(`${API_URL}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ id, ...movieData }),
    });
    if (!response.ok) {
        throw new Error('Error updating movie');
    }
    return { id, ...movieData };
};