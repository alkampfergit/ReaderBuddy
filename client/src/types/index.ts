export interface CreateBookmarkDto {
  title: string;
  url: string;
  description: string;
  tags: string[];
}

export interface BookmarkDto {
  id: number;
  title: string;
  url: string;
  description: string;
  createdAt: string;
  updatedAt: string;
  tags: TagDto[];
}

export interface TagDto {
  id: number;
  name: string;
  color: string;
}