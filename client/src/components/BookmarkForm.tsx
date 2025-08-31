import React, { useState } from 'react';
import { CreateBookmarkDto } from '../types';
import { bookmarkService } from '../services/api';

interface BookmarkFormProps {
  onBookmarkCreated: () => void;
}

const BookmarkForm: React.FC<BookmarkFormProps> = ({ onBookmarkCreated }) => {
  const [formData, setFormData] = useState<CreateBookmarkDto>({
    title: '',
    url: '',
    description: '',
    tags: []
  });
  const [tagInput, setTagInput] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleAddTag = () => {
    if (tagInput.trim() && !formData.tags.includes(tagInput.trim())) {
      setFormData(prev => ({
        ...prev,
        tags: [...prev.tags, tagInput.trim()]
      }));
      setTagInput('');
    }
  };

  const handleRemoveTag = (tagToRemove: string) => {
    setFormData(prev => ({
      ...prev,
      tags: prev.tags.filter(tag => tag !== tagToRemove)
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    setError(null);

    try {
      await bookmarkService.createBookmark(formData);
      // Reset form
      setFormData({
        title: '',
        url: '',
        description: '',
        tags: []
      });
      setTagInput('');
      onBookmarkCreated();
    } catch (err) {
      setError('Failed to create bookmark. Please try again.');
      console.error('Error creating bookmark:', err);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="bookmark-form">
      <h2>Add New Bookmark</h2>
      {error && <div className="error-message">{error}</div>}
      
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="title">Title *</label>
          <input
            type="text"
            id="title"
            name="title"
            value={formData.title}
            onChange={handleInputChange}
            required
            placeholder="Enter bookmark title"
          />
        </div>

        <div className="form-group">
          <label htmlFor="url">URL *</label>
          <input
            type="url"
            id="url"
            name="url"
            value={formData.url}
            onChange={handleInputChange}
            required
            placeholder="https://example.com"
          />
        </div>

        <div className="form-group">
          <label htmlFor="description">Description</label>
          <textarea
            id="description"
            name="description"
            value={formData.description}
            onChange={handleInputChange}
            placeholder="Optional description"
            rows={3}
          />
        </div>

        <div className="form-group">
          <label htmlFor="tags">Tags</label>
          <div className="tag-input-container">
            <input
              type="text"
              id="tags"
              value={tagInput}
              onChange={(e) => setTagInput(e.target.value)}
              placeholder="Enter a tag and press Add"
              onKeyPress={(e) => e.key === 'Enter' && (e.preventDefault(), handleAddTag())}
            />
            <button type="button" onClick={handleAddTag} disabled={!tagInput.trim()}>
              Add Tag
            </button>
          </div>
          
          {formData.tags.length > 0 && (
            <div className="tags-container">
              {formData.tags.map((tag, index) => (
                <span key={index} className="tag">
                  {tag}
                  <button
                    type="button"
                    className="tag-remove"
                    onClick={() => handleRemoveTag(tag)}
                  >
                    ×
                  </button>
                </span>
              ))}
            </div>
          )}
        </div>

        <button type="submit" disabled={isSubmitting || !formData.title || !formData.url}>
          {isSubmitting ? 'Creating...' : 'Create Bookmark'}
        </button>
      </form>
    </div>
  );
};

export default BookmarkForm;