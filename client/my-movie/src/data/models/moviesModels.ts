export interface PagedResult<T> {
    items: T[];
    totalCount: number;
}

export interface MergedMovieDto extends MinimalMovieDto {
    prices: MergedPriceDto[];
}

export interface MergedPriceDto {
    price?: number | null;
    provider: string;
}

export interface MinimalMovieDto extends MovieBaseDto {
    writer?: string | null;
    actors?: string | null;
    plot?: string | null;
    language?: string | null;
    country?: string | null;
    awards?: string | null;
    metascore?: string | null;
    rating?: string | null;
    votes?: string | null;
    rated?: string | null;
    released?: string | null;
    runtime?: string | null;
    genre?: string | null;
    director?: string | null;
}

export interface MovieBaseDto {
    id: string;
    title: string;
    year: string;
    poster: string;
    type: string;
}