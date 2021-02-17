using System;
using System.Threading;
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
			bool live = false;
			List<TestStep> steps = new List<TestStep>();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify PVP Countdown Text")) {
				preview = driver.FindElement("xpath","//div[contains(@class,'pvp-expires')]/span").GetAttribute("innerText");
				
				if (preview.Contains(":59")) {
					time = "Preview Pass · 59:59";
				}
				else if (preview.Contains(":58")) {
					time = "Preview Pass · 59:58";
				}
				else if (preview.Contains(":57")) {
					time = "Preview Pass · 59:57";
				}
				else if (preview.Contains(":56")) {
					time = "Preview Pass · 59:56";
				}
				else if (preview.Contains(":55")) {
					time = "Preview Pass · 59:55";
				}
				else if (preview.Contains(":54")) {
					time = "Preview Pass · 59:54";
				}
				else if (preview.Contains(":53")) {
					time = "Preview Pass · 59:53";
				}
				else if (preview.Contains(":52")) {
					time = "Preview Pass · 59:52";
				}
				else if (preview.Contains(":51")) {
					time = "Preview Pass · 59:51";
				}
				else {
					time = step.Data;
				}
				steps.Add(new TestStep(order, "Verify PVP Countdown Text", time, "verify_value", "xpath", "//div[contains(@class,'pvp-expires')]/span", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}