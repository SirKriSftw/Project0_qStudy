using qS_DL;
using qS_Models;
using System;
using System.IO;
using System.Collections.Generic;

namespace qS_UI
{
    class MainMenu
    {
        int currUserID { get; set; } = -1;
        string connectionString { get; set; } = File.ReadAllText("./connectionString.txt");
        int loadedTestID { get; set; }
        string loadedTestName { get; set; }
        List<Question> loadedTest = new List<Question>();

        public MainMenu()
        {
            start();
        }

        public void start()
        {
            System.Console.WriteLine("Login Required");
            System.Console.WriteLine("Please enter your username");
            string user = Console.ReadLine();
            System.Console.WriteLine("Please enter your password");
            string pass = Console.ReadLine();
            currUserID = login(user, pass);

            if(currUserID !=-1)
            {
                loggedInMenu();
            }
            else
            {
                System.Console.WriteLine("Login failed. Bye bye");
            }
            
        }

        public void loggedInMenu()
        {
            System.Console.WriteLine("--------------Welcome to qStudy!--------------");
            bool isStudying = true;
            bool testLoaded = false;
            while(isStudying)
            {
                System.Console.WriteLine("1. Load a test");
                System.Console.WriteLine("2. Create a test");
                if(testLoaded)
                {
                System.Console.WriteLine("3. Edit loaded test");
                System.Console.WriteLine("4. Delete loaded test");
                System.Console.WriteLine("5. Take loaded test");
                System.Console.WriteLine("6. Exit");
                }
                else
                {
                    System.Console.WriteLine("3. Exit");
                }
                    
                string choice = Console.ReadLine();
                switch(choice)
                {
                    case "1":
                        int testID = viewTests(currUserID);                        
                        loadedTest = loadTest(testID);
                        testLoaded = true;
                    break;
                    case "2":
                        makeTest();
                    break;
                    case "3":
                        if(testLoaded)
                        {
                            editTest(loadedTest);
                        }
                        else
                        {
                            isStudying = false;
                        }                        
                    break;
                    case "4":
                        if (deleteTest(currUserID))
                        {
                            testLoaded = false;
                        }                        
                    break; 
                    case "5":
                        takeTest(loadedTest);
                    break;
                    default:
                        isStudying = false;
                    break;
                }
            }
        }

        public int login(string username, string password)
        {
            UserRepo userRepo = new UserRepo(connectionString);
            return userRepo.login(username, password);
        }
        #region Displaying test
        public void displayTest(int testID)
        {
            TestRepo testRepo = new TestRepo(connectionString);
            List<Question> testQuestions = testRepo.getTestQuestions(testID);
            int questionNo = 1;
            foreach(Question currQuestion in testQuestions)
            {
                System.Console.Write(questionNo + ". ");
                currQuestion.displayQuestion();
                questionNo++;
            }
        }
        public List<int> displayTest(List<Question> test)
        {
            int questionNo = 1;
            List<int> questionIDs = new List<int>();
            foreach(Question currQuestion in test)
            {
                System.Console.Write(questionNo + ". ");
                currQuestion.displayQuestion();
                questionNo++;
                questionIDs.Add(currQuestion.questionID);
            }
            return questionIDs;
        }
        #endregion
        #region Load Test
        public int viewTests(int userID)
        {
            TestRepo testRepo = new TestRepo(connectionString);
            List<Test> tests = testRepo.getUsersTest(userID);
            List<int> testIDs = new List<int>();
            int testNo = 1;
            foreach(Test test in tests)
            {
                testIDs.Add(test.testID);
                System.Console.WriteLine(testNo + ". " + test.name);
            }
            int index = Convert.ToInt32(Console.ReadLine());
            return testIDs[index - 1];
        }

        public List<Question> loadTest(int testID)
        {           
            TestRepo testRepo = new TestRepo(connectionString);
            List<Question> test = testRepo.getTestQuestions(testID);
            loadedTestID = testID;
            return test;
        }
        #endregion
        #region Create Test
        public void makeTest()
        {
            System.Console.WriteLine("Enter test's name");
            string name = Console.ReadLine();
            TestRepo testRepo = new TestRepo(connectionString);
            int testID = testRepo.saveTest(currUserID, name);
            makeQuestion(testID);
            
        }
        private void makeQuestion(int testID)
        {
            QuestionRepo questionRepo = new QuestionRepo(connectionString);
            ChoiceRepo choiceRepo = new ChoiceRepo(connectionString);
            bool isAddingQuestions = true;
            Question currQuestion = new Question();
            while(isAddingQuestions)
            {
                System.Console.WriteLine("Select type of question");
                System.Console.WriteLine("1. Multiple Choice");
                System.Console.WriteLine("2. Free Response");
                System.Console.WriteLine("Leave blank to exit");
                string type = Console.ReadLine();
                switch(type)
                {
                    case "1":
                        currQuestion = currQuestion.createMCQuestion(testID);
                        int qID = questionRepo.AddQuestion(currQuestion);
                        choiceRepo.AddChoices(qID, currQuestion.choices);
                    break;
                    case "2":
                        currQuestion = currQuestion.createQuestion(testID);
                        questionRepo.AddQuestion(currQuestion);
                    break;
                    default:
                        if(string.IsNullOrEmpty(type))
                        {
                            isAddingQuestions = false;
                        }
                        else
                        {
                            System.Console.WriteLine("Invalid option");
                        }
                    break;

                }
            }
        }
        #endregion
        
        public void editTest(List<Question> test)
        {
            QuestionRepo questionRepo = new QuestionRepo(connectionString);
            ChoiceRepo choiceRepo = new ChoiceRepo(connectionString);
            List<int> questionIDs;
            bool isEditing = true;
            while(isEditing)
            {
                questionIDs = displayTest(test);
                System.Console.WriteLine("Which question would you like to edit?");
                string input = Console.ReadLine();
                int questionID = -1;
                if(string.IsNullOrEmpty(input))
                {
                    isEditing = false;
                }
                else
                {
                    int questionIDIndex = Convert.ToInt32(input);
                    questionID = questionIDs[questionIDIndex-1];
                    int questionIndex = 0;
                    while(test[questionIndex].questionID != questionID && questionIndex < test.Count)
                    {
                        questionIndex++;
                    }
                    System.Console.WriteLine("What would you like to edit?");
                    System.Console.WriteLine("  1. The question");
                    System.Console.WriteLine("  2. The answer");
                    if(test[questionIndex].typeID == 1)
                    {
                        System.Console.WriteLine("  3. The choices");
                        System.Console.WriteLine("  4. Exit");
                    }
                    else
                    {
                        System.Console.WriteLine("  3. Exit"); 
                    }                   
                    string toEdit = Console.ReadLine();
                    switch(toEdit)
                    {
                        case "1":
                            System.Console.WriteLine("What would you like to edit the quesiton to?");
                            string changeQTo = Console.ReadLine();
                            questionRepo.EditQuestion(questionID, changeQTo);
                            test[questionIndex].question = changeQTo;
                        break;
                        case "2":
                            System.Console.WriteLine("What would you like to edit the answer to?");
                            string changeATo = Console.ReadLine();
                            questionRepo.EditAnswer(questionID, changeATo);
                            test[questionIndex].answer = changeATo;
                        break;
                        case "3":
                            if(test[questionIndex].typeID == 1)
                            {
                            System.Console.WriteLine("What choice would you like to edit?");
                            string choiceLetter = Console.ReadLine();
                            System.Console.WriteLine("What would you like to edit the " + choiceLetter + " choice to?");
                            string changeCTo = Console.ReadLine();
                            choiceRepo.EditChoice(questionID, choiceLetter, changeCTo);
                                foreach(Choice choice in test[questionIndex].choices)
                                {
                                    if(choice.choiceLetter.ToString() == choiceLetter)
                                    {
                                        choice.choice = changeCTo;
                                    }
                                }
                            }
                            else
                            {
                                isEditing = false;
                            }
                        break;
                        default:
                            isEditing = false;
                        break;
                    }
                }
            }
        }
        public bool deleteTest(int userID)
        {
            TestRepo testRepo = new TestRepo(connectionString);
            System.Console.WriteLine("Are you sure you want to delete?");
            string userChoice = Console.ReadLine();
            userChoice = userChoice.ToUpper();
            if(userChoice.Equals("Y") || userChoice.Equals("YES"))
            {
                testRepo.deleteTest(userID, loadedTestID);
                return true;
            }
            return false;
            
        }   
        public void takeTest(List<Question> questions)
        {
            Test test = new Test();
            double score = test.takeTest(questions);
            System.Console.WriteLine("Percentage correct: {0}%", Math.Round(score, 2));
        }
    }
}
