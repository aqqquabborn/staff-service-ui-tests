using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace _1_test
{
    public class Tests
    {
        private IWebDriver driver;
        private const string BaseUrl = "https://staff-testing.testkontur.ru/";
        private static readonly string Login =
            Environment.GetEnvironmentVariable("STAFF_LOGIN")
            ?? throw new InvalidOperationException(
                "Переменная окружения STAFF_LOGIN не задана. См. README.md");
        private static readonly string Password =
            Environment.GetEnvironmentVariable("STAFF_PASSWORD")
            ?? throw new InvalidOperationException(
                "Переменная окружения STAFF_PASSWORD не задана. См. README.md");

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl(BaseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
    
        // Заполнение формы логина и клик по кнопке "Войти".
        // Используется и в позитивном, и в негативном сценариях авторизации.
        private void FillLoginForm(string login, string password)
        {
            driver.FindElement(By.Id("Username")).SendKeys(login);
            driver.FindElement(By.Id("Password")).SendKeys(password);
            driver.FindElement(By.CssSelector("#login_form button[value='login']")).Click();
        }

        // Полная авторизация валидными данными — заполнение формы + ожидание редиректа.
        // Используется как предусловие во всех тестах, где нужен залогиненный пользователь.
        private void Authorize()
        {
            FillLoginForm(Login, Password);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlToBe(BaseUrl + "news"));
        }
        [Test]
        public void Authorization_ValidCredentials_RedirectsToNewsPage()
        {
            Authorize();

            Assert.That(
                driver.Url,
                Is.EqualTo(BaseUrl + "news"),
                $"После успешной авторизации ожидался редирект на {BaseUrl}news, но текущий URL: {driver.Url}"
            );
        }
        [Test]
        public void SideMenuNavigation_CommunityItemClick_OpensCommunitiesPage()
        {
            Authorize();

            // кликаем по пункту "Сообщества" в главном меню
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var communityLink = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector("[data-tid='PageMainMenu'] [data-tid='Community']")
            ));
            communityLink.Click();

            // проверяем, что перешли на страницу сообществ
            wait.Until(ExpectedConditions.UrlToBe(BaseUrl + "communities"));

            Assert.That(
                driver.Url,
                Is.EqualTo(BaseUrl + "communities"),
                $"После клика по пункту 'Сообщества' в главном меню ожидался переход на {BaseUrl}communities, но текущий URL: {driver.Url}"
            );
        }
        [Test]
        public void Search_EmployeeByLastName_EmployeeAppearsInResults()
        {
            const string employeeFullName = "Киселев Егор Алексеевич";
            const string searchQuery = "Киселев";

            Authorize();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // открываем строку поиска
            wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector("[data-tid='SearchBar']")
            )).Click();

            // вводим фамилию в появившийся input
            var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("[data-tid='SearchBar'] input[type='text']")
            ));
            searchInput.SendKeys(searchQuery);

            // ждём появления искомого сотрудника в выпадающем списке результатов
            var employeeLocator = By.CssSelector(
                $"[data-tid='ComboBoxMenu__item'] [title='{employeeFullName}']"
            );

            var found = wait.Until(ExpectedConditions.ElementIsVisible(employeeLocator));

            Assert.That(
                found.Displayed,
                Is.True,
                $"В результатах поиска по запросу '{searchQuery}' не найден сотрудник '{employeeFullName}'"
            );
        }
        [Test]
        public void Authorization_InvalidPassword_ShowsErrorMessage()
        {
            const string invalidPassword = "wrong_password_123";
            const string expectedErrorText = "Неверный логин или пароль";

            FillLoginForm(Login, invalidPassword);

            // ждём появления сообщения об ошибке на форме логина
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var errorBlock = wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("[data-valmsg-summary='true']")
            ));

            // проверяем, что редиректа на страницу новостей НЕ произошло
            Assert.That(
                driver.Url,
                Does.Not.Contain("/news"),
                $"При вводе неверного пароля не ожидался редирект на страницу новостей, но текущий URL: {driver.Url}"
            );

            // проверяем текст сообщения об ошибке
            Assert.That(
                errorBlock.Text,
                Does.Contain(expectedErrorText),
                $"Ожидалось сообщение об ошибке '{expectedErrorText}', но фактический текст: '{errorBlock.Text}'"
            );
        }
                [Test]
        public void MainMenuNavigation_EventsItemClick_OpensEventsPage()
        {
            Authorize();

            // кликаем по пункту "Мероприятия" в главном меню
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var eventsLink = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector("[data-tid='PageMainMenu'] [data-tid='Events']")
            ));
            eventsLink.Click();

            // проверяем, что перешли на страницу мероприятий
            wait.Until(ExpectedConditions.UrlToBe(BaseUrl + "events"));

            Assert.That(
                driver.Url,
                Is.EqualTo(BaseUrl + "events"),
                $"После клика по пункту 'Мероприятия' в главном меню ожидался переход на {BaseUrl}events, но текущий URL: {driver.Url}"
            );
        }
    }
}