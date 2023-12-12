#include <assert.h>
#include <iostream>
#include <sstream>

#include "../../utils/input_utils.h"
#include "condition_recorder.h"

using namespace std;

namespace advc_2023::day12
{
    static tile_type get_tile_type(const char c)
    {
        switch (c)
        {
        case '#': return tile_type::damaged;
        case '.': return tile_type::normal;
        case '?': return tile_type::unknown;
        }

        assert(false);
        return tile_type::none;
    }

    //----------------------------------------------------------------

    string Condition::to_string() const
    {
        stringstream ss;

        ss << "[" << m_tile_str << "](";
        for (const auto num : m_expected)
        {
            const char c = static_cast<char>(num + '0');
            ss << c << " " ;
        }
        ss.seekp(-1, std::ios_base::end);

        ss << ")";
        return ss.str();
    }

    //----------------------------------------------------------------

    void Condition::parse(const std::string& line)
    {
        const auto parts = utils::split(line, " ");
        assert(parts.size() == 2);

        const auto& tile_part = parts.front();
        const auto& condition_part = parts.back();

        for (const auto& condition_str : utils::split(condition_part, ","))
        {
            const int condition_int = stoi(condition_str);
            assert(condition_int < 256);

            m_expected.push_back(static_cast<uint8_t>(condition_int));
        }

        m_tile_str = tile_part;
        for (const char c : tile_part)
        {
            m_tiles.push_back(get_tile_type(c));
        }
    }

    //----------------------------------------------------------------

    void Condition_recorder::parse(const vector<string>& lines)
    {
        for (const auto& line : lines)
        {
            auto condition = make_shared<Condition>();
            condition->parse(line);
            m_conditions.push_back(condition);
        }
    }

}