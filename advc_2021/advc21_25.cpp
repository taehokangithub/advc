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

#include <assert.h>
#include <memory.h>
#include <stdlib.h>

using namespace std;


namespace ADVC_21_25
{
	const char UNIT_RIGHT = '>';
	const char UNIT_DOWN = 'v';
	const char UNIT_NIL = '.';

	class Unit
	{

	public:
		enum class UnitType { Nil, Down, Right };

		Unit() = default;

		Unit(const char c)
		{
			switch (c)
			{
			case UNIT_RIGHT: m_unitType = UnitType::Right; break;
			case UNIT_DOWN: m_unitType = UnitType::Down; break;
			case UNIT_NIL: m_unitType = UnitType::Nil; break;
			default:
				assert(false);
			}
		}

		Unit(const Unit& other)
		{
			m_unitType = other.m_unitType;
		}

		UnitType GetUnitType() const { return m_unitType; }

		void Clear() { m_unitType = UnitType::Nil; }

		char GetChar() const
		{
			switch (m_unitType)
			{
			case UnitType::Down: return UNIT_DOWN;
			case UnitType::Right: return UNIT_RIGHT;
			case UnitType::Nil: return UNIT_NIL;
			}

			assert(false);
			return UNIT_NIL;
		}

	private:
		UnitType m_unitType{ UnitType::Nil };
	};

	class Grid
	{
	public:
		void ParseLine(string line)
		{
			vector<Unit> newVector;

			for (char c : line)
			{
				newVector.push_back(Unit(c));
			}

			m_maxX = (int)newVector.size();
			m_grid.push_back(std::move(newVector));
			
			m_maxY = (int)m_grid.size();
		}

		void Solve()
		{
			int round = 0;
			while (Round())
			{
				round++;
				//cout << "-------------" << round << "--------------" << endl;
				//PrintGrid();
			}
			round++;

			cout << "total round " << round << endl;
		}

	private:
		using GRID_TYPE = vector<vector<Unit>>;

		bool Round()
		{
			GRID_TYPE newGrid = m_grid;

			bool hasAnyMoves = false;
			for (int y = 0; y < m_maxY; y++)
			{
				for (int x = 0; x < m_maxX; x++)
				{
					const Unit& unit = GetUnit(m_grid, x, y);

					if (unit.GetUnitType() == Unit::UnitType::Right)
					{
						const Unit& right = GetRight(m_grid, x, y);
						if (right.GetUnitType() == Unit::UnitType::Nil)
						{
							GetRight(newGrid, x, y) = unit;
							GetUnit(newGrid, x, y).Clear();
							hasAnyMoves = true;
						}
					}
					
				}
			}

			m_grid = newGrid;

			for (int y = 0; y < m_maxY; y++)
			{
				for (int x = 0; x < m_maxX; x++)
				{
					const Unit& unit = GetUnit(m_grid, x, y);

					if (unit.GetUnitType() == Unit::UnitType::Down)
					{
						const Unit& down = GetDown(m_grid, x, y);

						if (down.GetUnitType() == Unit::UnitType::Nil)
						{
							GetDown(newGrid, x, y) = unit;
							GetUnit(newGrid, x, y).Clear();
							hasAnyMoves = true;
						}
					}
				}
			}

			m_grid = std::move(newGrid);
			return hasAnyMoves;
		}

		Unit& GetUnit(GRID_TYPE& grid, int x, int y)
		{
			return grid[y][x];
		}

		Unit& GetRight(GRID_TYPE& grid, int x, int y)
		{
			return grid[y][(x + 1) % m_maxX];
		}

		Unit& GetDown(GRID_TYPE& grid, int x, int y)
		{
			return grid[(y + 1) % m_maxY][x];
		}

		void PrintGrid()
		{
			for (int y = 0; y < m_maxY; y++)
			{
				for (int x = 0; x < m_maxX; x++)
				{
					cout << m_grid[y][x].GetChar();
				}
				cout << endl;
			}
		}

	private:
		GRID_TYPE m_grid;
		int m_maxX{};
		int m_maxY{};


	};
	// ----------------------------------------------------------

	int RunMain(string path)
	{
		cout << "Starting program : reading " << path << endl;

		ifstream fin(path);

		if (fin.fail())
		{
			cout << "Could not open " << path << endl;
			return -1;
		}

		Grid grid;

		while (!fin.eof())
		{
			string line;
			std::getline(fin, line);

			if (line.length())
			{
				grid.ParseLine(line);
			}
		}

		grid.Solve();
		return 0;
	}
}


int main()
{
	ADVC_21_25::RunMain("../../../data/advc21_25_sample.txt");
	ADVC_21_25::RunMain("../../../data/advc21_25.txt");
	

	return 0;
}