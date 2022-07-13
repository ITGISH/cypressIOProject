using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cypressIOProject.Report
{
    public static class ReporterFactory
    {
        public static ExtentHtmlReporter GetExtentHTMLReporter()
        {
            //To obtain the current solution path/project path
            var directory = Directory.GetCurrentDirectory();
            directory = directory.Remove(directory.IndexOf("bin"));
            directory += "Report";

            var reporter = new ExtentHtmlReporter(directory + "\\");

            reporter.Config.CSS = "css-string";
            reporter.Config.DocumentTitle = "cypress IO";
            reporter.Config.ReportName = nameof(TodoPageTest);
            reporter.Config.EnableTimeline = true;
            reporter.Config.Encoding = "utf-8";
            reporter.Config.JS = "js-string";
            reporter.Config.Theme = Theme.Dark;

            return reporter;
        }
    }
}
