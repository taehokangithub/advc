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
	ifstream fin("data/advc21_02.txt");

	int pos{};
	int depth{};
	int aim{};

	while (!fin.eof())
	{
		string dir;
		int val;

		fin >> dir >> val;

		if (dir.length() == 0)
		{
			break;
		}

		if (dir == "forward")
		{
			pos += val;
			depth += aim * val;
			cout << "Going forward";
		}
		else if (dir == "up")
		{
			aim -= val;
			cout << "going up     ";
		}
		else if (dir == "down")
		{
			aim += val;
			cout << "going down   ";
		}
		else
		{
			cout << "unknown dir " << dir << endl;
			exit(0);
		}

		cout << " " << val << ": Pos " << pos << ", depth " << depth << ", aim " << aim << endl;
	}

	cout << "ans : " << (pos * depth) << endl;

}