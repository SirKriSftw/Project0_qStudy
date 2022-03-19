﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using qS_Models;

namespace qS_DL
{
    public class ChoiceRepo
    {
        private string _connectionString;
        public ChoiceRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AddChoices(int qID, List<Choice> choices)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Choices VALUES (@questionID, @choiceLetter, @choice)";
                foreach(Choice choice in choices)
                {
                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@questionID", qID);
                        cmd.Parameters.AddWithValue("@choiceLetter", choice.choiceLetter);
                        cmd.Parameters.AddWithValue("@choice", choice.choice);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            return false;
        }
        //public List<String> getChoices(int question)
    }
}
