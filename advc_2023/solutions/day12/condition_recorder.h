#include <memory>
#include <string>
#include <unordered_map>
#include <vector>

namespace advc_2023::day12
{
    enum class tile_type
    {
        none,
        normal,
        damaged,
        unknown,
    };

    // -----------------------------------------------------------------------

    class Condition
    {
    public:
        uint64_t get_possible_counts() const;
        void parse(const std::string& line);
        void unfold(int count);

        std::string to_string() const;

    private:
        uint64_t get_possible_count_internal(int condition_index, int pos, tile_type override_type) const;
        bool expect(uint8_t number, int pos) const;

       
        std::vector<tile_type> m_tiles;
        std::vector<uint8_t> m_expected;
        std::string m_tile_str;

        mutable std::unordered_map<int64_t, uint64_t> m_cached_answers;
    };

    // -----------------------------------------------------------------------

    class Condition_recorder
    {
    public:
        uint64_t get_possible_counts() const;
        void unfold();
        void parse(const std::vector<std::string>& lines);

    private:
        std::vector<std::shared_ptr<Condition>> m_conditions;
    };
}