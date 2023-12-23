#include <memory>
#include <string>
#include <vector>

#include "../../utils/grid.h"

namespace advc_2023::day23
{
    enum class tile_type : uint8_t
    {
        none,
        path,
        forest,
        slope_up,
        slope_right,
        slope_down,
        slope_left,
        visited,
    };

    enum class search_type : uint8_t
    {
        normal,
        ignore_slope,
    };

    class Search;

    using hiking_grid = utils::Grid<tile_type>;
    using search_list = std::vector<std::shared_ptr<Search>>;


    class Search
    {
    public:
        Search() = default;
        Search(const Search& other) = default;
        Search(Search&& other) = default;
        Search(const hiking_grid& grid, const utils::Point& curpos) : m_grid(grid), m_curpos(curpos), m_steps(0) {}

        static search_list  get_possible_steps(std::shared_ptr<Search> search, search_type st);

        void                set_visited() { m_grid.set(m_curpos, tile_type::visited); }
        const utils::Point& get_curpos() const { return m_curpos; }
        const hiking_grid&  get_grid() const { return  m_grid; }
        const int           get_steps() const { return m_steps; }
        void                draw() const;

    private:
        hiking_grid m_grid;
        utils::Point m_curpos;
        int m_steps{};
    };

    class Hiking
    {

    public:
        void parse(const std::vector<std::string>& lines);
        int get_longest_path() const;
        int get_longest_path_ignore_slope() const;

    private:
        void draw() const;
        hiking_grid m_grid;
        utils::Point m_enter_loc;
        utils::Point m_exit_loc;
    };
}