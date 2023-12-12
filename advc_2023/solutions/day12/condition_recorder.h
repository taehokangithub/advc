#include <memory>
#include <string>
#include <unordered_map>
#include <vector>

namespace advc_2023::day12
{
    enum class tile_type
    {
        normal,
        damaged,
        unknown,
        error,
    };

    class Condition;

    // -----------------------------------------------------------------------

    class Tiles
    {
    public:
        Tiles(const Tiles& tiles) : m_cond(tiles.m_cond), m_tiles(tiles.m_tiles) {}
        Tiles(const Condition* cond) : m_cond(cond) {}

        void parses(const std::string& line);
        uint64_t get_possible_counts();  // it's non-const!
        std::string to_string() const;
        void unfold(int count);

    private:
        bool expect(uint8_t number, int pos);
        static char get_tile_char(tile_type t);

        std::vector<tile_type> m_tiles;
        const Condition* m_cond;
    };

    // -----------------------------------------------------------------------

    class Condition
    {
    public:
        uint64_t get_possible_counts() const;
        void parse(const std::string& line);
        void unfold(int count);

        const std::vector<uint8_t>& get_expected() const { return m_expected; }
        const std::string& get_tile_str() const { return m_tile_str; }

        std::unordered_map<std::string, uint64_t>& get_cached_answers() const { return m_cached_answers; }

    private:
        
        std::unique_ptr<Tiles> m_tiles;
        std::vector<uint8_t> m_expected;
        std::string m_tile_str;

        mutable std::unordered_map<std::string, uint64_t> m_cached_answers;
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