USE CDC;
CREATE TABLE books (
   id INT AUTO_INCREMENT PRIMARY KEY,
   title VARCHAR(255) NOT NULL UNIQUE,
   summary VARCHAR(500),
   abstract TEXT NOT NULL,
   price DECIMAL(10, 2) NOT NULL CHECK (price >= 20),
   number_of_pages INT NOT NULL CHECK (number_of_pages >= 100),
   isbn VARCHAR(255) NOT NULL UNIQUE,
   publish_date DATETIME NOT NULL,
   author_id INT NOT NULL,
   category_id INT NOT NULL,
   created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

   FOREIGN KEY (author_id) REFERENCES authors(id),
   FOREIGN KEY (category_id) REFERENCES categories(id)
);