
#include <algorithm>
#include <assert.h>
#include <iostream>
#include <sstream>
#include <map>

#include "../../utils/input_utils.h"
#include "bids_set.h"

using namespace std;

namespace advc_2023::day07
{
    Bid::Bid(const string& str, rule_type rule)
    {
        const auto strings = utils::split(str, " ");
        assert(strings.size() == 2);

        Hand hand(strings.front(), rule);
        m_hand = std::move(hand);

        m_bid = stoi(strings.back());
    }

    // ----------------------------------------------------------

    void Bids_set::parse(const std::vector<std::string>& lines, rule_type rule)
    {
        for (const auto& line : lines)
        {
            auto bid = make_shared<Bid>(line, rule);
            m_bids.push_back(bid);
        }
    }

    // ----------------------------------------------------------

    int Bids_set::get_total_winnings() const
    {
        auto bids_sorted = m_bids;
        std::sort(bids_sorted.begin(), bids_sorted.end(), [](const shared_ptr<Bid>& lhs, const shared_ptr<Bid>& rhs)
        {
            return Hand::compare(lhs->get_hand(), rhs->get_hand());
        });

        int sum{ 0 };

        for (int i = 0; i < (int)bids_sorted.size(); i++)
        {
            const auto bid = bids_sorted[i];
            const int rank = ((int)bids_sorted.size()) - i;
            const int winning = rank * bid->get_bid_amount();

            sum += winning;

#ifdef DBG_PRINT
            cout << " RANK [" << rank << "] " << bid->get_hand().to_string() << ", bid " << bid->get_bid_amount() << ", winning " << winning << ", total " << sum << endl;
#endif

        }

        return sum;
    }
}