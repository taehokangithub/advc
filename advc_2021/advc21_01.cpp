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

int main()
{
	ifstream fin("data/advc21_01.txt");

	int prev = 0;
	int cntIncrease = 0;
	std::vector<int> data;

	while (!fin.eof())
	{
		int val;
		fin >> val;

		cout << "Input : " << val << endl;
		data.push_back(val);
	}

	for (int i = 0; i < (int)data.size() - 3; i ++)
	{
		const int cur = data[i] + data[i + 1] + data[i + 2];

		if (cur > prev && prev != 0)
		{
			cntIncrease ++;
		}

		cout << "Inspecting [" << i << "] " << cur << ", prev" << prev << ", cnt " << cntIncrease << endl;

		prev = cur;
	}

	cout << "answer : " << cntIncrease << endl;
}