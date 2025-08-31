import React from 'react';
import { BookmarkDto } from '../types';

interface BookmarkListProps {
  bookmarks: BookmarkDto[];
  onDeleteBookmark: (id: number) => void;
}

const BookmarkList: React.FC<BookmarkListProps> = ({ bookmarks, onDeleteBookmark }) => {
  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString();
  };

  if (bookmarks.length === 0) {
    return (
      <div className="bookmark-list empty">
        <p>No bookmarks found. Create your first bookmark above!</p>
      </div>
    );
  }

  return (
    <div className="bookmark-list">
      <h2>Your Bookmarks ({bookmarks.length})</h2>
      
      <div className="bookmarks-grid">
        {bookmarks.map((bookmark) => (
          <div key={bookmark.id} className="bookmark-card">
            <div className="bookmark-header">
              <h3>
                <a 
                  href={bookmark.url} 
                  target="_blank" 
                  rel="noopener noreferrer"
                  className="bookmark-title"
                >
                  {bookmark.title}
                </a>
              </h3>
              <button
                onClick={() => onDeleteBookmark(bookmark.id)}
                className="delete-button"
                title="Delete bookmark"
              >
                🗑️
              </button>
            </div>
            
            <div className="bookmark-url">
              <small>{bookmark.url}</small>
            </div>
            
            {bookmark.description && (
              <div className="bookmark-description">
                <p>{bookmark.description}</p>
              </div>
            )}
            
            {bookmark.tags.length > 0 && (
              <div className="bookmark-tags">
                {bookmark.tags.map((tag) => (
                  <span 
                    key={tag.id} 
                    className="tag" 
                    style={{ backgroundColor: tag.color, color: 'white' }}
                  >
                    {tag.name}
                  </span>
                ))}
              </div>
            )}
            
            <div className="bookmark-meta">
              <small>Created: {formatDate(bookmark.createdAt)}</small>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default BookmarkList;