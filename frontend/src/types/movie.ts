export interface Movie  {
    id: number;
    title: string;
    year: number;
    genre: string;
    duration: number;

}

export interface MovieCreateDto {
    title: string;
    year: number;
    genre: string;
    duration: number;
}

export interface Actor {
    id: number;
    name: string;
    birthYear: number;
    role: string;
}

export interface Review {
    id:  number;
    reviewerName: string;
    comment: string;
    rating: number;
}

export interface ReviewCreateDto {
    reviewerName: string;
    rating: number;
    comment: string;
}

export interface MovieDetails {
    id: number;
    title: string;
    year: number;
    genre: string;
    duration: number;
    synopsis: string;
    language: string;
    actors: Actor[];
    reviews: Review[];
}

export interface TopGenreReport {
    genre: string;
    movieCount: number;
}

export interface AverageRatingReport {
    title: string;
    averageRating: number;
    reviewCount: number;
}

export interface ActiveActorReport {
    name: string;
    movieCount: number;
}