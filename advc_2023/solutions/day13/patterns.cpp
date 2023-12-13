#include <assert.h>
#include <iostream>

#include "../../utils/input_utils.h"
#include "patterns.h"

using namespace std;

namespace advc_2023::day13
{
    Pattern::Pattern(const std::vector<std::string>& lines)
    {
        utils::Point size{ (int)lines.front().size(), (int)lines.size() };
        m_grid.set_size(size);

        for (const auto& line : lines)
        {
            for (const char c : line)
            {
                const tile_type t = (c == '#' ? tile_type::rock : tile_type::ash);
                m_grid.add_element(t);
            }
        }
    }

    // --------------------------------------------------------------

    bool Pattern::compare_two_lines(mirror_type mtype, int pos1, int pos2) const
    {
        assert(mtype != mirror_type::none);

        if (mtype == mirror_type::vertical)
        {
            for (int y = 0; y < m_grid.get_size().y; y++)
            {
                assert(m_grid.is_valid_point(pos1, y));
                assert(m_grid.is_valid_point(pos2, y));

                if (m_grid.get(pos1, y) != m_grid.get(pos2, y))
                {
                    return false;
                }
            }
        }
        else if (mtype == mirror_type::horizontal)
        {
            for (int x = 0; x < m_grid.get_size().x; x++)
            {
                assert(m_grid.is_valid_point(x, pos1));
                assert(m_grid.is_valid_point(x, pos2));

                if (m_grid.get(x, pos1) != m_grid.get(x, pos2))
                {
                    return false;
                }
            }
        }

        return true;
    }

    // --------------------------------------------------------------

    bool Pattern::check_mirror(mirror_type mtype, int pos) const
    {
        if (m_mirror_type == mtype && m_mirror_pos == pos)
        {
            return false; // Avoid picking inheritted mirror. For part 1, these are none anyway
        }

        const int end = (mtype == mirror_type::horizontal ? m_grid.get_size().y : m_grid.get_size().x);

        assert(pos < end);

        for (int i = pos; i < end; i ++)
        {
            const int opp_index = pos - (i - pos) - 1;
            if (opp_index < 0)
            {
                break;
            }

            if (false == compare_two_lines(mtype, i, opp_index))
            {
                return false;
            }
        }
        return true;
    }

    // --------------------------------------------------------------

    void Pattern::set_mirror_point()
    {
        for (int x = 1; x < m_grid.get_size().x; x++)
        {
            if (check_mirror(mirror_type::vertical, x))
            {
                m_mirror_type = mirror_type::vertical;
                m_mirror_pos = x;
                return;
            }
        }
        for (int y = 1; y < m_grid.get_size().y; y++)
        {
            if (check_mirror(mirror_type::horizontal, y))
            {
                m_mirror_type = mirror_type::horizontal;
                m_mirror_pos = y;
                return;
            }
        }
    }

    // --------------------------------------------------------------

    std::shared_ptr<Pattern> Pattern::get_smudged_pattern() const
    {
        for (int x = 0; x < m_grid.get_size().x; x++)
        {
            for (int y = 0; y < m_grid.get_size().y; y++)
            {
                const utils::Point p(x, y);
                const auto toggled_tile = (m_grid.get(p) == tile_type::rock ? tile_type::ash : tile_type::rock);

                auto pattern = make_shared<Pattern>(*this);
                pattern->m_grid.set(p, toggled_tile);   // create a smudge

                pattern->set_mirror_point();
                if (pattern->m_mirror_pos != m_mirror_pos || pattern->m_mirror_type != m_mirror_type)
                {
                    return pattern;
                }
            }
        }
        assert(false);
        return nullptr;
    }

    // --------------------------------------------------------------

    int Pattern::get_pattern_score() const
    {
        assert(m_mirror_type != mirror_type::none);
        assert(m_mirror_pos > 0);

        int score = m_mirror_pos;
        if (m_mirror_type == mirror_type::horizontal)
        {
            score *= 100;
        }
        return score;
    }

    // --------------------------------------------------------------

    void Patterns::parse(const vector<string>& lines)
    {
        vector<string> cur_lines;

        auto create_pattern = [&]()
            {
                m_patterns.push_back(make_shared<Pattern>(cur_lines));
                m_patterns.back()->set_mirror_point();
            };

        for (const auto& line : lines)
        {
            if (line.empty())
            {
                create_pattern();
                cur_lines.clear();
            }
            else
            {
                cur_lines.push_back(line);
            }
        }

        if (!cur_lines.empty())
        {
            create_pattern();
        }
    }

    // --------------------------------------------------------------

    int Patterns::find_mirrior_number() const
    {
        int score = 0;

        for (const auto& pattern : m_patterns)
        {
            score += pattern->get_pattern_score();
        }

        return score;
    }

    // --------------------------------------------------------------

    int Patterns::find_smudged_mirror_number() const
    {
        int score = 0;

        for (const auto& pattern : m_patterns)
        {
            const auto smudged = pattern->get_smudged_pattern();
            assert(smudged);

            score += smudged->get_pattern_score();
        }

        return score;
    }
}