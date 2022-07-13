using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cypressIOProject.Models.Pages
{
    public class MainPage
    {
        public By MainPageLink { get; set; }
        public By CommandsDropDown { get; set; }
        public List<By> DropDownElements { get; set; }
        public By UtilitiesLink { get; set; }
        public By CypressAPILink { get; set; }
        public By GitHubLink { get; set; }

        public MainPage()
        {
            MainPageLink = By.ClassName("navbar-brand");

            CommandsDropDown = By.ClassName("dropdown-toggle");

            DropDownElements = new List<By>();
            for(int i = 0; i < 17; i++)
                DropDownElements.Add(By.XPath($"//*[@id=\"navbar\"]/ul[1]/li[1]/ul/li[{i+1}]/a"));

            UtilitiesLink = By.XPath("//*[@id=\"navbar\"]/ul[1]/li[2]/a");

            CypressAPILink = By.XPath("//*[@id=\"navbar\"]/ul[1]/li[3]/a");

            GitHubLink = By.XPath("//*[@id=\"navbar\"]/ul[2]/li/a");
        }
    }
}
