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

int solve(const vector<vector<bool>>& treeMap, int stepX, int stepY)
{
	int x = 0, y = 0;
	int cnt = 0;

	do
	{
		const auto& oneLine = treeMap[y];

		if (oneLine[x % oneLine.size()])
		{
			cnt ++;
		}

		cout << "[" << x << ',' << y << "] = " << oneLine[x % oneLine.size()] << ", total " << cnt << endl;

		x += stepX;
		y += stepY;
	}
	while (y < treeMap.size());

	return cnt;
}

int main()
{
	ifstream fin("data/advc_03.txt");

	vector<vector<bool>> treeMap;

	while (!fin.eof())
	{
		string str;
		fin >> str;

		if (!str.length())
			break;

		vector<bool> oneline;
		for (char c : str)
		{
			if (c == '.')
			{
				oneline.push_back(false);
			}
			else if (c == '#')
			{
				oneline.push_back(true);
			}
			else
			{
				cout << "Error unknown character " << c << endl;
			}
		}

		treeMap.push_back(oneline);
	}

	int cnt = solve(treeMap, 3, 1);
	cnt *= solve(treeMap, 1, 1);
	cnt *= solve(treeMap, 5, 1);
	cnt *= solve(treeMap, 7, 1);
	cnt *= solve(treeMap, 1, 2);

	cout << cnt << endl;

}