using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWebAPI.Models.DTOs.Employee
{
    public class EmployeeCompleteResult
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CellphoneNumber { get; set; }
        public string PersonalPictureFileName { get; set; }
        public List<Department.DepartmentResult> Departments { get; set; }
        public bool IsActive { get; set; }
    }
}