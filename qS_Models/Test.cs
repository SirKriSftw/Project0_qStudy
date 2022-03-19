using System;
using System.Collections.Generic;

namespace qS_Models
{
    public class Test
    {
        public int testID { get; set; }
        public string name { get; set; }
        public List<Question> testQuestions { get; set; }

        public Test(){}
        public Test(int testID, string name)
        {
            this.testID = testID;
            this.name = name;
        }

        public void displayTestName()
        {
            System.Console.WriteLine(testID + ". " + name);
        }
        //public double takeTest(int testID)
    }
}
