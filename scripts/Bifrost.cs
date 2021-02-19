using System;
using System.Globalization;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;
using log4net;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Net;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{		
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public void Execute(DriverManager driver, TestStep step)
		{
			JObject jsonValue;
			JObject def;
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
			List<TestStep> steps = new List<TestStep>();
			IWebElement ele;
			VerifyError err = new VerifyError();
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(driver.GetDriver());
			
			string fileLocation = "https://api.foxsports.com/bifrost/v1/nascar/scoreboard/segment/202102?groupId=2&apikey=jE7yBJVRNAwdDesMgTzTXUUSx1It41Fq";
			var jsonFile = new WebClient().DownloadString(fileLocation);
			var json = JObject.Parse(jsonFile);
			jsonValue = json;
            DataManager.CaptureMap["IND_EVENTID"] = json["currentSectionId"].ToString();
			log.Info("Current Section ID from Bifrost: " + DataManager.CaptureMap["IND_EVENTID"]);
			
			foreach (JToken race in jsonValue["sectionList"]) {
				if (DataManager.CaptureMap["IND_EVENTID"] == race["id"].ToString()) {
					log.Info("HERE. Storing values.");
					log.Info(race["title"]);
					DataManager.CaptureMap["IND_EVENT"] = race["title"].ToString();
					DataManager.CaptureMap["IND_TRACK"] = race["subtitle"].ToString();
					DataManager.CaptureMap["IND_LOC"] = race["subtitle2"].ToString();
				}
			}
			


		}
	}
}