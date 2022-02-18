using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IAccessory
    {
        List<string> GetDepartmentOfQuotation();
        List<SaleModel> getSale();
        List<UserModel> getAllUser();
        List<SaleModel> getUserQuotation();
        string InsertCustomer(string customer);
        List<CustomerModel> getCustomers();
        string InsertEndUser(string enduser);
        List<EndUserModel> getEndUsers();
        List<DepartmentModel> getDepartment();

    }
}
