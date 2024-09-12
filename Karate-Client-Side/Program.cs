
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;

namespace StudentApiClient
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("https://localhost:7020/api/KarateAPI/"); // Set this to the correct URI for your API

            //this will show all Persons 
            await GetAllPersons();

            //this will show the info for Person 1
            await GetPersonById(1); // Example: Get Person with ID 1

            //this will show the info for Person 20 and show not found because 20 is not there
            await GetPersonById(20); // Example: Get Person with ID 20

            //this will add new Person
            var newPerson = new Person
            {
                Name = "Salem Berek",
                Address = "street",
                Phone = "774279865",
                DateOfBirth = DateTime.Now,
                Gender = 0,
                Email = "",
                ImagePath = ""
            };
            await AddPerson(newPerson); // Example: Add a new Person

            //this will show all Persons after adding new one
            await GetAllPersons();

            //this will delete Person 1058
            await DeletePerson(1058); // Example: Delete Person with ID 1058

            //this will show all students after deleting Person 1058
            await GetAllPersons();

            //this will Update Person 1059
            await UpdatePerson(1059, new Person
            {
                Name = "Salem Berek",
                Address = "Ali Street",
                Phone = "774279865",
                DateOfBirth = DateTime.Now,
                Gender = 0,
                Email = "",
                ImagePath = ""
            }); // Example: Update Person with ID 1058

            //Get Person with ID 1059 is Found Or Not
            await ExistsPersonByUserId(1059);

            //Get Person with ID 1060 is Found Or Not
            await ExistsPersonByUserId(1060);

            //Get Count Of Persons 
            await CountPersons();
            //this will show all students after Updating Person 1058
            await GetAllPersons();


        }

        static async Task GetAllPersons()
        {
            string email = "";
            string ImagePath = "";
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine("\nFetching all Persons...\n");
                var response = await httpClient.GetAsync("All");

                if (response.IsSuccessStatusCode)
                {
                    var persons = await response.Content.ReadFromJsonAsync<List<Person>>();
                    if (persons != null && persons.Count > 0)
                    {
                        foreach (var person in persons)
                        {
                            email = person.Email == null || person.Email.ToString() == string.Empty ? "No Email" : person.Email;
                            ImagePath = person.ImagePath == null || person.ImagePath.ToString() == string.Empty ? "No Image" : person.ImagePath;


                            Console.WriteLine($"ID: {person.PersonID}, Name: {person.Name}, Address: {person.Address}, Phone: {person.Phone}, Date Of Birth: {person.DateOfBirth}," +
                                $" Gender: {person.Gender}, Email: {email}, ImagePath: {ImagePath}");
                        }
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("No Person found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task GetPersonById(int id)
        {
            string email = "";
            string ImagePath = "";
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nFetching Person with ID {id}...\n");

                var response = await httpClient.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    var person = await response.Content.ReadFromJsonAsync<Person>();
                    if (person != null)
                    {
                        email = person.Email == null || person.Email.ToString() == string.Empty ? "No Email" : person.Email;
                        ImagePath = person.ImagePath == null || person.ImagePath.ToString() == string.Empty ? "No Image" : person.ImagePath;

                        Console.WriteLine($"ID: {person.PersonID}, Name: {person.Name}, Address: {person.Address}, Phone: {person.Phone}, Date Of Birth: {person.DateOfBirth}," +
                            $" Gender: {person.Gender}, Email: {email}, ImagePath: {ImagePath}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: Not accepted ID {id}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found: Person with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task AddPerson(Person newPerson)
        {
            string email = "";
            string ImagePath = "";

            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine("\nAdding a new Person...\n");

                var response = await httpClient.PostAsJsonAsync("Add", newPerson);

                if (response.IsSuccessStatusCode)
                {
                    var addedPerson = await response.Content.ReadFromJsonAsync<Person>();

                    email = addedPerson.Email == null || addedPerson.Email.ToString() == string.Empty ? "No Email" : addedPerson.Email;
                    ImagePath = addedPerson.ImagePath == null || addedPerson.ImagePath.ToString() == string.Empty ? "No Image" : addedPerson.ImagePath;

                    Console.WriteLine($"Added Student - ID: {addedPerson.PersonID}, Name: {addedPerson.Name}, Address: {addedPerson.Address}, Phone: {addedPerson.Phone}," +
                        $" Date Of Birth: {addedPerson.DateOfBirth}, Gender: {addedPerson.Gender}, Email: {email}, ImagePath: {ImagePath}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("Bad Request: Invalid Person data.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task UpdatePerson(int id, Person updatedPerson)
        {
            string email = "";
            string ImagePath = "";

            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nUpdating Person with ID {id}...\n");
                var response = await httpClient.PutAsJsonAsync($"{id}", updatedPerson);

                if (response.IsSuccessStatusCode)
                {
                    var person = await response.Content.ReadFromJsonAsync<Person>();

                    email = person.Email == null || person.Email.ToString() == string.Empty ? "No Email" : person.Email;
                    ImagePath = person.ImagePath == null || person.ImagePath.ToString() == string.Empty ? "No Image" : person.ImagePath;


                    Console.WriteLine($"Updated Student: ID: {person.PersonID}, Name: {person.Name}, Address: {person.Address}, Phone: {person.Phone}," +
                   $" Date Of Birth: {person.DateOfBirth}, Gender: {person.Gender}, Email: {email}, ImagePath: {ImagePath}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("Failed to update Person: Invalid data.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Person with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task DeletePerson(int id)
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nDeleting Person with ID {id}...\n");
                var response = await httpClient.DeleteAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Person with ID {id} has been deleted.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: Not accepted ID {id}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found: Person with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task ExistsPersonByUserId(int id)
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nExists Person with ID {id}...\n");
                var response = await httpClient.GetAsync($"exists/{id}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Person with ID {id} is here.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: Not accepted ID {id}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found: Person with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task CountPersons()
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nCount Persons...\n");
                var response = await httpClient.GetAsync($"Count/Persons");

                if (response.IsSuccessStatusCode)
                {
                    var Total = await response.Content.ReadFromJsonAsync<int>();

                    Console.WriteLine($"Number Of Persons is: {Total}.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found: No Persons.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }



    }



    public class Person
    {
        public int PersonID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gender { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
    }
}
