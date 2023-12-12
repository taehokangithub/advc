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

    bool Tiles::expect(uint8_t number, int pos)
    {
        // must be a start of damaged
        assert(pos == 0 || m_tiles[pos - 1] == tile_type::normal);

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

            if (tile == tile_type::unknown)
            {
                m_tiles[pos] = tile_type::damaged;
            }
            else if (tile == tile_type::normal)    // "unknown" is fine
            {
                return false;
            }

            pos++;
        }

        if (end_index < (int)m_tiles.size())
        {
            m_tiles[end_index] = tile_type::normal;
        }

        return true;
    }

    //----------------------------------------------------------------

    void Tiles::unfold(int count)
    {
        const auto original = m_tiles;

        for (int i = 0; i < count - 1; i++)
        {
            m_tiles.push_back(tile_type::unknown);

            m_tiles.insert(m_tiles.end(), original.begin(), original.end());
        }
    }

    //----------------------------------------------------------------

    uint64_t Tiles::get_possible_counts()
    {
        int condition_index = 0;

        DBG_OUT1 << "Entering " << to_string() << endl;

        for (int i = 0; i < (int) m_tiles.size(); i++)
        {
            const auto tile = m_tiles[i];
            assert(tile != tile_type::error);
            DBG_OUT1 << " : [" << i << "] " << get_tile_char(tile) << endl;

            // We have found all numbers
            if (condition_index >= (int)m_cond->get_expected().size())
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
                    const int expected_number = m_cond->get_expected()[condition_index];
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
                    const string& keystr = m_cond->get_tile_str().substr(i) + std::to_string(condition_index);
                    auto& cached_answer = m_cond->get_cached_answers();
                    
                    if (auto ite = cached_answer.find(keystr); ite != cached_answer.end())
                    {
                        DBG_OUT1 << "Hitting cache " << ite->first << " = " << ite->second << ", total cache " << cached_answer.size() << endl;
                        return ite->second;
                    }

                    DBG_OUT1 << "   ==> branching " << endl;
                    Tiles t1(*this);
                    Tiles t2(*this);

                    // Decide their fate to continue
                    t1.m_tiles[i] = tile_type::damaged;
                    t2.m_tiles[i] = tile_type::normal;

                    const auto ans1 = t1.get_possible_counts();
                    const auto ans2 = t2.get_possible_counts();
                    const auto ans = ans1 + ans2;

                    cached_answer[keystr] = ans;

                    DBG_OUT1 << "==> saving answers of " << keystr << " : " << ans1 << " + " << ans2 << " => " << ans << ", TOTAL CACHE " << cached_answer.size() << endl;

                    return ans1 + ans2;
                }
            }
        }

        if (condition_index != (int)m_cond->get_expected().size())
        {
            return 0;
        }

        DBG_OUT1 << "<=== get myself of " << to_string() << endl;

        return 1;   // found only 1 (myself) possible count
    }
    //----------------------------------------------------------------

    void Condition::unfold(int count)
    {
        m_tiles->unfold(count);

        const auto original = m_expected;

        for (int i = 0; i < count - 1; i++)
        {
            m_expected.insert(m_expected.end(), original.begin(), original.end());
            m_tile_str += '?' + m_tile_str;
        }

    }

    //----------------------------------------------------------------

    uint64_t Condition::get_possible_counts() const
    {
        Tiles copied_tiles(*m_tiles); 
         
        DBG_OUT1 << "------------------------------------" << endl;

        const auto ans = copied_tiles.get_possible_counts();  // it's a non-const method so we use copied one

        DBG_OUT2 << "!!!! FINAL ANSWER " << ans << " FROM " << m_tiles->to_string() << endl << endl;

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