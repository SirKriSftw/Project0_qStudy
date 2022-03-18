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
        public void displayTest(int testID)
        {
            string connectionString = File.ReadAllText("./connectionString.txt");
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

        //public bool login(string username, string password)
        //public void logout()

        //public void takeTest()
        //public void makeTest()
        //public void viewTest()
        //public void editTest()
        //public void deleteTest()
    }
}
