using clsKarateDataAccesse;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Reflection.PortableExecutable;
using static clsKarateDataAccesseLayer.clsDataPerson;

namespace clsKarateDataAccesseLayer
{
    public class clsDataPerson
    {
        public class PersonDTO
        {
            public PersonDTO(int PersonID, string Name, string Address, string Phone,
                             DateTime DateOfBirth, byte Gender, string Email, string ImagePath)
            {
                this.PersonID = PersonID;
                this.Name = Name;
                this.Address = Address;
                this.Phone = Phone;
                this.DateOfBirth = DateOfBirth;
                this.Gender = Gender;
                this.Email = Email;
                this.ImagePath = ImagePath;
            }


            public int PersonID { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public DateTime DateOfBirth { get; set; }
            public byte Gender { get; set; }
            public string Email { get; set; }
            public string ImagePath { get; set; }

        }


        public static PersonDTO GetRowInfoByPersonID(int PersonID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    using (SqlCommand Command = new SqlCommand("sp_GetPersonByID", connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();

                        using (SqlDataReader reader = Command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                return new PersonDTO
                                 (
                                 reader.GetInt32(reader.GetOrdinal("PersonID")),
                                 reader.GetString(reader.GetOrdinal("Name")),
                                 reader.GetString(reader.GetOrdinal("Address")),
                                 reader.GetString(reader.GetOrdinal("Phone")),
                                 reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                 reader.GetByte(reader.GetOrdinal("Gender")),

                                 reader.IsDBNull(reader.GetOrdinal("Email")) ? "" :
                                 reader.GetString(reader.GetOrdinal("Email")),

                                 reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? "" :
                                 reader.GetString(reader.GetOrdinal("ImagePath"))
                                 );

                            }
                            else
                            {
                                return default;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }
            return default;

        }
        public static int AddNewRow(PersonDTO personDTO)
        {
            int PersonID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))

                {
                    using (SqlCommand Command = new SqlCommand("SP_AddNewPerson", connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@Name", personDTO.Name);
                        Command.Parameters.AddWithValue("@Address", personDTO.Address);
                        Command.Parameters.AddWithValue("@Phone", personDTO.Phone);
                        Command.Parameters.AddWithValue("@DateOfBirth", personDTO.DateOfBirth);
                        Command.Parameters.AddWithValue("@Gender", personDTO.Gender);

                        if (personDTO.Email != null && personDTO.Email.ToString() != string.Empty)
                            Command.Parameters.AddWithValue("@Email", personDTO.Email);
                        else
                            Command.Parameters.AddWithValue("@Email", DBNull.Value);


                        if (personDTO.ImagePath != "" && personDTO.ImagePath != null)
                            Command.Parameters.AddWithValue("@ImagePath", personDTO.ImagePath);
                        else
                            Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);


                        SqlParameter outputIdParam = new SqlParameter("@PersonID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        Command.Parameters.Add(outputIdParam);

                        connection.Open();
                        Command.ExecuteNonQuery();

                        PersonID = (int)outputIdParam.Value;

                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }
            return PersonID;

        }
        public static bool UpdateRow(PersonDTO personDTO)
        {

            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    using (SqlCommand Command = new SqlCommand("sp_UpdatePerson", connection))
                    {

                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@PersonID", personDTO.PersonID);
                        Command.Parameters.AddWithValue("@Name", personDTO.Name);
                        Command.Parameters.AddWithValue("@Address", personDTO.Address);
                        Command.Parameters.AddWithValue("@Phone", personDTO.Phone);
                        Command.Parameters.AddWithValue("@DateOfBirth", personDTO.DateOfBirth);
                        Command.Parameters.AddWithValue("@Gender", personDTO.Gender);

                        if (personDTO.Email != null && personDTO.Email.ToString() != string.Empty)
                            Command.Parameters.AddWithValue("@Email", personDTO.Email);
                        else
                            Command.Parameters.AddWithValue("@Email", DBNull.Value);

                        if (personDTO.ImagePath != null && personDTO.ImagePath.ToString() != string.Empty)
                            Command.Parameters.AddWithValue("@ImagePath", personDTO.ImagePath);
                        else
                            Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

                        connection.Open();
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
        public static List<PersonDTO> GetAllRows()
        {

            var PersonsList = new List<PersonDTO>();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    using (SqlCommand Command = new SqlCommand("sp_GetAllPeople", connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = Command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                PersonsList.Add(new PersonDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Address")),
                                reader.GetString(reader.GetOrdinal("Phone")),
                                reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                reader.GetByte(reader.GetOrdinal("Gender")),

                                reader.IsDBNull(reader.GetOrdinal("Email")) ? "" :
                                reader.GetString(reader.GetOrdinal("Email")),

                                reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? "" :
                                reader.GetString(reader.GetOrdinal("ImagePath"))
                                ));
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }


            return PersonsList;

        }
        public static bool DoesRowExist(int PersonID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    using (SqlCommand Command = new SqlCommand("SP_CheckPersonExists", connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();

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

        public static bool DeleteRow(int PersonID)
        {

            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    using (SqlCommand Command = new SqlCommand("sp_DeletePerson", connection))
                    {

                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();
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

        public static int CountPersons()
        {
            int countPesrons = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsAccesseSetting.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("CountPersons", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        SqlParameter returnParameter = new SqlParameter(@"ReturnVal", SqlDbType.TinyInt)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnParameter);

                        command.ExecuteNonQuery();

                        countPesrons = (int)returnParameter.Value;

                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }
            return countPesrons;
        }



    }

}
