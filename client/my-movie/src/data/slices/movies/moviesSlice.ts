import { createSlice } from "@reduxjs/toolkit";
import type { PagedResult, MergedMovieDto  } from "../../models/moviesModels";
import { moviesEndpoint } from "../api/endpoints/moviesEndpoint";

const initialState = {} as PagedResult<MergedMovieDto>;

const moviesSlice = createSlice({
    name: "Movies",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        // Add extra reducers here
        builder.addMatcher(
            moviesEndpoint.endpoints.getMovies.matchFulfilled,
            (state, { payload }) => {
                state.items = payload.items;
                state.totalCount = payload.totalCount
            }
        );
    },
});

export default moviesSlice.reducer;