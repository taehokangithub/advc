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

const std::set<string> FIELDS 
	{
		"byr",
		"iyr",
		"eyr",
		"hgt",
		"hcl",
		"ecl",
		"pid",
		"cid"
	};

const std::set<string> EYE_COLOURS
{
	"amb", "blu", "brn", "gry", "grn", "hzl", "oth"
};

bool isFieldValid(const string& field, const string& value)
{

/* 
	byr (Birth Year) - four digits; at least 1920 and at most 2002.
	iyr (Issue Year) - four digits; at least 2010 and at most 2020.
	eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
	hgt (Height) - a number followed by either cm or in:
	If cm, the number must be at least 150 and at most 193.
	If in, the number must be at least 59 and at most 76.
*/	
	if (field == "byr")
	{
		const int valueInt = std::stoi(value);
		return valueInt >= 1920 && valueInt <= 2002;
	}
	else if (field == "iyr")
	{
		const int valueInt = std::stoi(value);
		return valueInt >= 2010 && valueInt <= 2020;
	}	
	else if (field == "eyr")
	{
		const int valueInt = std::stoi(value);
		return valueInt >= 2020 && valueInt <= 2030;		
	}	
	else if (field == "hgt")
	{
		const string trail = value.substr(value.length() - 2);

		try
		{
			const int valueInt = std::stoi(value.substr(0, value.length() - 2));
			if (trail == "cm")
			{
				return valueInt >= 150 && valueInt <= 193;
			}
			else if (trail == "in")
			{
				return valueInt >= 59 && valueInt <= 76;
			}			
		}
		catch(std::exception& e)
		{
			return false;
		}
		return false;
	}
	/*
	hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
	ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
	pid (Passport ID) - a nine-digit number, including leading zeroes.
	cid (Country ID) - ignored, missing or not.	
	*/
	else if (field == "hcl")
	{
		if (value[0] != '#' || value.length() != 7)
		{
			cout << "failed colour test by " << value[0] << " or length " << value.length() << endl;
			return false;
		}
		const string color = value.substr(1);

		for (const char c : color)
		{
			if (c >= '0' && c <= '9')
				continue;
			if (c >= 'a' && c <= 'f')
				continue;
			cout << "failed colour test by " << c << endl;
			return false;
		}

		return true;
	}	
	else if (field == "ecl")
	{
		return (EYE_COLOURS.find(value) != EYE_COLOURS.end());
	}	
	else if (field == "pid")
	{
		// a nine-digit number, including leading zeroes.
		if (value.length() != 9)
			return false;

		for (char c : value)
		{
			if (c >= '0' && c <= '9')
				continue;

			return false;
		}
		return true;
	}	
	else if (field == "cid")
	{
		return true;
	}

	cout << "Error ! unknown field " << field << " in isFieldValid" << endl;
	exit(0);
	return false;
}

bool isValid(const std::map<string,string> receivedData)
{
	auto required = FIELDS;

	for (const auto& pair : receivedData)
	{
		const auto& field = pair.first;
		const auto& value = pair.second;
		cout << field << " = " << value << endl;

		if (const auto& ite = FIELDS.find(field); ite != FIELDS.end())
		{
			if (!isFieldValid(field, value))
			{
				cout << "Invalid field value " << field << ", " << value << endl;
				return false;
			}
			required.erase(field);
		}
		else
		{
			cout << "Error unknown field " << field << endl;
			exit(0);
		}
	}

	bool isValid = true;
	for (const auto& remaining : required)
	{
		if (remaining == "cid")
		{
			cout << "optional cid is missing" << endl;
		}
		else
		{
			cout << "missing field " << remaining << endl;

			isValid = false;
			break;
		}
	}

	return isValid;
}

int main()
{
	ifstream fin("data/advc_04.txt");

	std::map<string, string> receivedData;
	int total = 0;
	int count = 0;

	while (!fin.eof())
	{
		string line;
		std::getline(fin, line);

		if (!line.length())
		{
			bool result = isValid(receivedData);
			if (result)
			{
				count ++;
			}
			total ++;
			receivedData.clear();

			cout << "------------- " << (result ? "true" : "false") << " ----------------" << endl;
			continue;
		}

		stringstream s(line);

		while (!s.eof())
		{
			string str;
			s >> str;

			string field = str.substr(0, 3);
			string value = str.substr(4);

			receivedData[field] = value;
		}
	}

	cout << "Answer = " << count << " out of " << total << endl;

}