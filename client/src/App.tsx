import React, { useState, useEffect } from 'react';
import './App.css';
import BookmarkForm from './components/BookmarkForm';
import BookmarkList from './components/BookmarkList';
import SearchBar from './components/SearchBar';
import { BookmarkDto } from './types';
import { bookmarkService } from './services/api';

function App() {
  const [bookmarks, setBookmarks] = useState<BookmarkDto[]>([]);
  const [filteredBookmarks, setFilteredBookmarks] = useState<BookmarkDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isSearching, setIsSearching] = useState(false);

  // Load bookmarks on component mount
  useEffect(() => {
    loadBookmarks();
  }, []);

  const loadBookmarks = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const data = await bookmarkService.getAllBookmarks();
      setBookmarks(data);
      setFilteredBookmarks(data);
    } catch (err) {
      setError('Failed to load bookmarks. Please check if the API server is running.');
      console.error('Error loading bookmarks:', err);
    } finally {
      setIsLoading(false);
    }
  };

  const handleBookmarkCreated = () => {
    loadBookmarks(); // Refresh the list after creating a new bookmark
  };

  const handleDeleteBookmark = async (id: number) => {
    if (window.confirm('Are you sure you want to delete this bookmark?')) {
      try {
        await bookmarkService.deleteBookmark(id);
        await loadBookmarks(); // Refresh the list after deletion
      } catch (err) {
        setError('Failed to delete bookmark. Please try again.');
        console.error('Error deleting bookmark:', err);
      }
    }
  };

  const handleSearch = async (searchTerm: string) => {
    try {
      setIsSearching(true);
      setError(null);
      const results = await bookmarkService.searchBookmarks(searchTerm);
      setFilteredBookmarks(results);
    } catch (err) {
      setError('Failed to search bookmarks. Please try again.');
      console.error('Error searching bookmarks:', err);
    } finally {
      setIsSearching(false);
    }
  };

  const handleClearSearch = () => {
    setFilteredBookmarks(bookmarks);
    setIsSearching(false);
  };

  return (
    <div className="App">
      <header className="App-header">
        <h1>📚 ReaderBuddy - Bookmark Manager</h1>
        <p>Save and organize your reading bookmarks with tags</p>
      </header>

      <main className="App-main">
        {error && (
          <div className="error-banner">
            <p>{error}</p>
            <button onClick={() => setError(null)}>Dismiss</button>
          </div>
        )}

        <section className="bookmark-form-section">
          <BookmarkForm onBookmarkCreated={handleBookmarkCreated} />
        </section>

        <section className="search-section">
          <SearchBar onSearch={handleSearch} onClearSearch={handleClearSearch} />
        </section>

        <section className="bookmark-list-section">
          {isLoading ? (
            <div className="loading">Loading bookmarks...</div>
          ) : isSearching ? (
            <div className="loading">Searching...</div>
          ) : (
            <BookmarkList 
              bookmarks={filteredBookmarks} 
              onDeleteBookmark={handleDeleteBookmark} 
            />
          )}
        </section>
      </main>

      <footer className="App-footer">
        <p>ReaderBuddy - Your Personal Reading Assistant</p>
      </footer>
    </div>
  );
}

export default App;
