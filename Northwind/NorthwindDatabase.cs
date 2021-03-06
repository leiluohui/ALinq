using System;
using System.Data.Common;
using ALinq.Mapping;
using System.Reflection;
using ALinq;
using System.Data;

namespace NorthwindDemo
{
    //[DatabaseAttribute(Name = "Northwind")]
    public abstract partial class NorthwindDatabase : DataContext
    {
        protected NorthwindDatabase(DbConnection connection, Type providerType)
            : base(connection, providerType)
        {
        }

        protected NorthwindDatabase(string fileOrServerOrConnection, Type providerType)
            : base(fileOrServerOrConnection, providerType)
        {
        }

        public new virtual void CreateDatabase()
        {
            base.CreateDatabase();
            ImportData();
        }

        protected virtual void ImportData()
        {
            var data = new NorthwindData();

            this.Regions.InsertAllOnSubmit(data.regions);
            this.Employees.InsertAllOnSubmit(data.employees);

            this.Territories.InsertAllOnSubmit(data.territories);
            this.EmployeeTerritories.InsertAllOnSubmit(data.employeeTerritories);

            this.Customers.InsertAllOnSubmit(data.customers);
            this.Shippers.InsertAllOnSubmit(data.shippers);

            this.Categories.InsertAllOnSubmit(data.categories);
            this.Suppliers.InsertAllOnSubmit(data.suppliers);
            this.Products.InsertAllOnSubmit(data.products);

            this.Orders.InsertAllOnSubmit(data.orders);
            this.OrderDetails.InsertAllOnSubmit(data.orderDetails);
            ///**/
            this.SubmitChanges();
        }


        [Function(IsComposable = true)]
        public bool? AddUser([Parameter(DbType = null)] string name, [Parameter(DbType = null)] string email)
        {
            return ((bool?)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodBase.GetCurrentMethod())), name, email).ReturnValue));
        }

        [Function(IsComposable = true)]
        public object UpdateUser([Parameter(DbType = null)] int? id, [Parameter(DbType = null)] string name, [Parameter(DbType = null)] string email)
        {
            return ((object)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodBase.GetCurrentMethod())), id, name, email).ReturnValue));
        }

        [Function(IsComposable = true)]
        public object DeleteUser([Parameter(DbType = null)] int? ID)
        {
            return ((object)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodBase.GetCurrentMethod())), ID).ReturnValue));
        }

        [Function()]
        public int FindUser([Parameter()] string name)
        {
            ALinq.IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodBase.GetCurrentMethod())), name);
            return ((int)(result.ReturnValue));
        }



        [Function(Name = "dbo.GetEmployeeByID")]
        public ISingleResult<GetEmployeeByIDResult> GetEmployeeByID([Parameter(DbType = "Int")] System.Nullable<int> employeeID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), employeeID);
            return ((ISingleResult<GetEmployeeByIDResult>)(result.ReturnValue));
        }

        //存储过程
        [Function(Name = "dbo.AddCategory")]
        public void AddCategory(int categoryID, string categoryName, string description)
        {
            var result = ExecuteMethodCall(this, ((MethodInfo)(MethodBase.GetCurrentMethod())),
                categoryID, categoryName, description).ReturnValue;
            //return result == null ? 0 : (int)result;
        }

        [Function(Name = "dbo.AddCategory")]
        public int AddCategory1(int categoryID, string categoryName, string description)
        {
            var result = ExecuteMethodCall(this, ((MethodInfo)(MethodBase.GetCurrentMethod())),
                categoryID, categoryName, description).ReturnValue;
            return (int)result;
        }

        public int UpdateCategory(int categoryID, string name, string description)
        {
            var result = ExecuteMethodCall(this, ((MethodInfo)(MethodBase.GetCurrentMethod())),
                                           categoryID, name, description).ReturnValue;
            return result == null ? 0 : (int)result;
        }

        public int DeleteCategory(int categroyID)
        {
            var result = ExecuteMethodCall(this, ((MethodInfo)(MethodBase.GetCurrentMethod())),
                                           categroyID).ReturnValue;
            return result == null ? 0 : (int)result;
        }

        public partial class GetEmployeeByIDResult
        {
            public GetEmployeeByIDResult()
            {
            }

            [Column(DbType = "VarChar(20)", Name = "employee_lastName")]
            public string LastName
            {
                get;
                set;
            }

            [Column(DbType = "VarChar(10)", Name = "employee_firstName")]
            public string FirstName
            {
                get;
                set;
            }

            [Column(DbType = "VarChar(30)", Name = "employee_title")]
            public string Title
            {
                get;
                set;
            }

        }

        [Function(IsComposable = true)]
        public int Max(int value)
        {
            throw new NotImplementedException();
        }

        [Function(IsComposable = true)]
        public virtual DateTime Now()
        {
            throw new NotImplementedException();
        }
    }
}
