#include <assert.h>
#include <iostream>
#include <sstream>

#include "../../utils/input_utils.h"
#include "brick.h"

using namespace std;

namespace advc_2023::day22
{
    static bool check_linear_collision(int start_a, int end_a, int start_b, int end_b)
    {
        if (start_a > end_a)
        {
            std::swap(start_a, end_a);
        }
        if (start_b > end_b)
        {
            std::swap(start_b, end_b);
        }
        
        return (end_a >= start_b && end_b >= start_a);
    }

    // ----------------------------------------------------------------

    bool Brick::check_horizontal_collision(const Brick& other) const
    {
        return check_linear_collision(other.lower.x, other.upper.x, lower.x, upper.x)
            && check_linear_collision(other.lower.y, other.upper.y, lower.y, upper.y);
    }

    // ----------------------------------------------------------------

    void Brick::drop_down()
    {
        lower.z--;
        upper.z--;

        assert(lower.z > 0 && upper.z > 0);
    }

    // ----------------------------------------------------------------

    string Brick::to_string() const
    {
        stringstream ss;
        ss << "{" << lower.to_string_3d() << "-" << upper.to_string_3d() << "}";

        return ss.str();
    }

    // ----------------------------------------------------------------

    void Brick_map::parse(const vector<string>& lines)
    {
        for (const auto& line : lines)
        {
            const auto split = utils::split(line, "~");
            assert(split.size() == 2);

            auto brick = make_shared<Brick>();
            brick->lower = utils::Point::parse_from_comma_string(split.front());
            brick->upper = utils::Point::parse_from_comma_string(split.back());

            if (brick->lower.z > brick->upper.z)
            {
                std::swap(brick->lower, brick->upper);
            }

            m_supports[brick] = {};
            m_supported_by[brick] = {};

            add_to_floor(brick);
        }

        free_fall_all();
    }


    // ----------------------------------------------------------------

    void Brick_map::add_to_floor(std::shared_ptr<Brick> brick)
    {
        m_floors[brick->lower.z].insert(brick);

        if (brick->is_vertical())
        {
            m_floors[brick->upper.z].insert(brick);
        }
    }

    // ----------------------------------------------------------------

    void Brick_map::remove_from_floor(std::shared_ptr<Brick> brick)
    {
        m_floors[brick->lower.z].erase(brick);
        if (brick->is_vertical())
        {
            m_floors[brick->upper.z].erase(brick);
        }
    }

    // ----------------------------------------------------------------

    void Brick_map::free_fall(std::shared_ptr<Brick> brick)
    {
        //cout << "    falling brick " << brick->to_string() << endl;

        int fallen_floors = 0;

        for (int z = brick->lower.z; z > 1; z--)
        {
            bool has_found_support{ false };

            // check all bricks underneath
            //cout << "      => checking floor " << (z - 1) << endl;

            for (const auto other : m_floors[z - 1])
            {
                if (other == brick)
                {
                    continue;
                }
                if (brick->check_horizontal_collision(*other))
                {
                    m_supports[other].insert(brick);
                    m_supported_by[brick].insert(other);

                    has_found_support = true;

                    //cout << "        !!! found supporting brick " << other->to_string() << endl;
                }
            }

            if (has_found_support)
            {
                break;
            }

            fallen_floors++;
           
        }

        if (fallen_floors)
        {
            remove_from_floor(brick);

            brick->lower.z -= fallen_floors;
            brick->upper.z -= fallen_floors;
            add_to_floor(brick);

            //cout << "        fallen to ==> " << brick->to_string() << endl;
        }
    }

    // ----------------------------------------------------------------

    void Brick_map::free_fall_all()
    {
        for (auto [floor, brick_set] : m_floors)
        {
            for (auto& brick : brick_set)
            {
                if (brick->lower.z == floor)
                {
                    free_fall(brick);
                }
            }
        }
    }
    
    // ----------------------------------------------------------------

    int Brick_map::count_disintegratable_bricks() const
    {
        int sum = 0;

        for (auto& [floor, brick_set] : m_floors)
        {
            for (auto& brick : brick_set)
            {
                if (brick->lower.z == floor)
                {
                    const auto& supports = m_supports.find(brick)->second;

                    bool can_disintegrate{ true };

                    for (const auto& other_brick : supports)
                    {
                        const auto& other_supporters = m_supported_by.find(other_brick)->second;

                        if (other_supporters.size() == 1)
                        {
                            assert(*other_supporters.begin() == brick);

                            can_disintegrate = false;
                            break;
                        }
                    }

                    if (can_disintegrate)
                    {
                        sum++;
                    }
                }
            }
        }
   
        return sum;
    }

    // ----------------------------------------------------------------

    void Brick_map::chain_reaction_internal(std::shared_ptr<Brick> brick, brick_set_type& chained_away) const
    {
        const auto& supports = m_supports.find(brick)->second;
        vector<shared_ptr<Brick>> newly_chained_away;
        newly_chained_away.reserve(10);

        for (const auto& other_brick : supports)
        {
            const auto& other_supporters = m_supported_by.find(other_brick)->second;

            int count_alive = 0;

            for (const auto& other_supporter : other_supporters)
            {
                if (chained_away.find(other_supporter) == chained_away.end())
                {
                    count_alive++;
                }
            }

            if (count_alive == 0)
            {
                chained_away.insert(other_brick);
                newly_chained_away.push_back(other_brick);
            }
        }

        for (const auto& other_brick : newly_chained_away)
        {
            chain_reaction_internal(other_brick, chained_away);
        }
    }

    // ----------------------------------------------------------------

    int Brick_map::count_chain_reactions() const
    {
        int sum = 0;

        for (auto& [floor, brick_set] : m_floors)
        {
            for (auto& brick : brick_set)
            {
                if (brick->lower.z == floor)
                {
                    brick_set_type chained_away;
                    chained_away.insert(brick);
                    chain_reaction_internal(brick, chained_away);
                    sum += (int)(chained_away.size() - 1);
                    //cout << "checking chain reaction for " << brick->to_string() << " " << (chained_away.size()- 1) << " sum " << sum << endl;
                }
            }
        }
        return sum;
    }

}