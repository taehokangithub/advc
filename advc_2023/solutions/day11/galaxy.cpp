#include <assert.h>
#include <iostream>
#include <map>

#include "../../utils/input_utils.h"
#include "galaxy.h"

using namespace std;

namespace advc_2023::day11
{
    void Galaxy::parse(const vector<string>& lines)
    {
        const int size_y = (int)lines.size();
        const int size_x = (int)lines.front().size();

        m_grid.set_size(size_x, size_y);

        for (const auto& line : lines)
        {
            for (const char c : line)
            {
                m_grid.add_element(c == '#');
            }
        }

        assert(m_grid.is_all_set());

        for (int x = 0; x < size_x; x++)
        {
            for (int y = 0; y < size_y; y++)
            {
                if (m_grid.get(utils::Point(x, y)))
                {
                    m_stars.push_back({ x, y });
                }
            }
        }

        m_base_distance = get_base_distances();
        m_expansions = get_expansions();
    }

    // ----------------------------------------------------------

    int64_t Galaxy::get_base_distances() const
    {
        int64_t distance = 0;
        for (int i = 0; i < (int)m_stars.size() - 1; i++)
        {
            for (int k = i + 1; k < (int)m_stars.size(); k++)
            {
                distance += m_stars[i].get_manhattan_distance(m_stars[k]);
            }
        }
        return distance;
    }

    // ----------------------------------------------------------

    vector<Expansion> Galaxy::get_expansions() const
    {
        std::map<int, int> x_axis_stars;
        std::map<int, int> y_axis_stars;

        vector<Expansion> expansions;

        for (const auto& star : m_stars)
        {
            x_axis_stars[star.x] ++;
            y_axis_stars[star.y] ++;
        }

        const auto add_expansions_for_axis = [this, &expansions](const std::map<int, int>& axis_stars, const int max_size, const utils::Axis axis)
            {
                int count_so_far = 0;
                for (int i = 0; i < max_size; i++)
                {
                    if (auto ite = axis_stars.find(i); ite != axis_stars.end())
                    {
                        count_so_far += ite->second;
                    }
                    else
                    {
                        Expansion e;
                        e.axis = axis;
                        e.location = static_cast<short>(i);
                        e.left = static_cast<short>(count_so_far);
                        e.right = static_cast<short>(m_stars.size() - count_so_far);

                        expansions.push_back(e);
                    }
                }
            };

        add_expansions_for_axis(x_axis_stars, m_grid.get_size().x, utils::Axis::X);
        add_expansions_for_axis(y_axis_stars, m_grid.get_size().y, utils::Axis::Y);

        return expansions;
    }

    // ----------------------------------------------------------

    int64_t Galaxy::get_expansion_distances(int n) const
    {
        int64_t distance = m_base_distance;

        for (const auto& expansion : m_expansions)
        {
            distance += (int64_t)expansion.left * (int64_t)expansion.right * (int64_t)(n - 1);
        }

        return distance;
    }

}