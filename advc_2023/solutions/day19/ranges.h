#include <map>
#include <memory>
#include <string>
#include <vector>

#include "../../utils/point.h"

namespace advc_2023::day19
{
    constexpr int END_RANGE = 4000;

    // -------------------------------------------

    struct Range
    {
        int start{ 1 };
        int end{ END_RANGE };

        void        set_start(int val) { start = std::max(start, val); }
        void        set_end(int val) { end = std::min(end, val); }
        uint64_t    get_cases() const { return end - start + 1; }
    };

    // -------------------------------------------

    struct Accept_condition
    {
        std::map<utils::Axis, Range> ranges;

        std::string to_string() const;
        Range       get_range_for_axis(utils::Axis) const;
        uint64_t    get_all_cases() const;
    };

}