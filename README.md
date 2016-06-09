# FastSql
Light-weight, fast and easy to use C# library to retrieve, write (including bulk insert) and copy data in Microsoft SQL Server databases. There is support asynchronous operations.

## Samples:

1. Retrieving  entities

  ```csharp
  // simple command
  var q1 = SimpleCommand.ExecuteQuery<Employee>(connStr, "select * from Employee where CompanyID = @p0 and Age > @p1", 1, 40).ToArray();

  // mapped command with anonymous type
  var q2 = MappedCommand.ExecuteQuery<OtherEmployee, Employee>(connStr,
    "select * from Employee where CompanyID = @CompanyID and Age > @Age", new OtherEmployee() { CompanyID = 1, Age = 40 }).ToArray();
  ```  
  or asynchronously
  ```csharp
  // simple command
  var q1 = await SimpleCommand.ExecuteQueryAsync<Employee>(connStr, "select * from Employee where CompanyID = @p0 and Age > @p1", 1, 40).ToArray();

  // mapped command with anonymous type
  var q2 = await MappedCommand.ExecuteQueryAsync<OtherEmployee, Employee>(connStr,
    "select * from Employee where CompanyID = @CompanyID and Age > @Age", new OtherEmployee() { CompanyID = 1, Age = 40 }).ToArray();
  ```  

2. Retrieving anonimous entities

  ```csharp
  var proto = new { Company = default(string), Emp = default(string) };
  var q1 = SimpleCommand.ExecuteQueryAnonymous(proto, connStr, "select E.Name as Emp, C.Name as Company from Employee E join Company C on E.CompanyID = C.ID").ToArray();
  ```
  or asynchronously
  ```csharp
  var proto = new { Company = default(string), Emp = default(string) };
  var q1 = await SimpleCommand.ExecuteQueryAnonymousAsync(proto, connStr, "select E.Name as Emp, C.Name as Company from Employee E join Company C on E.CompanyID = C.ID").ToArray();
  ```
  

3. Bulk insert of entities to database

  ```csharp
  Employee[] newEmployees = new Employee[] { 
    new Employee() { CompanyID = 1, Name = "New Employee1", Age = 23, StartWorking = DateTime.UtcNow },
    new Employee() { CompanyID = 1, Name = "New Employee2", StartWorking = DateTime.UtcNow },
    new Employee() { CompanyID = 2, Name = "New Employee1" }
  };
  
  newEmployees.WriteToServer(connStr, "Employee");
  ```
or asynchronously
  ```csharp
  Employee[] newEmployees = new Employee[] { 
    new Employee() { CompanyID = 1, Name = "New Employee1", Age = 23, StartWorking = DateTime.UtcNow },
    new Employee() { CompanyID = 1, Name = "New Employee2", StartWorking = DateTime.UtcNow },
    new Employee() { CompanyID = 2, Name = "New Employee1" }
  };
  
  await newEmployees.WriteToServerAsync(connStr, "Employee");
  ```

More samples contains in Samples project. 


