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

void solve(const set<int>& v)
{
	static const int Sum = 2020;

	for (int a : v)
	{
		const int opp1 = Sum - a;

		for (int b : v)
		{
			const int opp2 = opp1 - b;

			if (v.find(opp2) != v.end())
			{
				cout << a << " * " << b << " * " << opp2 << " = " << (a * b * opp2) << endl;
			}
		}

	}
}

int main()
{
	std::set<int> v;

	do
	{
		string str;
		cin >> str;

		if (str == "q")
		{
			break;
		}

		v.insert(std::stoi(str));
	}
	while (true);

	solve(v);

}