using System;
using System.Text;
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
            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(driver.GetDriver());

			if (step.Name.Equals("Verify Event Title")) {
				byte[] bytes = Encoding.Default.GetBytes(step.Data);
				step.Data = Encoding.UTF8.GetString(bytes);
				xpath = "//div[contains(@id,'"+ DataManager.CaptureMap["IND_EVENTID"] +"')]//div[contains(@class,'scorechip-title')]";
				steps.Add(new TestStep(order, step.Name, step.Data, "verify_value", "xpath", xpath, wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Verify Event Track") || step.Name.Equals("Verify Event Course")) {
				xpath = "//div[contains(@id,'"+ DataManager.CaptureMap["IND_EVENTID"] +"')]//div[contains(@class,'scorechip-sub1')]";
				steps.Add(new TestStep(order, step.Name, step.Data, "verify_value", "xpath", xpath, wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Verify Event Location")) {
				xpath = "//div[contains(@id,'"+ DataManager.CaptureMap["IND_EVENTID"] +"')]//div[contains(@class,'scorechip-sub2')]";
				steps.Add(new TestStep(order, step.Name, step.Data, "verify_value", "xpath", xpath, wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}	
			
			else if (step.Name.Equals("Verify Event Time")) {
				xpath = "//div[contains(@id,'"+ DataManager.CaptureMap["IND_EVENTID"] +"')]//div[contains(@class,'pregame-info')]/span[1]";
				steps.Add(new TestStep(order, step.Name, step.Data, "verify_value", "xpath", xpath, wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}	
			
			else if (step.Name.Equals("Verify Event Channel")) {
				if(DataManager.CaptureMap["IND_CHANNEL"].Equals("FOX") || DataManager.CaptureMap["IND_CHANNEL"].Equals("FS1") || DataManager.CaptureMap["IND_CHANNEL"].Equals("FS2")) {
					xpath = "//div[contains(@id,'"+ DataManager.CaptureMap["IND_EVENTID"] +"')]//div[contains(@class,'pregame-info')]/img[@alt='"+ DataManager.CaptureMap["IND_CHANNEL"] +"']";
					steps.Add(new TestStep(order, step.Name, step.Data, "verify_value", "xpath", xpath, wait));
				}
				else {
					xpath = "//div[contains(@id,'"+ DataManager.CaptureMap["IND_EVENTID"] +"')]//div[contains(@class,'pregame-info')]/span[2]";
					steps.Add(new TestStep(order, step.Name, step.Data, "verify_value", "xpath", xpath, wait));
				}
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}			
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}