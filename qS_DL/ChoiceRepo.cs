using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace qS_DL
{
    public class ChoiceRepo
    {
        private string _connectionString;
        public ChoiceRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        //public List<String> getChoices(int question)
    }
}
