# DataTableServerSide.AspNetCore

Bu kütüphane, ASP.NET Core projelerinizde DataTables eklentisini kullanarak sunucu taraflı tablo işlemlerini kolaylaştırmak için tasarlanmıştır.


## Nasıl Kullanılır

1. **Kurulum :**

  - NuGet üzerinden kütüphaneyi projenize ekleyin.

    ```bash
    dotnet add package DataTableServerSide.AspNetCore
    ```

2. **Model oluşturma :**

  - Model sınıfınızı oluşturun.

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
3. **Controller'da Kullanım**
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

4. **İstemci tarafında DataTable Ayarları**

   HTML sayfanızda DataTable'ı aşağıdaki gibi yapılandırın:

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
    JavaScript ile DataTable'ı yapılandırın:
    
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

5. **Örnek Kullanım**

    - `PersonModel` sınıfınıza uygun bir model oluşturun.
    - Controller'daki `GetDataTable` yöntemine uygun veriyi sağlayın.
    - DataTable'ı ilgili HTML sayfanıza entegre edin ve JavaScript ile yapılandırın.

6. **Kendi Filtreleme ve Sıralama Mantığı**

    Kütüphane, verileri filtreleme ve sıralama için özelleştirmenize olanak tanır. `ApplyFilters` ve `ApplySorting` yöntemlerini düzenleyerek kendi mantığınızı ekleyebilirsiniz.

## Katkıda Bulunma

Eğer bir sorun bulursanız veya katkıda bulunmak istiyorsanız, lütfen bir [sorun](https://github.com/aliyavuztt/DataTableServerSide.AspNetCore/issues) açın veya bir pull talebi gönderin.

## Proje Demosu

Proje Demosu'nu [inceleyebilirsiniz](https://github.com/aliyavuztt/ServerSideTable.Demo)

## Lisans

Bu proje [MIT lisansı](LICENSE) altında lisanslanmıştır. Detaylı bilgi için lisans dosyasını inceleyebilirsiniz.
