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

int countAnswers(const vector<string>& input)
{
	std::map<char, int> collection;		
	for (const string& line : input)
	{
		for (const char c : line)
		{
			collection[c] ++;
		}
	}

	int ret = 0;
	for (const auto&[c, cnt] : collection)
	{
		if (cnt == input.size())
		{
			ret ++;
		}
	}

	return ret;
}

int main()
{
	ifstream fin("data/advc_06.txt");

	int ret = 0;
	std::vector<string> inputs;
	while (!fin.eof())
	{
		string str;
		std::getline(fin, str);

		if (str.length())
		{
			inputs.push_back(str);
		}
		else
		{
			ret += countAnswers(inputs);
			inputs.clear();
		}
	}

	cout << "answer : " << ret << endl;
}