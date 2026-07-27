В слое Infrastructure находятся все основные реализации, например:

- Взаимодействие с базой данных (реализация репозиториев).
- Контекст к базе данных (ApplicationDbContext).
- Вызов стороннего сервиса (реализация).

В слое Core находятся все основные сущности(Entity), классы, перечисления(Enum) и Dto.

В слое Application находится вся бизнес-логика.

Чтобы выполнить миграции нужно в терминале прописать:
dotnet ef migrations add InitialMigration --startup-project Start_Template_CSharp.Api --project Start_Template_CSharp.Infrastructure

Чтобы записать миграции в базу данных нужно в терминале прописать:
dotnet ef database update InitialMigration --startup-project Start_Template_CSharp.Api --project Start_Template_CSharp.Infrastructure
