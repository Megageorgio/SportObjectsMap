# Карта спортивных объектов РФ
![Изображение](https://github.com/megageorgio/SportObjectsMap/raw/master/preview.png)
* Для запуска приложения необходимо скачать билд в разделе с релизами данного репозитория. Предварительно требуется установка .NET 7 и nodejs, npm, работать должно как в ОС Windows, так и Linux. После скачивания и распаковки архива необходимо перейти в каталог, открыть консоль и вписать команду dotnet KorogodovMapApp.dll --urls http://0.0.0.0:80
* В качестве базы данных используется SQLite, но её можно достаточно быстро заменить на любую другую БД, для этого необходимо поставить соответствующий пакет nuget и изменить строку подключения в ApplicationContext
* Изображения отсутствуют, так как ссылки на них в наборе данных являются нерабочими, а поиск вручную займёт большое количество времени
* Проверить работу приложения можно на сайте: http://185.212.148.95/
