#include <assert.h>
#include <map>
#include "cards.h"

#include "../../utils/input_utils.h"

using namespace std;

namespace advc_2023::day04
{

    // ---------------------------------------------------------
    
    Cards::Cards(const string& str)
    {
        const auto texts = utils::split(str, ": ");
        assert((int)texts.size() == 2);

        const auto card_desc = utils::split(texts.front(), " ");
        assert((int)card_desc.size() == 2);

        m_card_id = stoi(card_desc.back());

        const auto contents = utils::split(texts.back(), " | ");
        const auto winning_numbers_strs = utils::split(contents.front(), " ");
        const auto my_numbers_strs = utils::split(contents.back(), " ");

        for (const string& num_str : winning_numbers_strs)
        {
            m_wining_numbers.insert(stoi(num_str));
        }

        for (const string& num_str : my_numbers_strs)
        {
            m_my_numbers.push_back(stoi(num_str));
        }
    }

    // ---------------------------------------------------------
    int Cards::get_matching_count() const
    {
        int cnt = 0;
        for (const int my_num : m_my_numbers)
        {
            if (m_wining_numbers.find(my_num) != m_wining_numbers.end())
            {
                cnt++;
            }
        }
        return cnt;
    }

    // ---------------------------------------------------------

    int Cards::get_points() const
    {
        const int cnt = get_matching_count();

        const int point = cnt > 0 ? (1 << (cnt - 1)) : 0;

        return point;
    }

    // ---------------------------------------------------------

    void CardDeck::parse(const vector<string>& lines)
    {
        for (const auto& line : lines)
        {
            m_cards.push_back(make_shared<Cards>(line));
        }
    }

    // ---------------------------------------------------------

    int CardDeck::get_total_points() const
    {
        int sum = 0;

        for (const auto& cards : m_cards)
        {
            sum += cards->get_points();
        }

        return sum;
    }

    // ---------------------------------------------------------

    int CardDeck::get_total_scratch_cards() const
    {
        std::map<int, int> scratch_cards;

        for (int i = 0; i < (int)m_cards.size(); i ++)
        {
            const auto& cards = m_cards[i];

            const int cnt = cards->get_matching_count();

            scratch_cards[i]++; // the original one

            for (int k = 0; k < cnt; k++)
            {
                const int target_index = i + k + 1;
                scratch_cards[target_index] += scratch_cards[i];
            }
        }

        int sum = 0;

        for (const auto& ite : scratch_cards)
        {
            sum += ite.second;
        }

        return sum;
    }
}
