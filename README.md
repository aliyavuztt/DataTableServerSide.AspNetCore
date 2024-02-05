# DataTableServerSide.AspNetCore

This library is designed to facilitate server-side table operations using the DataTables plugin in your ASP.NET Core projects.


## Nasıl Kullanılır

1. **Installation**

  - NuGet üzerinden kütüphaneyi projenize ekleyin.

    ```bash
    dotnet add package DataTableServerSide.AspNetCore
    ```

2. **Model Creation**

  - Create your model class.

    ```csharp
    public class PersonModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
    }
    ```
3. **Usage in Controller**
    ```csharp
    using Bogus;
    using DataTableServerSide.AspNetCore;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPersons()
        {
            var personList = Persons();
            var dataTableHelper = new DataTableHelper<YourModel>(data, Request);
            var result = dataTableHelper.GetDataTableResult();
            return Json(result);
        }

        public List<PersonModel> Persons()
        {
            List<PersonModel> persons = new();

            var faker = new Faker();

            for (int i = 0; i < 10000; i++)
            {
                PersonModel person = new()
                {
                    Id = i + 1,
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    Phone = faker.Phone.PhoneNumberFormat(),
                    Mail = faker.Internet.Email(),
                    Address = faker.Address.FullAddress(),
                    Age = faker.Random.Number(18, 80)

                };
                persons.Add(person);
            }

            return persons;
        }
    }
    ```

4. **DataTable Settings on the Client Side**

   Configure DataTable on your HTML page as follows:

    ```html
    <div class="mb-5 mt-5">
        <h2>Person Lists</h2>
        <hr />
        <table id="myTable" class="table">
            <thead>
                <tr>
                    <th scope="col">#</th>
                    <th scope="col">FirstName</th>
                    <th scope="col">LastName</th>
                    <th scope="col">Phone</th>
                    <th scope="col">Email</th>
                    <th scope="col">Address</th>
                    <th scope="col">Age</th>
                </tr>
            </thead>
        </table>
    </div>
    ```
    Configure DataTable with JavaScript:
    
    ```javascript
    $(document).ready(function () {
       $(document).ready(function () {
        var table = $('#myTable').dataTable({
            lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
            pageLength: 10,
            processing: true,
            serverSide: true,
            filter: true,
            ordering: true,
            ajax: {
                url: "/Home/GetPersons",
                type: "POST",
                datatype: "json"
            },
            columns: [
                { data: "id", name: "id" },
                { data: "firstName", name: "firstName" },
                { data: "lastName", name: "lastName" },
                { data: "phone", name: "phone" },
                { data: "mail", name: "mail" },
                { data: "address", name: "address" },
                { data: "age", name: "age" }
            ]
        });
    ```

5. **Example Usage**

    - Create a model that fits your `PersonModel` class.
    - Provide the appropriate data for the `GetDataTable` method in the Controller..
    - Integrate DataTable into your relevant HTML page and configure it with JavaScript.

6. **Custom Filtering and Sorting Logic**

    The library allows you to customize data filtering and sorting. You can add your logic by modifying the `ApplyFilters` and `ApplySorting` methods.

## Contributing
If you find an issue or want to contribute, please open an [issue](https://github.com/aliyavuztt/DataTableServerSide.AspNetCore/issues) or submit a pull request.

## Project Demo

You can check out the [Project Demo](https://github.com/aliyavuztt/ServerSideTable.Demo).

## License

This project is licensed under the [MIT lisansı](LICENSE). You can review the license file for detailed information.