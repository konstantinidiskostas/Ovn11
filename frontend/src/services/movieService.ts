import type { Movie } from "../types/movie";

const API_URL = 'http://localhost:5106/api/v1/Movie'

export const getMovies = async(): Promise<Movie[]> => {
    const response = await fetch(API_URL);
    if (!response.ok) {
        throw new Error('Error fetching movies');
    }
    return response.json();
};