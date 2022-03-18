using System;
using System.Collections.Generic;

namespace qS_Models
{
    public class Question
    {
        public int questionID { get; set; }
        public int testID { get; set; }        
        public int typeID { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public List<String> choices { get; set; }

        public Question(){}
        public Question(int questionID, int testID, int typeID, string question, string answer)
        {
            this.questionID = questionID;
            this.testID = testID;
            this.typeID = typeID;
            this.question = question;
            this.answer = answer;
            choices = new List<String>();
        }

        public virtual void displayQuestion()
        {
            System.Console.WriteLine(question);
            if(typeID == 1)
            {
                char choiceLetter = 'a';
                foreach(string choice in choices)
                {
                    System.Console.WriteLine("   " + choiceLetter + ". " + choice);
                    choiceLetter++;
                }   
            }
            System.Console.WriteLine();
        }

        //public bool answerQuestion(string answer)
    }
}
