#include <map>
#include <memory>
#include <string>
#include <vector>

#include "../../utils/grid.h"

namespace advc_2023::day23
{
    struct Edge;
    enum class tile_type : uint8_t;

    struct Node
    {
        int id;
        utils::Point loc;
        std::vector<Edge> edges;
    };

    // ----------------------------------------------------------

    struct Edge
    {
        std::shared_ptr<Node> target;
        uint16_t distance;
    };

    // ----------------------------------------------------------

    class Graph
    {
    public:
        Graph(const utils::Grid<tile_type>& grid, const utils::Point& enter_loc, const utils::Point& exit_loc);

        int find_longest_path() const;

    private:
        static int get_point_fingerprint(const utils::Point& p) { return p.y + (p.x << 8); }

        std::shared_ptr<Node> find_next_node(std::shared_ptr<Node> node, utils::Dir dir);
        std::shared_ptr<Node> get_or_create_node(const utils::Point& p);
        
        std::shared_ptr<Node> m_enter_node;
        std::shared_ptr<Node> m_exit_node;
        std::map<int, std::shared_ptr<Node>> m_nodes;

        const utils::Grid<tile_type>& m_grid;
    };
}