# UI-автотесты staff-service

5 UI-автотестов на C# + Selenium + NUnit для учебного тестового стенда Staff (https://staff-testing.testkontur.ru/).

## Тесты

1. `Authorization_ValidCredentials_RedirectsToNewsPage` — успешный вход с валидными данными
2. `Authorization_InvalidPassword_ShowsErrorMessage` — вход с неверным паролем показывает ошибку
3. `SideMenuNavigation_CommunityItemClick_OpensCommunitiesPage` — клик по пункту "Сообщества" в меню
4. `Search_EmployeeByLastName_EmployeeAppearsInResults` — поиск сотрудника через строку поиска
5. `Logout_AuthorizedUser_RedirectsToLogoutPage` — выход из системы через меню профиля

## Запуск

Логин и пароль читаются из переменных окружения `STAFF_LOGIN` и `STAFF_PASSWORD`. Это учётные данные тестового учебного стенда, в репозитории их нет.

```bash
export STAFF_LOGIN="your_login"
export STAFF_PASSWORD="your_password"
dotnet test
```

Требования: .NET SDK, Google Chrome.
