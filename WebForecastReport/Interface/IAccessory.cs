using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IAccessory
    {
        List<SaleModel> getSale();
        List<UserModel> getAllUser();
        string InsertCustomer(string customer);
        List<CustomerModel> getCustomers();
        string InsertEndUser(string enduser);
        List<EndUserModel> getEndUsers();
        List<DepartmentModel> getDepartment();

    }
}
