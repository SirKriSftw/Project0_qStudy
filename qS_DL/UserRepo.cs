using System;
using System.Data.SqlClient;

namespace qS_DL
{
    public class UserRepo
    {
        private string _connectionString;        
        public UserRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int login(string user, string pass)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT userID FROM Users WHERE username = @user AND password = @pass";
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            reader.Read();
                            return reader.GetInt32(0);
                        }
                        return -1;
                    }
                }
            }
        }

    }
}
