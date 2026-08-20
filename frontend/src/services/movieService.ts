import type { Movie, MovieCreateDto, MovieDetails, Review, ReviewCreateDto, Actor, TopGenreReport, AverageRatingReport, ActiveActorReport } from "../types/movie";

const API_URL = 'http://localhost:5106'

//######################################################
// Get all movies from the API
//######################################################
export const getMovies = async (genre?: string, search?: string) => {
  const params = new URLSearchParams();
  if (genre) params.append('genre', genre);
  if (search) params.append('search', search);
  const queryString = params.toString();
  const url = queryString
    ? `${API_URL}/api/v1/Movie?${queryString}`
    : `${API_URL}/api/v1/Movie`;

  const response = await fetch(url);
  if (!response.ok) throw new Error('Αποτυχία φόρτωσης ταινιών');
  return response.json();
};

//######################################################
// Post a new movie to the API
//######################################################
export const createMovie = async(movieData: MovieCreateDto): Promise<Movie> => {
    return fetch(`${API_URL}/api/v1/Movie`, {
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
    const response = await fetch(`${API_URL}/api/v1/Movie/${id}`, {
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

//######################################################
// Delete a movie from the API
//######################################################
export const deleteMovie = async(id: number): Promise<void> => {
    const response = await fetch(`${API_URL}/api/v1/Movie/${id}`, {
        method: 'DELETE',
    });
    if (!response.ok) {
        throw new Error('Error deleting movie');
    }
};

//######################################################
// Detail view GET
//######################################################
export const getMovieDetails = async (id: number): Promise<MovieDetails> => {
    const response = await fetch(`${API_URL}/api/v1/Movie/${id}/details`);

    if (!response.ok) {
        throw new Error('Error fetching movie details');
    }
    return response.json();
};

//######################################################
// Review POST
//######################################################
export const createReview = async(
    movieId: number,
    reviewData: ReviewCreateDto
): Promise<Review> => {
    const response = await fetch(`${API_URL}/api/movies/${movieId}/reviews`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json',},
    body: JSON.stringify(reviewData),
});
    if (!response.ok) {
        throw new Error('Error creating review');
    }

    return response.json();
};

//######################################################
// Get all actors
//######################################################
export const getAllActors = async (): Promise<Actor[]> => {
    const response = await fetch(`${API_URL}/api/v1/Actors`);
    if (!response.ok) {
        throw new Error('Error fetching actors');
    }
    return response.json();
};

//######################################################
// Add actor to movie
//######################################################
export const addActorToMovie = async (movieId: number, actorId: number, role: string): Promise<void> => {
    const response = await fetch(`${API_URL}/api/v1/movies/${movieId}/actors/${actorId}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ role }),
    });
    if (!response.ok) {
        throw new Error('Error adding actor to movie');
    }
};

//######################################################
// Update actor role in movie
//######################################################
export const updateActorRole = async (movieId: number, actorId: number, role: string): Promise<void> => {
    const response = await fetch(`${API_URL}/api/v1/movies/${movieId}/actors/${actorId}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ role }),
    });
    if (!response.ok) {
        throw new Error('Error updating actor role');
    }
};

//######################################################
// Reports
//######################################################
export const getTopMoviesPerGenre = async (): Promise<TopGenreReport[]> => {
    const response = await fetch(`${API_URL}/api/v1/Reports/top-movies-per-genre`);
    if (!response.ok) throw new Error('Error fetching genre report');
    return response.json();
};

export const getAverageRating = async (): Promise<AverageRatingReport[]> => {
    const response = await fetch(`${API_URL}/api/v1/Reports/average-rating`);
    if (!response.ok) throw new Error('Error fetching average rating report');
    return response.json();
};

export const getActiveActors = async (): Promise<ActiveActorReport[]> => {
    const response = await fetch(`${API_URL}/api/v1/Reports/active-actors`);
    if (!response.ok) throw new Error('Error fetching active actors report');
    return response.json();
};