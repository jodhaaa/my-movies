import { configureStore, combineReducers } from '@reduxjs/toolkit';
import moviesSlice from './slices/movies/moviesSlice';
import { apiSlice } from './slices/api/apiSlice';

const rootReducer = combineReducers({
    movies: moviesSlice,
    [apiSlice.reducerPath]: apiSlice.reducer,
});
export const store = configureStore({
    reducer: rootReducer,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware().concat(apiSlice.middleware),
});

export type RootState = ReturnType<typeof store.getState>;