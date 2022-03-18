using qS_Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;


namespace qS_DL
{
    public class TestRepo
    {
        private string _connectionString;
        public TestRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Question> getTestQuestions(int test)
        {
            List<Question> testQuestions = new List<Question>();
            bool paramAdded = false;
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Questions WHERE testID = @testID";
                SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@testID", test);
                    DataSet dataSet = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataSet, "Questions");

                    DataTable Questions = dataSet.Tables["Questions"];
                    foreach(DataRow row in Questions.Rows)
                    {
                        int questionID = (int)row["questionID"];
                        int typeID = (int)row["typeID"];
                        Question currQuestion = new Question(questionID, (int)row["testID"], typeID,(string)row["question"],(string)row["answer"]);
                        if(typeID == 1)
                        {
                            query = "SELECT * FROM Choices WHERE questionID = @questionID";
                            cmd.CommandText = query;
                            if(!paramAdded)
                            {
                                cmd.Parameters.Add("@questionID", SqlDbType.Int);
                                paramAdded = true;
                            }
                            cmd.Parameters["@questionID"].Value = questionID;
                            adapter = new SqlDataAdapter(cmd);
                            adapter.Fill(dataSet, "Choices");
                            DataTable Choices = dataSet.Tables["Choices"];
                            foreach(DataRow choiceRow in Choices.Rows)
                            {
                                currQuestion.choices.Add((string)choiceRow["choice"]);
                            }
                            dataSet = new DataSet();                           
                        }
                        testQuestions.Add(currQuestion);
                    }
            }
            return testQuestions;    
        }
    
        // public List<Test> getAllTests(int user)
    }
}
