using cypressIOProject.Models.Elements;
using cypressIOProject.Models.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;

namespace cypressIOProject
{
    public class TodoPageTest
    {
        public WebDriver driver;
        public TodoPage page;
        public const string TaskText = "newTodo";
        public const string NewTaskText = "new task text";

        [SetUp]
        public void Setup()
        {
            driver = new EdgeDriver();
            page = new TodoPage();
            driver.Navigate().GoToUrl("https://example.cypress.io/todo#/");
        }

        [Test,Order(1)]
        public void TestNormalOperations()
        {
            //driver.Navigate().GoToUrl("https://example.cypress.io/todo#/");
            var task = AddTask();
            EditTask(task);
            MarkTaskAsDone(task);
            MarkTaskAsNotDone(task);
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
            var checkboxValue = checkbox.GetAttribute("value").Equals("on",StringComparison.InvariantCultureIgnoreCase);
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
            foreach (var task in tasks)
            {
                EditTask(task);
            }

        }
        [Test, Order(3)]
        public void CheckAndUncheckAllTaskDone()
        {
            var tasks = driver.FindElements(page.TodoList);
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                var task = tasks[i];
                MarkTaskAsDone(task);
            }
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                var task = tasks[i];
                MarkTaskAsNotDone(task);
            }
        }


        [Test, Order(4)]
        public void DeleteAllTasks()
        {
            var tasks = driver.FindElements(page.TodoList);
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                var task = tasks[i];
                RemoveTask(task);
            }

        }
    }
}