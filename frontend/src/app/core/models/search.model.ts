export interface SearchResultItemDto {
  title: string;
  subtitle: string;
  category: string;
  linkUrl: string;
}

export interface GlobalSearchResponseDto {
  query: string;
  results: SearchResultItemDto[];
}
