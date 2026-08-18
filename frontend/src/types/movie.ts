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