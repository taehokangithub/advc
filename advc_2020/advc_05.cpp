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

int convert(string str)
{
	int ret = 0;
	//cout << "Checking : " << str << endl;
	for (int i = 0; i < str.length(); i ++)
	{
		const char c = str[i];
		const int pos = str.length() - i - 1;
		int adding = 0;
		if (c == 'B' || c == 'R')
			adding = (1 << pos);
		ret += adding;
		//cout << "[" << i << ":" << c << "] pos " << pos << ", adding " << adding << ", ret " << ret << endl;
	}
	return ret;
}

int main()
{
	ifstream fin("data/advc_05.txt");
	constexpr int maxSeat = 1024;
	bool checker[maxSeat] = {false, };

	int ret = 0;
	while (!fin.eof())
	{
		string str;
		std::getline(fin, str);

		if (str.length())
		{
			const int value = convert(str);

			checker[value] = true;
			cout << "Found seat " << value << endl;
			ret = std::max(ret, value);

		}
	}

	for (int i = 1; i < maxSeat - 1; i ++)
	{
		if (checker[i] == false)
		{
			//cout << "missing seat " << i << endl;
			if (checker[i - 1] && checker[i + 1])
			{
				cout << "Candidate found : " << i << endl;
			}
		}
	}

	//cout << "answer : " << ret << endl;
}