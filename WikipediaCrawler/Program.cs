using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Text.RegularExpressions;

class WikipediaCrawler {
	static public List<string> searchedLinks = new List<string>();

	static void Main() {
		List<string> pagelist = ["Nicholas_V_(disambiguation)"];
		Dictionary<string, int> pageProximityDict = new Dictionary<string, int>{};
		
		foreach (string item in pagelist) {
			List<string> items = new List<string>();
			items.Add(item);
			pageProximityDict.Add(item, GetProximityToJesus(items));
		}
		
		foreach (var item in pageProximityDict) {
			Console.WriteLine($"{item.Key}: {item.Value}");
		}
		
	}

	static List<string> GetPageLinks(List<string> pages) {
		List<string> links = new List<string>();
		int count = 1;
		foreach (string page in pages) {
			var html = @"https://en.wikipedia.org/wiki/"+page;
			HtmlWeb web = new HtmlWeb();
			var htmlDoc = web.Load(html);
			var htmlLinks = htmlDoc.DocumentNode.QuerySelectorAll("#bodyContent p a[href^=\"/\"]");
			
			System.Console.WriteLine("Checking {0} of {1} - {2}", count, pages.Count(), htmlLinks.Count());
			foreach(var link in htmlLinks) {
				Regex rgx = new Regex("[^a-zA-Z0-9 _-]");
				string sanitizedLink = rgx.Replace(link.InnerText, "");
				if(!searchedLinks.Contains(sanitizedLink)) {
					links.Add(sanitizedLink);
				}
			}
			count++;
		}

		return links;
	}

	static bool CanWeSeeJesus(List <string> pages) {
		bool jesusFound;
		List<string> links = GetPageLinks(new List<string>(pages));
		jesusFound = links.Contains("Christ") || links.Contains("Jesus") || links.Contains("Jesus_Christ");
		
		return jesusFound;
	}

	static int GetProximityToJesus(List<string> pages, int degreesOfSeparation = 1) {
		if(CanWeSeeJesus (pages)) {
			return degreesOfSeparation;
		} else {
			List<string> pageList = GetPageLinks(pages);
			degreesOfSeparation = GetProximityToJesus(pageList, degreesOfSeparation+1);
		}


		return degreesOfSeparation;
	}
}
