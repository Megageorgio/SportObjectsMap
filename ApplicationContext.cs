using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CsvHelper;
using System.Globalization;
using KorogodovMapApp.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Xml.Linq;
using System;

namespace KorogodovMapApp;

public sealed class ApplicationContext : DbContext
{
    public DbSet<SportObject> SportObjects => Set<SportObject>();
    public DbSet<SportObjectDetail> SportObjectDetails => Set<SportObjectDetail>();
    public DbSet<SportObjectType> SportObjectTypes => Set<SportObjectType>();
    public DbSet<SportType> SportTypes => Set<SportType>();
    public DbSet<Curator> Curators => Set<Curator>();

    public ApplicationContext()
    {
        Database.EnsureCreated();

        if (!SportObjects.Any())
        {
            InitData();
        }
    }

    private void InitData()
    {
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
                csv.TryGetField<string>("ОКТМО:", out var oktmo);
                csv.TryGetField<int?>("Общий объём финансирования:", out var totalFunding);

                csv.TryGetField<string>("Курирующий орган:", out var curatorName);
                csv.TryGetField<string>("Телефон курирующего органа:", out var curatorPhoneNumber);

                var sportObjectType = GetOrCreateSportObjectType(sportObjectTypeName);

                var sportObjectDetail = CreateSportObjectDetail(shortDescription, additionalDescription, email,
                    phoneNumber, address, city, federalSubject, municipalDistrict, actionStartDate, actionEndDate,
                    action, oktmo, totalFunding);

                var sportObject = CreateSportObject(id, name, sportObjectType, sportObjectDetail, x, y, url, workingHoursMondayToFriday, workingHoursSaturday, workingHoursSunday, isActive);

                var curator = GetOrCreateCurator(curatorName, curatorPhoneNumber);

                sportObject.Curator = curator;

                foreach (var sportTypeName in sportTypeNames.Split(',',
                             StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var sportType = GetOrCreateSportType(sportTypeName);
                    sportObject.SportTypes.Add(sportType);
                }

                SaveChanges();
            }
        }
    }

    private SportObjectType GetOrCreateSportObjectType(string sportObjectTypeName)
    {
        if (string.IsNullOrEmpty(sportObjectTypeName))
        {
            sportObjectTypeName = "Иное";
        }

        var sportObjectType = SportObjectTypes.FirstOrDefault(type => type.Name == sportObjectTypeName);

        if (sportObjectType == null)
        {
            sportObjectType = new SportObjectType
            {
                Name = sportObjectTypeName,
                Icon = sportObjectTypeName switch
                {
                    "бассейн" => "Pool",
                    "зал спортивный" => "Sport",
                    "многофункциональный спортивный комплекс" => "Star",
                    "стадион" or "манеж легкоатлетический" => "Run",
                    "комплекс горнолыжный" => "Mountain",
                    "канал гребной" or "канал для гребного слалома" => "WaterPark",
                    "велотрек" => "Bicycle2",
                    "трасса спортивная" => "Auto",
                    "Иное" => "Dot",
                    _ => string.Empty
                }
            };
            SportObjectTypes.Add(sportObjectType);
            SaveChanges();
        }

        return sportObjectType;
    }

    private SportObjectDetail CreateSportObjectDetail(string shortDescription, string additionalDescription,
        string email,
        string phoneNumber, string address, string city, string federalSubject, string municipalDistrict,
        DateTime? actionStartDate, DateTime? actionEndDate, string action, string oktmo, int? totalFunding)
    {
        if (actionStartDate?.Date.Year < 1000)
        {
            actionStartDate = actionStartDate.Value.Date.AddYears(2000);
        }

        if (actionEndDate?.Date.Year < 1000)
        {
            actionEndDate = actionEndDate.Value.Date.AddYears(2000);
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
            IsReconstruction = (action == "реконструкция"),
            OKTMO = oktmo,
            TotalFunding = totalFunding
        };

        SportObjectDetails.Add(sportObjectDetail);

        return sportObjectDetail;
    }

    private SportObject CreateSportObject(int id, string name, SportObjectType sportObjectType,
        SportObjectDetail sportObjectDetail, float x, float y, string url, string workingHoursMondayToFriday,
        string workingHoursSaturday, string workingHoursSunday, string isActive)
    {
        if (!string.IsNullOrEmpty(url) && !url.StartsWith("http"))
        {
            url = "http://" + url;
        }

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

        return sportObject;
    }

    private SportType GetOrCreateSportType(string sportTypeName)
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

        return sportType;
    }

    private Curator GetOrCreateCurator(string curatorName, string curatorPhoneNumber)
    {
        if (string.IsNullOrEmpty(curatorName))
        {
            return null;
        }

        var curator = Curators.FirstOrDefault(curator => curator.Name == curatorName);
        if (curator == null)
        {
            curator = new Curator
            {
                Name = curatorName,
                PhoneNumber = curatorPhoneNumber
            };
            Curators.Add(curator);
            SaveChanges();
        }

        return curator;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
}
