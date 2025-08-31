import axios from 'axios';
import { BookmarkDto, CreateBookmarkDto, TagDto } from '../types';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'https://localhost:7000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

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