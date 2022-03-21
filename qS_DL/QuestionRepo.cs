using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using qS_Models;

namespace qS_DL
{
    public class QuestionRepo
    {
        private string _connectionString;
        public QuestionRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public int AddQuestion(Question question)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Questions VALUES (@testID, @typeID, @question, @answer)";
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@testID",question.testID);
                    cmd.Parameters.AddWithValue("@typeID",question.typeID);
                    cmd.Parameters.AddWithValue("@question",question.question);
                    cmd.Parameters.AddWithValue("@answer",question.answer);
                    cmd.ExecuteNonQuery();
                }
                query = "SELECT Max(questionID) FROM Questions";
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
        public void EditQuestion(int questionID, string changeTo)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE Questions SET question = @changeTo WHERE questionID = @questionID";
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@changeTo", changeTo);
                    cmd.Parameters.AddWithValue("@questionID", questionID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EditAnswer(int questionID, string changeTo)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE Questions SET answer = @changeTo WHERE questionID = @questionID";
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@changeTo", changeTo);
                    cmd.Parameters.AddWithValue("@questionID", questionID);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
