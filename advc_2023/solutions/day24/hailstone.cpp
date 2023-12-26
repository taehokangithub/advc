#include <assert.h>
#include <iostream>
#include <map>
#include <sstream>

#include "../../utils/input_utils.h"
#include "../../utils/math.h"
#include "hailstone.h"

using namespace std;

namespace advc_2023::day24
{
    // ------------------------------------------------------

    void Hailstone::parse(const std::string& line)
    {
        auto split = utils::split(line, " @ ");
        assert(split.size() == 2);

        auto locations = utils::split(split.front(), ",");
        m_location.x = stoull(locations[0]);
        m_location.y = stoull(locations[1]);
        m_location.z = stoull(locations[2]);

        m_velocity = utils::Point::parse_from_comma_string(split.back());
    }

    // ------------------------------------------------------

    std::string Hailstone::to_string() const
    {
        stringstream ss;
        ss << "[" << m_location.x << "," << m_location.y << "," << m_location.z << "@";
        ss << m_velocity.x << "," << m_velocity.y << "," << m_velocity.z << "]";
        return ss.str();
    }

    // ------------------------------------------------------

    bool Hailstone::does_intersect_2d(const Hailstone& rhs, uint64_t from, uint64_t to) const
    {
        const auto my_line = Line3D(m_location, m_velocity);
        const auto other_line = Line3D(rhs.m_location, rhs.m_velocity);

        const auto& my_coef = my_line.get_coef_xy();
        const auto& other_coef = other_line.get_coef_xy();

        if (my_coef.a == other_coef.a)
        {
            return false;
        }

        // y = ax + b
        // a1x + b1 = a2x + b2
        // x = (b2 - b1) / (a1 - a2)

        const auto x = my_line.get_x_intersection_xy(other_line);
        const auto y = my_line.get_y_for_x(x);

#ifdef _DEBUG
        const auto other_x = other_line.get_x_intersection_xy(my_line);
        const auto other_y = other_line.get_y_for_x(x);
        assert(utils::is_almost_same(x, other_x));
        assert(utils::is_almost_same(y, other_y));
#endif

        const auto is_past = [](const Hailstone& hs, Line3D::val_type x, Line3D::val_type y)
            {
                return (x - hs.m_location.x) * hs.m_velocity.x < 0;
            };

        const bool is_in_area = (x >= from && x <= to && y >= from && y <= to);
        const bool mine_is_past = is_past(*this, x, y);
        const bool rhs_is_past = is_past(rhs, x, y);
        return is_in_area && !mine_is_past && !rhs_is_past;
    }

    // ------------------------------------------------------C

    void Hailstones::parse(const vector<string>& lines)
    {
        for (const auto& line : lines)
        {
            m_hailstones.push_back(make_shared<Hailstone>());
            m_hailstones.back()->parse(line);
        }
    }

    // ------------------------------------------------------

    int Hailstones::count_all_intersections_2d(uint64_t from, uint64_t to) const
    {
        int sum = 0;
        for (int i = 0; i < (int)m_hailstones.size() - 1; i++)
        {
            for (int k = i + 1; k < (int)m_hailstones.size(); k++)
            {
                if (m_hailstones[i]->does_intersect_2d(*m_hailstones[k], from, to))
                {
                    sum++;
                }
            }
        }
        return sum;
    }

    // ------------------------------------------------------

    Line3D Hailstones::get_passer() const
    {
        /*
        * Passer has one intersection with each of others
        *       amy, bmy (a-mine-xy, b-mine-xy) are unknown
        *       amz, bmz (a-mine-xz, b-mine-xz) are unknown
        *       we have 4 unknowns for the passer
        *
        * fistly xy plane (where aoy, boy means a-other-xy, b-other-xy, known constants)
        *   at x = (boy - bmy) / (amy - aoy)
        *      y = aoy * (boy - bmy) / (amy - aoy) + boy
        *
        * same for xz plane (where aoz, boz means a-other-xz, b-other-xz, known constans)
        *      x = (boz - bmz) / (amz - aoz)
        *      z = aoz * (boz - bmz) / (amz - aoz) + boz
        *
        * these 2 points (x,y,?) and (x,?,z) should be the same point to have an intersection, so
        *     (boy - bmy) / (amy - aoy) == (boz - bmz) / (amz - aoz)
        *
        * we have 1 equation for 4 variables, we can have 4 of them from 4 lines
        *
        * let's say  constants   a : aoy1, b : boy1, c : aoz1, d : boz1
        *            variables   X : amy, y : bmy, w : amz, z : bmz
        *       then, (b - y) / (x - a) == (d - z) / (w - c)
        *       then, bw - bc - yw + cy == dx - ad - xz + az
        *   --> This is non-linear equation, because of the variable products (yw, xz)
        *   --> We can eliminate them by subtracting equations
        *
        * For example, two lines-equations (relations between (line 1, passer) and (line 2, passer)
        *       b1w - b1c1 - yw + c1y == d1x - a1d1 - xz + a1z
        *       b2w - b2c2 - yw + c2y == d2x - a2d2 - xz + a2z
        *   --> subracting both sides
        *   --> (b1 - b2)w - b1c1 + b2c2 + (c1 - c2)y == (d1 - d2)x - a1d1 + a2d2 + (a1 - a2)z
        *
        *   --> (d1 - d2)x + (c2 - c1)y + (b2- b1)w + (a1 - a2)z = a1d1 - a2d2 - b1c1 + b2c2
        *
        *   --> These are linear equations. We need 4 of them to solve x, y, w, z
        */

        constexpr int MIN_LINES = 5;
        assert(m_hailstones.size() >= MIN_LINES); // at least 5 lines required

        vector<Line3D> lines;

        for (int i = 0; i < MIN_LINES; i++)
        {
            lines.push_back(m_hailstones[i]->get_line3d());
        }

        // parameters for Gaussian elimination
        vector<vector<double>> parameters;
        const auto& line1 = lines.front();
        const auto a1 = line1.get_coef_xy().a;
        const auto b1 = line1.get_coef_xy().b;
        const auto c1 = line1.get_coef_xz().a;
        const auto d1 = line1.get_coef_xz().b;

        for (int k = 1; k < MIN_LINES; k++)
        {
            const auto& line2 = lines[k];
            const auto a2 = line2.get_coef_xy().a;
            const auto b2 = line2.get_coef_xy().b;
            const auto c2 = line2.get_coef_xz().a;
            const auto d2 = line2.get_coef_xz().b;

            parameters.push_back({});
            auto& parameter_set = parameters.back();

            parameter_set.push_back(d1 - d2);
            parameter_set.push_back(c2 - c1);
            parameter_set.push_back(b2 - b1);
            parameter_set.push_back(a1 - a2);
            parameter_set.push_back(a1 * d1 - a2 * d2 - b1 * c1 + b2 * c2);
        }

        assert(parameters.size() == 4);

        auto ans = utils::gaussian_elimination(parameters);

        Line3D passer;
        passer.get_coef_xy().a = ans[0];
        passer.get_coef_xy().b = ans[1];
        passer.get_coef_xz().a = ans[2];
        passer.get_coef_xz().b = ans[3];

#ifdef _DEBUG
        for (const auto& line : lines)
        {
            assert(passer.has_intersection(line));
        }
#endif

        return passer;
    }

    // ------------------------------------------------------
    int64_t Hailstones::get_position_sum_from_passer(const Line3D& passer) const
    {
        Hailstone s;
        vector<BigPoint> intersections;
        vector<double> seconds;
        std::map<int64_t, int> ans_votes;

        for (const auto& stone : m_hailstones)
        {
            const auto line = stone->get_line3d();
            assert(passer.has_intersection(line));

            const auto intersection = passer.get_intersection(line);

            // deduce starting point
            const double second_x = (double)(intersection.x - stone->get_location().x) / (double)stone->get_velocity().x;

#ifdef _DEBUG
            const double second_y = (double)(intersection.y - stone->get_location().y) / (double)stone->get_velocity().y;
            const double second_z = (double)(intersection.z - stone->get_location().z) / (double)stone->get_velocity().z;

            utils::is_almost_same(second_x, second_y);
            utils::is_almost_same(second_x, second_z);
#endif
            intersections.push_back(intersection);
            seconds.push_back(second_x);
            
            if (intersections.size() >= 2)
            {
                auto td = seconds.front() - seconds.back();

                const double vx = (intersections.front().x - intersections.back().x) / td;
                const double vy = (intersections.front().y - intersections.back().y) / td;
                const double vz = (intersections.front().z - intersections.back().z) / td;

                const double x = intersections.front().x - vx * seconds.front();
                const double y = intersections.front().y - vy * seconds.front();
                const double z = intersections.front().z - vz * seconds.front();

                BigPoint starting_point{
                    (int64_t) std::round(x),
                    (int64_t) std::round(y),
                    (int64_t) std::round(z)
                };

                s = Hailstone(starting_point, {});
                ans_votes[s.get_position_sum()]++;
            }
        }

        // There are some bigint-double conversion errors in a few cases.
        // Tired of digging down the error, using democracy instead
        int64_t max_voted_ans = 0;
        int max_votes = 0;

        for (const auto& [ans, votes] : ans_votes)
        {
            if (votes > max_votes)
            {
                max_voted_ans = ans;
                max_votes = votes;
            }
        }

        return max_voted_ans;

    }

    // ------------------------------------------------------

    int64_t Hailstones::get_passer_initial_position() const
    {
        const auto passer = get_passer();

        return get_position_sum_from_passer(passer);
    }

}