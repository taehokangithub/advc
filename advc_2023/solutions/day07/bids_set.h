
#include <memory>
#include <string>
#include <vector>

#include "hands.h"

namespace advc_2023::day07
{
    class Bid
    {
    public:
        Bid(const std::string& str, rule_type rule);
        
        int get_bid_amount() const { return m_bid; }
        const Hand& get_hand() const { return m_hand; }

    private:
        Hand m_hand;
        int m_bid{ 0 };
    };

    // ---------------------------------------------

    class Bids_set
    {
    public:
        void parse(const std::vector<std::string>& lines, rule_type rule);
        int get_total_winnings() const;

    private:
        std::vector<std::shared_ptr<Bid>> m_bids;
    };
}