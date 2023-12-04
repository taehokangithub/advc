
#include <fstream>
#include <iostream>

#include "input_utils.h"

using namespace std;


namespace advc_2023::utils
{
    vector<string> get_lines(const string& file_name)
    {
        ifstream fin(file_name);
        vector<string> lines;

        if (fin.fail())
        {
            cout << "Could not open " << file_name << endl;
            return lines;
        }

        while (!fin.eof())
        {
            string line;
            std::getline(fin, line);

            if (line.length())
            {
                lines.push_back(line);
            }
        }

        return lines;
    }

    vector<string> split(const string& str, const string& delimeter)
    {
        size_t pos = 0;
        vector<string> ret;

        while (pos < str.length())
        {
            const size_t found_pos = str.find(delimeter, pos);

            string token;

            if (found_pos == string::npos)
            {
                token = str.substr(pos);
                pos = str.length();
            }
            else
            {
                token = str.substr(pos, found_pos - pos);
                pos = found_pos + delimeter.length();
            }
            
            if (token.length() > 0)
            {
                ret.push_back(token);
            }
        }

        return ret;
    }
}