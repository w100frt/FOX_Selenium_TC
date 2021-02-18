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
			int channel = 0;
			int attempts = 10;
			string classList = "";
			string title = "";
			string edit = "";
			string top = "";
			string channelName = "";
			bool topTitle = true;
			bool live = false;
			List<TestStep> steps = new List<TestStep>();
			VerifyError err = new VerifyError();

			if (step.Name.Equals("Capture Number of Additional Episodes"))
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
						steps.Add(new TestStep(order, "Run Template", "Shows_Episodes", "run_template", "xpath", "", wait));
						DataManager.CaptureMap["CURRENT_EPISODE_NUM"] = i.ToString();
						TestRunner.RunTestSteps(driver, null, steps);
						steps.Clear();
					}
				}
			}
		}
	} 
}
