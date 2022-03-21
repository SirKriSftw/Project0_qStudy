using qS_DL;
using qS_Models;
using System;
using System.IO;
using System.Collections.Generic;

namespace qS_UI
{
    class MainMenu
    {
        // Keeps track of the current user's ID
        int currUserID { get; set; } = -1; // Defaulted as -1 as to say "Not logged in"
        // Used to connect to DB
        string connectionString { get; set; } = File.ReadAllText("./connectionString.txt");
        // Keeps track of the currently loaded test's ID
        int loadedTestID { get; set; }
        // The currently loaded test's question (used so you dont have to query the DB everytime)
        List<Question> loadedTest = new List<Question>();

        public MainMenu()
        {
            // Start up the menu
            start();
        }

        /*
            The method that is fired as soon as a Main Menu object is instantiated.
            It will prompt user to login and terminate the program if the user fails to.
            If login successful, proceed to logged in menu.
        */
        public void start()
        {
            // Ask user for login info
            System.Console.WriteLine("Login Required");
            System.Console.WriteLine("Please enter your username");
            string user = Console.ReadLine();
            System.Console.WriteLine("Please enter your password");
            string pass = Console.ReadLine();
            currUserID = login(user, pass);

            // If login successful
            if(currUserID !=-1)
            {
                // Give logged in menu (where you can loaded and manipulate tests)
                loggedInMenu();
            }
            else
            {
                // Login unsuccessful, terminate program
                System.Console.WriteLine("Login failed. Bye bye");
            }
            
        }

        /*
            The method is fired after a sucessful login.
            At first there are 3 options:
                Load a test
                Create a test
                Exit
            After loaded a test there is 6 options:
                Load a test
                Create a test
                Edit loaded test
                Delete loaded test
                Take loaded test
                Exit
            Upon exiting the program terminates
        */
        public void loggedInMenu()
        {
            // Used to keep program looping
            bool isStudying = true;
            // Used to know if to display 3 or 6 options (if test is loaded or not)
            bool testLoaded = false;
            // The core loop of the program, used to determine what the user wishes to do
            while(isStudying)
            {
                Console.Clear();
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
                        // First print out all user's test then ask which test to take
                        int testID = viewTests(currUserID);
                        // If an invalid testID is chosen, return -1 and break
                        if(testID == -1) break; 
                        // If a valid testID is chosen, load it                       
                        loadedTest = loadTest(testID);
                        testLoaded = true;
                    break;
                    case "2":
                        // Call method to create a new test
                        makeTest();
                    break;
                    case "3":
                        if(testLoaded)
                        {
                            // Call method to edit loaded test
                            editTest(loadedTest);
                        }
                        else
                        {
                            isStudying = false;
                        }                        
                    break;
                    case "4":
                        // Delete loaded test if user confirms it
                        if (deleteTest(currUserID))
                        {   
                            // Test no longer loaded since it is deleted
                            testLoaded = false;
                        }                        
                    break; 
                    case "5":
                        // Take loaded test and return % result
                        takeTest(loadedTest);
                        // User hits enter to proceed and see menu
                        Console.ReadLine();
                    break;
                    default:
                        // Any other case, exit loop and terminate
                        isStudying = false;
                    break;
                }
            }
        }

        /*
            A method used to log the user in given a username and password.
            Uses the UserRepo object to verify the user exists and that it is their password
            Returns -1 if login fails
        */
        public int login(string username, string password)
        {
            UserRepo userRepo = new UserRepo(connectionString);
            return userRepo.login(username, password);
        }
        #region Displaying test
        // Displays test's question given a testID (Not currently used)
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
        /* 
            Displays test's questions given a list of questions (Used so we have to query the DB less often)
            Return list of all question IDs (used so when displaying to user the questions are in sequential order and not random IDs like '1 5 7 9 12')
        */
        public List<int> displayTest(List<Question> test)
        {
            int questionNo = 1;
            // Use list to keep track of all the question IDs
            List<int> questionIDs = new List<int>();
            foreach(Question currQuestion in test)
            {
                System.Console.Write(questionNo + ". ");
                currQuestion.displayQuestion();
                questionNo++;
                // Adds current question's ID to the list
                questionIDs.Add(currQuestion.questionID);
            }
            return questionIDs;
        }
        #endregion
        #region Load Test
        /*
            Displays all user's tests
            Returns the test ID of test selected
            If invalid test chosen, return -1
        */
        public int viewTests(int userID)
        {
            // TestRepo used to communicate with DB
            TestRepo testRepo = new TestRepo(connectionString);
            // List of all user's tests
            List<Test> tests = testRepo.getUsersTest(userID);
            // Keeps track of all the testIDs (used so when displaying to user the tests are in sequential order and not random IDs like '1 5 7 9 12')
            List<int> testIDs = new List<int>();
            int testNo = 1;
            // Print test numbers and names
            foreach(Test test in tests)
            {
                testIDs.Add(test.testID);
                System.Console.WriteLine(testNo + ". " + test.name);
            }
            // Make sure input is not empty
            string input = Console.ReadLine();
            if(!string.IsNullOrEmpty(input))
            {
                int index = Convert.ToInt32(input);
                // Make sure input was a given option
                if(index <= testIDs.Count)
                {
                    // Return select testID
                    return testIDs[index - 1];
                }
                else return -1;
            }
            else return -1;
            
        }

        /*
            Method used to load a test given its test ID
            Returns the list of questions for the given test
        */
        public List<Question> loadTest(int testID)
        {           
            TestRepo testRepo = new TestRepo(connectionString);
            List<Question> test = testRepo.getTestQuestions(testID);
            loadedTestID = testID;
            return test;
        }
        #endregion
        #region Create Test
        /*
            Method used to create a new test
        */
        public void makeTest()
        {
            System.Console.WriteLine("Enter test's name");
            string name = Console.ReadLine();
            TestRepo testRepo = new TestRepo(connectionString);
            // Save the test to the DB and make questions for test
            int testID = testRepo.saveTest(currUserID, name);
            makeQuestions(testID);
            
        }
        private void makeQuestions(int testID)
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
