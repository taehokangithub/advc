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

using namespace std;

namespace 
{
	const std::vector<string> sBags = {"bags", "bags,", "bags.", "bag,", "bag."};
	const string sContain = "contain";
	const string sSpace = " ";
	const string sShinygold = "shiny gold";
}

//---------------------------------------------------------------------------------
void sayTime(std::string message, std::chrono::system_clock::time_point startTime)
{
	auto curTime = std::chrono::system_clock::now();
	chrono::duration<long, std::nano> elapsed_seconds = curTime - startTime;

	cout << message << " " << (elapsed_seconds.count() / 1000000) << " us" << endl;
}

//---------------------------------------------------------------------------------
using RULES_TYPE = std::map<string, std::map<string, int>>;

void skipWord(istream& in, const string& expected)
{
	string str;
	in >> str;

	if (str != expected)
	{
		cout << "Error! expected " << expected << ", found " << str << endl;
		exit(-1);
	}
}

string getColour(istream& fin)
{
	string colour;
	string str;

	fin >> colour;

	bool valid{};

	while(!fin.eof())
	{
		fin >> str;

		for (string bag : sBags)
		{
			if (bag == str)
			{
				valid = true;
				break;
			}
		}

		if (valid)
		{
			break;
		}
		else 
		{
			colour += sSpace + str;
		}
	}

	if (!valid)
	{
		cout << "Error parsing colour, str " << str << endl;
		exit(0);
	}

	return colour;
}

void printRules(const RULES_TYPE& rules)
{
	for (const auto&[color, rule] : rules)
	{
		cout << "[" << color << "] ";
		for (const auto&[rc, count] : rule)
		{
			cout << "(" << rc << ":" << count << ") ";
		}
		cout << endl;
	}
}

long solve2(const RULES_TYPE& rules, const string& targetColour)
{
	long sum = 1;

	if (const auto& ite = rules.find(targetColour); ite != rules.end())
	{	
		const auto& myrule = ite->second;

		cout << "Examining [" << targetColour << "]" << endl;
		for (const auto&[ruleColour, count] : myrule)
		{
			const long subSum = solve2(rules, ruleColour);
			sum += count * subSum;

			cout << "	Examining [" << targetColour << "] child [" << ruleColour << "] " << subSum << " * " << count << " = " << (subSum * count) << " (" << sum << ")" << endl;
		}
		cout << "Examining Finished [" << targetColour << "] : " << sum << endl;
	}
	else
	{
		cout << "		Terminal node [" << targetColour << "] : " << sum << endl;
	}

	
	return sum;
	// 9337 too low
}

int solve(RULES_TYPE rules, const string& targetColour)
{
	std::set<string> answers;
	std::set<string> thisTurnAnswer;
	std::set<string> toFind;

	toFind.insert(targetColour);

	int phase = 0;

	while(!toFind.empty())
	{
		cout << "------------------------------------" << endl;
		cout << "Phase " << (++phase) << ", answers " << answers.size() << ", toFind " << toFind.size() << ", remaining " << rules.size() << endl;
		cout << "------------------------------------" << endl;

		for (const auto& [ruleColour, rule] : rules)
		{
			for (const string& colourToFind : toFind)
			{
				if (rule.find(colourToFind) != rule.end())
				{
					cout << "[" << ruleColour << "] ===> " << colourToFind << endl;
					thisTurnAnswer.insert(ruleColour);
					break;
				}
			}
		}

		for (const string& colour : thisTurnAnswer)
		{
			answers.insert(colour);
			rules.erase(colour);
		}

		toFind = std::move(thisTurnAnswer);
		thisTurnAnswer.clear();
	}

	cout << "Remaining rules " << rules.size() << ", answers " << answers.size() << endl;

	return answers.size();
}

int main()
{
	ifstream fin("data/advc_07.txt");

	RULES_TYPE rules;

	const auto startTime = chrono::system_clock::now();

	while (!fin.eof())
	{
		string line;
		std::getline(fin, line);

		if (line.length() == 0)
		{
			break;
		}

		stringstream ss{line};

		const string myColour = getColour(ss);
		skipWord(ss, sContain);

		if (rules.find(myColour) != rules.end())
		{
			cout << "Error! Rules for " << myColour << " already exists" << endl;
			exit(0);
		}

		while (!ss.eof())
		{
			int n = 0;
			ss >> n;

			if (n)
			{
				const string targetColour = getColour(ss);
				rules[myColour][targetColour] = n;
			}
			else
			{
				break;
			}
		}
	}

	sayTime("Input finished", startTime);

#if 1
	printRules(rules);
#endif 

	cout << "Total rules " << rules.size() << endl;

	const int ret = solve2(rules, sShinygold) - 1;

	sayTime("Solve finished", startTime);

	cout << "answer : " << ret << endl;
}