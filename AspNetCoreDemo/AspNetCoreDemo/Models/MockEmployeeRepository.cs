using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreDemo.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees;

        public MockEmployeeRepository()
        {
            _employees = new List<Employee>
            {
                new Employee { Id = 1,  Name = "Mary", Email = "mary@test.com", Department = Dept.HR },
                new Employee { Id = 2,  Name = "John", Email = "john@test.com", Department = Dept.IT},
                new Employee { Id = 3,  Name = "Sam", Email = "sam@test.com", Department = Dept.IT },
            };
        }

        public Employee GetEmployee(int id)
        {
            return _employees.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employees;
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employees.Max(e => e.Id) + 1;
            _employees.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }

        public Employee Delete(int Id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == Id);
            if (employee != null)
                _employees.Remove(employee);
            return employee;
        }
    }
}
