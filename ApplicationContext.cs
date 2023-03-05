using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CsvHelper;
using System.Globalization;
using KorogodovMapApp.Models;

namespace KorogodovMapApp;

public sealed class ApplicationContext : DbContext
{
    public DbSet<SportObject> SportObjects => Set<SportObject>();
    public DbSet<SportObjectDetail> SportObjectDetails => Set<SportObjectDetail>();
    public DbSet<SportObjectType> SportObjectTypes => Set<SportObjectType>();
    public DbSet<SportType> SportTypes => Set<SportType>();


    public ApplicationContext()
    {
        Database.EnsureCreated();

        if (SportObjects.Any())
        {
            return;
        }

        using var reader = new StreamReader(@"data.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            if (csv.TryGetField<int>("id:", out var id) &&
                csv.TryGetField<string>("Название:", out var name) &&
                csv.TryGetField<float>("Яндекс координата объекта X:", out var x) &&
                csv.TryGetField<float>("Яндекс координата объекта Y:", out var y) &&
                csv.TryGetField<string>("Активный:", out var isActive))
            {
                csv.TryGetField<string>("Тип спортивного комплекса:", out var sportObjectTypeName);
                csv.TryGetField<string>("Виды спорта:", out var sportTypeNames);
                csv.TryGetField<string>("URL сайта:", out var url);
                csv.TryGetField<string>("Режим работы Пн.-Пт.:", out var workingHoursMondayToFriday);
                csv.TryGetField<string>("Режим работы Сб.:", out var workingHoursSaturday);
                csv.TryGetField<string>("Режим работы Вс.:", out var workingHoursSunday);

                csv.TryGetField<string>("Краткое описание:", out var shortDescription);
                csv.TryGetField<string>("Детальное описание:", out var additionalDescription);
                csv.TryGetField<string>("E-mail:", out var email);
                csv.TryGetField<string>("Контактный телефон объекта:", out var phoneNumber);
                csv.TryGetField<string>("МО:", out var municipalDistrict);
                csv.TryGetField<string>("Субъект федерации:", out var federalSubject);
                csv.TryGetField<string>("Населённый пункт:", out var city);
                csv.TryGetField<string>("Адрес:", out var address);
                csv.TryGetField<string>("Действия с объектом:", out var action);
                csv.TryGetField<DateTime?>("Дата начала строительства / реконструкции:", out var actionStartDate);
                csv.TryGetField<DateTime?>("Дата завершения строительства / реконструкции:", out var actionEndDate);


                if (string.IsNullOrEmpty(sportObjectTypeName))
                {
                    sportObjectTypeName = "Иное";
                }

                var sportObjectType = SportObjectTypes.FirstOrDefault(type => type.Name == sportObjectTypeName);

                if (sportObjectType == null)
                {
                    sportObjectType = new SportObjectType
                    {
                        Name = sportObjectTypeName
                    };
                    SportObjectTypes.Add(sportObjectType);
                    SaveChanges();
                }

                var sportObjectDetail = new SportObjectDetail
                {
                    ShortDescription = shortDescription,
                    AdditionalDescription = additionalDescription,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Address = address,
                    City = city,
                    FederalSubject = federalSubject,
                    MunicipalDistrict = municipalDistrict,
                    ActionStartDate = actionStartDate,
                    ActionEndDate = actionEndDate,
                    IsReconstruction = (action == "реконструкция")
                };

                SportObjectDetails.Add(sportObjectDetail);

                var sportObject = new SportObject
                {
                    Id = id,
                    Name = name,
                    SportObjectType = sportObjectType,
                    SportObjectDetail = sportObjectDetail,
                    X = x,
                    Y = y,
                    URL = url,
                    WorkingHoursMondayToFriday = workingHoursMondayToFriday,
                    WorkingHoursSaturday = workingHoursSaturday,
                    WorkingHoursSunday = workingHoursSunday,
                    IsActive = (isActive == "Y")
                };

                SportObjects.Add(sportObject);

                SaveChanges();

                foreach (var sportTypeName in sportTypeNames.Split(',',
                             StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var sportType = SportTypes.FirstOrDefault(type => type.Name == sportTypeName);
                    if (sportType == null)
                    {
                        sportType = new SportType
                        {
                            Name = sportTypeName
                        };
                        SportTypes.Add(sportType);
                        SaveChanges();
                    }
                    sportObject.SportTypes.Add(sportType);
                }

                SaveChanges();
            }
        }

        SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
}
