using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clsKarateBussinseLayer;
using clsKarateDataAccesse;

namespace clsKarateBussinse
{
    public class clsUsers:clsPersons
    {
        public enum enMode { eAddNew = 0, eUpdate = 1 }
        public enMode mode = enMode.eAddNew;
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public byte Permission { get; set; }

        public clsUsers()
        {
            UserID = -1;
            UserName = "";
            Password = "";
            IsActive = false;
            Permission = 0;

            mode = enMode.eAddNew;
        }
        private clsUsers(int UserID, int PersonID, string UserName, string Password, bool IsActive, byte Permission
            , string Name, string Address, string Phone, DateTime DateOfBirth, byte Gender, string Email, string ImagePath)
        {

            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;
            this.Permission = Permission;
            this.Name = Name;
            this.Address = Address;
            this.Phone = Phone;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.ImagePath = ImagePath;
            this.Email = Email;

            mode = enMode.eUpdate;

        }

        bool _AddNewRow()
        {

            this.UserID = clsDataUsers.AddNewRow(this.PersonID, this.UserName, this.Password, this.IsActive, this.Permission);

            return this.UserID != -1;

        }
        bool _UpdateRow()
        {

            return clsDataUsers.UpdateRow(this.UserID, this.PersonID, this.UserName, this.Password, this.IsActive, this.Permission);


        }
        //public static clsUsers FindByUserID(int UserID)
        //{

        //    string UserName = "";
        //    string Password = "";
        //    bool IsActive = false;
        //    byte Permission = 0;
        //    int PersonID = -1;

        //    bool IsFound = clsDataUsers.GetRowInfoByUserID(UserID, ref PersonID, ref UserName, ref Password, ref IsActive, ref Permission);

        //        if (IsFound)
        //    {
        //        clsPersons persons = clsPersons.FindByPersonID(PersonID);

        //        return new clsUsers(UserID, PersonID, UserName, Password, IsActive, Permission, persons.Name, persons.Address, persons.Phone
        //            , persons.DateOfBirth, persons.Gender, persons.Email, persons.ImagePath);
        //    }
        //    else
        //        return null;
        // ;
        //}
        //public static clsUsers FindByUsernameAndPassword(string UserName,string Password )
        //{

        //    int PersonID = -1;
        //    bool IsActive = false;
        //    byte Permission = 0;
        //    int UserID = -1;

        //    bool IsFound = clsDataUsers.FindByUsernameAndPassword(UserName, Password, ref UserID, ref PersonID, ref IsActive, ref Permission);

        //    if (IsFound)
        //    {
        //        clsPersons persons = clsPersons.FindByPersonID(PersonID);

        //        return new clsUsers(UserID, PersonID, UserName, Password, IsActive, Permission, persons.Name, persons.Address, persons.Phone
        //            , persons.DateOfBirth, persons.Gender, persons.Email, persons.ImagePath);
        //    }
        //    else
        //        return null;

        //          }
        public static DataTable GetAllRows()
        {
            DataTable DT = clsDataUsers.GetAllRows();
            return DT;
        }
        public static int CountUsers()
        {
            return (clsDataUsers.CountUsers());
        }
        public static bool DeleteRow(int UserID)
        {
            return (clsDataUsers.DeleteRow(UserID));
        }
        public static bool DoesRowExist(int UserID)
        {
            return (clsDataUsers.DoesRowExist(UserID));
        }
        public static bool DoesRowExist(string UserName)
        {
            return (clsDataUsers.DoesRowExist(UserName));
        }
        public bool Save()
        {
            base.mode = (clsPersons.enMode)mode;
            if (!base.Save())
                return false;

            switch (mode)
            {
                case enMode.eAddNew:
                    {
                        if (_AddNewRow())
                        {
                            mode = enMode.eUpdate;
                            return true;
                        }
                        else
                            return false;

                    }
                case enMode.eUpdate:

                    return _UpdateRow();

            }

            return false;
        }

    }
}
