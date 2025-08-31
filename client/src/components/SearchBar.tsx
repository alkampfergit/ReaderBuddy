import React, { useState } from 'react';

interface SearchBarProps {
  onSearch: (searchTerm: string) => void;
  onClearSearch: () => void;
}

const SearchBar: React.FC<SearchBarProps> = ({ onSearch, onClearSearch }) => {
  const [searchTerm, setSearchTerm] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (searchTerm.trim()) {
      onSearch(searchTerm.trim());
    }
  };

  const handleClear = () => {
    setSearchTerm('');
    onClearSearch();
  };

  return (
    <div className="search-bar">
      <form onSubmit={handleSubmit}>
        <div className="search-input-container">
          <input
            type="text"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search bookmarks by title, description, or URL..."
            className="search-input"
          />
          <button type="submit" disabled={!searchTerm.trim()}>
            Search
          </button>
          {searchTerm && (
            <button type="button" onClick={handleClear} className="clear-button">
              Clear
            </button>
          )}
        </div>
      </form>
    </div>
  );
};

export default SearchBar;