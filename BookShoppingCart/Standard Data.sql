INSERT INTO Genres (GenreName)
SELECT GenreName
FROM (VALUES
    ('Adventure'),
    ('Science fiction'),
    ('Romance'),
    ('Horror')
) G(GenreName)
WHERE NOT EXISTS (
    SELECT 1 FROM Genres WHERE GenreName = G.GenreName
);

INSERT INTO Books (BookName, AuthorName, Image, GenreId, Price)
SELECT 
    B.BookName,
    B.AuthorName,
    G.Id,
    ABS(CHECKSUM(NEWID())) % 701 + 200 AS Price
FROM (
    VALUES
    -- Adventure
    ('The Call of the Wild', 'Jack London', 'CorruptFile.jpg', 'Adventure'),
    ('Odyssey', 'Homer', 'CorruptFile.jpg', 'Adventure'),
    ('Into the Wild', 'Jon Krakauer', 'CorruptFile.jpg', 'Adventure'),
    ('The Hobbit', 'J. R. R. Tolkien', 'CorruptFile.jpg', 'Adventure'),
    ('Treasure Island', 'Robert Louis Stevenson', 'CorruptFile.jpg', 'Adventure'),
    ('Around the World in Eighty Days', 'Jules Verne', 'CorruptFile.jpg', 'Adventure'),
    ('Into Thin Air: A Personal Account of the Mt. Everest Disaster', 'Jon Krakauer', 'CorruptFile.jpg', 'Adventure'),

    -- Science Fiction
    ('Frankenstein', 'Mary Shelley', 'Science', 'CorruptFile.jpg', 'fiction'),
    ('Foundation', 'Isaac Asimov', 'Science', 'CorruptFile.jpg', 'fiction'),
    ('The Stars My Destination', 'Alfred Bester', 'Science', 'CorruptFile.jpg', 'fiction'),
    ('Solaris', 'Stanislaw Lem', 'Science', 'CorruptFile.jpg', 'fiction'),
    ('The Moon is a Harsh Mistress', 'Robert Heinlein', 'Science', 'CorruptFile.jpg', 'fiction'),
    ('Ice', 'Anna Kavan', 'Science', 'CorruptFile.jpg', 'fiction'),
    ('Kindred', 'Octavia E. Butler', 'Science', 'CorruptFile.jpg', 'fiction'),

    -- Romance
    ('Outlander', 'Diana Gabaldon', 'CorruptFile.jpg', 'Romance'),
    ('Red, White & Royal Blue', 'Casey McQuiston', 'CorruptFile.jpg', 'Romance'),
    ('Seven Days in June', 'Tia Williams', 'CorruptFile.jpg', 'Romance'),
    ('The Notebook', 'Nicholas Sparks', 'CorruptFile.jpg', 'Romance'),
    ('The Hating Game: A Novel', 'Sally Thorne', 'CorruptFile.jpg', 'Romance'),
    ('People We Meet on Vacation', 'Emily Henry', 'CorruptFile.jpg', 'Romance'),

    -- Horror
    ('Dracula', 'Bram Stoker', 'CorruptFile.jpg', 'Horror'),
    ('The Haunting of Hill House', 'Shirley Jackson', 'CorruptFile.jpg', 'Horror'),
    ('The Shining', 'Stephen King', 'CorruptFile.jpg', 'Horror'),
    ('It', 'Stephen King', 'CorruptFile.jpg', 'Horror'),
    ('The Exorcist', 'William Peter Blatty', 'CorruptFile.jpg', 'Horror'),
    ('House of Leaves', 'Mark Z. Danielewski', 'CorruptFile.jpg', 'Horror')
) B(BookName, AuthorName, GenreName)
JOIN Genres G ON G.GenreName = B.GenreName
WHERE NOT EXISTS (
    SELECT 1
    FROM Books BK
    WHERE BK.BookName = B.BookName
      AND BK.AuthorName = B.AuthorName
      AND BK.GenreId = G.Id
);