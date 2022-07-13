using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cypressIOProject.Models.Pages
{
    public class TodoPage : MainPage
    {
        public By NewTodo { get; set; }
        public By TodoList { get; set; }
        public By AllSelector { get; set; }
        public By ActiveSelector { get; set; }
        public By CompletedSelector { get; set; }
        public By Edit { get; set; }
        public By Completed { get; set; }

        public TodoPage(): base()   
        {
            NewTodo = By.ClassName("new-todo");

            TodoList = By.ClassName("view");

            AllSelector = By.XPath("/html/body/section/div/footer/ul/li[1]/a");

            ActiveSelector = By.XPath("/html/body/section/div/footer/ul/li[2]/a");

            CompletedSelector = By.XPath("/html/body/section/div/footer/ul/li[3]/a");

            Edit = By.ClassName("edit");

            Completed = By.ClassName("completed");
        }
    }
}
