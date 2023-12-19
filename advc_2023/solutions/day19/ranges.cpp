
#include <assert.h>
#include <iostream>
#include <sstream>

#include "ranges.h"

using namespace std;

namespace advc_2023::day19
{
    // ---------------------------------------------

    std::string Accept_condition::to_string() const
    {
        stringstream ss;

        const auto x_range = get_range_for_axis(utils::Axis::X);
        const auto y_range = get_range_for_axis(utils::Axis::Y);
        const auto z_range = get_range_for_axis(utils::Axis::Z);
        const auto w_range = get_range_for_axis(utils::Axis::W);;

        ss << "[X:" << x_range.start << "-" << x_range.end << ",";
        ss << "Y:" << y_range.start << "-" << y_range.end << ",";
        ss << "Z:" << z_range.start << "-" << z_range.end << ",";
        ss << "W:" << w_range.start << "-" << w_range.end << "]";

        return ss.str();
    }

    // ---------------------------------------------

    Range Accept_condition::get_range_for_axis(utils::Axis axis) const
    {
        auto ite = ranges.find(axis);
        if (ite == ranges.end())
        {
            return {};
        }
        return ite->second;
    }

    // ---------------------------------------------

    uint64_t Accept_condition::get_all_cases() const
    {
        uint64_t ret = 1;
        ret *= get_range_for_axis(utils::Axis::X).get_cases();
        ret *= get_range_for_axis(utils::Axis::Y).get_cases();
        ret *= get_range_for_axis(utils::Axis::Z).get_cases();
        ret *= get_range_for_axis(utils::Axis::W).get_cases();
        return ret;
    }

}