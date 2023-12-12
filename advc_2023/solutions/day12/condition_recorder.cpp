#include <assert.h>
#include <iostream>

#include "../../utils/input_utils.h"
#include "condition_recorder.h"

using namespace std;

constexpr bool DISABLE_DBG_OUT1 = true;
#define DBG_OUT1 if (DISABLE_DBG_OUT1) {} else cout

constexpr bool DISABLE_DBG_OUT2 = true;
#define DBG_OUT2 if (DISABLE_DBG_OUT2) {} else cout

namespace advc_2023::day12
{
    //----------------------------------------------------------------

    bool Condition::expect(uint8_t number, int pos) const
    {
        // must be a start of damaged
        assert(pos == 0 || m_tiles[pos - 1] != tile_type::damaged);

        const int end_index = pos + number;
        if (end_index > (int)m_tiles.size())
        {
            return false;
        }

        if (end_index < (int)m_tiles.size() && m_tiles[end_index] == tile_type::damaged)
        {
            return false;
        }

        for (int count = 0; count < number; count ++)
        {
            if (pos == m_tiles.size())
            {
                return false;
            }

            const auto tile = m_tiles[pos];

            if (tile == tile_type::normal)    // "unknown" is fine
            {
                return false;
            }

            pos++;
        }

        return true;
    }

    //----------------------------------------------------------------

    uint64_t Condition::get_possible_count_internal(int condition_index, const int pos, tile_type override_type) const
    {
        DBG_OUT1 << "Entering " << to_string() << endl;

        for (int i = pos; i < (int) m_tiles.size(); i++)
        {
            const auto tile = (override_type == tile_type::none) ? m_tiles[i] : override_type;
            override_type = tile_type::none;

            assert(tile != tile_type::none);

            // We have found all numbers
            if (condition_index >= (int)m_expected.size())
            {
                if (tile == tile_type::damaged)
                {
                    DBG_OUT1 << "no more numbers" << endl;
                    return 0;
                }
            }
            else // We need more numbers
            {
                if (tile == tile_type::damaged)
                {
                    const int expected_number = m_expected[condition_index];
                    DBG_OUT1 << "  expecting [" << condition_index << "] " << expected_number << ":";

                    if (false == expect(expected_number, i)) // expected number could not exist
                    {
                        DBG_OUT1 << "failed" << endl;
                        return 0;
                    }

                    DBG_OUT1 << "OK" << endl;
                    i += expected_number;
                    condition_index++;
                }
                else if (tile == tile_type::unknown)
                {
                    const int64_t key = i + (((int64_t)condition_index) << 32);
                    
                    if (auto ite = m_cached_answers.find(key); ite != m_cached_answers.end())
                    {
                        DBG_OUT1 << "Hitting cache " << ite->first << " = " << ", total cache " << m_cached_answers.size() << endl;
                        return ite->second;
                    }

                    DBG_OUT1 << "   ==> branching " << endl;

                    const auto ans1 = get_possible_count_internal(condition_index, i, tile_type::damaged);
                    const auto ans2 = get_possible_count_internal(condition_index, i, tile_type::normal);
                    const auto ans = ans1 + ans2;

                    m_cached_answers[key] = ans;

                    DBG_OUT1 << "==> saving answers of " << key << " : " << ans1 << " + " << ans2 << " => " << ans << ", TOTAL CACHE " << m_cached_answers.size() << endl;

                    return ans1 + ans2;
                }
            }
        }

        if (condition_index != (int)m_expected.size())
        {
            return 0;
        }

        DBG_OUT1 << "<=== get myself of " << to_string() << endl;

        return 1;   // found only 1 (myself) possible count
    }
    //----------------------------------------------------------------

    void Condition::unfold(int count)
    {
        const auto original_expected = m_expected;

        for (int i = 0; i < count - 1; i++)
        {
            m_expected.insert(m_expected.end(), original_expected.begin(), original_expected.end());
            m_tile_str += '?' + m_tile_str;
        }

        const auto original_tiles = m_tiles;

        for (int i = 0; i < count - 1; i++)
        {
            m_tiles.push_back(tile_type::unknown);

            m_tiles.insert(m_tiles.end(), original_tiles.begin(), original_tiles.end());
        }
    }

    //----------------------------------------------------------------

    uint64_t Condition::get_possible_counts() const
    {
        
        DBG_OUT1 << "------------------------------------" << endl;

        const auto ans = get_possible_count_internal(0, 0, tile_type::none);

        DBG_OUT2 << "!!!! FINAL ANSWER " << ans << " FROM " << m_tile_str << endl << endl;

        return ans;
    }
    
    //----------------------------------------------------------------

    uint64_t Condition_recorder::get_possible_counts() const
    {
        uint64_t sum = 0;

        for (const auto& condition : m_conditions)
        {
            sum += condition->get_possible_counts();
        }

        return sum;
    }

    //----------------------------------------------------------------

    void Condition_recorder::unfold()
    {
        constexpr int unfold_times = 5;
        for (const auto& condition : m_conditions)
        {
            condition->unfold(unfold_times);
        }
    }
    
}