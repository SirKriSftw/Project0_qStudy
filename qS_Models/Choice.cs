using System;

namespace qS_Models
{
    public class Choice
    {
        public int questionID { get; set; }
        public char choiceLetter { get; set; }
        public string choice { get; set; }

        public Choice(){}
        public Choice(int questionID, char letter, string choice)
        {
            this.questionID = questionID;
            this.choiceLetter = letter;
            this.choice = choice;
        }
    }
}
