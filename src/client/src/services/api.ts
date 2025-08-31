import axios from 'axios';
import { BookmarkDto, CreateBookmarkDto, TagDto } from '../types';

// Dynamic API URL detection
const getApiBaseUrl = (): string => {
  // If REACT_APP_API_URL is explicitly set and we're in development mode, use it
  if (process.env.REACT_APP_API_URL && process.env.NODE_ENV === 'development') {
    return process.env.REACT_APP_API_URL;
  }
  
  // For production or when served by the C# backend, use relative path
  // This will automatically resolve to the same host/port as the current page
  return '/api';
};

const API_BASE_URL = getApiBaseUrl();

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add request interceptor to log the full URL being called
api.interceptors.request.use(request => {
  console.log('API Request URL:', request.baseURL + request.url);
  return request;
});

// Add response interceptor to enhance error messages
api.interceptors.response.use(
  response => response,
  error => {
    if (error.response) {
      // Server responded with error status
      error.message = `API Error (${error.response.status}): ${error.message} - URL: ${error.config.baseURL}${error.config.url}`;
    } else if (error.request) {
      // Request was made but no response received (likely connection error)
      error.message = `Connection Error: Unable to reach API server at ${error.config.baseURL}${error.config.url}`;
    }
    return Promise.reject(error);
  }
);

export const bookmarkService = {
  // Get all bookmarks
  getAllBookmarks: async (): Promise<BookmarkDto[]> => {
    const response = await api.get<BookmarkDto[]>('/bookmarks');
    return response.data;
  },

  // Get bookmark by ID
  getBookmarkById: async (id: number): Promise<BookmarkDto> => {
    const response = await api.get<BookmarkDto>(`/bookmarks/${id}`);
    return response.data;
  },

  // Create new bookmark
  createBookmark: async (bookmark: CreateBookmarkDto): Promise<BookmarkDto> => {
    const response = await api.post<BookmarkDto>('/bookmarks', bookmark);
    return response.data;
  },

  // Update bookmark
  updateBookmark: async (id: number, bookmark: CreateBookmarkDto): Promise<void> => {
    await api.put(`/bookmarks/${id}`, bookmark);
  },

  // Delete bookmark
  deleteBookmark: async (id: number): Promise<void> => {
    await api.delete(`/bookmarks/${id}`);
  },

  // Search bookmarks
  searchBookmarks: async (searchTerm: string): Promise<BookmarkDto[]> => {
    const response = await api.get<BookmarkDto[]>(`/bookmarks/search?searchTerm=${encodeURIComponent(searchTerm)}`);
    return response.data;
  },

  // Get all tags
  getAllTags: async (): Promise<TagDto[]> => {
    const response = await api.get<TagDto[]>('/bookmarks/tags');
    return response.data;
  },
};

export default api;