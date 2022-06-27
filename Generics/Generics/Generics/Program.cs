using Generics;
using Generics.Data;
using Generics.Entities;
using Generics.Repositories;
using Generics.SpecialCases;

static void GenericMathOperators()
{
    var result = Add(2, 3);
    Console.WriteLine($"2+3={result}");

    var result2 = Add(2.7, 3.3);
    Console.WriteLine($"2+3={result2}");
}

static T Add<T>(T x, T y) where T: notnull
{
    dynamic a = x;
    dynamic b = y;
    return a + b;
}

static void UsingStaticinGenericClasses()
{
    _ = new Container<string>();
    _ = new Container<string>();
    var container = new Container<int>();

    Console.WriteLine($"Container<string>: {Container<string>.InstanceCount}");
    Console.WriteLine($"Container<int>: {Container<int>.InstanceCount}");
    Console.WriteLine($"Container<bool>: {Container<bool>.InstanceCount}");
    Console.WriteLine($"Container<T>: {ContainerBase.InstanceCountBase}");

    container.PrintItem<string>("Hello from generic method in generic class");

    Console.ReadLine();
}

static void ImplementGenericDbRepositoryWithInterfaces_Event()
{

    var employeeRepository = new SqlRepository<Employee>(new StorageAppDbContext());
    employeeRepository.ItemAdded += EmployeeAdded_ItemAdded;

    AddEmployees(employeeRepository);
    AddManagers(employeeRepository);
    GetEmployeeById(employeeRepository);
    WriteAllToConsole(employeeRepository);


    var organizationRepository = new ListRepository<Organization>();
    AddOrganizations(organizationRepository);
    WriteAllToConsole(organizationRepository);

    Console.ReadLine();
}

static void EmployeeAdded_ItemAdded(object? sender, Employee e)
{
    Console.WriteLine($"Employee Added => {e.FirstName}");
}

/*
static void ImplementGenericDbRepositoryWithInterfaces_Delegate()
{
    //var itemAdded = new ItemAdded<Employee>(EmployeeAdded); // same as following
    //ItemAdded<Employee> itemAdded = EmployeeAdded;
    //var employeeRepository = new SqlRepository<Employee>(new StorageAppDbContext(), itemAdded);

    var employeeRepository = new SqlRepository<Employee>(new StorageAppDbContext(), EmployeeAdded);

    AddEmployees(employeeRepository);
    AddManagers(employeeRepository);
    GetEmployeeById(employeeRepository);
    WriteAllToConsole(employeeRepository);

    // Contravariance usage
    //IWriteRepository<Manager> repo = new SqlRepository<Employee>(new StorageAppDbContext());
    //repo.Add(new Manager());

    var organizationRepository = new ListRepository<Organization>();
    AddOrganizations(organizationRepository);
    WriteAllToConsole(organizationRepository);

    // Covariance usage
    //IReadRepository<IEntity> repo = new ListRepository<Organization>();

    Console.ReadLine();
}
*/

static void EmployeeAdded(Employee item)
{
    Console.WriteLine($"Employee Added => {item.FirstName}");
}

// Contravariance
static void AddManagers(IWriteRepository<Manager> managerRepository)
{
    var wesleyManager = new Manager { FirstName = "Wesley" };
    var wesleyManagerCopy = wesleyManager.Copy();
    managerRepository.Add(wesleyManager);

    if(wesleyManagerCopy is not null)
    {
        wesleyManagerCopy.FirstName += "_Copy";
        managerRepository.Add(wesleyManagerCopy);
    }

    var rogerManager = new Manager { FirstName = "Roger" };
    managerRepository.Add(rogerManager);

    managerRepository.Save();
}

static void WriteAllToConsole(IReadRepository<IEntity> repository)
{
    var items = repository.GetAll();
    foreach(var item in items)
    {
        Console.WriteLine(item);
    }
}

static void GetEmployeeById(IRepository<Employee> employeeRepository)
{
    var employee = employeeRepository.GetById(2);
    Console.WriteLine($"Employee with Id 2: {employee.FirstName}");
}

static void AddEmployees(IRepository<Employee> employeeRepository)
{
    var employees = new[]
    {
        new Employee { FirstName = "Shubham" },
        new Employee { FirstName = "Shukla" },
        new Employee { FirstName = "Maddy" },
        new Employee { FirstName = "Burns" }
    };

    employeeRepository.AddBatch(employees);
}

static void AddOrganizations(IRepository<Organization> organizationRepository)
{
    var organizations = new[]
    {
        new Organization { Name = "Citrix"},
        new Organization { Name = "Google" },
        new Organization { Name = "Apple" },
        new Organization { Name = "Sony" }
    };

    organizationRepository.AddBatch(organizations);
}

//static void AddBatch<T>(IWriteRepository<T> repository, T[] items)
//    where T : IEntity
//{
//    foreach(var item in items)
//    {
//        repository.Add(item);
//    }
//    repository.Save();
//}

static void StackDoubles()
{
    var stack = new GenericStack<double>();
    stack.Push(1.2);
    stack.Push(2.8);
    stack.Push(3.0);

    double sum = 0.0;

    while(stack.Count > 0)
    {
        double item = stack.Pop();
        Console.WriteLine($"Item: {item}");
        sum += item;
    }

    Console.WriteLine($"Sum: {sum}");
}

static void StackStrings()
{
    var stack = new GenericStack<string>();
    stack.Push("Shubham");
    stack.Push("Shukla");

    while (stack.Count > 0)
    {
        string item = stack.Pop();
        Console.WriteLine($"Item: {item}");
    }

}




GenericMathOperators();