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

bool solve1(int from, int to, char targetChar, string str)
{
	int charCount = 0;
	for (char c : str)
	{
		if (c == targetChar)
			charCount ++;
	}

	if (charCount >= from && charCount <= to)
	{
		return true;
		}
	return false;
}

bool solve2(int pos1, int pos2, char targetChar, string str)
{
	int cnt = 0;
	cnt += (str[pos1 - 1] == targetChar);
	cnt += (str[pos2 - 1] == targetChar);

	return cnt == 1;
}

int main()
{
	ifstream fin("data/advc_02.txt");

	int cnt = 0;
	while (!fin.eof())
	{
		string str;
		char dummy;
		char targetChar;
		int from, to;
		fin >> from >> dummy >> to >> targetChar >> dummy >> str;

		const bool isCorrect = solve2(from, to, targetChar, str);

		if (isCorrect)
			cnt ++;

		cout << "from " << from << " to " << to << " targetChar " << targetChar << ", str " << str << " ==> " << isCorrect << endl;
	}

	cout << cnt << endl;

}