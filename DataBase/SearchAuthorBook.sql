USE MyBooksDB;

SET @AuthorName = 'Ожегов Алексей Викторович';
SET @PublisherName = 'Газета "Ясно"';

SELECT b.Title, b.PrintRun, a.Name AS Author, p.Name AS Publisher
FROM Book b
JOIN Author a ON b.AuthorId = a.AuthorId
JOIN Publisher p ON b.PublisherId = p.PublisherId
WHERE (a.Name COLLATE utf8mb4_unicode_ci = @AuthorName COLLATE utf8mb4_unicode_ci OR @AuthorName IS NULL)
  AND (p.Name COLLATE utf8mb4_unicode_ci = @PublisherName COLLATE utf8mb4_unicode_ci OR @PublisherName IS NULL);