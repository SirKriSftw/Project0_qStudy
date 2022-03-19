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
        public List<Choice> choices { get; set; } = new List<Choice>();

        public Question(){}
        public Question(int questionID, int testID, int typeID, string question, string answer)
        {
            this.questionID = questionID;
            this.testID = testID;
            this.typeID = typeID;
            this.question = question;
            this.answer = answer;
        }
        public Question(int testID, int typeID, string question, string answer)
        {
            this.testID = testID;
            this.typeID = typeID;
            this.question = question;
            this.answer = answer;
        }
        public Question(int testID, int typeID, string question, string answer, List<Choice> choices)
        {
            this.testID = testID;
            this.typeID = typeID;
            this.question = question;
            this.answer = answer;
            this.choices = choices;
        }
        public void displayQuestion()
        {
            System.Console.WriteLine(question);
            if(typeID == 1)
            {
                foreach(Choice newChoice in choices)
                {
                    System.Console.WriteLine("   " + newChoice.choiceLetter + ". " + newChoice.choice);
                }   
            }
            System.Console.WriteLine();
        }

        public Question createQuestion(int testID)
        {
            System.Console.WriteLine("Enter the question");
            string question = Console.ReadLine();
            System.Console.WriteLine("Enter the answer");
            string answer = Console.ReadLine();
            return new Question(testID, 2, question, answer);;
        }
        public Question createMCQuestion(int testID)
        {
            System.Console.WriteLine("Enter the question");
            string question = Console.ReadLine();            
            bool isAddingChoices = true;
            char currLetter = 'a';
            while(isAddingChoices)
            {
                System.Console.WriteLine("Enter the " + currLetter +". choice");
                System.Console.WriteLine("==>(Leave blank to exit)<==");
                string choice = Console.ReadLine();
                if(!string.IsNullOrEmpty(choice))
                {
                    Choice newChoice = new Choice(questionID, currLetter, choice);
                    choices.Add(newChoice);
                    currLetter++;
                }
                else
                {
                    isAddingChoices = false;
                }

            }
            foreach(Choice readChoice in choices)
            {
                System.Console.WriteLine(readChoice.choiceLetter + ". " + readChoice.choice);
            }
            System.Console.WriteLine("Enter the correct choice letter");
            string answer = Console.ReadLine();
            return new Question(testID, 1, question, answer, choices);
        }
        //public bool answerQuestion(string answer)
    }
}
