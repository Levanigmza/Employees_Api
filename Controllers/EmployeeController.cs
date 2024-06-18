using Employee_Api.Common;
using Employee_Api.Model;
using Employee_Api.Packages;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace Employee_Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        IPKG_Employees package;
        public EmployeeController(IPKG_Employees package)
        {
            this.package = package;
        }



        [HttpPost("add")]
        public IActionResult AddEmployee([FromBody] Employee employee)
        {

            if (employee == null || string.IsNullOrEmpty(employee.Firstname) || string.IsNullOrEmpty(employee.Lastname) || string.IsNullOrEmpty(employee.Proffesion))
            {
                return StatusCode(400, "All fields  are required.");
            }

            try
            {
                package.add_employee(employee.Firstname, employee.Lastname, employee.Age, employee.Person_ID, employee.Proffesion, employee.PhoneNumber);
                return Ok("Employee successfully added.");
            }
            catch (OracleException ex)
            {
                var errorInfo = ErrorCodes.GetErrorInfo(ex.Number);
                return StatusCode(errorInfo.StatusCode, errorInfo.ErrorMessage);
            }
        }




        [HttpPost("update/{PersonID}")]
        public IActionResult UpdateEmployee(long PersonID, [FromBody] Employee employee)
        {
            if (employee == null || string.IsNullOrEmpty(employee.Firstname) || string.IsNullOrEmpty(employee.Lastname) || string.IsNullOrEmpty(employee.Proffesion))
            {
                return StatusCode(400, "All fields are required.");
            }
            try
            {
                package.update_employee(PersonID, employee.Firstname, employee.Lastname, employee.Age, employee.Person_ID, employee.Proffesion, employee.PhoneNumber);
                return Ok("Employee successfully updated.");
            }
            catch (OracleException ex)
            {
                var errorInfo = ErrorCodes.GetErrorInfo(ex.Number);
                return StatusCode(errorInfo.StatusCode, errorInfo.ErrorMessage);
            }
        }




        [HttpDelete("delete/{PersonID}")]
        public IActionResult DeleteEmployee(Int64 PersonID)
        {
            if (PersonID == null)
            {
                return StatusCode(400, "Person ID  field is required");
            }
            try
            {
                package.delete_employee(PersonID);
                return Ok("person has been deleted");
            }
            catch (OracleException ex)

            {
                var errorInfo = ErrorCodes.GetErrorInfo(ex.Number);
                return StatusCode(errorInfo.StatusCode, errorInfo.ErrorMessage);
            }


        }






        [HttpGet("getemployee/{PersonID}")]
        public IActionResult GetEmployee(long PersonID)
        {
            if (PersonID == null)
            {
                return StatusCode(400, "Person ID field is required");
            }

            package.getemployee(PersonID);

            Employee employee = package.getemployee(PersonID);

            if (employee == null)
            {
                return NotFound("Person ID does not exist in the database.");
            }
            else
            {
                return Ok(employee);
            }
        }


        [HttpGet("getemployeeAll")]
        public IActionResult GetEmployeeAll()
        {
            List<Employee> employees = new List<Employee>();
            employees = package.GetEmployees();
            return Ok(employees);
        }







    }



}
