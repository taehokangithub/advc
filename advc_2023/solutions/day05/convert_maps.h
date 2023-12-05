
#include <memory>
#include <string>
#include <vector>

namespace advc_2023::day05
{
    struct Seed_range
    {
        uint64_t start, end;
        std::string to_string() const;
    };

    // ---------------------------------------------------

    class Rule
    {
    public:
        Rule(const std::string& str);

        bool is_in_range(uint64_t src)              const { return src >= m_src_start && src < m_src_end; }

        uint64_t get_src_start()                    const { return m_src_start; }
        uint64_t get_src_end()                      const { return m_src_end; }
        uint64_t convert(uint64_t src)              const { return src + m_delta; }
        Seed_range convert(const Seed_range& range) const { return { range.start + m_delta, range.end + m_delta }; }

        std::string to_string()                     const;

    private:
        uint64_t m_dest_start;
        uint64_t m_src_start;
        uint64_t m_src_end;
        uint64_t m_range;
        int64_t m_delta;
    };

    // ---------------------------------------------------

    class Converter
    {
    public:
        void add_rule(std::shared_ptr<Rule> rule) { m_rules.push_back(rule); }
        bool has_rule() const { return !m_rules.empty(); }
        void sort();

        uint64_t convert(uint64_t seed) const;
        std::vector<Seed_range> convert_range(const Seed_range& seed_range) const;

    private:
        std::vector<std::shared_ptr<Rule>> m_rules;
    };

    // ---------------------------------------------------

    class Converter_maps
    {
    public:
        void parse(const std::vector<std::string>& lines);
        uint64_t find_lowest_final_value() const;
        uint64_t find_lowest_final_value_v2() const;

    private:
        uint64_t convert_seed_through(uint64_t seed) const;

        std::vector<uint64_t> m_seeds;
        std::vector<Seed_range> m_seed_ranges;
        std::vector<std::shared_ptr<Converter>> m_converters;
    };
}