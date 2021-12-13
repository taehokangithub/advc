
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/*
 *  Private Leaderboard Json Parser
 *	Displays per-day rankings and finish times for each users 
 *
 *  Get private leaderboard json from the site, such as
 *  https://adventofcode.com/2021/leaderboard/private/view/786608.json
 *  And save it in the same folder as "advc_data.json"
 */
class Advc_Ranking
{
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

		public void PrintPerDay()
		{
			foreach(var item in m_days.OrderBy(d => d.Key).Reverse())
			{
				Day day = item.Value;
				day.Sort();

				Console.WriteLine("");
				Console.WriteLine($"* DAY {item.Key} :");
				for (int i = 0; i < day.Records[0].Count; i ++)
				{
					UserSolveRecord u1 = day.Records[0]?[i];

					string line = String.Format("{0,20} {1}", u1.User.Name, u1.GetTimeString());
					if (day.Records[1].Count > i)
					{
						UserSolveRecord u2 = day.Records[1]?[i];
						line += String.Format("\t{0,20} {1}", u2.User.Name, u2.GetTimeString());
					}
					Console.WriteLine(line);
				}
				
			}
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


    static void Run()
    {
		var jsonText = File.ReadAllText("advc_data.json");

		JObject jsonObject = JsonConvert.DeserializeObject<JObject>(jsonText);
		JToken membersObject = jsonObject.GetValue("members");
		
		List<JToken> members = membersObject.Values().ToList();

		Leaderboard board = new Leaderboard();

		foreach(JObject member in members)
		{
			board.AddUserJObject(member);
		}

		board.PrintPerDay();
    }

    static void Main()
    {
        Run();
    }    
}