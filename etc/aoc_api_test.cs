using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SI.Jet.JetPOC
{
    public static class JetAPITest
    {
		public static string jsonText = "";
		public static DateTime jsonTime = new DateTime();

		public static bool LocalTest = false;

		[FunctionName("aoc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
			JetPocCommon.Log("Starting aoc", log);

			if (LocalTest)
			{
				string fileName = "/Users/taehokang/work/rank/arrayMani/dotnet/advc_ranking/advc_data.json";
				jsonText = System.IO.File.ReadAllText(fileName);
				jsonTime = DateTime.Now;
			}
			else if (jsonText.Length == 0 || jsonTime.AddMinutes(15) < DateTime.Now)
			{
				HttpClient client = new HttpClient();
				string uri = "https://adventofcode.com";
				string param = "/2021/leaderboard/private/view/786608.json";
				// https://adventofcode.com/2019/leaderboard/private/view/786608.json
				client.BaseAddress = new Uri(uri);

				var message = new HttpRequestMessage(HttpMethod.Get, param);

				var cookie = JetPocCommon.APICookie;
				if (cookie.Length == 0)
				{
					cookie = "53616c7465645f5f7f1c933796d53957e9779df3592b23762bce5b39616d6dc789c51a27939244c21c8ca0c4632abaea159fe148ad8228bc7aec03b85bf72374";
				}
				message.Headers.Add("Cookie", $"session={cookie}");
				HttpResponseMessage response = await client.SendAsync(message);

				string txt = await response.Content.ReadAsStringAsync();
				Console.WriteLine($"Response : [{response.ToString()}]");
				jsonTime = DateTime.Now;
				jsonText = txt;
			}
			else
			{
				Console.WriteLine($"Using json cache. CurTime {DateTime.Now}, jsonTime {jsonTime}, {(DateTime.Now - jsonTime).TotalSeconds} seconds passed");
			}


			string responseText = BuildReport(jsonText);

			return new ContentResult{ Content = responseText, ContentType = "text/html"};
        }

		class User
		{
			public string Name { get; set; }
			public int ID { get; set; }
			public int LocalScore { get; set; }
		}

		class UserSolveRecord
		{
			public User User { get; }
			public long Timestamp { get; }
			public UserSolveRecord(User user, long time)
			{
				User = user;
				Timestamp = time;
			}
			public string GetTimeString()
			{
				DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds(Timestamp);
				return offset.UtcDateTime.ToString();
			}
		}
		class Day
		{
			public List<List<UserSolveRecord>> Records { get; set; }
			bool sorted = false;

			public Day()
			{
				Records = new List<List<UserSolveRecord>>();

				Records.Add(new List<UserSolveRecord>()); // Part 1
				Records.Add(new List<UserSolveRecord>()); // Part 2
			}

			public void AddRecord(User user, JObject dayRecord)
			{
				foreach(var partRecord in dayRecord)
				{

					int part = int.Parse(partRecord.Key);
					long time = partRecord.Value["get_star_ts"].ToObject<long>();

					UserSolveRecord userSolveRecord = new UserSolveRecord(user, time);
					Records[part - 1].Add(userSolveRecord);
				}
				sorted = false;
			}

			public void Sort()
			{
				if (!sorted)
				{
					foreach(var list in Records)
					{
						list.Sort((a, b) => (a.Timestamp > b.Timestamp) ? 1 : -1);
					}
					sorted = true;
				}

			}
		}

		class Leaderboard
		{
			Dictionary<int, User> m_users = new Dictionary<int, User>();
			Dictionary<int, Day> m_days = new Dictionary<int, Day>();

			public void AddUserJObject(JObject member)
			{
				User user = new User();
				user.ID = member["id"].ToObject<int>();
				user.LocalScore = member["local_score"].ToObject<int>();
				user.Name = member["name"].ToObject<string>();
				user.Name = user.Name.Substring(0, Math.Min(user.Name.Length, 20));

				m_users[user.ID] = user;

				JObject completionData = member["completion_day_level"] as JObject;

				foreach (JValue dayProperty in completionData.Properties().Select(p => p.Name))
				{
					Day day = GetDayOrCreate(dayProperty.ToObject<int>());

					JObject dayRecord = completionData[dayProperty.ToString()] as JObject;

					day.AddRecord(user, dayRecord);
				}
			}

			public string BuildReport()
			{
				string response = @"
					<head>
					<meta charset='utf-8'/>
					<title>Day 4 - Advent of Code 2021</title>
					<link href='//fonts.googleapis.com/css?family=Source+Code+Pro:300&subset=latin,latin-ext' rel='stylesheet' type='text/css'/>
					<link rel='shortcut icon' href='/favicon.png'/>
					</head>	<body>
					<table border=0 style = 'width:100%; padding:30px; color:#cccccc; font-family:""Source Code Pro"", monospace;
					background:#0f0f23; font-size: 12pt; min-width: 60em;'>
					<tr style='color:#A0A0A0; font-size: 9pt'> <td width='20%'> &nbsp; </td> <td width='30%'> &nbsp; </td> </td> <td width='20%'> &nbsp; </td> 
					<td width='30%'> Table refreshes every 15 minutes <br> Last Refreshed : " + jsonTime + @" 
					</td> </tr>
				";

				foreach(var item in m_days.OrderBy(d => d.Key).Reverse())
				{
					Day day = item.Value;
					day.Sort();

					response += "<tr> <td style='color:#99EE99'>";
					response += $"<br><h2>DAY {item.Key} </h2> </td> <td> &nbsp; </td> <td> &nbsp; </td> <td> &nbsp; </td> </tr>";

					for (int i = 0; i < day.Records[0].Count; i ++)
					{
						UserSolveRecord u1 = day.Records[0]?[i];

						response += "<tr> <td style='text-align:right'> " + u1.User.Name + "</td> <td style='text-align:center'> " + u1.GetTimeString() + "</td>";

						if (day.Records[1].Count > i)
						{
							UserSolveRecord u2 = day.Records[1]?[i];
							response += "<td style='text-align:right'> " + u2.User.Name + "</td> <td style='text-align:center'> " + u2.GetTimeString() + "</td> </tr>";
						}
						else
						{
							response += "<td> &nbsp; </td> <td> &nbsp; </td> </tr>";
						}

					}
				}
				
				response += "</table> </body> </html>";
				return response;
			}

			private Day GetDayOrCreate(int day)
			{
				Day ret;
				if (!m_days.TryGetValue(day, out ret))
				{
					ret = new Day();
					m_days[day] = ret;
				}
				return ret;
			}
		}	

		static string BuildReport(string jsonText)
		{
			string ret;
			try
			{
				JObject jsonObject = JsonConvert.DeserializeObject<JObject>(jsonText);
				JToken membersObject = jsonObject.GetValue("members");
				
				List<JToken> members = membersObject.Values().ToList();

				Leaderboard board = new Leaderboard();

				foreach(JObject member in members)
				{
					board.AddUserJObject(member);
				}

				ret = board.BuildReport();
			}
			catch(Exception e)
			{
				ret = $"Error parsing json : {e.ToString()}\n{jsonText}";
			}

			return ret;
		}
    }
}
