#include <algorithm>
#include <cmath>
#include <chrono>
#include <cstdio>
#include <fstream>
#include <iomanip>
#include <iostream>
#include <list>
#include <map>
#include <memory>
#include <queue>
#include <set>
#include <sstream>
#include <stack>
#include <unordered_map>
#include <unordered_set>
#include <vector>

#include <memory.h>
#include <stdlib.h>

namespace 
{
	//---------------------------------------------------------------------------------
	void sayTime(std::string message, std::chrono::system_clock::time_point startTime)
	{
		auto curTime = std::chrono::system_clock::now();
		std::chrono::duration<long, std::nano> elapsed_seconds = curTime - startTime;

		std::cout << message << " " << (elapsed_seconds.count() / 1000000) << " us" << std::endl;
	}

	//---------------------------------------------------------------------------------
	void readAndSkip(std::istream& in, const std::string str)
	{
		std::string strIn;

		in >> strIn;

		if (strIn != str) 
		{
			std::cout << "Error! expected " << str << " but found " << strIn;
			exit(0);
		}
	}

	//---------------------------------------------------------------------------------
	void readAndSkip(std::string s, const std::string str)
	{
		std::stringstream ss(s);
		readAndSkip(ss, str);
	}

}

//---------------------------------------------------------------------------------
using namespace std;
using std::cout;

namespace 
{
	constexpr int s_printThreshold = 0;
	using CARD_DECK = list<int>;
	map<string, bool> s_subResults;
}

//---------------------------------------------------------------------------------

string getCanonicalForm(const CARD_DECK& pa, const CARD_DECK& pb)
{
	stringstream ss;
	ss << "( ";
	for (int card : pa)
	{
		ss << card << " ";
	}
	ss << ") ( ";
	for (int card : pb)
	{
		ss << card << " ";
	}
	ss << ")";
	return ss.str();
}

//---------------------------------------------------------------------------------
CARD_DECK getSubCardDeck(const CARD_DECK& deck)
{
	CARD_DECK sub{deck};
	
	int front = sub.front();
	sub.pop_front();

	while (sub.size() > front)
	{
		sub.pop_back();
	}

	return sub;
}

//---------------------------------------------------------------------------------
// returns true if pa wins
bool oneRound_hasLeftWon(CARD_DECK& pa, CARD_DECK& pb, int depth = 0)
{
	const string gameId = getCanonicalForm(pa, pb);

	if (depth <= s_printThreshold + 1)
		cout << "["<< depth << "] Starting a game for " << gameId << endl;
	set<string> configurations;
	set<string> reverseConf;
	optional<bool> ret;

	while(pa.size() && pb.size())
	{
		bool paWins;

		const string canonical = getCanonicalForm(pa, pb);
		const string reversed = getCanonicalForm(pb, pa);

		if (configurations.find(reversed) != configurations.end())
		{
			cout << "Found reversed! " << endl << canonical << endl << reversed << endl;
			cout << "No idea why this never happens but leaving the code for future reference" << endl;
			exit(0);
		}

		if (s_subResults.find(canonical) != s_subResults.end())
		{
			ret = s_subResults[canonical];
			break;
		}

		if (s_subResults.find(reversed) != s_subResults.end())
		{
			cout << "Found reversed in subresult. " << reversed << endl;
			cout << "No idea why this never happens but leaving the code for future reference" << endl;
			ret = !s_subResults[reversed];
			break;
		}

		if (configurations.find(canonical) != configurations.end())
		{
			ret = true;
			break;
		}

		configurations.insert(canonical);
		reverseConf.insert(reversed);

		if (pa.size() > pa.front() && pb.size() > pb.front())
		{
			if (depth <= s_printThreshold)
			{
				cout << "["<< depth << "] In processing " << canonical << endl;
				cout << "["<< depth << "] Opening a new game, pa " << pa.front() << " size " << pa.size() << " pb " << pb.front() << " size " << pb.size() << endl;
			}

			CARD_DECK subPa = getSubCardDeck(pa);
			CARD_DECK subPb = getSubCardDeck(pb);
			paWins = oneRound_hasLeftWon(subPa, subPb, depth + 1);
		}
		else
		{
			paWins = pa.front() > pb.front();
		}

		CARD_DECK& winner = paWins ? pa : pb;
		CARD_DECK& loser = paWins ? pb : pa;

		winner.push_back(winner.front());
		winner.push_back(loser.front());
		winner.pop_front();
		loser.pop_front();
	}
	
	ret = ret.has_value() ? *ret : (pa.size() != 0);

	for (const string& c : configurations)
	{
		s_subResults[c] = *ret;
	}
	for (const string& c : reverseConf)
	{
		s_subResults[c] = !*ret;
	}

	if (depth <= s_printThreshold + 1)
	{
		cout << "["<< depth << "] Ending a game for " << gameId 
			<< ", new visit " << (configurations.size() * 2) 
			<< ", total visit " << s_subResults.size() << endl;
		cout << "["<< depth << "] Final deck ==> " << getCanonicalForm(pa, pb) << endl;
	}

	return *ret;
}

void solve(CARD_DECK& pa, CARD_DECK& pb)
{
	const CARD_DECK& winner = oneRound_hasLeftWon(pa, pb) ? pa : pb;

	int sum = 0;
	int multi = winner.size();

	for (int card : winner)
	{
		sum += (multi--) * card;
	}

	cout << "Ans : " << sum << endl;			
}

//---------------------------------------------------------------------------------
int main()
{
	ifstream fin("data/advc20_22.txt");

	const auto startTime = chrono::system_clock::now();

	CARD_DECK decks[2];

	int curPlayer{};

	while (!fin.eof())
	{
		string line;
		std::getline(fin, line);

		if (line.length() == 0) 
		{
			if (++curPlayer == 2)
				break;
			continue;
		}

		if (line.substr(0, 6) == "Player")
		{
			continue;
		}

		CARD_DECK& deck = decks[curPlayer];
		
		deck.push_back(stoi(line));
	}

	for (int i = 0; i < 2; i ++)
	{
		cout << "Player " << i << endl;
		for (int card : decks[i])
		{
			cout << card << " ";
		}
		cout << endl;
	}

	solve(decks[0], decks[1]);

	sayTime("Solve finished", startTime);
}