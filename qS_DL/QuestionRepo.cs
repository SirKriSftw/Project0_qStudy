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

        //public Question getQuestion(int question)
    }
}
