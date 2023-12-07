
#include <memory>
#include <string>
#include <vector>

namespace advc_2023::day07
{
    enum class rule_type
    {
        NORMAL, JOKER
    };

    class Hand
    {
        enum hand_type
        {
            HAND_TYPE_NONE,
            HAND_TYPE_ONE_PAIR,
            HAND_TYPE_TWO_PAIRS,
            HAND_TYPE_THREE_OF_A_KIND,
            HAND_TYPE_FULL_HOUSE,
            HAND_TYPE_FOUR_OF_A_KIND,
            HAND_TYPE_FIVE_OF_A_KIND,
            HAND_TYPE_ERROR,
        };

    public:
        Hand() {};
        Hand(const std::string& str, rule_type rule);
        std::string to_string() const;
        static bool compare(const Hand& lhs, const Hand& rhs);
        
    private:
        static hand_type get_hand_type(const std::vector<int> cards, rule_type rule);
        static std::string get_hand_type_name(hand_type type);

        std::vector<int>    m_cards;
        std::string         m_str;
        hand_type           m_type{ HAND_TYPE_ERROR };
    };
}