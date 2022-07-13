using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using cypressIOProject.Models.Elements;
using cypressIOProject.Models.Pages;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;

namespace cypressIOProject
{
    [TestFixture]
    public class TodoPageTest
    {
        public WebDriver driver;
        public TodoPage page;
        public const string TaskText = "newTodo";
        public const string NewTaskText = "new task text";

        //Instance of extents reports

        public ExtentReports extent;
        public ExtentHtmlReporter reporter;
        public ExtentTest test;
        [SetUp]
        public void Setup()
        {
            driver = new EdgeDriver();
            page = new TodoPage();
            driver.Navigate().GoToUrl("https://example.cypress.io/todo#/");
        }

        [OneTimeSetUp]
        public void OnTimeSetup()
        {
            //To obtain the current solution path/project path
            var directory = Directory.GetCurrentDirectory();
            directory = directory.Remove(directory.IndexOf("bin"));
            directory += "Report";
            extent = new ExtentReports();
            //Add reporter
            reporter = new ExtentHtmlReporter(directory+"\\");

            reporter.Config.CSS = "css-string";
            reporter.Config.DocumentTitle = "cypress IO";
            reporter.Config.ReportName = nameof(TodoPageTest);
            reporter.Config.EnableTimeline = true;
            reporter.Config.Encoding = "utf-8";
            reporter.Config.JS = "js-string";
            reporter.Config.Theme = Theme.Dark;

            extent.AttachReporter(reporter);

            //Add QA system info 
            extent.AddSystemInfo("Host Name", ".net");
            extent.AddSystemInfo("Environment", "QA");
            extent.AddSystemInfo("Username", "Iyad");


        }

        [TearDown]
        public void TestEnding()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
            {
                test.Log(Status.Pass, "passed");
                extent.Flush();
            }
            else
            {
                test.Log(Status.Fail, "failed");
                extent.Flush();
            }
        }

        [Test, Order(1)]
        public void TestNormalOperations()
        {
            test = extent.CreateTest(nameof(TestNormalOperations));
            //driver.Navigate().GoToUrl("https://example.cypress.io/todo#/");
            test.CreateNode("Add Task");
            var task = AddTask();
            test.CreateNode("Edit Task");
            EditTask(task);
            test.CreateNode("set Task as done");
            MarkTaskAsDone(task);
            test.CreateNode("set Task as not done");
            MarkTaskAsNotDone(task);
            test.CreateNode("Remove Task");
            RemoveTask(task);
            driver.Quit();

        }
        public IWebElement AddTask()
        {
            var tasks = driver.FindElements(page.TodoList);
            int numberOfTasks = tasks.Count;

            var newTodo = driver.FindElement(page.NewTodo);
            newTodo.Click();
            newTodo.SendKeys(TaskText);

            tasks.First().Click();

            tasks = driver.FindElements(page.TodoList);
            var task = tasks.Last();

            Assert.IsTrue(task.FindElement(TodoElement.Label).Text.Equals(TaskText) && numberOfTasks + 1 == tasks.Count);
            return task;
        }

        public void EditTask(IWebElement task)
        {
            var tasks = driver.FindElements(page.TodoList);
            int numberOfTasks = tasks.Count;
            var label = task.FindElement(TodoElement.Label);

            Actions action = new Actions(driver);
            action.MoveToElement(task).DoubleClick().Perform();

            var edit = driver.FindElement(page.Edit);
            edit.SendKeys(Keys.Control + "a");
            edit.SendKeys(Keys.Delete);
            edit.SendKeys(NewTaskText);
            driver.FindElement(By.XPath("/html/body/section/div/header/h1")).Click();

            tasks = driver.FindElements(page.TodoList);
            Assert.IsTrue(task.FindElement(TodoElement.Label).Text.Equals(NewTaskText) && numberOfTasks == tasks.Count);
        }
        public void RemoveTask(IWebElement task)
        {
            var tasks = driver.FindElements(page.TodoList);
            int numberOfTasks = tasks.Count;
            var text = task.FindElement(TodoElement.Label).Text;
            task.Click();
            task.FindElement(TodoElement.Remove).Click();
            tasks = driver.FindElements(page.TodoList);
            Assert.IsTrue(numberOfTasks - 1 == tasks.Count());
            foreach (var t in tasks)
            {
                if (t.FindElement(TodoElement.Label).Text.Equals(text))
                {
                    Assert.Fail();
                }
            }
        }
        public void MarkTaskAsDone(IWebElement task)
        {
            var checkbox = task.FindElement(TodoElement.CheckBox);
            checkbox.Click();
            var checkboxValue = checkbox.GetAttribute("value").Equals("on", StringComparison.InvariantCultureIgnoreCase);
            var completedTasks = driver.FindElements(page.Completed);
            foreach (var t in completedTasks)
            {
                var completedTask = t.FindElement(page.TodoList);
                if (completedTask.FindElement(TodoElement.Label).Text.Equals(task.FindElement(TodoElement.Label).Text))
                {
                    break;
                }
            }
            Assert.IsTrue(checkboxValue);
        }
        public void MarkTaskAsNotDone(IWebElement task)
        {
            var checkbox = task.FindElement(TodoElement.CheckBox);
            var completedTasks = driver.FindElements(page.Completed);
            IWebElement completedTask = null;
            foreach (var t in completedTasks)
            {
                completedTask = t.FindElement(page.TodoList);
                if (completedTask.FindElement(TodoElement.Label).Text.Equals(task.FindElement(TodoElement.Label).Text))
                {
                    break;
                }
            }
            checkbox.Click();
            var checkboxValue = checkbox.GetAttribute("value").Equals("on", StringComparison.InvariantCultureIgnoreCase);
            Assert.IsFalse(checkboxValue && !(completedTask.FindElement(TodoElement.Label).Text.Equals(task.FindElement(TodoElement.Label).Text)));
        }

        [Test, Order(2)]
        public void EditAllTasks()
        {
            var tasks = driver.FindElements(page.TodoList);
            test = extent.CreateTest(nameof(EditAllTasks));

            foreach (var task in tasks)
            {
                var oldText = task.FindElement(TodoElement.Label).Text;
                EditTask(task);
                test.CreateNode($"Task text changed: {oldText}-> {task.FindElement(TodoElement.Label).Text}");
            }

        }
        [Test, Order(3)]
        public void CheckAndUncheckAllTaskDone()
        {
            test = extent.CreateTest(nameof(CheckAndUncheckAllTaskDone));
            var tasks = driver.FindElements(page.TodoList);
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                var task = tasks[i];
                var text = task.FindElement(TodoElement.Label).Text;

                MarkTaskAsDone(task);
                test.CreateNode($"task {text} checked");
            }
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                var task = tasks[i];
                var text = task.FindElement(TodoElement.Label).Text;
                MarkTaskAsNotDone(task);
                test.CreateNode($"task {text} unchecked");

            }
        }


        [Test, Order(4)]
        public void DeleteAllTasks()
        {
            test = extent.CreateTest(nameof(DeleteAllTasks));
            var tasks = driver.FindElements(page.TodoList);
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                var task = tasks[i];
                var text = task.FindElement(TodoElement.Label).Text;
                RemoveTask(task);
                test.CreateNode($"task {text} deleted");
            }

        }
    }
}