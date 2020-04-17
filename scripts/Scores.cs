using System;
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
			IWebElement chip;
			int size;
			string title;
			string date;
			string data;
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify Displayed Day on Top Scores")) {
				TimeSpan time = DateTime.UtcNow.TimeOfDay;
				int now = time.Hours;
				int et = now - 4;
				if (et < 11){
					log.Info("Current Eastern Time hour is " + et + ". Default to Yesterday.");
					step.Data = "YESTERDAY";
				}
				else {
					log.Info("Current Eastern Time hour is " + et + ". Default to Today.");
					step.Data = "TODAY";		
				} 					

				steps.Add(new TestStep(order, "Verify Displayed Day on Top Scores", step.Data, "verify_value", "xpath", "//div[contains(@class,'scores-date')]//div[contains(@class,'sm')]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Scroll Top Scores Page to Yesterday")) {
				title = "//div[contains(@class,'scores-date')]//div[contains(@class,'sm')]";
				ele = driver.FindElement("xpath", title);
				date = ele.GetAttribute("innerText");
				
				if (!date.Equals("YESTERDAY")) {
					do {
						js.ExecuteScript("window.scrollBy(0,-250)");
						log.Info("Scrolling up on page...");
						ele = driver.FindElement("xpath", title);
						date = ele.GetAttribute("innerText");
						chip = driver.FindElement("xpath","(//div[@class='scores']//a)[1]");
					}
					while (!date.Equals("YESTERDAY") || !chip.Displayed);
					steps.Add(new TestStep(order, "Verify Displayed Day on Top Scores", "YESTERDAY", "verify_value", "xpath", title, wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				else {
					log.Info("Page defaulted to YESTERDAY");
				}
			}
			
			else if (step.Name.Equals("Scroll Top Scores Page Down to Today")) {
				title = "//div[contains(@class,'scores-date')]//div[contains(@class,'sm')]";
				ele = driver.FindElement("xpath", title);
				date = ele.GetAttribute("innerText");
				
				if (!date.Equals("TODAY")) {
					do {
						js.ExecuteScript("window.scrollBy(0,250)");
						log.Info("Scrolling down on page...");
						ele = driver.FindElement("xpath", title);
						date = ele.GetAttribute("innerText");
						size = driver.FindElements("xpath", "//div[@class='scores']//a").Count;
						chip = driver.FindElement("xpath","(//div[@class='scores']//a)["+ size +"]");
					}
					while (!date.Equals("TODAY") || !chip.Displayed);
				}
				else {
					log.Info("Page defaulted to TODAY");
				}
			}
			
			else if (step.Name.Equals("Scroll Top Scores Page to Tomorrow")) {
				title = "//div[contains(@class,'scores-date')]//div[contains(@class,'sm')]";
				ele = driver.FindElement("xpath", title);
				date = ele.GetAttribute("innerText");
				
				if (!date.Equals("TOMORROW")) {
					do {
						js.ExecuteScript("window.scrollBy(0,250)");
						log.Info("Scrolling down on page...");
						ele = driver.FindElement("xpath", title);
						date = ele.GetAttribute("innerText");
						size = driver.FindElements("xpath", "//div[@class='scores']//a").Count;
						chip = driver.FindElement("xpath","(//div[@class='scores']//a)["+ size +"]");
					}
					while (!date.Equals("TOMORROW") || !chip.Displayed);
					steps.Add(new TestStep(order, "Verify Displayed Day on Top Scores", "TOMORROW", "verify_value", "xpath", title, wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				else {
					log.Info("Page defaulted to TOMORROW");
				}
			}
			
			else if (step.Name.Equals("Verify League Title on Top Scores")) {
				switch(step.Data) {
					case "Scorestrip" : 
						title = "//div[contains(@class,'homepage-module')]//a[@class='score-chip']";
						break;
					case "Yesterday" : 
						title = "(//div[@class='scores'])[1]//a[@class='score-chip']";
						break;
					case "Today" : 
						title = "(//div[@class='scores'])[2]//a[@class='score-chip']";
						break;
					case "Tomorrow" : 
						title = "(//div[@class='scores'])[3]//a[@class='score-chip']";
						break;
					default: 
						title = "//div[@class='scores']//a[@class='score-chip']";
						break;
				}
				
				size = driver.FindElements("xpath", title).Count;
				for (int i = 1; i <= size; i++) {
					ele = driver.FindElement("xpath", "(" + title + "//div[@class='highlight-text']//div[contains(@class,'league-title')])[" + i + "]");
					data = ele.GetAttribute("innerText");
					
					if(!String.IsNullOrEmpty(data)) {
						log.Info("Score Chip " + i + " League Title equals " + data);
					}
					else {
						err.CreateVerificationError(step, "Expected League Title", data);
					}
				}
			}
			
			else if(step.Name.Equals("Click Scorechip By Number")) {
				data = step.Data;
				steps.Add(new TestStep(order, "Click Event " + data, "", "click", "xpath", "(//a[@class='score-chip'])["+ data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if(step.Name.Equals("Capture Team Info from Chip")) {
				data = step.Data;
				steps.Add(new TestStep(order, "Capture Away Team Abbreviation", "AWAY_TEAM_ABB"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,'abbreviation')])[1]", wait));
				steps.Add(new TestStep(order, "Capture Away Team", "AWAY_TEAM"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,' team')])[1]", wait));
				steps.Add(new TestStep(order, "Capture Away Team Score", "AWAY_TEAM_SCORE"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,'team-score')])[1]", wait));
				steps.Add(new TestStep(order, "Capture Home Team Abbreviation", "HOME_TEAM_ABB"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,'abbreviation')])[2]", wait));
				steps.Add(new TestStep(order, "Capture Home Team", "HOME_TEAM"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,' team')])[2]", wait));
				steps.Add(new TestStep(order, "Capture Home Team Score", "HOME_TEAM_SCORE"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,'team-score')])[2]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}