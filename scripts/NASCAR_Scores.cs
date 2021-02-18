using System;
using System.Globalization;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;
using log4net;
using System.Threading;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{		
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public void Execute(DriverManager driver, TestStep step)
		{
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
			List<TestStep> steps = new List<TestStep>();
			IWebElement ele;
			DateTime today;
			string data = "";
			string xpath = "";
			VerifyError err = new VerifyError();
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();

			string[] nascarGroups = {"CUP SERIES", "GANDER RV & OUTDOORS TRUCK SERIES", "XFINITY SERIES"};
			
			if (step.Name.Equals("Verify Golf Groups")) {
				data = "//div[contains(@class,'scores-home-container')]//div[contains(@class,'active')]//ul";
				steps.Add(new TestStep(order, "Open Conference Dropdown", "", "click", "xpath", "//a[@class='dropdown-menu-title']", wait));
				steps.Add(new TestStep(order, "Verify Dropdown is Displayed", "", "verify_displayed", "xpath", data, wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				
				data = data + "//li";
				steps.Add(new TestStep(order, "Verify Number of Groups", "3", "verify_count", "xpath", data, wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				
				var groups = driver.FindElements("xpath", data); 
				for (int i = 0; i < groups.Count; i++) {
					if (nascarGroups[i].Equals(groups[i].GetAttribute("innerText"))) {
						log.Info("Success. " + nascarGroups[i] + " matches " + groups[i].GetAttribute("innerText"));
					}
					else {
						log.Error("***Verification FAILED. Expected data [" + nascarGroups[i] + "] does not match actual data [" + groups[i].GetAttribute("innerText") + "] ***");
						err.CreateVerificationError(step, nascarGroups[i], groups[i].GetAttribute("innerText"));
					}
				}
			}
			
			else if (step.Name.Equals("Determine Current Race")) {
				today = DateTime.Today;
				// determine week of season by today's date and time
				if (today >= DateTime.Parse("02/01/2021") && today < DateTime.Parse("02/09/2021 11:00:00")) {
					data = "4469";
				}
				else if (today >= DateTime.Parse("02/09/2021 11:00:01") && today < DateTime.Parse("02/15/2021 11:00:00")) {
					data = "4431";
				}
				else if (today >= DateTime.Parse("02/15/2021 11:00:01") && today < DateTime.Parse("02/22/2021 11:00:00")) {
					data = "4503";
				}
				else if (today >= DateTime.Parse("02/22/2021 11:00:01") && today < DateTime.Parse("03/01/2021 11:00:00")) {
					data = "4465";
				}
				else if (today >= DateTime.Parse("03/01/2021 11:00:01") && today < DateTime.Parse("03/08/2021 11:00:00")) {
					data = "4437";
				}
				else if (today >= DateTime.Parse("03/08/2021 11:00:01") && today < DateTime.Parse("03/15/2021 11:00:00")) {
					data = "4435";
				}
				else if (today >= DateTime.Parse("03/15/2021 11:00:01") && today < DateTime.Parse("03/22/2021 11:00:00")) {
					data = "4463";
				}
				else {
					data = "4436";
				}		
				
				xpath = "//div[contains(@id,'"+ data +"')]";
				ele = driver.FindElement("xpath", xpath);
                js.ExecuteScript("arguments[0].scrollIntoView(true);", ele);
				log.Info("*TEMPORARY FIX* : Scroll to Score Chip " + data);
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}