
#include <assert.h>
#include <iostream>
#include <sstream>
#include <map>

#include "hands.h"

using namespace std;

namespace advc_2023::day07
{
    constexpr int value_joker = 1;

    string Hand::to_string() const
    {
        stringstream ss;
        ss << "[" << m_str << ":" << get_hand_type_name(m_type) << "]";

        return ss.str();
    }

    // -------------------------------------------------

    Hand::Hand(const std::string &str, rule_type rule) : m_str(str)
    {
        for (const char c : str)
        {
            int val = 0;
            if (c >= '2' && c <= '9')
            {
                val = c - '0';
            }
            else
            {
                switch (c)
                {
                case 'T': val = 10; break;
                case 'J': val = (rule == rule_type::JOKER ? value_joker : 11); break;
                case 'Q': val = 12; break;
                case 'K': val = 13; break;
                case 'A': val = 14; break;
                }
            }
            assert(val != 0);
            m_cards.push_back(val);
        }

        m_type = get_hand_type(m_cards, rule);
    }

    // -------------------------------------------------

    Hand::hand_type Hand::get_hand_type(std::vector<int> cards, rule_type rule)
    {
        std::map<int, int> card_cnt;
        for (int val : cards)
        {
            card_cnt[val] ++;
        }

        int best_matching = 0;
        int second_matching = 0;
        int best_card = 0;

        for (const auto&[card, cnt] : card_cnt)
        {
            if (best_matching <= cnt)
            {
                second_matching = best_matching;
                best_matching = cnt;
                best_card = card;
            }
            else if (second_matching < cnt)
            {
                second_matching = cnt;
            }
        }

        if (rule == rule_type::JOKER)
        {
            if (best_card != value_joker)
            {
                best_matching += card_cnt[value_joker];
            }
            else
            {
                best_matching += second_matching;
            }
            
        }

        if (best_matching == 5)
        {
            return HAND_TYPE_FIVE_OF_A_KIND;
        }
        else if (best_matching == 4)
        {
            return HAND_TYPE_FOUR_OF_A_KIND;
        }
        else
        {
            if (best_matching == 3)
            {
                return second_matching == 2 ? HAND_TYPE_FULL_HOUSE : HAND_TYPE_THREE_OF_A_KIND;
            }
            else if (best_matching == 2)
            {
                return second_matching == 2 ? HAND_TYPE_TWO_PAIRS : HAND_TYPE_ONE_PAIR;
            }
        }
        return HAND_TYPE_NONE;
    }

    // -------------------------------------------------

    std::string Hand::get_hand_type_name(Hand::hand_type type)
    {
        switch (type)
        {
            case HAND_TYPE_NONE : return "None";
            case HAND_TYPE_ONE_PAIR : return "One Pair";
            case HAND_TYPE_TWO_PAIRS : return "Two Pairs";
            case HAND_TYPE_THREE_OF_A_KIND : return "Three";
            case HAND_TYPE_FULL_HOUSE : return "Full House";
            case HAND_TYPE_FOUR_OF_A_KIND : return "Four";
            case HAND_TYPE_FIVE_OF_A_KIND : return "Five";
        }
        
        assert(false);
        return string();
    }

    // -------------------------------------------------

    bool Hand::compare(const Hand &lhs, const Hand &rhs)
    {
        if (lhs.m_type != rhs.m_type)
        {
            return lhs.m_type > rhs.m_type;
        }
        for (int i = 0; i < (int) lhs.m_cards.size(); i ++)
        {
            const int lcard = lhs.m_cards[i];
            const int rcard = rhs.m_cards[i];
            if (lcard != rcard)
            {
                return lcard > rcard;
            }
        }
        // the 2 hands are the same - supposed not to happend
        assert(false);
        return false;
    }

}