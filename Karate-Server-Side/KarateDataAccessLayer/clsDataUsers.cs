using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace clsKarateDataAccesse
{
    public class clsDataUsers
    {
        public static bool GetRowInfoByUserID(int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive, ref byte Permission)
        {

            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand Command = new SqlCommand("sp_GetUserByID", connection))
                    {

                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@UserID", UserID);


                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {

                            if (Reader.Read())
                            {
                                IsFound = true;
                                PersonID = (int)Reader["PersonID"];
                                UserName = (string)Reader["UserName"];
                                Password = (string)Reader["Password"];
                                IsActive = (bool)Reader["IsActive"];
                                Permission = (byte)Reader["Permission"];


                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }

            return IsFound;


        }
        public static bool FindByUsernameAndPassword(string UserName, string Password, ref int UserID, ref int PersonID, ref bool IsActive, ref byte Permission)
        {

            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand Command = new SqlCommand("sp_GetUserByUserNameAndPassword", connection))
                    {

                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@UserName", UserName);
                        Command.Parameters.AddWithValue("@Password", Password);



                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {

                            if (Reader.Read())
                            {
                                IsFound = true;
                                UserID = (int)Reader["UserID"];
                                PersonID = (int)Reader["PersonID"];
                                IsActive = (bool)Reader["IsActive"];
                                Permission = (byte)Reader["Permission"];


                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }

            return IsFound;


        }
        public static int AddNewRow(int PersonID, string UserName, string Password, bool IsActive, byte Permission)
        {

            int UserID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand Command = new SqlCommand("sp_AddNewUserUser", connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@PersonID", PersonID);
                        Command.Parameters.AddWithValue("@UserName", UserName);
                        Command.Parameters.AddWithValue("@Password", Password);
                        Command.Parameters.AddWithValue("@IsActive", IsActive);
                        Command.Parameters.AddWithValue("@Permission", Permission);


                        SqlParameter outputIdParam = new SqlParameter("@UserID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        Command.Parameters.Add(outputIdParam);

                        Command.ExecuteNonQuery();

                        UserID = (int)Command.Parameters["@UserID"].Value;


                    }
                }
            }

            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }



            return UserID;

        }
        public static bool UpdateRow(int UserID, int PersonID, string UserName, string Password, bool IsActive, byte Permission)
        {

            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    connection.Open();



                    using (SqlCommand Command = new SqlCommand("sp_UpdateUser", connection))
                    {

                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@UserID", UserID);
                        Command.Parameters.AddWithValue("@PersonID", PersonID);
                        Command.Parameters.AddWithValue("@UserName", UserName);
                        Command.Parameters.AddWithValue("@Password", Password);
                        Command.Parameters.AddWithValue("@IsActive", IsActive);
                        Command.Parameters.AddWithValue("@Permission", Permission);



                        RowsAffected = Command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);

                return false;
            }


            return (RowsAffected > 0);

        }
        public static DataTable GetAllRows()
        {

            DataTable DT = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand Command = new SqlCommand("sp_GetAllUsers", connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {

                            while (Reader.HasRows)
                            {
                                DT.Load(Reader);
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }


            return DT;

        }
        public static bool DeleteRow(int UserID)
        {

            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand Command = new SqlCommand("sp_DeleteUser", connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@UserID", UserID);



                        RowsAffected = Command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }

            return (RowsAffected > 0);

        }
        public static bool DoesRowExist(int UserID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand Command = new SqlCommand("SP_CheckUserExists", connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@UserID", UserID);



                        SqlParameter returnParameter = new SqlParameter(@"ReturnVal", SqlDbType.TinyInt)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        Command.Parameters.Add(returnParameter);

                        Command.ExecuteNonQuery();

                        IsFound = ((int)returnParameter.Value == 1);



                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }


            return IsFound;
        }
        public static bool DoesRowExist(string UserName)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand Command = new SqlCommand("sp_CheckUserNameExist", connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@UserName", UserName);



                        SqlParameter returnParameter = new SqlParameter(@"ReturnVal", SqlDbType.TinyInt)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        Command.Parameters.Add(returnParameter);

                        Command.ExecuteNonQuery();

                        IsFound = ((int)returnParameter.Value == 1);



                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }


            return IsFound;
        }
        public static int CountUsers()
        {

            int countUsers = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand Command = new SqlCommand("dbo.CountUser", connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;


                        SqlParameter returnParameter = new SqlParameter(@"ReturnVal", SqlDbType.TinyInt)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        Command.Parameters.Add(returnParameter);

                        Command.ExecuteNonQuery();

                        countUsers = (int)returnParameter.Value;


                    }
                }
            }

            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }

            return countUsers;

        }


    }
}
