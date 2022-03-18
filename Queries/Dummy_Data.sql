INSERT INTO Users VALUES ('user1','pass1')
INSERT INTO Users VALUES ('user2','pass2')
INSERT INTO Users VALUES ('user3','pass3')
INSERT INTO Users VALUES ('user4','pass4')
SELECT * FROM Users

INSERT INTO Tests VALUES (1,'firstTest')
INSERT INTO Tests VALUES (2,'secondTest')
INSERT INTO Tests VALUES (3,'thirdTest')
INSERT INTO Tests VALUES (4,'fourthTest')
SELECT * FROM Tests

INSERT INTO Types VALUES ('Multiple Choice') , ('Free Response') , ('True/False')
SELECT * FROM Types


INSERT INTO Questions VALUES (1, 1, 'What is 2+2?', 'a'), (1, 1, 'What is 10 * 10?', 'b'), (1, 1, 'What is 100 / 10?', 'c')
INSERT INTO Choices VALUES (1,'a','4'), (1,'b','3'), (1,'c','5'),(1,'d','Fish')
INSERT INTO Choices VALUES (2,'a','1000'), (2,'b','100'), (2,'c','10'),(2,'d','Dolphin')
INSERT INTO Choices VALUES (3,'a','20'), (3,'b','1'), (3,'c','10'),(3,'d','Tiger')
INSERT INTO Questions VALUES (2, 2, 'What year did WW1 start?', '1914'), (2, 1, 'Who gave the gettysburg address?', 'b'), (2, 2, 'How many years did the One Hundred Years War last?', '116')
INSERT INTO Choices VALUES (5,'a','JFK'), (5,'b','Abraham Lincoln'), (5,'c','FDR')
SELECT * FROM Questions
SELECT * FROM Choices
SELECT * FROM Questions JOIN Choices ON Questions.questionID = Choices.questionID WHERE Questions.testID = 1
SELECT * FROM Questions JOIN Choices ON Questions.questionID = Choices.questionID WHERE Questions.testID = 2