These files are not fully running version for some reasone

advc_ranking.cs 
: It's a stand-alone dotnet code providing text format, but you need to provide your session cookie in the code
			//! You need session cookie here
    		message.Headers.Add("Cookie", "session=");

aoc_api_test.cs 
: It's for Azure function app code, providing html format
  You need to set up Azure function app project to utilise this code
  Still you need to provide your session cookie in the code
			message.Headers.Add("Cookie", $"session=");