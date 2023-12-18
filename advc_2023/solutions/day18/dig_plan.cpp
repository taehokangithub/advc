#include <algorithm>
#include <assert.h>
#include <iostream>
#include <queue>
#include <sstream>
#include <unordered_set>

#include "../../utils/input_utils.h"
#include "dig_plan.h"

using namespace std;

constexpr bool DISABLE_DBG_OUT = true;
#define DBG_OUT if (DISABLE_DBG_OUT) {} else cout

namespace advc_2023::day18
{
    void Dig_plan::parse(const vector<string>& lines)
    {
        for (const auto& line : lines)
        {
            const auto parts = utils::split(line, " ");
            assert(parts.size() == 3);

            auto ite = parts.begin();
            const string& dir_part = *(ite++);
            const string& distance_part = *(ite++);
            const string& colour_part = *(ite++);

            Dig_instruction inst;
            assert(dir_part.size() == 1);
            assert(colour_part.size() == 9);

            const char dir_char = dir_part.front();
            inst.distance = stoi(distance_part);
            inst.colour = colour_part.substr(2, 6);
            
            switch (dir_char)
            {
            case 'U': inst.dir = utils::Dir::Up; break;
            case 'D': inst.dir = utils::Dir::Down; break;
            case 'R': inst.dir = utils::Dir::Right; break;
            case 'L': inst.dir = utils::Dir::Left; break;
            default: assert(false);
            }

            m_instructions.push_back(inst);
        }
    }

    // ---------------------------------------------

    std::string Horizontal_line::to_string() const
    {
        stringstream ss;
        ss << "[" << x_start << "-" << x_end << ":" << y << "]";
        return ss.str();
    }

    // ------------------------------------------------------

    void Dig_plan::swap_instructions()
    {
        for (auto& inst : m_instructions)
        {
            stringstream ss;
            ss << std::hex << inst.colour.substr(0, 5);
            ss >> inst.distance;

            switch (inst.colour.back())
            {
            case '0': inst.dir = utils::Dir::Right; break;
            case '1': inst.dir = utils::Dir::Down; break;
            case '2': inst.dir = utils::Dir::Left; break;
            case '3': inst.dir = utils::Dir::Up; break;
            default:assert(false);
            }
        }
    }

    // ------------------------------------------------------
    void Dig_plan::setup_horizontal_lines() const
    {
        utils::Point cur_loc(0, 0);

        for (const auto& inst : m_instructions)
        {
            if (inst.dir == utils::Dir::Right)
            {
                Horizontal_line hl;
                hl.y = cur_loc.y;
                hl.x_start = cur_loc.x;
                hl.x_end = cur_loc.x + inst.distance;

                m_lines.push_back(hl);
            }
            else if (inst.dir == utils::Dir::Left)
            {
                Horizontal_line hl;
                hl.y = cur_loc.y;
                hl.x_start = cur_loc.x - inst.distance;
                hl.x_end = cur_loc.x;

                m_lines.push_back(hl);
            }

            cur_loc.move(inst.dir, inst.distance);
        }

        std::sort(m_lines.begin(), m_lines.end(), [](const Horizontal_line& a, const Horizontal_line& b)
            {
                if (a.y == b.y)
                {
                    assert(a.x_start != b.x_start);
                    assert(a.x_end != b.x_end);

                    return a.x_start < b.x_start;
                }
                return a.y < b.y;
            });
    }
    // ------------------------------------------------------

    uint64_t Dig_plan::get_cubic_meters() const
    {
        setup_horizontal_lines();
        vector<Horizontal_line> ongoing_lines;
        vector<Horizontal_line> new_lines;
        uint64_t sum = 0;

        for (auto line : m_lines)
        {
            DBG_OUT << "Handling " << line.to_string() << endl;

            for (auto& ongoing : ongoing_lines)
            {
                if (!ongoing.is_valid())
                {
                    continue;
                }

                DBG_OUT << "  ONGOING : " << ongoing.to_string() << endl;
                uint64_t local_added{ 0 };
                // take the result first
                if (ongoing.y != line.y)
                {
                    local_added = static_cast<uint64_t>(line.y - ongoing.y) * static_cast<uint64_t>(ongoing.x_end - ongoing.x_start + 1);
                    sum += local_added;
                    DBG_OUT << "   Taking sum from " << ongoing.to_string() << ", " << local_added << " => " << sum << endl;

                    ongoing.y = line.y;
                }

                if (!line.is_valid())
                {
                    continue;
                }

                local_added = 0;

                bool is_handled_internal{ true };
                if (ongoing.x_end == line.x_start) // expansion to the right
                {
                    ongoing.x_end = line.x_end;
                    DBG_OUT << "    : EXPANSION RIGHT " << ongoing.to_string() << endl;
                }
                else if (ongoing.x_start == line.x_end) // expansion to the left
                {
                    ongoing.x_start = line.x_start;
                    DBG_OUT << "    : EXPANSION LEFT " << ongoing.to_string() << endl;
                }
                else if (ongoing.x_end == line.x_end) // negation from the right
                {
                    local_added = (line.x_end - line.x_start);
                    ongoing.x_end = line.x_start;
                    DBG_OUT << "    : NEGATION RIGHT " << ongoing.to_string() << endl;
                }
                else if (ongoing.x_start == line.x_start) // negation from the left
                {
                    local_added = (line.x_end - line.x_start);
                    ongoing.x_start = line.x_end;
                    DBG_OUT << "    : NEGATION LEFT " << ongoing.to_string() << endl;
                }
                else if (ongoing.x_start < line.x_start && ongoing.x_end > line.x_end)  // SPLIT!
                {
                    Horizontal_line new_line;
                    new_line.y = line.y;
                    new_line.x_start = line.x_end;
                    new_line.x_end = ongoing.x_end;
                    new_lines.push_back(new_line);

                    ongoing.x_end = line.x_start;

                    local_added = line.x_end - line.x_start; // -1;
                    DBG_OUT << "    : SPLIT " << ongoing.to_string() << " and " << new_line.to_string() << endl;
                }
                else
                {
                    assert(ongoing.x_end < line.x_start || line.x_end < ongoing.x_start);
                    is_handled_internal = false;
                }

                if (is_handled_internal)
                {
                    sum += local_added;
                    line.invalidate();
                    DBG_OUT << "      ==> Changed cur_line to" << ongoing.to_string() << ", local added " << local_added << " => " << sum << endl;
                }
            }

            if (line.is_valid())
            {
                DBG_OUT << "  ==> pushing " << line.to_string() << endl;
                ongoing_lines.push_back(line);
            }

            std::sort(ongoing_lines.begin(), ongoing_lines.end(), [](const Horizontal_line& a, const Horizontal_line& b)
                {
                    if (a.y == b.y)
                    {
                        return a.x_start < b.x_start;
                    }
                    return a.y < b.y;
                });

            if (new_lines.size())
            {
                ongoing_lines.insert(ongoing_lines.begin(), new_lines.begin(), new_lines.end());
                new_lines.clear();
            }

            for (int i = 0; i < (int)ongoing_lines.size() - 1; i++)
            {
                auto& line1 = ongoing_lines[i];
                auto& line2 = ongoing_lines[i + 1];
                if (line1.x_end == line2.x_start)
                {
                    DBG_OUT << "* Merging " << line1.to_string() << " + " << line2.to_string();
                    line1.x_end = line2.x_end;  // merge
                    line2.invalidate();

                    DBG_OUT << " => " << line1.x_end << endl;

                    for (int k = i + 1; k < (int)ongoing_lines.size() - 1; k++)
                    {
                        ongoing_lines[k] = ongoing_lines[k + 1];
                    }
                    ongoing_lines.pop_back();
                }
            }
        }

        sum++; // the final 0-length line
        
        return sum;
    }

}