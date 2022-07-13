using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cypressIOProject.Models.Elements
{
    public class TodoElement
    {
        public static By CheckBox  = By.TagName("input");
        public static By Label = By.TagName("label");
        public static By Remove = By.ClassName("destroy");
    }
}
