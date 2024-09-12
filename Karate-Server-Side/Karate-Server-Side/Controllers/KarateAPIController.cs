using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using clsKarateBussinseLayer;
using clsKarateDataAccesseLayer;
using static clsKarateDataAccesseLayer.clsDataPerson;
using clsKarateBussinse;

namespace Karate_Server_Side.Controllers
{
    [Route("api/KarateAPI")]
    [ApiController]
    public class KarateAPIController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllPersons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<PersonDTO>> GetAllPersons()
        {
            List<PersonDTO> Persons = clsPersons.GetAllRows();

            if (Persons == null || Persons.Count == 0)
            {
                return NotFound("No Persons Found!");
            }

            return Ok(Persons);
        }

        [HttpGet("{id}", Name = "GetPersonById")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<PersonDTO> GetPersonById(int id)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            clsPersons person = clsPersons.FindByPersonID(id);

            if (person == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            PersonDTO PDTO = person.PDTO;

            //we return the DTO not the student object.
            return Ok(PDTO);

        }

        [HttpPost("Add",Name = "AddPerson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonDTO> AddPerson(PersonDTO newPersonDTO)
        {
            //we validate the data here
            if (newPersonDTO == null)
            {
                return BadRequest("Invalid student data.");
            }


            clsPersons persons = new clsPersons(new PersonDTO(newPersonDTO.PersonID, newPersonDTO.Name, newPersonDTO.Address, newPersonDTO.Phone
                , newPersonDTO.DateOfBirth, newPersonDTO.Gender, newPersonDTO.Email, newPersonDTO.ImagePath));


            if (persons.Save())

            {
                newPersonDTO.PersonID = persons.PersonID;

                return CreatedAtRoute("GetPersonById", new { id = newPersonDTO.PersonID }, newPersonDTO);
            }
            else
            {
                return StatusCode(500, new { message = "Error adding person" });

            }



        }

        [HttpPut("{id}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonDTO> UpdatePerson(int id, PersonDTO updatedPerson)
        {
            if (updatedPerson == null || id < 1)
            {
                return BadRequest("Invalid Person data.");
            }


            clsPersons Perosn = clsPersons.FindByPersonID(id);


            if (Perosn == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            Perosn.Name = updatedPerson.Name;
            Perosn.Address = updatedPerson.Address;
            Perosn.Phone = updatedPerson.Phone;
            Perosn.DateOfBirth = updatedPerson.DateOfBirth;
            Perosn.Gender = updatedPerson.Gender;
            Perosn.Email = updatedPerson.Email;
            Perosn.ImagePath = updatedPerson.ImagePath;


            if (Perosn.Save())
            {
                return Ok(Perosn.PDTO);
            }
            else
            {
                return StatusCode(500, new { message = "Error updating user" });
            }


        }

        [HttpDelete("{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePerson(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            
            if (clsPersons.DeleteRow(id))

                return Ok($"Person with ID {id} has been deleted.");
            else
                return NotFound($"Person with ID {id} not found. no rows deleted!");
        }

        [HttpGet("exists/{id}", Name = "ExistsPersonByPersonId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> ExistsPersonByUserId(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (clsPersons.DoesRowExist(id))
            {
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }
        }

        [HttpGet("Count/Persons", Name = "CountPersons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> CountPersons()
        {
            int Persons=clsPersons.CounttRows();

            if (Persons <= 0)
            {
                return NotFound("No Persons");
            }

            return Ok(Persons);


        }


         [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadDirectory = @"C:\Karate-People-Images";

            // Generate a unique filename
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Ok(new { filePath });
        }


        [HttpGet("GetImage/{fileName}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetImage(string fileName)
        {
            if(fileName==null|| fileName=="")
            {
                return BadRequest("No Image Found.");

            }

            var filePath =  fileName;

            if (!System.IO.File.Exists(filePath))
                return NotFound("Image not found.");

            var image = System.IO.File.OpenRead(filePath);
            var mimeType = GetMimeType(filePath);

            return File(image, mimeType);
        }

        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }



    }
}
