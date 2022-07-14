using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using cypressIOProject.Controllers;
using cypressIOProject.Models.Elements;
using cypressIOProject.Models.Pages;
using cypressIOProject.Report;
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
        public const string TaskText = "newTodo";
        public const string NewTaskText = "new task text";
        public TodoPageController TodoPageController { get; set; }
        //Instance of extents reports

        public ExtentReports extent;
        public ExtentHtmlReporter reporter;
        public ExtentTest test;
        [SetUp]
        public void Setup()
        {
            driver = new EdgeDriver();
            driver.Navigate().GoToUrl("https://example.cypress.io/todo#/");
            TodoPageController = new TodoPageController(driver);

        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {

            extent = new ExtentReports();
            //Add reporter
            reporter = ReporterFactory.GetExtentHTMLReporter(nameof(TodoPageTest));

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
            driver.Quit();

        }

        [Test, Order(1)]
        public void TestNormalOperations()
        {
            test = extent.CreateTest(nameof(TestNormalOperations));
            //driver.Navigate().GoToUrl("https://example.cypress.io/todo#/");
            test.CreateNode("Add Task");
            var task = TodoPageController.AddTask(TaskText);
            test.CreateNode("Edit Task");
            TodoPageController.EditTask(task,NewTaskText);
            test.CreateNode("set Task as done");
            TodoPageController.MarkTaskAsDone(task);
            test.CreateNode("set Task as not done");
            TodoPageController.MarkTaskAsNotDone(task);
            test.CreateNode("Remove Task");
            TodoPageController.RemoveTask(task);

        }


        [Test, Order(2)]
        public void EditAllTasks()
        {
            var tasks = TodoPageController.GetTasksElements();
            test = extent.CreateTest(nameof(EditAllTasks));

            foreach (var task in tasks)
            {
                var oldText = task.FindElement(TodoElement.Label).Text;
                TodoPageController.EditTask(task,NewTaskText);
                test.CreateNode($"Task text changed: {oldText}-> {task.FindElement(TodoElement.Label).Text}");
            }

        }
        [Test, Order(3)]
        public void CheckAndUncheckAllTaskDone()
        {
            test = extent.CreateTest(nameof(CheckAndUncheckAllTaskDone));
            var tasks = TodoPageController.GetTasksElements();
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                var task = tasks[i];
                var text = task.FindElement(TodoElement.Label).Text;

                TodoPageController.MarkTaskAsDone(task);
                test.CreateNode($"task {text} checked");
            }
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                var task = tasks[i];
                var text = task.FindElement(TodoElement.Label).Text;
                TodoPageController.MarkTaskAsNotDone(task);
                test.CreateNode($"task {text} unchecked");

            }
        }


        [Test, Order(4)]
        public void DeleteAllTasks()
        {
            test = extent.CreateTest(nameof(DeleteAllTasks));
            var tasks = TodoPageController.GetTasksElements();
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                var task = tasks[i];
                var text = task.FindElement(TodoElement.Label).Text;
                TodoPageController.RemoveTask(task);
                test.CreateNode($"task {text} deleted");
            }

        }
    }
}