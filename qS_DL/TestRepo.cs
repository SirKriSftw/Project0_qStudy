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

        public List<Test> getUsersTest(int user)
        {
            List<Test> usersTests = new List<Test>();
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Tests WHERE userID = @userID";
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@userID", user);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                int testID = reader.GetInt32(0);
                                string testName = reader.GetString(2);  

                                Test readTest = new Test(testID, testName);
                                usersTests.Add(readTest);
                            }
                        }
                    }
                }
            }
            return usersTests;
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
                                Choice newChoice = new Choice(questionID, char.Parse((string)choiceRow["choiceLetter"]), (string)choiceRow["choice"]);
                                currQuestion.choices.Add(newChoice);
                            }
                            dataSet = new DataSet();                           
                        }
                        testQuestions.Add(currQuestion);
                    }
            }
            return testQuestions;    
        }
        public int saveTest( int userID, string testName)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))  
            {
                conn.Open();
                string query = "INSERT INTO Tests VALUES (@userID, @testName)";
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@testName", testName);
                    cmd.ExecuteNonQuery();
                }
                query = "SELECT Max(testID) FROM Tests";
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            reader.Read();
                            return reader.GetInt32(0);
                        }
                    }
                }
            }
            return 0;
        }
        public bool deleteTest(int user, int test)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Tests WHERE userID = @userID AND testID = @testID";
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@userID",user);
                    cmd.Parameters.AddWithValue("@testID",test);
                    cmd.ExecuteNonQuery();
                }
            }
            return false;
        }
    }
}
