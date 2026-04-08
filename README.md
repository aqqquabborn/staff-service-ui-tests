# UI-автотесты staff-service

UI-автотесты на **C# + Selenium + NUnit** для учебного тестового стенда Staff — [staff-testing.testkontur.ru](https://staff-testing.testkontur.ru/).

## Описание

Набор из 5 UI-автотестов, покрывающих базовые сценарии работы с тестовым стендом Staff: авторизацию, навигацию по разделам и поиск сотрудников.

## Тесты

| # | Тест | Описание |
|---|------|----------|
| 1 | `Authorization_ValidCredentials_RedirectsToNewsPage` | Успешный вход с валидными данными |
| 2 | `Authorization_InvalidPassword_ShowsErrorMessage` | Вход с неверным паролем показывает сообщение об ошибке |
| 3 | `SideMenuNavigation_CommunityItemClick_OpensCommunitiesPage` | Переход в раздел «Сообщества» через главное меню |
| 4 | `Search_EmployeeByLastName_EmployeeAppearsInResults` | Поиск сотрудника через строку поиска в шапке |
| 5 | `MainMenuNavigation_EventsItemClick_OpensEventsPage` | Переход в раздел «Мероприятия» через главное меню |

## 🛠 Требования

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Google Chrome](https://www.google.com/chrome/)

## Запуск

Логин и пароль читаются из переменных окружения `STAFF_LOGIN` и `STAFF_PASSWORD`. Это учётные данные тестового учебного стенда — **в репозитории их нет**.

### Linux / macOS

```bash
export STAFF_LOGIN="your_login"
export STAFF_PASSWORD="your_password"
dotnet test
```

### Windows (PowerShell)

```powershell
$env:STAFF_LOGIN="your_login"
$env:STAFF_PASSWORD="your_password"
dotnet test
```

## Стек

- **C#** 
- **Selenium WebDriver** 
- **NUnit** 
- **Google Chrome** 
