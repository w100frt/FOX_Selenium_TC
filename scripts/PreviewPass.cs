using System;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using log4net;
using System.Collections.ObjectModel;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		public void Execute(DriverManager driver, TestStep step)
		{
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
			IWebElement ele;
			int size = 0;
			string preview = "";
			string time = "";
			string path = "";
			bool live = false;
			List<TestStep> steps = new List<TestStep>();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify PVP Countdown Text")) {
				path = "//div[contains(@class,'pvp-expires')]/span";
				preview = driver.FindElement("xpath", path).GetAttribute("innerText");
				
				if (preview.Contains("59:")) {
					time = "Preview Pass · 59:5";
				}
				else {
					time = step.Data;
				}
				
				byte[] bytes = Encoding.Default.GetBytes(time);
				time = Encoding.UTF8.GetString(bytes);
				
				path = driver.FindElement("xpath", path).Text;
				
				steps.Add(new TestStep(order, "Verify PVP Countdown Text", time, "verify_value", "xpath", "substring("+ path +", '1', '19')", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}