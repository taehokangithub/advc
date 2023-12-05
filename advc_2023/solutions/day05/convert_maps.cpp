#include "convert_maps.h"

#include <assert.h>
#include <algorithm>
#include <limits>
#include <sstream>
#include <vector>

#include "../../utils/input_utils.h"

using namespace std;

#define DBG_PRINT true

namespace advc_2023::day05
{
    Rule::Rule(const string& str)
    {
        stringstream ss(str);
        ss >> m_dest_start >> m_src_start >> m_range;
        m_src_end = m_src_start + m_range;
        m_delta = m_dest_start - m_src_start;
    }

    // ----------------------------------------------------------

    string Seed_range::to_string() const
    {
        stringstream ss;
        ss << "[" << start << "-" << end << "]";
        return ss.str();
    }

    // ----------------------------------------------------------

    string Rule::to_string() const
    {
        stringstream ss;
        ss << "[SRC " << m_src_start << ":" << m_src_end << "/D " << m_delta << "]";
        return ss.str();
    }


    // ----------------------------------------------------------
    void Converter::sort()
    {
        std::sort(m_rules.begin(), m_rules.end(), [](const shared_ptr<Rule>& c1, const shared_ptr<Rule>& c2) {
            return c1->get_src_start() < c2->get_src_start();
        });
    }

    // ----------------------------------------------------------

    void Converter_maps::parse(const vector<string>& lines)
    {
        string first_line = lines.front();
        const auto seed_line_parts = utils::split(first_line, ": ");
        assert((int)seed_line_parts.size() == 2);

        // parses seed

        const auto seed_strs = utils::split(seed_line_parts.back(), " ");

        uint64_t prev_seed = 0;

        for (const auto& seed_str : seed_strs)
        {
            const uint64_t seed = std::stoull(seed_str);
            m_seeds.push_back(seed);

            if (prev_seed > 0)
            {
                const uint64_t range{ seed };   // different meaning assigned to a diferent name
                const uint64_t seed_end = prev_seed + range;
                m_seed_ranges.push_back({ prev_seed, seed_end });
                prev_seed = 0;
            }
            else
            {
                prev_seed = seed;
            }
        }

        // parse rules

        auto converter = std::make_shared<Converter>();

        // skip first 2 lines (seeds and seed-to-soil title)
        for (int i = 2; i < (int)lines.size(); i++)
        {
            const string& line = lines[i];

            const auto& split_line = utils::split(line, " ");
            if (split_line.back() == "map:")
            {
                assert(converter->has_rule());
                m_converters.push_back(converter);

                converter = std::make_shared<Converter>();
            }
            else
            {
                converter->add_rule(std::make_shared<Rule>(line));
            }
        }

        if (converter->has_rule())
        {
            m_converters.push_back(converter);
        }

        for (auto& converter : m_converters)
        {
            converter->sort();
        }
    }

    // ----------------------------------------------------------

    uint64_t Converter::convert(uint64_t seed) const
    {
        for (const auto& rule : m_rules)
        {
            if (rule->is_in_range(seed))
            {
                return rule->convert(seed);
            }
        }
        return seed;
    }

    // ----------------------------------------------------------

    vector<Seed_range> Converter::convert_range(const Seed_range& seed_range) const
    {
        vector<Seed_range> ret_seed_ranges;

        Seed_range cur_range{ seed_range };
        int rule_index = 0;
        

        while (rule_index < (int)m_rules.size() && cur_range.start < cur_range.end)
        {
            const auto& rule = m_rules[rule_index++];
            const bool is_last_rule = rule_index == m_rules.size();

            uint64_t end_seed = seed_range.end;

            if (cur_range.start < rule->get_src_start())
            {
                end_seed = std::min(end_seed, rule->get_src_start());
                rule_index--; // reuse the current rule
                ret_seed_ranges.push_back({ cur_range.start, end_seed });
            }
            else if(cur_range.start >= rule->get_src_end())
            {
                if (!is_last_rule)
                {
                    continue;   // skip to the next rule
                }
                ret_seed_ranges.push_back({ cur_range.start, end_seed });
            }
            else
            {
                end_seed = std::min(end_seed, rule->get_src_end());
                ret_seed_ranges.push_back(rule->convert({ cur_range.start, end_seed }));
            }

            cur_range.start = end_seed;
        }

        return ret_seed_ranges;
    }

    // ----------------------------------------------------------

    uint64_t Converter_maps::convert_seed_through(const uint64_t seed) const
    {
        uint64_t ret = seed;

        for (const auto& converter : m_converters)
        {
            ret = converter->convert(ret);
        }

        return ret;
    }

    // ----------------------------------------------------------

    uint64_t Converter_maps::find_lowest_final_value() const
    {
        uint64_t ret = numeric_limits<uint64_t>::max();

        for (const auto seed : m_seeds)
        {
            const auto val = convert_seed_through(seed);
            ret = std::min(ret, val);
        }

        return ret;
    }

    // ----------------------------------------------------------

    uint64_t Converter_maps::find_lowest_final_value_v2() const
    {
        vector<Seed_range> current_ranges{ m_seed_ranges };
        vector<Seed_range> next_ranges;

        for (const auto& converter : m_converters)
        {
            next_ranges.clear();

            for (const Seed_range& range : current_ranges)
            {
                const auto& new_ranges = converter->convert_range(range);
                next_ranges.insert(next_ranges.end(), std::make_move_iterator(new_ranges.begin()), std::make_move_iterator(new_ranges.end()));
            }

            current_ranges = std::move(next_ranges);
        }

        std::sort(current_ranges.begin(), current_ranges.end(), [](const Seed_range& a, const Seed_range& b) {
                return a.start < b.start;
            });

        return current_ranges.front().start;
    }
}