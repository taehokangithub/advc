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


namespace 
{

	//---------------------------------------------------------------------------------
	void sayTime(std::string message, std::chrono::system_clock::time_point startTime)
	{
		auto curTime = std::chrono::system_clock::now();
		chrono::duration<long, std::nano> elapsed_seconds = curTime - startTime;

		cout << message << " " << (elapsed_seconds.count() / 1000000) << " us" << endl;
	}

	enum SIDES 
	{
		TOP, RIGHT, BOTTOM, LEFT, ALLSIDES
	};

	const string SIDENAMES[ALLSIDES] = { "TOP", "RIGHT", "BOTTOM", "LEFT" };

	SIDES getOpponent(SIDES side)
	{
		return (SIDES)((((int)side) + 2) % (int)ALLSIDES);
	}

	void readAndSkip(istream& in, const string str)
	{
		string strIn;

		in >> strIn;

		if (strIn != str) 
		{
			cout << "Error! expected " << str << " but found " << strIn;
			exit(0);
		}
	}

	const vector<string> PATTERN = {
		"                  # ",
		"#    ##    ##    ###",
 		" #  #  #  #  #  #   "
	};
}



// --------------------------------------------------------
class Tile 
{
	public:
		void setId(int id_in) { m_id = id_in; }
		int getId() const { return m_id; }
		void addTileLine(const string& line) { m_data.push_back(line); }
		const string& getTileLine(int index) { return m_data[index]; }
		const string& getSide(SIDES side) const { return m_sides[side]; }
		int getTileHeight() const { return m_data.size(); }
		int getTileWidth() const { return m_data[0].length(); }
		void clear() { m_data.clear(); }

		void rotate() 
		{
			vector<string> newData;
			const int tileWidth = m_data[0].length();

			for (int i = 0; i < tileWidth; i ++)
			{
				string s;
				for (int k = m_data.size() - 1; k >= 0; k --)
				{
					s.push_back(m_data[k][i]);
				}
				newData.push_back(s);
			}

			m_data = std::move(newData);
			setSideLines();
		}

		void flip()
		{
			for (int i = 0; i < (int) m_data.size(); i ++)
			{
				m_data[i] = std::move(reverse(m_data[i]));
			}
			setSideLines();
		}

		void setSideLines()
		{
			const int tileWidth = getTileWidth();
			const int tileHeight = getTileHeight();

			setSide(TOP, m_data[0]);
			setSide(BOTTOM, reverse(m_data[tileHeight - 1]));

			string line;
			
			for (int i = tileHeight - 1; i >= 0; i --)
			{
				line.push_back(m_data[i][0]);
			}
			setSide(LEFT, line);
			line.clear();

			for (int i = 0; i < tileHeight; i ++)
			{
				line.push_back(m_data[i][tileWidth - 1]);
			}
			setSide(RIGHT, line);
		}

		int findPattern(const vector<string>& pattern, bool shouldImprint = false)
		{
			int cnt = 0;
			const int patternHeight = pattern.size();
			const int patternWidth = pattern[0].length();
			for (int y = 0; y < getTileHeight() - patternHeight; y ++)
			{
				for (int x = 0; x < getTileWidth() - patternWidth; x ++)
				{
					if (isPatternIncluded(x, y, pattern))
					{
						cnt ++;
						if (shouldImprint)
						{
							imprintPattern(x, y, pattern);
						}
					}
				}
			}
			return cnt;
		}

		int countSharp() const 
		{
			int cnt{};
			for (const string& line : m_data)
			{
				for (char c : line)
				{
					if (c == '#')
					{
						cnt ++;
					}
				}
			}
			return cnt;
		}

		void printSides() const
		{
			cout << "TILE ID " << m_id << endl;

			for (int i = 0; i < ALLSIDES; i ++)
			{
				cout << "[" << SIDENAMES[i] << "] " << m_sides[i] << endl; 
			}
		}

		void printTile() const 
		{
			cout << "TILE ID " << m_id << endl;

			for (int i = 0; i < (int) m_data.size(); i ++)
			{
				cout << m_data[i] << endl;
			}

			cout << endl;
		}

		static string reverse(const string& s) 
		{
			string ret;
			for (int i = s.length() - 1; i >= 0; i --) 
			{
				ret.push_back(s[i]);
			}
			return ret;
		}
	private:
		void imprintPattern(const int startX, const int startY, const vector<string>& pattern)
		{
			const int width = pattern[0].length();
			const int height = pattern.size();

			for (int y = 0; y < height; y ++)
			{
				for (int x = 0; x < width; x ++)
				{
					if (pattern[y][x] == '#')
					{
						m_data[y + startY][x + startX] = 'O';
					}
				}
			}			
		}
		bool isPatternIncluded(const int startX, const int startY, const vector<string>& pattern)
		{
			const int width = pattern[0].length();
			const int height = pattern.size();

			for (int y = 0; y < height; y ++)
			{
				for (int x = 0; x < width; x ++)
				{
					if (const char c = pattern[y][x]; c == '#')
					{
						if (c != m_data[y + startY][x + startX])
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		void setSide(SIDES side, const string& line)
		{
			m_sides[side] = line;
		}

		int m_id;
		vector<string> m_data;

		string m_sides[ALLSIDES];
};

class Solver
{
	public:
		void addTile(Tile&& tile_in) 
		{ 
			tile_in.setSideLines();
			m_tiles.push_back(tile_in); 
		}

		void solve();

	private:
		void printAllSides()
		{
			for (const auto&[line, tiles] : m_sideRegistry)
			{
				cout << "[" << line << "] : ( ";

				assert(tiles.size() < 3);
				for (const auto& tile : tiles)
				{
					cout << tile->getId() << " ";
				}
				cout << ")";

				if (auto ite = m_sideRegistry.find(Tile::reverse(line)); ite != m_sideRegistry.end())
				{
					const auto& revTiles = ite->second;
					for (int i = 0; i < (int)revTiles.size(); i ++)
					{
						assert(revTiles[i] == tiles[i]);
					}
				}

				cout << endl;
			}
		}
		std::vector<Tile*> findCorners() 
		{
			std::map<int, int> cntUnconnected;
			std::vector<Tile*> corners;

			for (const auto&[line, tiles] : m_sideRegistry)
			{
				assert(tiles.size() <= 2);

				if (tiles.size() == 1)
				{
					Tile* const cornerTile = tiles[0];
					const int tileId = cornerTile->getId();

					cntUnconnected[tileId] ++;
					if (cntUnconnected[tileId] == 4)
					{
						corners.push_back(cornerTile);
						//cout << tileId << " : " << cntUnconnected[tileId] << endl;
					}
				}
			}
			return corners;
		}
		void registerAllSides()
		{
			for (auto& tile : m_tiles)
			{
				for (int i = 0; i < ALLSIDES; i ++)
				{
					const string& line = tile.getSide((SIDES)i);
					m_sideRegistry[line].push_back(&tile);
					m_sideRegistry[Tile::reverse(line)].push_back(&tile);
				}
			}
		}
		void rotateToLeftTop(Tile* const tile)
		{
			for (int i = 0; i < 2; i ++)
			{
				for (int k = 0; k < ALLSIDES; k ++)
				{
					if (m_sideRegistry[tile->getSide(TOP)].size() == 1
						&& m_sideRegistry[tile->getSide(LEFT)].size() == 1)
					{
						return;
					}
					tile->rotate();
				}
				tile->flip();
			}
			cout << "Error! could not rotate tile " << tile->getId() << " to locate it to left top corner" << endl;
			assert(false);
		}

		Tile* getFacingTile(const Tile* tile, SIDES side)
		{
			const auto& candidate = m_sideRegistry[tile->getSide(side)];

			if (candidate.size() == 1)
				return nullptr;
			
			assert(candidate.size() == 2);

			Tile* foundTile = nullptr;

			for (auto t : candidate)
			{
				if (t->getId() != tile->getId())
				{
					foundTile = t;
					break;
				}
			}

			assert(foundTile);

			const string& facingEdge = Tile::reverse(tile->getSide(side));
			const SIDES oppFacingSide = getOpponent(side);

			for (int i = 0; i < 2; i ++)
			{
				for (int k = 0; k < ALLSIDES; k ++)
				{
					if (foundTile->getSide(oppFacingSide) == facingEdge)
					{
						return foundTile;
					}
					foundTile->rotate();
				}
				foundTile->flip();
			}

			cout << "Error! could not find tile " << tile->getId() << "'s " << SIDENAMES[side] << ", candidate " << foundTile->getId() << endl;
			assert(false);

			return nullptr;
		}
		void combine(Tile* const startTile);

		std::map<string, std::vector<Tile*>> m_sideRegistry;
		std::vector<Tile> m_tiles;
		Tile m_bigTile;
};

void Solver::combine(Tile* const startTile)
{
	rotateToLeftTop(startTile);

	std::vector<std::vector<Tile*>> combinedTiles;
	std::vector<Tile*> combinedLine;

	for (Tile* headTile = startTile; headTile; headTile = getFacingTile(headTile, BOTTOM))
	{
		for (Tile* tile = headTile; tile; tile = getFacingTile(tile, RIGHT))
		{
			combinedLine.push_back(tile);
		}
		combinedTiles.push_back(std::move(combinedLine));
		combinedLine.clear();
	}

	m_bigTile.clear();

	for (const auto& line : combinedTiles)
	{
#if 1
		cout << "[COMBINED] ";
		for (const auto tile : line)
		{
			cout << "[" << tile->getId() << "] ";
		}
		cout << endl;
#endif
		Tile* const firstTile = line[0];

		for (int y = 1; y < firstTile->getTileHeight() - 1; y ++)
		{
			string bigLine;

			for (const auto tile : line)
			{
				const string& tileLine = tile->getTileLine(y);
				bigLine += tileLine.substr(1, tileLine.length() - 2);
			}

			m_bigTile.addTileLine(bigLine);
		}
	}

	//m_bigTile.printTile();
}

void Solver::solve() 
{
	cout << "[SOLVE] numtiles = " << m_tiles.size() << endl;
	registerAllSides();
	const auto& corners = findCorners();
	combine(corners[0]);

	for (int i = 0; i < 2; i ++)
	{
		for (int k = 0; k < ALLSIDES; k ++)
		{
			const int cnt = m_bigTile.findPattern(PATTERN, true);

			if (cnt > 0)
			{
				m_bigTile.printTile();
				cout << "Ans : " << m_bigTile.countSharp() << endl;
				return;
			}
			m_bigTile.rotate();
		}
		m_bigTile.flip();
	}	
}

int main()
{
	ifstream fin("data/advc_20.txt");

	const auto startTime = chrono::system_clock::now();

	Solver sol;

	constexpr int TILELENGTH {10};

	while (!fin.eof())
	{
		string line;

		std::getline(fin, line);

		if (line.length() == 0) 
		{
			break;
		}

		stringstream ss(line);

		readAndSkip(ss, "Tile");

		int tileId;
		ss >> tileId;

		assert(tileId != 0);
		readAndSkip(ss, ":");

		Tile tile;
		tile.setId(tileId);

		for (int i = 0; i < TILELENGTH; i ++)
		{
			std::getline(fin, line);
			tile.addTileLine(line);

			if (line.length() != 10) 
				cout << "[" << tileId << "/" << i << "]" << "line " << line << ", length " << line.length() << endl;
			assert(line.length() == TILELENGTH);

		}

		sol.addTile(std::move(tile));

		std::getline(fin, line);
		assert(line.length() == 0);
	}

	sol.solve();

	sayTime("Solve finished", startTime);
}