
#include <memory>
#include <string>
#include <vector>
#include <unordered_set>

namespace advc_2023::day04
{
    // -------------------------------------------

    class Cards
    {
    public:
        Cards(const std::string& str);
        int get_matching_count() const;
        int get_points() const;

    private:
        int m_card_id;
        std::unordered_set<int> m_wining_numbers;
        std::vector<int> m_my_numbers;
    };

    // -------------------------------------------

    class CardDeck
    {
    public:
        void parse(const std::vector<std::string>& lines);
        int get_total_points() const;
        int get_total_scratch_cards() const;

    private:
        std::vector<std::shared_ptr<Cards>> m_cards;
    };
}