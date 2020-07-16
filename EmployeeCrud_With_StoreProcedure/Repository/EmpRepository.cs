using EmployeeCrud_With_StoreProcedure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace EmployeeCrud_With_StoreProcedure.Repository
{
    public class EmpRepository
    {
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ToString();
            con = new SqlConnection(constr);
        }
        public bool AddEmployee(EmpModel obj)
        {
            DateTime Utcnow = DateTime.UtcNow;
            String format = "MM/dd/yyyy hh:mm:ss tt";
            String joindate = Utcnow.ToString(format);

            connection();
            SqlCommand com = new SqlCommand("AddNewEmployee", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Name", obj.Name);
            com.Parameters.AddWithValue("@City", obj.City);
            com.Parameters.AddWithValue("@Address", obj.Address);
            com.Parameters.AddWithValue("@Salary", obj.Salary);
            com.Parameters.AddWithValue("@Gender", obj.Gender);
            com.Parameters.AddWithValue("@JoinDate", joindate);
            com.Parameters.AddWithValue("@IsActive", obj.IsActive);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public List<EmpModel> GetAllEmployees()
        {
            connection();
            List<EmpModel> EmpList = new List<EmpModel>();


            SqlCommand com = new SqlCommand("GetEmployees", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {
                // string date = Convert.ToString(dr["JoinDate"]);
                // DateTime oDate = Convert.ToDateTime(date);

                EmpList.Add(

                    new EmpModel
                    {

                        Empid = Convert.ToInt32(dr["Id"]),
                        Name = Convert.ToString(dr["Name"]),
                        City = Convert.ToString(dr["City"]),
                        Address = Convert.ToString(dr["Address"]),
                        Salary = Convert.ToString(dr["Salary"]),
                        JoinDate = Convert.ToString(dr["JoinDate"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"])
                    }
                    );
            }
            return EmpList;
        }

        public List<EmpModel> ShortingEmployee(string searchvalue, int Pageno, int pageSize, string SortColumn, string SortOrder, string searchByFromdate, string searchByTodate,string fromtime,string totime)
        {

            connection();
            List<EmpModel> EmpList = new List<EmpModel>();


            SqlCommand com = new SqlCommand("Shorting_and_Pagination", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Search", searchvalue);
            com.Parameters.AddWithValue("@PageNo", Pageno);
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@SortColumn", SortColumn);
            com.Parameters.AddWithValue("@SortOrder", SortOrder);
            com.Parameters.AddWithValue("@StartDate", searchByFromdate);
            com.Parameters.AddWithValue("@EndDate", searchByTodate);
            com.Parameters.AddWithValue("@StartTime", fromtime);
            com.Parameters.AddWithValue("@EndTime", totime);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                //string date = Convert.ToString(dr["JoinDate"]);
                //DateTime oDate = Convert.ToDateTime(date);
                //string confirndate = oDate.Month + "/" + oDate.Day + "/" + oDate.Year;
                EmpList.Add(

                    new EmpModel
                    {

                        Empid = Convert.ToInt32(dr["Id"]),
                        Name = Convert.ToString(dr["Name"]),
                        City = Convert.ToString(dr["City"]),
                        Address = Convert.ToString(dr["Address"]),
                        Salary = Convert.ToString(dr["Salary"]),
                        JoinDate = Convert.ToString(dr["JoinDate"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        TotalCount = Convert.ToInt32(dr["TotalCount"]),
                        RowNum = Convert.ToInt32(dr["RowNum"])

                    }
                    ) ;
            }
            return EmpList;
        }

        public bool UpdateEmployee(EmpModel obj)
        {
            connection();
            SqlCommand com = new SqlCommand("UpdateEmployee", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@EmpId", obj.Empid);
            com.Parameters.AddWithValue("@Name", obj.Name);
            com.Parameters.AddWithValue("@City", obj.City);
            com.Parameters.AddWithValue("@Address", obj.Address);
            com.Parameters.AddWithValue("@Salary", obj.Salary);
            com.Parameters.AddWithValue("@Gender", obj.Gender);
            com.Parameters.AddWithValue("@JoinDate", obj.JoinDate);
            com.Parameters.AddWithValue("@IsActive", obj.IsActive);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {
                return false;
            }
        }
        public bool DeleteEmployee(int Id)
        {

            connection();
            SqlCommand com = new SqlCommand("DeleteEmployeeRecored", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@EmpId", Id);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
