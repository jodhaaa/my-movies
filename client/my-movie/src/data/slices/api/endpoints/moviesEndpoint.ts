import { apiSlice } from '../apiSlice';
import type { PagedResult, MergedMovieDto } from '../../../models/moviesModels';

export const moviesEndpoint = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getMovies: builder.query<PagedResult<MergedMovieDto>,
            { offset: number; pageSize: number, sortBy: string }>({
                query: ({offset, pageSize, sortBy}) => ({
                    url: 'movies',
                    method: 'GET',
                    params: { offset, pageSize, sortBy }
                }),
                providesTags: ["Movies"]
            })
    }),
});

export const {
    useGetMoviesQuery,
    useLazyGetMoviesQuery
} = moviesEndpoint;