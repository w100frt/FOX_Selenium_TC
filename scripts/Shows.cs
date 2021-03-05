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
			int overlay;
			int size = 0;
			int episode = 1;
			int attempts = 10;
			string classList = "";
			string edit = "";
			string top = "";
			string episodeNumber = "";
			string title = "";
			bool topTitle = true;
			bool live = false;
			List<TestStep> steps = new List<TestStep>();
			VerifyError err = new VerifyError();

			if (step.Name.Equals("Capture Episode Title"))
            {
				string eTitle = driver.FindElement("xpath","(//div[contains(@class,'pdg-top-20')])[" + episode + "]").GetAttribute("innerText");
				DataManager.CaptureMap["TITLE"] = eTitle;
				log.Info("Episode Title: " + eTitle);
            }
			
			else if (step.Name.Equals("Capture Number of Additional Episodes"))
			{
				size = driver.FindElements("xpath", "//div[contains(@class,'video-overlay')]").Count;

				DataManager.CaptureMap["EPISODES"] = size.ToString();
				log.Info("Number of additional episodes: " + size);
			}

			else if (step.Name.Equals("Select Additional Episodes"))
			{
				if (DataManager.CaptureMap.ContainsKey("EPISODES"))
				{
					size = Int32.Parse(DataManager.CaptureMap["EPISODES"]);
					for (int i = 1; i <= size; i++)
					{
						steps.Add(new TestStep(order, "Run Template", "Shows_Episodes_temp", "run_template", "xpath", "", wait));
						DataManager.CaptureMap["CURRENT_EPISODE_NUM"] = i.ToString();
						TestRunner.RunTestSteps(driver, null, steps);
						steps.Clear();
					}
				}
			}

			else if (step.Name.Equals("Select Additional Episode"))
			{
				//if (DataManager.CaptureMap.ContainsKey("EPISODES " +""))
				//{
				episode = Int32.Parse(DataManager.CaptureMap["CURRENT_EPISODE_NUM"]);
				steps.Clear();
				string eTitle = DataManager.CaptureMap["TITLE"];
				episodeNumber = driver.FindElement("xpath", "(//div[contains(@class,'video-overlay')])[" + episode + "]").GetAttribute("aria-label");

				steps.Add(new TestStep(order, "Select Episode " + episode + " - " + episodeNumber, "", "click", "xpath", "(//div[contains(@class,'video-overlay')])[" + episode + "]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				//}
			}

			if (step.Name.Equals("Verify Video is Playing"))
			{
				episode = Int32.Parse(DataManager.CaptureMap["CURRENT_EPISODE_NUM"]);
				string eTitle = DataManager.CaptureMap["TITLE"];

				ele = driver.FindElement("xpath", "//div[@class='mgn-btm-35'][//div[contains(@class,'fs-21') and contains(.," + eTitle + ")]]//div[@aria-label='Video Player']");
				classList = ele.GetAttribute("className");
				classList = classList.Substring(classList.IndexOf("jw-state-") + 9);
				classList = classList.Substring(0, classList.IndexOf(" "));

				// state returns idle if overlay button is present
				overlay = driver.FindElements("xpath", "//div[@class='overlays']/div").Count;
				if (overlay > 1)
				{
					steps.Add(new TestStep(order, "Click Overlay Play Button", "", "click", "xpath", "//*[@class='overlay-play-button']", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					ele = driver.FindElement("xpath", "//div[@aria-label='Video Player']");
					classList = ele.GetAttribute("className");
					classList = classList.Substring(classList.IndexOf("jw-state-") + 9);
					classList = classList.Substring(0, classList.IndexOf(" "));
				}

				// check video state. if not playing, wait and check again for 10 seconds
				do
				{
					log.Info("Video State: " + classList);
					if (!classList.Equals("playing"))
					{
						Thread.Sleep(1000);
						ele = driver.FindElement("xpath", "(//div[@aria-label='Video Player'])[" + episode + "]");
						classList = ele.GetAttribute("className");
						classList = classList.Substring(classList.IndexOf("jw-state-") + 9);
						classList = classList.Substring(0, classList.IndexOf(" "));
					}
				}
				while (!classList.Equals("playing") && attempts-- > 0);
				if (classList.Equals("playing"))
				{
					log.Info("Verification PASSED. Video returned " + classList);
				}
				else
				{
					log.Error("***Verification FAILED. Video returned " + classList + " ***");
					err.CreateVerificationError(step, "playing", classList);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
				}
			}
		}
	}
}
