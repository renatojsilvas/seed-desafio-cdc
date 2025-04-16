USE CDC;
ALTER TABLE authors
ADD CONSTRAINT uq_author_email UNIQUE (email);