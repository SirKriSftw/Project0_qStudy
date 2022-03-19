using qS_DL;
using qS_Models;
using System;
using System.IO;
using System.Collections.Generic;

namespace qS_UI
{
    class MainMenu
    {
        int currUserID { get; set; } = 1;
        string connectionString { get; set; } = File.ReadAllText("./connectionString.txt");
        List<Question> loadedTest = new List<Question>();

        public MainMenu()
        {
            start();
        }

        public void start()
        {
            // System.Console.WriteLine("Login Required");
            // System.Console.WriteLine("Please enter your username");
            // string user = Console.ReadLine();
            // System.Console.WriteLine("Please enter your password");
            // string pass = Console.ReadLine();


            System.Console.WriteLine("--------------Welcome to qStudy!--------------");
            bool isStudying = true;
            bool testLoaded = false;
            while(isStudying)
            {
                System.Console.WriteLine("1. Load a test");
                System.Console.WriteLine("2. Create a test");
                System.Console.WriteLine("3. Edit a test");
                System.Console.WriteLine("4. Delete a test");
                if(testLoaded)
                {
                System.Console.WriteLine("5. Take currently loaded test");
                System.Console.WriteLine("6. Exit");
                }
                else
                {
                    System.Console.WriteLine("5. Exit");
                }
                    
                string choice = Console.ReadLine();
                switch(choice)
                {
                    case "1":
                        //print all tests from logged in user
                        //take input for which test to load
                        //load test
                        viewTests(1);
                        int testID = Convert.ToInt32(Console.ReadLine());
                        loadedTest = loadTest(testID);
                        testLoaded = true;
                    break;
                    case "2":
                        makeTest();
                    break;
                    case "3":
                        //print all tests from logged user
                        //take input for which test to edit
                        //edit test
                    break;
                    case "4":
                        //print all tests from logged user
                        //take input for which test to delete
                        //delete test
                    break; 
                    case "5":
                        if(testLoaded)
                        {
                            displayTest(loadedTest);
                            //take loaded test
                        }
                        else
                        {
                            isStudying = false;
                        }
                    break;
                    default:
                        isStudying = false;
                    break;
                }

            }
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

        public void displayTest(List<Question> test)
        {
            int questionNo = 1;
            foreach(Question currQuestion in test)
            {
                System.Console.Write(questionNo + ". ");
                currQuestion.displayQuestion();
                questionNo++;
            }
        }
        #endregion
        //public bool login(string username, string password)
        //public void logout()

        //public void takeTest()
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


        public void viewTests(int userID)
        {
            TestRepo testRepo = new TestRepo(connectionString);
            List<Test> tests = testRepo.getUsersTest(userID);
            foreach(Test test in tests)
            {
                test.displayTestName();
            }
        }

        public List<Question> loadTest(int testID)
        {           
            TestRepo testRepo = new TestRepo(connectionString);
            List<Question> test = testRepo.getTestQuestions(testID);
            return test;
        }
        //public void editTest()
        //public void deleteTest()
    }
}
