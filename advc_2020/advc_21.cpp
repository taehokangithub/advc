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
using INGR_LIST = set<string>;

//---------------------------------------------------------------------------------
class Desc
{
	public:
		void addIngrediants(const string& str) { m_ingredients.insert(str); }
		void addAlleg(const string& str) { m_allegs.insert(str); }

		const INGR_LIST& getAllegs() const { return m_allegs; }
		const INGR_LIST& getIngrediants() const { return m_ingredients; }

		int countExcept(const INGR_LIST& confirmedIngrs)
		{
			int cnt = 0;
			for (const auto& ingr : m_ingredients)
			{
				if (confirmedIngrs.find(ingr) == confirmedIngrs.end())
				{
					cnt ++;
				}
			}
			return cnt;
		}

	private:
		INGR_LIST m_ingredients;
		INGR_LIST m_allegs;
};

//---------------------------------------------------------------------------------
class Alleg
{
	public:
		void setName(const string& name) { m_name = name; }
		string getName() const { return m_name; }
		void addDesc(Desc* desc) { m_descs.push_back(desc); }
		bool hasConfirmed() const { return m_candidates.size() == 1; }
		string getConfirmed() const { assert(hasConfirmed()); return *m_candidates.begin(); }
		void process();
		void printCandidates() 
		{
			cout << "[Alleg:" << m_name << "]";
			for (const auto& ingr : m_candidates)
			{
				cout << " " << ingr;
			}
			cout << endl;
		}
		bool removeCandidates(const INGR_LIST& toRemove);
	private:
		string m_name;
		vector<Desc*> m_descs;
		INGR_LIST m_candidates;
};

//---------------------------------------------------------------------------------
void Alleg::process()
{
	assert(m_descs.size() > 0);

	const auto& list = m_descs[0]->getIngrediants();
	m_candidates = m_descs[0]->getIngrediants();

	for (int i = 1; i < (int)m_descs.size(); i ++)
	{
		INGR_LIST newList;

		for (const auto& ingr : m_descs[i]->getIngrediants())
		{
			if (m_candidates.find(ingr) != m_candidates.end())
			{
				newList.insert(ingr);
			}
		}

		m_candidates = std::move(newList);
	}
}

//---------------------------------------------------------------------------------
bool Alleg::removeCandidates(const INGR_LIST& toRemove)
{
	if (hasConfirmed())
	{
		return false;
	}

	bool somethingChanged = false;

	for (const string& ingr : toRemove)
	{
		if (m_candidates.find(ingr) != m_candidates.end())
		{
			m_candidates.erase(ingr);
			somethingChanged = true;
		}
	}

	return somethingChanged;
}

//---------------------------------------------------------------------------------
class Solver
{
	public:
		Desc* getNewDesc() 
		{
			m_descs.push_back(make_unique<Desc>());
			return m_descs[m_descs.size() - 1].get();
		}

		Alleg& getAlleg(const string& name)
		{
			if (m_allegs.find(name) == m_allegs.end())
			{
				Alleg& ret = m_allegs[name];
				ret.setName(name);
			}

			return m_allegs[name];
		}

		void printAllDescs()
		{
			for (const auto& desc : m_descs)
			{
				cout << "[ ";
				for (const string& alleg : desc->getAllegs())
				{
					cout << alleg << " ";
				}
				cout << "] : " << endl << "     ";
				for (const string& ingr : desc->getIngrediants())
				{
					cout << ingr << " ";
				}
				cout << endl;
			}
		}

		void printAllAllegs() 
		{
			cout << "------- All Allegs --------- " << endl;
			for (auto&[allegName, alleg] : m_allegs)
			{
				alleg.printCandidates();
			}			
		}

		void solve();

	private:
		vector<unique_ptr<Desc>> m_descs;
		map<const string, Alleg> m_allegs;
};

//---------------------------------------------------------------------------------
void Solver::solve()
{
	for (auto&[allegName, alleg] : m_allegs)
	{
		alleg.process();
	}

	printAllAllegs();

	bool somethingChanged{true};

	INGR_LIST confirmedIngrs;
	map<string,string> mapAllegToIngr;

	while(somethingChanged)
	{
		somethingChanged = false;
		INGR_LIST ingrToRemove;

		for (auto&[allegName, alleg] : m_allegs)
		{
			if(alleg.hasConfirmed())
			{
				const string& ingr = alleg.getConfirmed();
				if (confirmedIngrs.find(ingr) == confirmedIngrs.end())
				{
					ingrToRemove.insert(ingr);
					confirmedIngrs.insert(ingr);
					mapAllegToIngr[allegName] = ingr;
				}

			}
		}

		for (auto&[allegName, alleg] : m_allegs)
		{
			somethingChanged = alleg.removeCandidates(ingrToRemove) || somethingChanged;
		}
	}

	printAllAllegs();

	int ans = 0;
	for (const auto& desc : m_descs)
	{
		ans += desc->countExcept(confirmedIngrs);
	}

	cout << "Ans : " << ans << endl;
	cout << "Canonical : ";

	const string& firstAlleg = mapAllegToIngr.begin()->first;
	for (const auto& [allegName,ingr] : mapAllegToIngr)
	{
		if (allegName != firstAlleg)
		{
			cout << ",";
		}
		cout << ingr;
	}
	cout << endl;
}

//---------------------------------------------------------------------------------
int main()
{
	ifstream fin("data/advc20_21.txt");

	const auto startTime = chrono::system_clock::now();

	Solver solver;

	int index = 0;
	while (!fin.eof())
	{
		string line;
		std::getline(fin, line);

		if (line.length() == 0) 
		{
			break;
		}

		stringstream ss(line);
		Desc* const desc = solver.getNewDesc();

		while (!ss.eof())
		{
			string str;
			ss >> str;

			if (str[0] == '(')
			{
				readAndSkip(str.substr(1), "contains");

				while (!ss.eof())
				{
					ss >> str;
					str.pop_back();

					desc->addAlleg(str);

					Alleg& alleg = solver.getAlleg(str);
					alleg.addDesc(desc);
				}
			}
			else
			{
				desc->addIngrediants(str);
			}
		}
	}

	solver.solve();
	sayTime("Solve finished", startTime);
}