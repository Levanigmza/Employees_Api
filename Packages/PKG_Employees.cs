using System.Data;
using Employee_Api.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Employee_Api.Packages
{


    public interface IPKG_Employees
    {


        public void delete_employee(Int64 Person_ID);
        void add_employee(string p_first_name, string p_last_name, int p_age, Int64 p_person_id, string p_profession, string p_phone_number);
        void update_employee(Int64 Person_ID, string p_first_name, string p_last_name, int p_age, Int64 p_new_person_id, string p_profession, string p_phone_number);
        public Employee getemployee(Int64 Person_ID);
        public List<Employee> GetEmployees();


    }




    public class PKG_Employees : PKG_BASE, IPKG_Employees
    {
        public PKG_Employees(IConfiguration config) : base(config) { }



        public void add_employee(string p_first_name, string p_last_name, int p_age, Int64 p_person_id, string p_profession, string p_phone_number)
        {
            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("olerning.levanigmza_pkg_employee.employee_add", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = p_first_name;
                    cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = p_last_name;
                    cmd.Parameters.Add("p_age", OracleDbType.Int32).Value = p_age;
                    cmd.Parameters.Add("p_person_id", OracleDbType.Int64).Value = p_person_id;
                    cmd.Parameters.Add("p_profession", OracleDbType.Varchar2).Value = p_profession;
                    cmd.Parameters.Add("p_phone_number", OracleDbType.Varchar2).Value = p_phone_number;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public void update_employee(Int64 Person_ID, string p_first_name, string p_last_name, int p_age, Int64 p_new_person_id, string p_profession, string p_phone_number)
        {
            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("olerning.levanigmza_pkg_employee.employee_update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = p_first_name;
                    cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = p_last_name;
                    cmd.Parameters.Add("p_age", OracleDbType.Int32).Value = p_age;
                    cmd.Parameters.Add("p_person_id", OracleDbType.Int64).Value = Person_ID;
                    cmd.Parameters.Add("p_new_person_id", OracleDbType.Int64).Value = p_new_person_id;
                    cmd.Parameters.Add("p_profession", OracleDbType.Varchar2).Value = p_profession;
                    cmd.Parameters.Add("p_phone_number", OracleDbType.Varchar2).Value = p_phone_number;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public void delete_employee(Int64 Person_ID)
        {
            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("olerning.levanigmza_pkg_employee.employee_delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_person_id", OracleDbType.Int64).Value = Person_ID;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }






        public Employee getemployee(Int64 Person_ID)
        {
            Employee employee = null;

            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand command = new OracleCommand("olerning.levanigmza_pkg_employee.get_employee", conn))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("p_person_id", OracleDbType.Int64).Value = Person_ID;
                        command.Parameters.Add("p_first_name", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                        command.Parameters.Add("p_last_name", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                        command.Parameters.Add("p_age", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        command.Parameters.Add("p_profession", OracleDbType.Varchar2, 200).Direction = ParameterDirection.Output;
                        command.Parameters.Add("p_phone_number", OracleDbType.Varchar2, 200).Direction = ParameterDirection.Output;


                        command.ExecuteNonQuery();


                        employee = new Employee
                        {
                            Firstname = command.Parameters["p_first_name"].Value.ToString(),
                            Lastname = command.Parameters["p_last_name"].Value.ToString(),
                            Age = ((OracleDecimal)command.Parameters["p_age"].Value).ToInt32(),
                            Person_ID = Person_ID,
                            Proffesion = command.Parameters["p_profession"].Value.ToString(),
                            PhoneNumber = command.Parameters["p_phone_number"].Value.ToString(),
                        };
                        conn.Close();

                        return employee;

                    }
                    catch (OracleException ex)
                    {
                        if (ex.Message.Contains("ORA-20005"))
                        {
                            return null;
                        }
                        else
                        {
                            throw ex;
                        }
                    }

                }

            }

        }


        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = new OracleCommand();

            cmd.Connection = conn;
            cmd.CommandText = "olerning.levanigmza_pkg_employee.get_employee_all";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Employee employee = new Employee();
                employee.Firstname = (reader["FIRST_NAME"].ToString());
                employee.Lastname = reader["LAST_NAME"].ToString();
                employee.Age = int.Parse(reader["AGE"].ToString());
                employee.Person_ID = Convert.ToInt64(reader["PERSON_ID"]);
                employee.Proffesion = (reader["PROFESSION"].ToString());
                employee.PhoneNumber = (reader["PHONE_NUMBER"].ToString());
                employees.Add(employee);
            }
            conn.Close();

            return employees;

        }










    }
}
