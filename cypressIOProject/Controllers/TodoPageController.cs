using cypressIOProject.Models.Elements;
using cypressIOProject.Models.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cypressIOProject.Controllers
{
    public class TodoPageController
    {
        private WebDriver driver;
        private TodoPage page;

        public TodoPageController(WebDriver webDriver)
        {
            this.driver = webDriver; 
            page = new TodoPage();
        }
        public IWebElement AddTask(string taskText)
        {
            var tasks = driver.FindElements(page.TodoList);
            int numberOfTasks = tasks.Count;

            var newTodo = driver.FindElement(page.NewTodo);
            newTodo.Click();
            newTodo.SendKeys(taskText);

            tasks.First().Click();

            tasks = driver.FindElements(page.TodoList);
            var task = tasks.Last();

            Assert.IsTrue(task.FindElement(TodoElement.Label).Text.Equals(taskText) && numberOfTasks + 1 == tasks.Count);
            return task;
        }

        public void EditTask(IWebElement task, string newTaskText)
        {
            var tasks = driver.FindElements(page.TodoList);
            int numberOfTasks = tasks.Count;
            var label = task.FindElement(TodoElement.Label);

            Actions action = new Actions(driver);
            action.MoveToElement(task).DoubleClick().Perform();

            var edit = driver.FindElement(page.Edit);
            edit.SendKeys(Keys.Control + "a");
            edit.SendKeys(Keys.Delete);
            edit.SendKeys(newTaskText);
            driver.FindElement(By.XPath("/html/body/section/div/header/h1")).Click();

            tasks = driver.FindElements(page.TodoList);
            Assert.IsTrue(task.FindElement(TodoElement.Label).Text.Equals(newTaskText) && numberOfTasks == tasks.Count);
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
        public List<IWebElement> GetTasksElements()
        {
            return driver.FindElements(page.TodoList).ToList();
        }
    }
}
