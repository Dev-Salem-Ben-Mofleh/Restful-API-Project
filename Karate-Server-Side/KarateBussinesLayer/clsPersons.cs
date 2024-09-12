using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clsKarateDataAccesseLayer;
using static clsKarateDataAccesseLayer.clsDataPerson;

namespace clsKarateBussinseLayer
{
    public class clsPersons
    {

        public enum enMode { eAddNew = 0, eUpdate = 1 }
        public enMode mode = enMode.eAddNew;

        public PersonDTO PDTO
        {
            get { return (new PersonDTO(this.PersonID, this.Name, this.Address, this.Phone,this.DateOfBirth,this.Gender,this.Email
                ,this.ImagePath)); }
        }
        public int PersonID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gender { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public clsPersons(PersonDTO PDTO, enMode cMode = enMode.eAddNew)

        {
            this.PersonID = PDTO.PersonID;
            this.Name = PDTO.Name;
            this.Address = PDTO.Address;
            this.Phone = PDTO.Phone;
            this.DateOfBirth = PDTO.DateOfBirth;
            this.Gender = PDTO.Gender;
            this.Email = PDTO.Email;
            this.ImagePath = PDTO.ImagePath;

            mode = cMode;
        }
        public clsPersons()
        {
            PersonID = -1;
            Name = "";
            Address = "";
            Phone = "";
            DateOfBirth = DateTime.MinValue;
            Gender = 0;
            Email = "";
            ImagePath = "";

            mode = enMode.eAddNew;
        }

        bool _AddNewRow()
        {

            this.PersonID = clsDataPerson.AddNewRow(PDTO);

            return (this.PersonID != -1);

        }
        bool _UpdateRow()=> clsDataPerson.UpdateRow(PDTO);

        public static clsPersons FindByPersonID(int PersonID)
        {

            PersonDTO PDTO=clsDataPerson.GetRowInfoByPersonID(PersonID);

            if(PDTO!=null)
            {
                return new clsPersons(PDTO, enMode.eUpdate);
            }
            else
                return null;
        }
        public static List<PersonDTO> GetAllRows()=> clsDataPerson.GetAllRows();
        public bool Save()
        {
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

        public static bool DoesRowExist(int PersonID) => (clsDataPerson.DoesRowExist(PersonID));

        public static bool DeleteRow(int ID) => clsDataPerson.DeleteRow(ID);
        public static int CounttRows() => clsDataPerson.CountPersons();


    }
}
