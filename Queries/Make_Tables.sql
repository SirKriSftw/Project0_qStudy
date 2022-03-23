CREATE TABLE Users(
	userID int PRIMARY KEY Identity,
	username varchar(50) UNIQUE,
	password varchar(50)
)
SELECT * FROM Users

CREATE TABLE Tests(
	testID int PRIMARY KEY Identity,
	userID int FOREIGN KEY REFERENCES Users(userID) on DELETE cascade,
	name varchar(50) NOT NULL
)
SELECT * FROM Tests

CREATE TABLE Types(
	typeID int PRIMARY KEY Identity,
	type varchar(50) NOT NULL
)
SELECT * FROM Types

CREATE TABLE Questions(
	questionID int PRIMARY KEY Identity,
	testID int FOREIGN KEY REFERENCES Tests(testID) on DELETE cascade,
	typeID int FOREIGN KEY REFERENCES Types(typeID),
	question varchar(200) NOT NULL,
	answer varchar(200) NOT NULL
)
SELECT * FROM Questions

CREATE TABLE Choices(
	questionID int FOREIGN KEY REFERENCES Questions(questionID) on DELETE cascade,
	choiceLetter varchar(10) NOT NULL,
	choice varchar(100) NOT NULL
)
SELECT * FROM Choices

DROP TABLE Choices, Questions, Types, Tests, Users